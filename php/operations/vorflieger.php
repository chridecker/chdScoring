<?php
require_once("../host.inc");
if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
$query = "SELECT min(wl.durchgang) as durchgang FROM wettkampf_leitung wl JOIN durchgang_airfield da ON (da.durchgang = wl.durchgang) WHERE da.airfield = ".$airfield." AND wl.status < 3";
$res = mysqli_fetch_object(mysqli_query($link,$query));
$durchgang = $res->durchgang;
$query = "SELECT count(t.id) as count FROM wettkampf_leitung wl JOIN teilnehmer t ON (t.id = wl.teilnehmer) WHERE t.id = - ".$airfield." AND wl.durchgang = ".$durchgang;
$res = mysqli_fetch_object(mysqli_query($link,$query));

//Lade Vorflieger
if($res->count == 0){
	$query = "INSERT INTO teilnehmer (`id`,`vorname`,`nachname`,`club`,`land`,`license`,`bild`) VALUES (".(- $airfield).",'Max','Musterflieger','Vorflieger',13,'AUT-1234',1)";
	mysqli_query($link,$query);
	$query = "INSERT INTO wettkampf_leitung (`start`,`teilnehmer`,`durchgang`,`status`,`start_time`) VALUES (0,".(- $airfield).",".$durchgang.",1,'".date("00:00:00")."')";
	mysqli_query($link,$query);
	mysqli_query($link,"UPDATE wettkampf_leitung SET status = 0 WHERE durchgang = ".$durchgang." AND teilnehmer > 0");
}

//Lösche Vorflieger
if($res->count == 1	){
	mysqli_query($link,"DELETE FROM teilnehmer WHERE id = - ".$airfield);
	mysqli_query($link,"DELETE FROM wertung WHERE teilnehmer = - ".$airfield);
	mysqli_query($link,"DELETE FROM durchgang WHERE teilnehmer = - ".$airfield);
	mysqli_query($link,"DELETE FROM wettkampf_leitung WHERE teilnehmer = - ".$airfield);
}
