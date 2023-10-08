<?php
require_once("../host.inc");
$query = "INSERT INTO wertung (`teilnehmer`,`durchgang`,`figur`,`judge`,`wert`) VALUES (".$_GET['teilnehmer'].",".$_GET['durchgang'].",".$_GET['figur'].",".$_GET['judge'].",".$_GET['wert'].")";
mysqli_query($link,$query);