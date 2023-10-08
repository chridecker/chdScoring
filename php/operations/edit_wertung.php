<?php
require_once("../host.inc");
if($edit == 1){
	if(isset($_GET['teilnehmer'],$_GET['durchgang'],$_GET['judge'],$_GET['figur'])){
		mysqli_query($link,"DELETE FROM wertung WHERE teilnehmer = ".$_GET['teilnehmer']." AND durchgang = ".$_GET['durchgang']." AND judge = ".$_GET['judge']." AND figur = ".$_GET['figur']);
		mysqli_query($link,"DELETE FROM judge_log WHERE teilnehmer = ".$_GET['teilnehmer']." AND durchgang = ".$_GET['durchgang']." AND judge = ".$_GET['judge']);
	}
}