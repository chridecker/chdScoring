<?php
require_once("../host.inc");

$durchgang = $_GET['durchgang'];

$wert = floatval($_GET['wert']);

echo $query = "INSERT INTO `wertung` (`teilnehmer`, `durchgang`, `figur`, `judge`, `wert`) VALUES (".$_GET['teilnehmer'].", ".$durchgang.", ".$_GET['figur'].", ".$_GET['judge'].", ".$wert.")";
mysqli_query($link,$query);