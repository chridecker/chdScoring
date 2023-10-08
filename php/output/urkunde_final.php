<?php
require_once("../host.inc");
if(isset($_GET["db"])){$link = mysqli_connect($host,$user,$password,$_GET["db"]);}
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;

//$durchgaenge = $result_config->durchgaenge;
if(isset($_GET['bewerb']))$bewerb = $_GET['bewerb'];
else $bewerb = 1;
if(isset($_GET['logo']))$logo = $_GET['logo'];
//Bewerb
$query_bewerb = "SELECT name , number FROM bewerb WHERE id = ".$bewerb;
$res_bewerb = mysqli_fetch_object(mysqli_query($link,$query_bewerb));
if($bewerb != 1){
	$turnier = $res_bewerb->name;
	$turnier_no = $res_bewerb->number;
}
//if($finale == 1 && $bewerb != 1)$durchgaenge = $final_durchgang - 1; 


//Create Table & INSERT Preliminaries
$query = "CREATE TEMPORARY TABLE bewerb".$bewerb." SELECT teilnehmer, sum(wert_prom) - min(wert_prom) as prom FROM durchgang WHERE durchgang < ".$final_durchgang." GROUP BY teilnehmer ORDER BY prom DESC, min(wert_prom) DESC"; 
if($result_config->end_finale == 1 ) $query .= " LIMIT ".$final_teilnehmer;
mysqli_query($link,$query);
//Set Prelim to durchgang 1
mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD durchgang int(11) FIRST");
mysqli_query($link,"UPDATE bewerb".$bewerb." SET durchgang = 1");
//Norm Prelim Rounds to AVG of TOP half
$res_count_teilnehmer = mysqli_fetch_object(mysqli_query($link,"SELECT ROUND(count(teilnehmer) * ".$normalizationBaseLimit.") as tBase FROM bewerb".$bewerb." WHERE durchgang = 1"));

$final_teilnehmer_count_base = $res_count_teilnehmer->tBase;
$final_teilnehmer_count_base = round(($final_teilnehmer * $normalizationBaseLimit),0);
if($final_teilnehmer_count_base == 0){$res_teilnehmer_round_bewerb = 1;}
$res_max_prom = mysqli_fetch_object(mysqli_query($link,"SELECT AVG(t1.prom) as rBase FROM (SELECT prom FROM bewerb".$bewerb." WHERE durchgang = 1 ORDER BY prom DESC LIMIT ".$final_teilnehmer_count_base.") as t1"));
mysqli_query($link,"UPDATE bewerb".$bewerb." SET prom = prom / ".$res_max_prom->rBase." * 1000 WHERE durchgang = 1");

//INSERT Semifinal Results
for($durchgang = $final_durchgang;$durchgang<=$durchgaenge;$durchgang++){
	
	$query_teilnehmer_round_bewerb = "SELECT ROUND(count(t.id) * ".$normalizationBaseLimit.") as tBase FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id = ".$bewerb.";";
	$res_teilnehmer_round_bewerb = mysqli_fetch_object(mysqli_query($link,$query_teilnehmer_round_bewerb))->tBase;
	if($res_teilnehmer_round_bewerb == 0){$res_teilnehmer_round_bewerb = 1;}
	$query_round_r_base = "SELECT AVG(av.wert) as rBase FROM (SELECT d.wert_abs as wert FROM durchgang as d, teilnehmer as t, teilnehmer_bewerb as bw, bewerb as b WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY d.wert_abs DESC LIMIT ".$res_teilnehmer_round_bewerb.") as av";
	
	$rBaseVal = mysqli_fetch_object(mysqli_query($link,$query_round_r_base))->rBase;
	
	$query = "INSERT INTO bewerb".$bewerb." SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert_abs) / ".($rBaseVal)." * 1000),2) as prom FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY t.id ASC;";
	
	mysqli_query($link,$query);
	mysqli_query($link,"UPDATE bewerb".$bewerb." SET durchgang = ".($durchgang - $final_durchgang + 2)." WHERE durchgang = ".$durchgang);
}

$durchgaenge = $durchgaenge - $final_durchgang + 2;
$res_durchgaenge = mysqli_fetch_object(mysqli_query($link,"SELECT max(durchgang) as md FROM bewerb".$bewerb));
$query = "SELECT distinct(teilnehmer) FROM bewerb".$bewerb;
$res = mysqli_query($link,$query);
if($res_durchgaenge->md <= 2){
	while($teilnehmer = mysqli_fetch_object($res)){
		$query_gesamt = "SELECT sum(prom) as gesamt FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
		$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
		$query_result = "INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
		mysqli_query($link,$query_result);
		mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD declined float");
		$query_result = "UPDATE bewerb".$bewerb." SET declined = 0 WHERE durchgang = ".($durchgaenge+1)." AND teilnehmer = ".$teilnehmer->teilnehmer;
		mysqli_query($link,$query_result);
	}
}

