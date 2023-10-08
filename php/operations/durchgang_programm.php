<?php
require_once("../host.inc");
if(isset($_GET['durchgang']) && isset($_GET['programm'])){
	$query = "UPDATE durchgang_programm SET programm = ".$_GET['programm']." WHERE durchgang = ".$_GET['durchgang'];
	mysqli_query($link,$query);
}