<?php 
require_once("../host.inc");
if(isset($_GET['area'],$_GET['id'],$_GET['type'])){
	$check = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM area_access WHERE area = ".$_GET['area']." AND ".$_GET['type']." = ".$_GET['id']))->area;
	if($check)mysqli_query($link,"DELETE FROM area_access WHERE area = ".$_GET['area']." AND ".$_GET['type']." = ".$_GET['id']);
	else mysqli_query($link,"INSERT INTO area_access (`area`,`".$_GET['type']."`) VALUES (".$_GET['area'].",".$_GET['id'].")");
}