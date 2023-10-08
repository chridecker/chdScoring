<?php
require_once("../host.inc");
$teilnehmer = $_GET['teilnehmer'];
$durchgang= $_GET['durchgang'];
$wertung = $_GET['wertung'];
$start = $_GET['start'];
$ende  = $_GET['ende'];
for($figur=$start;$figur<=$ende;$figur++){
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) WHERE dp.durchgang = ".$durchgang);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$judge = $obj_judges->id;
		$query = "INSERT INTO wertung (`teilnehmer`, `durchgang`, `figur`, `judge`, `wert`) VALUES (".$teilnehmer.", ".$durchgang.", ".$figur.", ".$judge.",".$wertung.")";
		mysqli_query($link,$query);
	}
}
?>
