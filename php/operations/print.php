<?php
require_once("../host.inc");

$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;
//Teilnehmer Durchgangsansicht
$wert_durchgang = 0;
$teilnehmer = $_GET['teilnehmer'];
if(strlen($teilnehmer)>3)$teilnehmer = getIdFromLic($teilnehmer,$link);
$durchgang = $_GET['durchgang'];
$query_teilnehmer = "SELECT * FROM teilnehmer WHERE id = ".$teilnehmer;
$result_teilnehmer = mysqli_fetch_object(mysqli_query($link,$query_teilnehmer));
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$durchgang;
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
$query_programm = "SELECT p.title FROM programm p JOIN durchgang_programm dp ON (dp.programm = p.id) WHERE dp.durchgang = ".$durchgang;
$result_programm = mysqli_fetch_object(mysqli_query($link,$query_programm));
$count_judges = mysqli_fetch_object(mysqli_query($link,"SELECT count(j.id) as judges FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang));
$judges = $count_judges->judges;
$duration = mysqli_fetch_object(mysqli_query($link,"SELECT duration FROM durchgang WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang))->duration;
?>
<table>
<tr>
<th colspan="<?php echo ($judges + 2);?>" class="headline" style="text-align:left; vertical-align:bottom;">
Scoring Table <?php echo $turnier." (".$turnier_date.") - ".$turnier_ort;?></th>
<th colspan="2"><img src="operations/load_image.php?id=2" /></th>
</tr>
<tr class="teilnehmer">
<th colspan="3">Pilot</th><th colspan="2">FAI - License</th><th colspan="2">Flight duration</th><th colspan="100">Round</th></tr>
<tr class="teilnehmer">
<td># <?php echo $result_teilnehmer->id;?></td>
<td colspan="2">
<?php echo $result_teilnehmer->nachname." ".$result_teilnehmer->vorname." / ".$result_teilnehmer->club." / ".mysqli_fetch_object(mysqli_query($link,"SELECT UPPER(short) as short FROM country_images WHERE img_id = ".$result_teilnehmer->land))->short;?></td>
<td colspan="2"><?php echo $result_teilnehmer->license;?></td>
<td colspan="2"><?php echo str_pad(floor($duration/60),2,"0",STR_PAD_LEFT).":".str_pad(($duration % 60),2,"0",STR_PAD_LEFT);?></td>
<td colspan="3">R <?php echo $durchgang." / ".$result_programm->title;?></td></tr>
<tr class="header">
<th class="figur">&#8470;</th>
<th class="figur">Manoeuvre</th>
<th class="figur">K</th>
<?php
$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang);
while($obj_judges = mysqli_fetch_object($res_judges)){?>
	<th class="judge">Judge <?php echo $obj_judges->id;?></th>
    <?php
}?>    
<th class="judge">Total</th></tr>
<?php
$min_count = 0;
$max_count = 0;
for($figur=$obj_count_figur->anfang;$figur<=$obj_count_figur->ende;$figur++){
	if(($figur % 2) == 0 )echo "<tr class='gerade'>";
	else echo "<tr>";
	$query_figur = "SELECT f.name as figur, f.wert as k FROM figur as f WHERE f.id = ".$figur;
	$result_figur = mysqli_fetch_object(mysqli_query($link,$query_figur));
	echo "<td>".str_pad(($figur - $obj_count_figur->anfang + 1),2,0,STR_PAD_LEFT)."</td><td class='figur'>".$result_figur->figur."</td><td>".$result_figur->k."</td>";
	$wertungen = array();
	$count = 0;
	$no = 0;
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$judge = $obj_judges->id;
		$query_min_max = "SELECT min(abs(wert)) as min, max(abs(wert)) as max FROM wertung WHERE figur = ".($figur - $obj_count_figur->anfang + 1)." AND durchgang = ".$durchgang." AND teilnehmer = ".$teilnehmer;
		$min_max = mysqli_fetch_object(mysqli_query($link,$query_min_max));
		$min = $min_max->min;
		$max = $min_max->max;
		$query_wert = "SELECT w.wert as wert FROM wertung as w, figur as f WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = ".($figur - $obj_count_figur->anfang + 1)." AND w.judge = ".$judge." AND w.figur = f.id + 1 - ".$obj_count_figur->anfang;
		if($result_wert = mysqli_fetch_object(mysqli_query($link,$query_wert))){
			$wert = abs($result_wert->wert);
			if($wert == $min && $min_count < 1 && $score_mode == 0){
					echo "<td align='center' style='text-decoration:line-through;'>";
					$min_count++;
			}
				elseif($wert == $max && $max_count < 1 && $score_mode == 0){
					echo "<td align='center' style='text-decoration:line-through;'>";
					$max_count++;
			}
			else echo "<td align='center'>";
			$count++;
			if($result_wert->wert < 0){
				echo $wert."*";
				
			}
			else {
				echo $wert;
			}
		}
		else echo "<td align='center'>&nbsp;";
		echo "</td>";
	}
	echo "<td align='center'>";
	if($count == $judges && $score_mode == 0){
		$query_figurwert = "SELECT (sum(abs(w.wert))-min(abs(w.wert))-max(abs(w.wert)))*f.wert as erg FROM figur as f, wertung as w WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = f.id + 1 - ".$obj_count_figur->anfang." AND f.id = ".$figur;
		$res_figurwert = mysqli_fetch_object(mysqli_query($link,$query_figurwert));
		$wert_durchgang += $res_figurwert->erg;
		echo $res_figurwert->erg;
	}
	else if($count == $judges && $score_mode == 1){
		$query_figurwert = "SELECT (sum(abs(w.wert)))*f.wert as erg FROM figur as f, wertung as w WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = f.id + 1 - ".$obj_count_figur->anfang." AND f.id = ".$figur;
		$res_figurwert = mysqli_fetch_object(mysqli_query($link,$query_figurwert));
		$wert_durchgang += $res_figurwert->erg;
		echo $res_figurwert->erg;
	}
	else echo "0";
	echo "</td>";
	?></tr><?php
	$min_count = 0;
	$max_count = 0;
}
echo "<tr class='gesamt'><th colspan='3'></th>";
$wert_durchgang = mysqli_fetch_object(mysqli_query($link,"SELECT wert_abs FROM durchgang WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang))->wert_abs;
$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang);
while($obj_judges = mysqli_fetch_object($res_judges)){
	$j_wert = 0;
	for($k=$obj_count_figur->anfang;$k<=$obj_count_figur->ende;$k++){
		$query_judge = "SELECT (abs(w.wert)*f.wert) as gesamt FROM figur as f, wertung as w WHERE f.id = ".$k." AND w.judge = ".$obj_judges->id." AND w.durchgang = ".$durchgang." AND w.teilnehmer = ".$teilnehmer." AND w.figur = f.id + 1 - ".$obj_count_figur->anfang;
		$obj_judge = mysqli_fetch_object(mysqli_query($link,$query_judge));
		$j_wert += $obj_judge->gesamt;
	}
	?>
	<td style="text-align:center; padding-bottom:0.1cm;">
	<?php echo $j_wert;
	echo " (".number_format(-1 * ( 100 - 100 * $j_wert / $wert_durchgang),1)."%)";
	echo "<br>";
	echo $obj_judges->name." ".$obj_judges->vorname; ?></td>
    <?php
}
//$wert_durchgang = mysqli_fetch_object(mysqli_query($link,"SELECT wert_abs FROM durchgang WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang))->wert_abs;
//if($score_mode == 1) $wert_durchgang = $wert_durchgang / $judges;
echo "<th>".number_format($wert_durchgang,2)."</th></tr>";
?>
<tr class="footer"><td><?php echo date("d.m.Y H:i") ; ?></td><td colspan="<?php echo $judges+3;?>"><?php echo $system_name." ".$version." &copy; ".$year;?></td></tr>
</table>
