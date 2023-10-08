<?php
require_once("../host.inc");
if($_GET['confirm'] == "yes" && $result_config->del_on == 1){
	$query = "TRUNCATE TABLE ";
	mysqli_query($link,$query."durchgang");
	mysqli_query($link,$query."wertung");
	mysqli_query($link,$query."wettkampf_leitung");
	mysqli_query($link,$query."judge_log");
	mysqli_query($link,$query."tbl_result");
	$ordnername = "../logs/";
	if ($dh = opendir($ordnername)) {
		while (($file = readdir($dh)) !== false) {
			if ($file!="." AND $file !="..") {
				unlink($ordnername.$file);
			}
		}
		closedir($dh);
	}
}
