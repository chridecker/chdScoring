<?php
require_once("../host.inc");
if(isset($_GET["db"])){$link = mysqli_connect($host,$user,$password,$_GET["db"]);}

if(isset($_GET['bewerb']))$bewerb = $_GET['bewerb'];
else $bewerb = 1;
if(isset($_GET['logo']))$logo = $_GET['logo'];
//Bewerb
$query_bewerb = "SELECT name, number FROM bewerb WHERE id = ".$bewerb;
$res_bewerb = mysqli_fetch_object(mysqli_query($link,$query_bewerb));
if($bewerb != 1){
	$turnier = $res_bewerb->name;
	$turnier_no = $res_bewerb->number;
}

if($finale == 1) $durchgaenge = $final_durchgang - 1;

for($durchgang = 1;$durchgang<=$durchgaenge;$durchgang++){
	$query_teilnehmer_round_bewerb = "SELECT ROUND(count(t.id) * ".$normalizationBaseLimit.") as tBase FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id = ".$bewerb.";";
	$res_teilnehmer_round_bewerb = mysqli_fetch_object(mysqli_query($link,$query_teilnehmer_round_bewerb))->tBase;
	if($res_teilnehmer_round_bewerb == 0){$res_teilnehmer_round_bewerb = 1;}
	$query_round_r_base = "SELECT AVG(av.wert) as rBase FROM (SELECT d.wert_abs as wert FROM durchgang as d, teilnehmer as t, teilnehmer_bewerb as bw, bewerb as b WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY d.wert_abs DESC LIMIT ".$res_teilnehmer_round_bewerb.") as av";
	
	$rBaseVal = mysqli_fetch_object(mysqli_query($link,$query_round_r_base))->rBase;
	
	if($durchgang == 1 )$query = "CREATE TEMPORARY TABLE bewerb".$bewerb." SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert_abs) / ".($rBaseVal)." * 1000),2) as prom FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY t.id ASC;";
	else $query = "INSERT INTO bewerb".$bewerb." SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert_abs) / ".($rBaseVal)." * 1000),2) as prom FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY t.id ASC;";
	$res = mysqli_query($link,$query);
}
$res_durchgaenge = mysqli_fetch_object(mysqli_query($link,"SELECT max(durchgang) as md FROM durchgang"));
$query = "SELECT distinct(teilnehmer) FROM bewerb".$bewerb;
$res = mysqli_query($link,$query);
if($res_durchgaenge->md <= 1){
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
	while($teilnehmer = mysqli_fetch_object($res)){
		$query_gesamt = "SELECT sum(prom) - min(prom) as gesamt FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
		$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
		$query_result = "INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
		mysqli_query($link,$query_result);
		mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD declined float");
		$query_gesamt = "SELECT min(prom) as min FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
		$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
		$query_result = "UPDATE bewerb".$bewerb." SET declined = ".$res_gesamt->min." WHERE durchgang = ".($durchgaenge+1)." AND teilnehmer = ".$teilnehmer->teilnehmer;
		mysqli_query($link,$query_result);
	}
}
$csv = new SplTempFileObject();
$csv->setCsvControl(';');

$header = array(5+$durchgaenge*2);
$header[0] = "Rank";
$header[1] = "Name";
$header[2] = "Nation";
$header[3] = "FAI-License";
$j=4;
for($i=1;$i<=$durchgaenge;$i++){
	$res_programm = mysqli_fetch_object(mysqli_query($link,"SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".$i));
	$header[$j++] = "Round ".$i." (".$res_programm->title;
    $header[$j++] = "";
}
$header[$durchgaenge*2+4] = "Total";
$csv->fputcsv($header);

$header = array(5+$durchgaenge*2);
$header[0] = "";
$header[1] = "";
$header[2] = "";
$header[3] = "";
$j=4;
for($i=1;$i<=$durchgaenge;$i++){
	$res_programm = mysqli_fetch_object(mysqli_query($link,"SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".$i));
	$header[$j++] = "‰";
    $header[$j++] = "Points";
}
$header[$durchgaenge*2+4] = "";
$csv->fputcsv($header);


$count=0;
$query_teilnehmer = "SELECT t.*, b.teilnehmer, b.declined, b.prom as gesamt FROM bewerb".$bewerb." as b, teilnehmer as t WHERE b.durchgang = ".($durchgaenge+1)." AND b.teilnehmer = t.id ORDER BY prom DESC, declined DESC";

$comp_data =  array(5+$durchgaenge*2);
if($result_teilnehmer = mysqli_query($link,$query_teilnehmer)){
while($teilnehmer = mysqli_fetch_object($result_teilnehmer)){
    $count++;
	$country = mysqli_fetch_object(mysqli_query($link,"SELECT name, UPPER(short) as code FROM country_images WHERE img_id = ".$teilnehmer->land));
    $comp_data[0] = $count;
    $comp_data[1] = strtoupper($teilnehmer->nachname)." ".$teilnehmer->vorname;
    $comp_data[2]=  mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$teilnehmer->land))->name;
    $comp_data[3] = $country->code."-".$teilnehmer->license;
    
    $j = 4;
	for($i=1;$i<=$durchgaenge;$i++){
		$query_durchgang = "SELECT b.prom, d.wert_abs FROM bewerb".$bewerb." as b, durchgang as d WHERE b.durchgang = ".$i." AND b.teilnehmer = ".$teilnehmer->id." AND d.durchgang = ".$i." AND d.teilnehmer = ".$teilnehmer->id;
		if($wert = mysqli_fetch_object(mysqli_query($link,$query_durchgang))){
			$comp_data[$j++] =  number_format($wert->prom,2,",","");
            $comp_data[$j++] = number_format($wert->wert_abs,2,",","");
		}
		else {
            $comp_data[$j++] = number_format(0,2,",","");
            $comp_data[$j++] = "-";
            }
	}
    $comp_data[4+$durchgaenge*2] = number_format($teilnehmer->gesamt,2,",","");
    $csv->fputcsv($comp_data);
    $comp_data =  array(5+$durchgaenge*2);
}
}


$csv->rewind();

header("Content-Type:text/csv");
header('Content-Disposition: attachment; filename="prelim.csv"');

$csv->fpassthru();
