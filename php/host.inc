<?php
session_start();
/*-----------------------------------------------------------------------------------
-----------------------------Database Connection-------------------------------------
-----------------------------------------------------------------------------------*///
//$database = "cs";
//$database = "cs_rc3";
//$database = "cs_rookie"; // rc3

if(isset($_GET['db'])){$database = $_GET['db'];}
$normalizationBaseLimit = 0;
//echo $database;
include("db_connection.inc");
$link = mysqli_connect($host,$user,$password,$database);
/*-----------------------------------------------------------------------------------
--------------------------------    LOGIN       -------------------------------------
-----------------------------------------------------------------------------------*/
/*
if(isset($_COOKIE['cs_user']))$_SESSION['cs_user'] = $_COOKIE['cs_user'];
if(isset($_SESSION['cs_user'])){
	$user = $_SESSION['cs_user'];
	$user_level =  mysqli_fetch_object(mysqli_query($link,"SELECT level FROM user WHERE username = '".$user."'"))->level;
}
if(isset($user,$user_level)){
	switch ($user_level) {
		case 0:
			break;
		case 1:
			break;//
		case 5:
			header("location: /public");
			break;
		default:
			header("location: login.php");
			exit;
	}
}
//else header("location: login.php");
*/

/*-----------------------------------------------------------------------------------
-----------------------------Configurations------------------------------------------
-----------------------------------------------------------------------------------*/
$result_config = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM stammdaten"));
$count_judges = mysqli_fetch_object(mysqli_query($link,"SELECT count(id) as judges FROM judge"));
$count_subevent = mysqli_fetch_object(mysqli_query($link,"SELECT count(id) as subevent FROM bewerb"));
$count_teinehmeranzahl = mysqli_fetch_object(mysqli_query($link,"SELECT DISTINCT(count(id)) as teilnehmer FROM teilnehmer"));
$judges = $count_judges->judges;
if($result_config->end_finale == 1)$durchgaenge = $result_config->end_final_durchgang - 1;
else $durchgaenge = $result_config->durchgaenge;
$turnier = $result_config->turnier;
$veranstalter = $result_config->veranstalter;
$jury = $result_config->jury;
$org_leiter = $result_config->org_leiter;
$turnier_ort = $result_config->ort;
$turnier_no = $result_config->number;
if((strtotime($result_config->end_datum) - strtotime($result_config->datum)) > 0)$turnier_date = date("d",strtotime($result_config->datum))." - ".date("d.m.Y",strtotime($result_config->end_datum));
else $turnier_date = date("d.m.Y",strtotime($result_config->end_datum));
$subevents = $count_subevent->subevent;
$teilnehmer_anzahl = $count_teinehmeranzahl->teilnehmer;
$finale = $result_config->finale;
$final_durchgang = $result_config->final_durchgang;
$final_teilnehmer = $result_config->final_teilnehmer;
$end_finale = $result_config->end_finale;
$end_final_durchgang = $result_config->end_final_durchgang;
$end_final_teilnehmer = $result_config->end_final_teilnehmer;
$airfield_anzahl = $result_config->airfields;
$score_mode = $result_config->score_mode;
$judge_pin = $result_config->judge_pin;
$edit = $result_config->edit;

/*-----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------
-----------------------------Setting Fucntions---------------------------------------
-------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------*/

