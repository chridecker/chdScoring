<?php
require_once("../host.inc");

$teilnehmer = substr($_GET['delete'],-4,2);
$durchgang = substr($_GET['delete'],-1,1);


$query = "DELETE FROM wertung WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang;
mysqli_query($link,$query);
$query = "DELETE FROM durchgang WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang;
mysqli_query($link,$query);
mysqli_query($link,$query);

load($teilnehmer,$durchgang,$link);

