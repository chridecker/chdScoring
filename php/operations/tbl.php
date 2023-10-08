<?php
require_once("../host.inc");
if(!isset($durchgang))$durchgang = $_GET['durchgang'];
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$durchgang;
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
$res_panels = mysqli_query($link,"SELECT jp.panel, count(jp.judge) as judges FROM durchgang_panel dp JOIN judge_panel jp ON (jp.panel = dp.panel) WHERE durchgang = ".$durchgang." GROUP BY dp.panel ORDER BY judges ASC, dp.panel ASC");
$panel = 1;
while($panels = mysqli_fetch_object($res_panels)){
	mysqli_query($link,"CREATE TEMPORARY TABLE statistik (judge int, teilnehmer int, wertung double)");
	$res_teilnehmer = mysqli_query($link,"SELECT DISTINCT(teilnehmer) FROM durchgang WHERE durchgang = ".$durchgang);
	while($obj_teilnehmer = mysqli_fetch_object($res_teilnehmer)){
		$teilnehmer = $obj_teilnehmer->teilnehmer;
		$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND dp.panel = ".$panels->panel);
		while($obj_judges = mysqli_fetch_object($res_judges)){
			$judge = $obj_judges->id;
			$query = "SELECT sum(abs(w.wert) * f.wert) as wertung FROM wertung as w, figur as f WHERE w.figur = f.id + 1 - ".$obj_count_figur->anfang." AND w.judge = ".$judge." and w.durchgang = ".$durchgang." AND teilnehmer = ".$teilnehmer;
			$res = mysqli_query($link,$query);
			while($obj = mysqli_fetch_object($res)){
				mysqli_query($link,"INSERT INTO statistik (`judge`,`teilnehmer`,`wertung`) VALUES (".$judge.",".$teilnehmer.",".$obj->wertung.")");
			}
		}
	}
	$res = mysqli_fetch_object(mysqli_query($link,"SELECT avg(wertung) as avg, STDDEV_SAMP(wertung) as std FROM statistik"));
	$avg = $res->avg;
	$std = $res->std;
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND dp.panel = ".$panels->panel);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$judge = $obj_judges->id;
		$res = mysqli_query($link,"SELECT avg(wertung) as avg, STDDEV_SAMP(wertung) as std FROM statistik WHERE judge = ".$judge);
			while($obj = mysqli_fetch_object($res)){
				mysqli_query($link,"UPDATE statistik SET wertung = wertung - ".($obj->avg - $avg)." WHERE judge = ".$judge);
				mysqli_query($link,"UPDATE statistik SET wertung = wertung - (wertung - ".$avg." - (wertung - ".$avg.") * ".($std/$obj->std).") WHERE judge = ".$judge);
			}
	}
	$res_teilnehmer = mysqli_query($link,"SELECT DISTINCT(teilnehmer) FROM statistik");
	while($obj_teilnehmer = mysqli_fetch_object($res_teilnehmer)){
		$teilnehmer = $obj_teilnehmer->teilnehmer;
		$wert = mysqli_fetch_object(mysqli_query($link,"SELECT AVG(wertung) as avg, STDDEV_SAMP(wertung) as std FROM statistik WHERE teilnehmer = ".$teilnehmer));
		$avg = $wert->avg;
		$std = $wert->std;
		$new_avg = 0;
		$new_std= 0;
		while($avg != $new_avg && $std != $new_std){
			if($new_avg != 0 && $new_std != 0){
				$avg = $new_avg;
				$std = $new_std;
			}
			$low = $avg - 1.645 * $std;
			$high = $avg + 1.645 * $std;
			$intervall = mysqli_fetch_object(mysqli_query($link,"SELECT AVG(wertung) as avg, STDDEV_SAMP(wertung) as std FROM statistik WHERE teilnehmer = ".$teilnehmer." AND wertung <= ".$high." AND wertung >= ".$low));
			$new_avg = $intervall->avg;
			$new_std = $intervall->std;
		}
		if($panel == 1){
			if($score_mode == 1)mysqli_query($link,"UPDATE durchgang SET wert_abs = ".$new_avg." WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang);
			mysqli_query($link,"INSERT INTO tbl_result (`teilnehmer`,`durchgang`,`wert`) VALUES (".$teilnehmer.",".$durchgang.",".$new_avg.")");
		}
		else {
			if($score_mode == 1)mysqli_query($link,"UPDATE durchgang SET wert_abs = wert_abs + ".$new_avg." WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang);
			mysqli_query($link,"UPDATE tbl_result SET wert = wert + ".$new_avg." WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang);
			
		}
	}
	mysqli_query($link,"TRUNCATE statistik");
	$panel++;
}