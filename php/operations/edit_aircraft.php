<?php
require_once("../host.inc");
// Change Model Aircraft
if(isset($_GET['name'],$_GET['wingspan'],$_GET['length']) && $_GET['change'] == 1)mysqli_query($link,"UPDATE model_aircraft SET name = '".$_GET['name']."', wingspan = ".$_GET['wingspan'].", length = ".$_GET['length'].", weight = ".$_GET['weight'].", prop_circuit = ".$_GET['prop'].", ident_mark = ".$_GET['ident']." WHERE id = ".$_GET['id']);

//Add Modell Aircraft
if(isset($_GET['teilnehmer'],$_GET['new']) && $_GET['new'] == 1){
	$new_id = mysqli_fetch_object(mysqli_query($link,"SELECT max(id) + 1 as new_id FROM model_aircraft"))->new_id;
	$query_new = "INSERT INTO model_aircraft (`id`,`teilnehmer`,`name`,`wingspan`,`length`,`weight`,`prop_circuit`,`ident_mark`) VALUES (".$new_id.",".$_GET['teilnehmer'].",'New Model',0,0,0,0,0)";
	mysqli_query($link,$query_new);
}

//Delete Modell Aircraft
if(isset($_GET['id'],$_GET['delete']) && $_GET['delete'] == 1){
	mysqli_query($link,"DELETE FROM model_aircraft WHERE id = ".$_GET['id']);
}