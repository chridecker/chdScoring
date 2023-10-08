<?php 
require_once("../host.inc");
if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
$res = mysqli_fetch_object(mysqli_query($link,"SELECT wl.durchgang FROM wettkampf_leitung wl JOIN durchgang_airfield da ON (da.durchgang = wl.durchgang) WHERE wl.status = 1 AND da.airfield = ".$airfield));
$query = "UPDATE wettkampf_leitung SET status = 0 WHERE status = 1 AND durchgang = ".$res->durchgang;
mysqli_query($link,$query);
?>