/*-----------------------------------------------------------------------------------
-----------------------------------------Start-------------------------------------
-----------------------------------------------------------------------------------*/
function start($durchgang,$link){
	$result_config = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM stammdaten"));
	$result_teilnehmer = mysqli_fetch_object(mysqli_query($link,"SELECT count(id) as anzahl FROM teilnehmer"));
	$finale = $result_config->finale;
	$final_durchgang = $result_config->final_durchgang;
	$final_teilnehmer = $result_config->final_teilnehmer;
	$end_finale = $result_config->end_finale;
	$end_final_durchgang = $result_config->end_final_durchgang;
	$end_final_teilnehmer = $result_config->end_final_teilnehmer;
	$airfields = $result_config->airfields;
	$durchgaenge = $result_config->durchgaenge;
	
	//Start Final Rounds
	if($end_finale == 1 && $durchgang >= $end_final_durchgang && $durchgang > 1){
		$query = "CREATE TEMPORARY TABLE final SELECT teilnehmer, sum(wert_prom) - min(wert_prom) as prom FROM durchgang WHERE durchgang < ".$final_durchgang." GROUP BY teilnehmer ORDER BY prom DESC, min(wert_prom) DESC LIMIT ".$final_teilnehmer;
		mysqli_query($link,$query);
		mysqli_query($link,"ALTER TABLE final ADD durchgang int(11) FIRST");
		mysqli_query($link,"UPDATE final SET durchgang = 1");
		$res_max_prom = mysqli_fetch_object(mysqli_query($link,"SELECT max(prom) as max_prom FROM final WHERE durchgang = 1"));
		mysqli_query($link,"UPDATE final SET prom = prom / ".$res_max_prom->max_prom." * 1000 WHERE durchgang = 1");
		//INSERT Semifinal Results
		for($d = $final_durchgang;$d<=($end_final_durchgang - 1);$d++){
			$query = "INSERT INTO final SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert_abs) / (SELECT max(d.wert_abs) FROM durchgang as d, teilnehmer as t WHERE d.durchgang = ".$d." AND d.teilnehmer = t.id) * 1000),2) as prom FROM durchgang as d, teilnehmer as t WHERE d.durchgang = ".$d." AND d.teilnehmer = t.id ORDER BY t.id ASC;";
			mysqli_query($link,$query);
			mysqli_query($link,"UPDATE final SET durchgang = ".($d - $final_durchgang + 2)." WHERE durchgang = ".$d);
		}
		$durchgaenge = $durchgaenge - $final_durchgang + 2;
		$res_durchgaenge = mysqli_fetch_object(mysqli_query($link,"SELECT max(durchgang) as md FROM final"));
		$query = "SELECT distinct(teilnehmer) FROM final";
		$res = mysqli_query($link,$query);
		if($res_durchgaenge->md <= 2){
			while($teilnehmer = mysqli_fetch_object($res)){
				$query_gesamt = "SELECT sum(prom) as gesamt FROM final WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
				$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
				$query_result = "INSERT INTO final (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
				mysqli_query($link,$query_result);
			}
		}
		else {
			while($teilnehmer = mysqli_fetch_object($res)){
				$min_final = mysqli_fetch_object(mysqli_query($link,"SELECT min(prom) as streicher FROM final WHERE teilnehmer = ".$teilnehmer->teilnehmer));
				$query_gesamt = "SELECT (sum(prom) - ".$min_final->streicher.") as gesamt FROM final WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
				$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
				$query_result = "INSERT INTO final (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
				mysqli_query($link,$query_result);
			}
		}
		
		$res= mysqli_query($link,"SELECT teilnehmer FROM final WHERE durchgang = ".($durchgaenge + 1)." ORDER BY prom DESC LIMIT ".$end_final_teilnehmer);
		$start = 1;
		while($obj = mysqli_fetch_object($res)){
			$query_insert = "INSERT INTO wettkampf_leitung (`start`,`teilnehmer`,`durchgang`,`status`,`start_time`) VALUES (".$start.",".$obj->teilnehmer.",".$durchgang.",0,'".date("00:00:00")."')";
			mysqli_query($link,$query_insert);
			$start++;
			//3 Panel Final 
			if($result_config->three_panel_final == 1){
				$res_panels = mysqli_query($link,"SELECT dp.panel, count(jp.judge) as judges FROM durchgang_panel dp JOIN judge_panel jp ON(jp.panel = dp.panel) WHERE durchgang = ".$durchgang." GROUP BY dp.panel ORDER BY judges ASC, jp.panel ASC");
				$counter = 1;
				while($obj_panels = mysqli_fetch_object($res_panels)){
					$res_judges = mysqli_query($link,"SELECT judge FROM judge_panel WHERE panel = ".$obj_panels->panel);
					while($obj_judges = mysqli_fetch_object($res_judges)){
						$obj_figuren = mysqli_fetch_object(mysqli_query($link,"SELECT count(fp.figur) as anzahl FROM durchgang_programm dp JOIN figur_programm fp ON (dp.programm = fp.programm) WHERE dp.durchgang = ".$durchgang));
						for($i=1;$i<=$obj_figuren->anzahl;$i++){
							mysqli_query($link,"INSERT INTO wertung (`teilnehmer`,`durchgang`,`judge`,`figur`,`wert`) VALUES (".$obj->teilnehmer.",".$durchgang.",".$obj_judges->judge.",".$i.",0)");
						}
						if($counter == 1){
							for($i=2;$i<=$obj_figuren->anzahl;$i+=4){
								mysqli_query($link,"DELETE FROM wertung WHERE teilnehmer = ".$obj->teilnehmer." AND durchgang = ".$durchgang." AND judge = ".$obj_judges->judge." AND figur = ".$i." AND wert = 0");
							}
						}
						elseif($counter == 2){
							for($i=4;$i<=$obj_figuren->anzahl;$i+=4){
								mysqli_query($link,"DELETE FROM wertung WHERE teilnehmer = ".$obj->teilnehmer." AND durchgang = ".$durchgang." AND judge = ".$obj_judges->judge." AND figur = ".$i." AND wert = 0");
							}
						}
						elseif($counter == 3){
							for($i=1;$i<=$obj_figuren->anzahl;$i+=2){
								mysqli_query($link,"DELETE FROM wertung WHERE teilnehmer = ".$obj->teilnehmer." AND durchgang = ".$durchgang." AND judge = ".$obj_judges->judge." AND figur = ".$i." AND wert = 0");
							}
						}
					}
				$counter++;	
				}
			}
		}
		$query_status = "UPDATE wettkampf_leitung SET status = 1 WHERE durchgang = ".$durchgang." AND start = 1";
		mysqli_query($link,$query_status);
	}	
	//------------------------------------------------------------------------------------------------
	//------------------------------------------------------------------------------------------------
	//------------------------------------------------------------------------------------------------
	//------------------------------------------------------------------------------------------------
	//Start Semifinal Rounds
	elseif($finale == 1 && $durchgang >= $final_durchgang && $durchgang > 1){
		$query = "SELECT teilnehmer, sum(wert_prom) - min(wert_prom) as result, min(wert_prom) as streicher FROM durchgang WHERE durchgang < ".$final_durchgang." GROUP BY teilnehmer ORDER BY result DESC, streicher DESC LIMIT ".$final_teilnehmer;
		$res = mysqli_query($link,$query);
		$start = 1;
		while($obj = mysqli_fetch_object($res)){
			$query_insert = "INSERT INTO wettkampf_leitung (`start`,`teilnehmer`,`durchgang`,`status`,`start_time`) VALUES (".$start.",".$obj->teilnehmer.",".$durchgang.",0,'".date("00:00:00")."')";
			mysqli_query($link,$query_insert);
			$start++;
		}
		$query_status = "UPDATE wettkampf_leitung SET status = 1 WHERE durchgang = ".$durchgang." AND start = 1";
		mysqli_query($link,$query_status);
	}
	//------------------------------------------------------------------------------------------------
	//------------------------------------------------------------------------------------------------
	//------------------------------------------------------------------------------------------------
	//------------------------------------------------------------------------------------------------
	//------------------------------------------------------------------------------------------------
	//Start Normal Rounds Airfield
	else {
		if($finale == 1)$durchgaenge = $final_durchgang - 1;
		else $durchgaenge = $result_config->durchgaenge;
		if($durchgang == 1)startreihenfolge($result_teilnehmer->anzahl,$durchgaenge,$airfields,$link);
		for($a=1;$a<=$airfields;$a++){
			$query_status = "UPDATE wettkampf_leitung SET status = 1 WHERE durchgang = ".$a." AND start = 1";
			mysqli_query($link,$query_status);
		}
	}
}

