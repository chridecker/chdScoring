<?php 
require_once("../host.inc");
// Copy Teilnehmer from actual cs to person.pilot
$link2 = mysqli_connect($host,$user,$password);
//$startid = mysqli_fetch_object(mysqli_query($link2,"SELECT max(id) as id FROM person.pilot"))->id;
//mysqli_query($link2,"INSERT INTO person.pilot SELECT t.id + ".$startid." as id, t.vorname, t.nachname, t.land, t.club, t.license, t.email, t.telefon, t.strasse, t.ort, t.plz, t.info, i.img_type, i.img_data FROM ".$database.".teilnehmer as t JOIN ".$database.".images as i WHERE i.img_id = t.bild AND t.id = 5");
// Copy Judges from catual cs to person.judge
$startid = mysqli_fetch_object(mysqli_query($link2,"SELECT max(id) as id FROM person.judge"))->id;
mysqli_query($link2,"INSERT INTO person.judge SELECT t.id + ".$startid." as id, t.name, t.vorname, t.club, t.land, t.license, i.img_type, i.img_data FROM ".$database.".judge as t JOIN ".$database.".images as i WHERE i.img_id = t.bild");