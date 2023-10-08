<?php
require_once("../host.inc");

$durchgang = $_GET['durchgang'];

$wert = floatval($_GET['wert']);

echo $query = "UPDATE `wertung` SET `wert` = ".$wert." WHERE `teilnehmer` = ".$_GET['teilnehmer']." AND `durchgang` =  ".$durchgang." AND `figur` = ".$_GET['figur']." AND `judge` =  ".$_GET['judge'].";";
mysqli_query($link,$query);