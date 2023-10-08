<?php
require_once("../host.inc");
$count_judges = 5; //mysqli_fetch_object(mysqli_query($link,"SELECT count(j.id) as judges FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) WHERE dp.durchgang = ".$_GET['durchgang']));
//round_log($_GET['teilnehmer'],$_GET['durchgang'],$count_judges->judges,$link);
speichern($_GET['teilnehmer'],$_GET['durchgang'],$_GET['wert'],$link);
?>