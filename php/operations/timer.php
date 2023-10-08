<?php
require_once("../host.inc");
if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
$query = "SELECT wl.teilnehmer, wl.durchgang FROM wettkampf_leitung wl JOIN durchgang_airfield da ON (da.durchgang = wl.durchgang) WHERE da.airfield = ".$airfield." AND wl.status = 1";
$res_teilnehmer = mysqli_fetch_object(mysqli_query($link,$query));
if($_GET['action'] == "start"){
	mysqli_query($link,"UPDATE wettkampf_leitung wl SET wl.start_time = CURTIME() WHERE status = 1 AND teilnehmer = ".$res_teilnehmer->teilnehmer." AND durchgang = ".$res_teilnehmer->durchgang);
}
elseif($_GET['action'] == "stop"){
	mysqli_query($link,"UPDATE wettkampf_leitung wl SET wl.start_time = '".date("00:00:00")."' WHERE status = 1 AND teilnehmer = ".$res_teilnehmer->teilnehmer." AND durchgang = ".$res_teilnehmer->durchgang);
}
