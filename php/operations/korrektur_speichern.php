<?php
require_once("../host.inc");
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$_GET['durchgang'];
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
$query = "UPDATE wertung SET wert = ".$_GET['wert']." WHERE teilnehmer = ".$_GET['teilnehmer']." AND durchgang = ".$_GET['durchgang']." AND figur = ".$_GET['figur']." AND judge = ".$_GET['judge'];
mysqli_query($link,$query);
$count_judges = mysqli_fetch_object(mysqli_query($link,"SELECT count(j.id) as judges FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$_GET['durchgang']));
$judges = $count_judges->judges;

$wert_durchgang = 0;
if($score_mode == 0){
	for($figur=$obj_count_figur->anfang;$figur<=$obj_count_figur->ende;$figur++){
		$wertungen = array();
		$count = 0;
		$no = 0;
		$k = 0;
		$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$_GET['durchgang']);
		while($obj_judges = mysqli_fetch_object($res_judges)){
			$judge = $obj_judges->id;
			$query_wert = "SELECT f.wert as k, w.wert as wert FROM figur as f, wertung as w WHERE w.teilnehmer = ".$_GET['teilnehmer']." AND w.durchgang = ".$_GET['durchgang']." AND f.id = ".$figur." AND w.judge = ".$judge." AND f.id = w.figur - 1 + ".$obj_count_figur->anfang;
			$result_wert = mysqli_fetch_object(mysqli_query($link,$query_wert));
			if($result_wert->wert == -1){
				if($no<1)$wertungen[] = 11;
				else $wertungen[]=$result_wert->wert;
				$no++;
			}
			else {
				$wertungen[]=$result_wert->wert;
			}
			$k = $result_wert->k;
		}
		sort($wertungen);
		$wert_figur = 0;
		for($i=1;$i<$judges-1;$i++)$wert_figur += $wertungen[$i];
		$wert_durchgang += $wert_figur * $k;
	}
}
else if($score_mode == 1){
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$_GET['durchgang']);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$i = $obj_judges->id;
		$endwert = 0;
		$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$_GET['durchgang'];
		$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
		for($figur=$obj_count_figur->anfang;$figur<=$obj_count_figur->ende;$figur++){
			$query = "SELECT (f.wert * ABS(w.wert)) as wertung FROM wertung as w, figur as f WHERE w.teilnehmer = ".$_GET['teilnehmer']." AND w.durchgang = ".$_GET['durchgang']." AND w.judge = ".$i." AND f.id = ".$figur." AND w.figur = f.id + 1 - ".($obj_count_figur->anfang);
			if($res = mysqli_fetch_object(mysqli_query($link,$query)))$endwert += $res->wertung;
		}
		$wert_durchgang += $endwert;
	}
	$wert_durchgang = $wert_durchgang / $judges;
}


update($_GET['teilnehmer'],$_GET['durchgang'],$wert_durchgang,$link);
