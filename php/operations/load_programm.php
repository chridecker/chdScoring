<?php 
require_once("../host.inc");


$file = "../programme.xml";
$xml = simplexml_load_file($file);

foreach($xml->programm as $programm){
	$query = "SELECT min(f.id) as anfang, max(f.id) as ende, count(f.id) as anzahl, p.title as programm FROM figur f JOIN figur_programm fp ON (fp.figur = f.id) JOIN programm p ON (p.id = fp.programm) WHERE p.title = '".$programm['name']."';";
	$res = mysqli_query($link,$query);
	$update = true;
	while($obj= mysqli_fetch_object($res)){
		if($programm['name'] == $obj->programm){
			$update = false;
			foreach($programm->figuren->figur as $figur){
				$query = "UPDATE figur SET name = '".$figur."', wert = ".$figur['k']." WHERE id = ".($figur['id'] + $obj->anfang - 1).";";
				//mysqli_query($link,$query);
			}
		}
	}
	if($update){
		$new_id = mysqli_fetch_object(mysqli_query($link,"SELECT max(id) + 1 as neu FROM programm"))->neu;
		$new_figur_id = mysqli_fetch_object(mysqli_query($link,"SELECT max(id) + 1 as neu FROM figur"))->neu;
		mysqli_query($link,"INSERT INTO programm (`id`,`title`,`description`) VALUES (".$new_id.",'".$programm['name']."','".$programm->beschreibung."')");
		foreach($programm->figuren->figur as $figur){
			mysqli_query($link,"INSERT INTO figur (`id`,`name`,`wert`) VALUES (".$new_figur_id.",'".$figur."','".$figur['k']."')");
			mysqli_query($link,"INSERT INTO figur_programm (`figur`,`programm`) VALUES (".$new_figur_id.",".$new_id.")");
			$new_figur_id++;
		}
	}
}
