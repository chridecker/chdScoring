<?php
require_once("../host.inc");
if(isset($_GET['lmr']))$lmr = $_GET['lmr'] === 'true'? true: false;
else $lmr = false;
if(isset($_GET['durchgang'])){
	if($_GET['durchgang'] >= $result_config->end_final_durchgang && $result_config->three_panel_final == 1 && $lmr){
		mysqli_query($link,"DELETE FROM durchgang_panel WHERE durchgang = ".$_GET['durchgang']);
		mysqli_query($link,"INSERT INTO durchgang_panel (`durchgang`,`panel`) VALUES (".$_GET['durchgang'].",".$_GET['l'].")");
		mysqli_query($link,"INSERT INTO durchgang_panel (`durchgang`,`panel`) VALUES (".$_GET['durchgang'].",".$_GET['m'].")");
		mysqli_query($link,"INSERT INTO durchgang_panel (`durchgang`,`panel`) VALUES (".$_GET['durchgang'].",".$_GET['r'].")");
		mysqli_query($link,"DELETE FROM durchgang_panel WHERE durchgang = ".($_GET['durchgang'] + 1));
		mysqli_query($link,"INSERT INTO durchgang_panel (`durchgang`,`panel`) VALUES (".($_GET['durchgang'] + 1).",".$_GET['l'].")");
		mysqli_query($link,"INSERT INTO durchgang_panel (`durchgang`,`panel`) VALUES (".($_GET['durchgang'] + 1).",".$_GET['m'].")");
		mysqli_query($link,"INSERT INTO durchgang_panel (`durchgang`,`panel`) VALUES (".($_GET['durchgang'] + 1).",".$_GET['r'].")");
	}
	else{
		$query = "UPDATE durchgang_panel SET panel = ".$_GET['panel']." WHERE durchgang = ".$_GET['durchgang'];
		mysqli_query($link,$query);
	}
}