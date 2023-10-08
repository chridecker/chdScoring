<?php
require_once("host.inc");
$durchgang = $_GET['durchgang'];
$res = mysqli_query($link,"SELECT teilnehmer FROM wettkampf_leitung WHERE durchgang = ".$durchgang);
while($teilnehmer = mysqli_fetch_object($res)){
	for($f=1;$f<=17;$f++){
		for($j=1;$j<=5;$j++){
			mysqli_query($link,"INSERT INTO wertung (`teilnehmer`,`durchgang`,`judge`,`figur`,`wert`) VALUES (".$teilnehmer->teilnehmer.",".$durchgang.",".$j.",".$f.",".rand(5,10).")");
		}
	}
}
