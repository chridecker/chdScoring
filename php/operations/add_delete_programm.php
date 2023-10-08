<?php 
require_once("../host.inc");
if(isset($_GET['figuren'])){
	$figuren = $_GET['figuren'];
	$max = mysqli_fetch_object(mysqli_query($link,"SELECT max(f.id) as figur, max(p.id) as programm FROM programm as p, figur as f"));
	$title = "Custom Programm";
	$query = "INSERT INTO programm (`id`,`title`,`description`) VALUES (".($max->programm + 1).",'".$title."','Enter the Custom Programm Desciption')";
	mysqli_query($link,$query);
	for($i=$max->figur + 1;$i<=($max->figur + $figuren);$i++){
		mysqli_query($link,"INSERT INTO figur (`id`,`name`,`wert`) VALUES (".$i.",'Manoeuvre ".($i - $max->figur)."',0)");
		mysqli_query($link,"INSERT INTO figur_programm (`figur`,`programm`) VALUES (".$i.",".($max->programm + 1).")");
	}
}
else if(isset($_GET['programm'])){
	$programm = $_GET['programm'];
	if($res_figuren = mysqli_query($link,"SELECT figur FROM figur_programm WHERE programm = ".$programm)){
		while($obj_figuren = mysqli_fetch_object($res_figuren)){
			mysqli_query($link,"DELETE FROM figur WHERE id = ".$obj_figuren->figur);
			mysqli_query($link,"DELETE FROM figuren_images WHERE figur = ".$obj_figuren->figur);
		}
		mysqli_free_result($res_figuren);
		mysqli_query($link,"DELETE FROM figur_programm WHERE programm = ".$programm);
		mysqli_query($link,"DELETE FROM programm WHERE id = ".$programm);
	}
}
mysqli_close($link);