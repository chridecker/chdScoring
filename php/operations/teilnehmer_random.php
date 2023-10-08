<?php 
require_once("../host.inc");

$query = "SELECT id FROM teilnehmer WHERE id > 0";
$res = mysqli_query($link,$query);
$id = array();
while($obj = mysqli_fetch_object($res)){
	$id[] = $obj->id + $teilnehmer_anzahl;
}
mysqli_query($link,"UPDATE teilnehmer SET id = id + ".$teilnehmer_anzahl);
mysqli_query($link,"UPDATE teilnehmer_bewerb SET teilnehmer = teilnehmer + ".$teilnehmer_anzahl);
mysqli_query($link,"UPDATE model_aircraft SET teilnehmer = teilnehmer + ".$teilnehmer_anzahl);
shuffle($id);
for($i=0;$i<count($id);$i++){
	//echo $id[$i] - $teilnehmer_anzahl."->".($i+1)."<br>";
	$query = "UPDATE teilnehmer SET id = ".($i+1)." WHERE id = ".$id[$i];
	mysqli_query($link,$query);
	$query = "UPDATE teilnehmer_bewerb SET teilnehmer = ".($i+1)." WHERE teilnehmer = ".$id[$i];
	mysqli_query($link,$query);
	$query = "UPDATE model_aircraft SET teilnehmer = ".($i+1)." WHERE teilnehmer = ".$id[$i];
	mysqli_query($link,$query);
}


