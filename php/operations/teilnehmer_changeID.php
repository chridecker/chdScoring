<?php
require_once("../host.inc");
$teilnehmer = $_GET['id'];
$direction = $_GET['direction'];
if($direction == "up" && $teilnehmer > 1){
	// Teilnehmer darüber auf -99 setzten
	$upper = mysqli_fetch_object(mysqli_query($link,"SELECT id FROM teilnehmer WHERE id < ".$teilnehmer." ORDER BY id DESC LIMIT 1"))->id;
	mysqli_query($link,"UPDATE teilnehmer SET id = -99 WHERE id = ".$upper);
	mysqli_query($link,"UPDATE teilnehmer_bewerb SET teilnehmer = -99 WHERE teilnehmer = ".$upper);
	mysqli_query($link,"UPDATE model_aircraft SET teilnehmer = -99 WHERE teilnehmer = ".$upper);
	// Teilnehmer nach oben verschieben
	mysqli_query($link,"UPDATE teilnehmer SET id = ".$upper." WHERE id = ".$teilnehmer);
	mysqli_query($link,"UPDATE teilnehmer_bewerb SET teilnehmer = ".$upper." WHERE teilnehmer = ".$teilnehmer);
	mysqli_query($link,"UPDATE model_aircraft SET teilnehmer = ".$upper." WHERE teilnehmer = ".$teilnehmer);
	// Teilnehmer -99 auf alte ID setzen
	mysqli_query($link,"UPDATE teilnehmer SET id = ".$teilnehmer." WHERE id = -99");
	mysqli_query($link,"UPDATE teilnehmer_bewerb SET teilnehmer = ".$teilnehmer." WHERE teilnehmer = -99");
	mysqli_query($link,"UPDATE model_aircraft SET teilnehmer = ".$teilnehmer." WHERE teilnehmer = -99");
}
if($direction == "down"){
	// Teilnehmer darunter auf -99 setzten
	$lower = mysqli_fetch_object(mysqli_query($link,"SELECT id FROM teilnehmer WHERE id > ".$teilnehmer." ORDER BY id ASC LIMIT 1"))->id;
	mysqli_query($link,"UPDATE teilnehmer SET id = -99 WHERE id = ".$lower);
	mysqli_query($link,"UPDATE teilnehmer_bewerb SET teilnehmer = -99 WHERE teilnehmer = ".$lower);
	mysqli_query($link,"UPDATE model_aircraft SET teilnehmer = -99 WHERE teilnehmer = ".$lower);
	// Teilnehmer nach oben verschieben
	mysqli_query($link,"UPDATE teilnehmer SET id = ".$lower." WHERE id = ".$teilnehmer);
	mysqli_query($link,"UPDATE teilnehmer_bewerb SET teilnehmer = ".$lower." WHERE teilnehmer = ".$teilnehmer);
	mysqli_query($link,"UPDATE model_aircraft SET teilnehmer = ".$lower." WHERE teilnehmer = ".$teilnehmer);
	// Teilnehmer -99 auf alte ID setzen
	mysqli_query($link,"UPDATE teilnehmer SET id = ".$teilnehmer." WHERE id = -99");
	mysqli_query($link,"UPDATE teilnehmer_bewerb SET teilnehmer = ".$teilnehmer." WHERE teilnehmer = -99");
	mysqli_query($link,"UPDATE model_aircraft SET teilnehmer = ".$teilnehmer." WHERE teilnehmer = -99");
}
