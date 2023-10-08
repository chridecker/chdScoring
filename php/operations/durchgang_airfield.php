<?php
require_once("../host.inc");
if(isset($_GET['durchgang']) && isset($_GET['airfield'])){
	$query = "UPDATE durchgang_airfield SET airfield = ".$_GET['airfield']." WHERE durchgang = ".$_GET['durchgang'];
	mysqli_query($link,$query);
}