else {
	$count = 0;
	while($teilnehmer = mysqli_fetch_object($res)){
		$count++;
		if($count <= $final_teilnehmer){
			$min_final = mysqli_fetch_object(mysqli_query($link,"SELECT min(prom) as streicher FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer));
			$query_gesamt = "SELECT (sum(prom) - ".$min_final->streicher.") as gesamt FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
			$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
			$query_result = "INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
			mysqli_query($link,$query_result);
			mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD declined float");
			$query_gesamt = "SELECT min(prom) as streicher FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
			$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
			$query_result = "UPDATE bewerb".$bewerb." SET declined = ".$res_gesamt->streicher." WHERE durchgang = ".($durchgaenge+1)." AND teilnehmer = ".$teilnehmer->teilnehmer;
			mysqli_query($link,$query_result);
		}
		else {
			$query_gesamt = "SELECT sum(prom) as gesamt FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
			$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
			$query_result = "INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
			mysqli_query($link,$query_result);
			mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD declined float");
			$query_result = "UPDATE bewerb".$bewerb." SET declined = 0 WHERE durchgang = ".($durchgaenge+1)." AND teilnehmer = ".$teilnehmer->teilnehmer;
			mysqli_query($link,$query_result);
		}
	}
}

?>
<link rel="stylesheet" type="text/css" href="../css/print_urkunde.css" />

<?php $rank = 1;?>
<?php
$query_teilnehmer = "SELECT t.*, b.teilnehmer, b.declined, b.prom as gesamt FROM bewerb".$bewerb." as b, teilnehmer as t WHERE b.durchgang = ".($durchgaenge+1)." AND b.teilnehmer = t.id ORDER BY prom DESC, declined DESC";
if($result_teilnehmer = mysqli_query($link,$query_teilnehmer)){
	while($teilnehmer = mysqli_fetch_object($result_teilnehmer)){?>
	<table align='center'>
		<tr><th class="veranstalter" colspan="3"><?php echo $veranstalter;?></th></tr>
		<tr><td><br></td></tr>
		<tr><td><br></td></tr>
		<tr><td colspan="3" class="logo"><img src="../operations/load_image.php?id=<?php echo $result_config->urkunde_id;?>"></td></tr>
		<tr><td><br></td></tr>
		<tr class="headline"><th colspan='3'>URKUNDE</th></tr>
		<tr><td><br></td></tr>
		<?php 
		$query_sub = "SELECT name FROM bewerb WHERE id = ".$bewerb;
		$sub = mysqli_fetch_object(mysqli_query($link,$query_sub));?>
		<tr class="header"><th class="turnier" colspan="3"><?php echo $turnier;?></th></tr>	
		<tr><td><br></td></tr>
		<tr><td><br></td></tr>
		<tr><td><br></td></tr>
		<tr class="ergebnis"><th colspan="3" class="name"><?php echo strtoupper($teilnehmer->nachname)." ".$teilnehmer->vorname;?></th></tr>
		<tr><td><br></td></tr>
		<tr class="ergebnis"><th colspan="3" class="club">errang in der Klasse <?php echo $sub->name; ?></th></tr>
		<!-- <tr class="ergebnis"><th colspan="3" class="punkte">mit <?php echo number_format($teilnehmer->gesamt,2,",","");?> 
        &permil; Punkten</th></tr> -->
		<tr><td><br></td></tr>
		<tr class="ergebnis"><th colspan="3" class="platz"><i>den</i> <?php echo $rank;?>. Platz</th></tr>
		<tr><td><br></td></tr>
		<!-- <tr class="bild"><td colspan="3" class="gruppenbild"><img src="../operations/load_image.php?id=<?php echo $result_config->gruppe_id;?>"></td></tr> -->
		<tr><td><br></td></tr>
		<tr><td><br></td></tr>
		<tr><td><br></td></tr>
		<!--<tr class="footer"><td colspan="3" class="spacer"></td></tr>-->
        <tr class="footer">
        <td class="signature"><?php if(isset($_GET['cm']) && $_GET['cm']==true)echo "<img src='../bilder/cm_sign.png'>";?></td>
        <td>&nbsp;</td>
        <td class="signature"><?php if(isset($_GET['org']) && $_GET['org']==true)echo "<img src='../bilder/org_sign.png'>";?></td></tr>
		<tr class="footer"><th class="name"><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT CONCAT(vorname, ' ', nachname) as name FROM official WHERE id = ".$result_config->wettkampf_leiter))->name;?></th><td></td><th class="name"><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT CONCAT(vorname, ' ', nachname) as name FROM official WHERE id = ".$result_config->org_leiter))->name;?></th></tr>
		<tr class="footer"><th class="funktion"><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT club as name FROM official WHERE id = ".$result_config->wettkampf_leiter))->name;?></th><td></td><th class="funktion"><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT club as name FROM official WHERE id = ".$result_config->org_leiter))->name;?></th></tr>
		<tr><td colspan="3" class="ort"><?php echo $turnier_ort;?>, <?php echo date("d.m.Y",strtotime($result_config->end_datum));?></td></tr>
		</table>
		<?php 
		$rank++;
	}
}
?>