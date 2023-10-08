<?php 
require_once("../host.inc");

//Config File load XML
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;
$bewerb = 1;
$durchgaenge = $result_config->durchgaenge;

?>
<link rel="stylesheet" type="text/css" href="../css/overview.css" />
<table>
<tr class="headline">
<th><img src="../operations/load_image.php?id=2" width="100" /></th><th colspan="2">chdScoring - Technical Overview</th></tr>
<tr><th>Competition</th><td colspan="2"><?php echo $turnier." (".$turnier_no.")";?></td></tr>
<tr><th>Location / Date</th><td colspan="2"><?php echo $turnier_ort." / ".$turnier_date;?></td></tr>
<tr><th>Event Host</th><td colspan="1"><?php echo $veranstalter;?></td><td colspan="1"><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT CONCAT(vorname, ' ',nachname) as name FROM official WHERE id =".$result_config->org_leiter))->name;?></td></tr>
<tr><th>Competition Manager</th><td colspan="2">
<?php echo mysqli_fetch_object(mysqli_query($link,"SELECT CONCAT(vorname,' ',nachname) as name FROM official WHERE id = ".$result_config->wettkampf_leiter))->name;?></td></tr>
<tr><th>Pilots / Rounds</th><td colspan="2"><?php echo $teilnehmer_anzahl." Pilots"; echo " / ".$durchgaenge." Rounds"; if($score_mode == 1) echo " (TBL Algorithm used)";?></td></tr>
<tr><th>Programs</th><td colspan="2">
<?php for($i=1;$i<=$durchgaenge;$i++){
	$query_programm = "SELECT title FROM programm as p JOIN durchgang_programm as dp ON (p.id = dp.programm) WHERE dp.durchgang = ".$i;
	$obj_programm = mysqli_fetch_object(mysqli_query($link,$query_programm));
	echo " ".$obj_programm->title." ";
	if($i<$durchgaenge)echo "|";
}
?>
</td></tr>
<tr class="header"><th colspan="3">Officials</th></tr>
<tr><th>Head of Jury</th><td><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT CONCAT(vorname,' ',nachname) as name FROM official WHERE id = ".$jury))->name;?></td><td><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT license FROM official WHERE id = ".$jury))->license;?></td></tr>

<?php 
$query = "SELECT * FROM judge ORDER BY id ASC";
$res = mysqli_query($link,$query);
while($judge = mysqli_fetch_object($res)){?>
	<tr><th>Judge <?php echo $judge->id;?></th><td><?php echo $judge->name." ".$judge->vorname;?></td><td><?php echo $judge->license;?></td></tr>
	<?php
}?>
<tr class="header"><th colspan="3">Results</th></tr>
<?php 
if($finale == 1){
	$query = "CREATE TEMPORARY TABLE bewerb".$bewerb." SELECT teilnehmer, sum(wert_prom) - min(wert_prom) as prom FROM durchgang WHERE durchgang < ".$final_durchgang." GROUP BY teilnehmer ORDER BY prom DESC, min(wert_prom) DESC LIMIT ".$final_teilnehmer;
	mysqli_query($link,$query);
	mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD durchgang int(11) FIRST");
	mysqli_query($link,"UPDATE bewerb".$bewerb." SET durchgang = 1");
	
	for($durchgang = $final_durchgang;$durchgang<=$durchgaenge;$durchgang++){
		$query = "INSERT INTO bewerb".$bewerb." SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert_abs) / (SELECT max(d.wert_abs) FROM durchgang as d, teilnehmer as t, teilnehmer_bewerb as bw, bewerb as b WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb.") * 1000),2) as prom FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY t.id ASC;";
		mysqli_query($link,$query);
		mysqli_query($link,"UPDATE bewerb".$bewerb." SET durchgang = ".($durchgang - $final_durchgang + 2)." WHERE durchgang = ".$durchgang);
	}
	$durchgaenge = $durchgaenge - $final_durchgang + 2;
	$res_durchgaenge = mysqli_fetch_object(mysqli_query($link,"SELECT max(durchgang) as md FROM bewerb".$bewerb));
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
	$query = "SELECT t.* FROM teilnehmer as t JOIN (SELECT teilnehmer, prom as result FROM bewerb".$bewerb." WHERE durchgang = ".($durchgaenge + 1)." GROUP BY teilnehmer ORDER BY result DESC LIMIT 3) as r ON (r.teilnehmer = t.id)";
}
else $query = "SELECT t.* FROM teilnehmer as t JOIN (SELECT teilnehmer, sum(wert_prom) - min(wert_prom) as result FROM durchgang GROUP BY teilnehmer ORDER BY result DESC, min(wert_prom) DESC LIMIT 3) as r ON (r.teilnehmer = t.id)";
$res = mysqli_query($link,$query);
$count = 0;
while($teilnehmer = mysqli_fetch_object($res)){
	$count++;?>
	<tr><th>Rank <?php echo $count;?></th><td><?php echo $teilnehmer->vorname." ".$teilnehmer->nachname;?></td><td><?php echo $teilnehmer->club." / ";
	echo mysqli_fetch_object(mysqli_query($link,"SELECT UPPER(short) as short FROM country_images WHERE img_id = ".$teilnehmer->land))->short;?></td></tr>
	<?php
}?>
<tr><td colspan="3"></td></tr>
<tr class="footer"><td colspan="3"><?php echo $system_name." ".$version." &copy;".$year;?></td></tr>
</table>