//Startreihenfolge Grunddurchgaenge
function startreihenfolge($teilnehmer,$durchgaenge,$airfields,$link){
	if($airfields > 1){
		$rest = $teilnehmer % $airfields;
		$anzahl = round($teilnehmer / $airfields);
	}
	else {
		$rest = $teilnehmer % $durchgaenge;
		$anzahl = round($teilnehmer / $durchgaenge);
	}
	$gruppe = 0;
	$count = 0;
	$t = 0;
	$tt = array();
	$res_teilnehmer = mysqli_query($link,"SELECT id FROM teilnehmer ORDER BY id ASC");
	while($obj_t = mysqli_fetch_object($res_teilnehmer))array_push($tt,$obj_t->id);
	for($d=0;$d<$durchgaenge;$d++){
		for($i=0;$i<$anzahl;$i++){
			if($t < $teilnehmer)$gruppen[$d][$i] = $tt[$t];
			$t++;
		}
		if($d == ($durchgaenge-1) && $rest == 1)$gruppen[$d][$anzahl] = $t;
	}
	for($j=1;$j<=$durchgaenge;$j++){
		$start = 0;
		$gruppe = $j - 1;
		for($d=$gruppe;$d<count($gruppen);$d++){
			for($i=0;$i<count($gruppen[$d]);$i++){
				$start++;
				$query = "INSERT INTO wettkampf_leitung (`start`,`teilnehmer`,`durchgang`,`status`,`start_time`) VALUES ";
				$query .= "(".$start.",".$gruppen[$d][$i].",".$j.",0,'".date("00:00:00")."')";
				mysqli_query($link,$query);
			}
		}
		if($j > 1){
			$gruppe = 0;
			for($d=$gruppe;$d<($j-1);$d++){
				for($i=0;$i<count($gruppen[$d]);$i++){
				$start++;
				$query = "INSERT INTO wettkampf_leitung (`start`,`teilnehmer`,`durchgang`,`status`,`start_time`) VALUES ";
				$query .= "(".$start.",".$gruppen[$d][$i].",".$j.",0,'".date("00:00:00")."')";
				mysqli_query($link,$query);
				}
			}
		}
	}
}

