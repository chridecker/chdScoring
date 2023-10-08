<?php
require_once("../host.inc");
error_reporting(0);
$query_online = "SELECT * FROM judge WHERE id = ".$_GET['judge'];
$res_online = mysqli_fetch_object(mysqli_query($link,$query_online));
$judge = $res_online->id;
$query_teilnehmer = "SELECT wl.teilnehmer, wl.durchgang, wl.start_time FROM judge_panel jp JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN wettkampf_leitung wl ON (wl.durchgang = dp.durchgang) WHERE wl.status = 1 AND jp.judge = ".$judge." ORDER BY wl.durchgang ASC LIMIT 1";
if($res_teilnehmer = mysqli_fetch_object(mysqli_query($link,$query_teilnehmer)))$durchgang = $res_teilnehmer->durchgang;

$diff =  strtotime(date("H:i:s")) - strtotime($res_teilnehmer->start_time);
//$diff =  $res_teilnehmer->start_time;

$query_judge = "SELECT t.* FROM teilnehmer as t, wettkampf_leitung as w WHERE t.id = w.teilnehmer AND w.status = 1 AND t.id = ".$res_teilnehmer->teilnehmer;
if($result_judge = mysqli_fetch_object(mysqli_query($link,$query_judge)))$teilnehmer = $result_judge->id;

$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$durchgang;
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
$figuren =  $obj_count_figur->anzahl;

if($confirm = mysqli_fetch_object(mysqli_query($link,"SELECT confirm FROM judge_log WHERE judge = ".$judge." AND teilnehmer = ".$result_judge->id." AND durchgang = ".$durchgang)))$confirm = $confirm->confirm;
else $confirm = 0;
if($judge_pin == 0)$confirm = 1;

header('Content-Type: text/json');
echo "{";
	echo "\"Round\": ".$durchgang.", \"Timer\": ".$diff.", \"Confirm\": ".$confirm.", \"Edit\": ".$edit.",";
	echo "\"Judge\": ";
		echo "{\"Id\": ".$judge.", \"Name\": \"".$res_online->name." ".$res_online->vorname."\"},";
	echo "\"Pilot\": ";
		echo "{\"Id\": ".$result_judge->id.", \"Name\": \"".$result_judge->vorname." ".$result_judge->nachname."\"},";
	echo "\"Schedule\": ";
		echo "{\"Count\": ".$obj_count_figur->anzahl.", \"Begin\": ".$obj_count_figur->anfang.",";
		$query = "SELECT * FROM figur WHERE id BETWEEN ".$obj_count_figur->anfang." AND ".$obj_count_figur->ende." ORDER BY id ASC";
		$query_figur = "SELECT f.id FROM figur f JOIN figur_programm fp ON (fp.figur = f.id) JOIN durchgang_programm dp ON (dp.programm = fp.programm) WHERE dp.durchgang = ".$durchgang." AND f.id NOT IN (SELECT (w.figur + ".$obj_count_figur->anfang." - 1) as id FROM wertung w WHERE w.teilnehmer = ".$result_judge->id." AND w.durchgang = ".$durchgang." AND w.judge = ".$judge.") ORDER BY f.id LIMIT 1";
		$res = mysqli_query($link,$query);
		echo "\"FigureLst\": [";
		while($figur = mysqli_fetch_object($res))
		{
			echo "{\"Id\": ".$figur->id.", \"K\":".$figur->wert.",";
			echo "\"Name\": \"".$figur->name."\",";
			/* if($wert = mysqli_fetch_object(mysqli_query($link,"SELECT wert FROM wertung WHERE teilnehmer = ".$result_judge->id." AND judge = ".$judge." AND durchgang = ".$durchgang." AND figur = ".($figur->id - $obj_count_figur->anfang + 1))))
			{
				if($wert->wert < 0 ) {echo "Score: N/O"; }
				else {echo "Score: ".abs($wert->wert).",";}
			} */
			echo "},";
		}
		echo "]";
	echo "},";
echo "}";
// mysqli_free_result($res);
// mysqli_close($link);