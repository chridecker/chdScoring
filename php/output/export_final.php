<?php
require_once("../host.inc");

if(isset($_GET['bewerb']))$bewerb = $_GET['bewerb'];
else $bewerb = 1;

//Bewerb
$query_bewerb = "SELECT name,number FROM bewerb WHERE id = ".$bewerb;
$res_bewerb = mysqli_fetch_object(mysqli_query($link,$query_bewerb));

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


$final_teilnehmer_count_base = round(($final_teilnehmer * $normalizationBaseLimit),0); //round()
if($final_teilnehmer_count_base == 0){$final_teilnehmer_count_base = 1;}

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


$csv= new SplTempFileObject();
$csv->setCsvControl(';');

$header = array(5+$durchgaenge*2);
$header[0] = "Rank";
$header[1] = "Name";
$header[2] = "Nation";
$header[3] = "FAI-License";
$j=4;
for($i=1;$i<=$durchgaenge;$i++){

    if($i == 1){
        $header[$j++] = "Preliminaries";
        $header[$j++] = "";
    }
	else {
		$res_programm = mysqli_fetch_object(mysqli_query($link,"SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".($i + $final_durchgang - 2)));
		$header[$j++] = "Final ".($i - 1)."(".$res_programm->title.")";
        $header[$j++] = "";
	}
}
$header[$durchgaenge*2+4] = "Total";
$csv->fputcsv($header);


$comp_data =  array(5+$durchgaenge*2);

$count = 1;
$query_teilnehmer = "SELECT t.*, b.teilnehmer, b.declined, b.prom as gesamt FROM bewerb".$bewerb." as b, teilnehmer as t WHERE b.durchgang = ".($durchgaenge+1)." AND b.teilnehmer = t.id ORDER BY prom DESC, declined DESC";
if($result_teilnehmer = mysqli_query($link,$query_teilnehmer)){
while($teilnehmer = mysqli_fetch_object($result_teilnehmer)){
    $country = mysqli_fetch_object(mysqli_query($link,"SELECT name, UPPER(short) as code FROM country_images WHERE img_id = ".$teilnehmer->land));
    $comp_data[0] = $count;
    $comp_data[1] = strtoupper($teilnehmer->nachname)." ".$teilnehmer->vorname;
    $comp_data[2]=  mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$teilnehmer->land))->name;
    $comp_data[3] = $country->code."-".$teilnehmer->license;
	$query_min = "SELECT min(prom) as min FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->id;
	$res_min = mysqli_fetch_object(mysqli_query($link,$query_min));

    $j=4;
	for($i=1;$i<=$durchgaenge;$i++){
		$query_durchgang = "SELECT b.prom, d.wert_abs FROM bewerb".$bewerb." as b, durchgang as d WHERE b.teilnehmer = ".$teilnehmer->id." AND b.durchgang = ".$i." AND d.durchgang = b.durchgang + ".($final_durchgang - 2)." AND d.teilnehmer = ".$teilnehmer->id;
		if($wert = mysqli_fetch_object(mysqli_query($link,$query_durchgang))){
            $comp_data[$j++] =  number_format($wert->prom,2,",","");
            if($i > 1){
                $comp_data[$j++] = number_format($wert->wert_abs,2,",","");
            }
            else {
                $comp_data[$j++] = "";
            }
		}
		else{
            $comp_data[$j++] = number_format(0,2,",","");
            $comp_data[$j++] = "-";
        }
        
	}
    $count++;
    $csv->fputcsv($comp_data);
    $comp_data =  array(5+$durchgaenge*2);
}
}

$csv->rewind();

header("Content-Type:text/csv");
header('Content-Disposition: attachment; filename="final.csv"');

$csv->fpassthru();