//Save Log Datei
function round_log($teilnehmer,$durchgang,$judges,$link){
	$log_file = fopen("../logs/".$durchgang."_".$teilnehmer.".log","w");
	$obj = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM teilnehmer WHERE id = ".$teilnehmer));
	$log = "P".$teilnehmer."/ R".$durchgang."\r\n".date("y-m-d H:m:s")."\r\n";
	$res = mysqli_query($link,"SELECT wert, judge FROM wertung WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang." ORDER BY figur ASC, judge ASC");
	$count = 0;
	while($obj = mysqli_fetch_object($res)){
		if($obj->wert < 0)$log .= "n";
		else $log .= $obj->wert;
		$count++;
		if($count == $judges){$log .= "\r\n";$count = 0;}
		else $log .="|";
	}
	fwrite($log_file,$log);
	fclose($log_file);
}

//Status ändern
function status($teilnehmer, $durchgang, $stat,$link){
	mysqli_query($link,"UPDATE wettkampf_leitung SET status = ".$stat." WHERE durchgang = ".$durchgang." AND teilnehmer = ".$teilnehmer);
}
//direkt Teilnehmer laden
function load($teilnehmer,$durchgang,$link){
	mysqli_query($link,"UPDATE wettkampf_leitung SET status = 0, start_time = '".date("00:00:00")."' WHERE status = 1 AND durchgang = ".$durchgang);
	status($teilnehmer,$durchgang,1,$link);
}
//Wertung übertragen in Durchgang
function speichern($teilnehmer,$durchgang,$wert,$link){
	$res = mysqli_query($link,"SELECT start_time FROM wettkampf_leitung WHERE status = 1 AND durchgang = ".$durchgang." AND teilnehmer = ".$teilnehmer);
	while($obj_teilnehmer = mysqli_fetch_object($res)){$diff =  strtotime(date("H:i:s")) - strtotime($obj_teilnehmer->start_time);}
	mysqli_query($link, "INSERT INTO durchgang (`teilnehmer`, `durchgang`, `wert_abs`,`wert_prom`,`duration`) VALUES (".$teilnehmer.", ".$durchgang.", ".$wert.",0,".$diff.")");
	umrechnung($durchgang,$link);
	status($teilnehmer, $durchgang,2,$link);
}
//Durchgang berechnen
function umrechnung($durchgang,$link){
	$query_prom = "SELECT max(wert_abs) as prom FROM durchgang WHERE durchgang = ".$durchgang;
	$result_prom = mysqli_fetch_object(mysqli_query($link,$query_prom)); 
	
	//$baseValue = getNormalizedBase($durchgang,0.5,$link);
	
	$query_update = "UPDATE durchgang SET wert_prom = ROUND((wert_abs / ".round($result_prom->prom,2)." * 1000),2) WHERE durchgang = ".$durchgang;
	mysqli_query($link,$query_update);
}

// Normalisieren auf Haelfte Teilnehmer
function getNormalizedBase($durchgang,$base, $link){
	$query_prom = "SELECT AVG(temp.wert_abs) as avg_value FROM (SELECT wert_abs FROM durchgang WHERE durchgang = ".$durchgang." ORDER BY wert_abs DESC LIMIT ".getCountNormalization($durchgang,$base,$link)." ) AS temp";
	$result_prom = mysqli_fetch_object(mysqli_query($link,$query_prom));
	return $result_prom->avg_value;
	// mysqli_query($link,"UPDATE durchgang SET wert_prom = ROUND((wert_abs / ".$result_prom->prom." * 1000),2) WHERE durchgang = ".$durchgang);
}
// Anzahl Normalisierungsteilnehmer zu Runde
function getCountNormalization($durchgang,$base,$link){
	$query_count = "SELECT ROUND(COUNT(wert_abs) * ".$base.") as limit_value FROM durchgang WHERE durchgang = ".$durchgang;
	$result_count = mysqli_fetch_object(mysqli_query($link,$query_count));
	return $result_count->limit_value;
}

//Updaten
function update($teilnehmer,$durchgang,$wert,$link){
	mysqli_query($link, "UPDATE durchgang SET wert_abs = ".$wert." WHERE teilnehmer =".$teilnehmer." AND durchgang =".$durchgang);
	umrechnung($durchgang,$link);
}
//Create Random Code
function genCode($numbers,$small,$caps,$special,$length){ //Nummern,kleinbuchstaben,GROSSBUCHSTABEN,sonderzeichen
	$code = "";
	while(strlen($code) < $length){
		$c = rand(1,4);
		if($c == 1 && $small)$code .= chr(mt_rand(97,122)); // kleinbuchstaben
		else if ($c == 2 && $numbers)$code .= chr(mt_rand(48,57)); //Zahlen
		else if ($c == 3 && $caps)$code .= chr(mt_rand(65,90)); //Großbuchstaben
		else if ($c == 4 && $special)$code .= chr(mt_rand(33,47));//SPezial
	}
	return $code;
}
//Get ID from License Code
function getIdFromLic($scan,$link){
	$pos = 99;
	for($i=0;$i<=9;$i++){
	$new_pos = strpos($scan,"".$i);
	if(gettype($new_pos) == 'integer')if($new_pos < $pos) $pos = $new_pos;
	}
	$license = substr($scan,$pos);
	$land = strtolower(substr($scan,0,$pos));
	$res = mysqli_query($link,"SELECT t.id FROM teilnehmer t JOIN country_images ci ON (t.land = ci.img_id) WHERE t.license = '".$license."' AND ci.short = '".$land."'");
	if($teilnehmer = mysqli_fetch_object($res))$id=$teilnehmer->id;
	else $id = 0;
	return $id; 
}
// Highlight search text in string
function highlightString($string,$search){
	$newstring = "";
	while(substr_count($string,$search) > 0){
			$newstring .= substr($string,0,strpos($string,$search))."<i style='font-style:normal; background-color:red;'>".$search."</i>";
			$string = substr($string,strpos($string,$search) + strlen($search),strlen($string));
	}
	return $newstring.$string;
}
?>