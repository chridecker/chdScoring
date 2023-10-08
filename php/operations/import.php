<?php
require_once("../host.inc");
$link = mysqli_connect($host,$user,$password);
$person = $_POST['person'];
$type = $_POST['type'];
$x = 0;
$iterate = true;
while($iterate){
	if(strpos($person,",") > 0){
		$id =  substr($person,0,strpos($person,","));
		$x = strpos($person,",")+ 1 ;
		$person = substr($person,$x,strlen($person));
	}
	else {
		$id =  $person;
		$iterate = false;
	}
	if($res_bild = mysqli_query($link,"SELECT img_type, img_data FROM person.".$type." WHERE id = ".$id)){
		$bild = mysqli_fetch_object($res_bild);
		$res_teilnehmer = mysqli_query($link,"SELECT * FROM person.".$type." WHERE id = ".$id);
		$teilnehmer = mysqli_fetch_object($res_teilnehmer);
		$newid = mysqli_fetch_object(mysqli_query($link,"SELECT max(img_id) + 1 as maxid FROM cs.images"))->maxid;
		if($type == 'pilot'){
			$query = "INSERT INTO cs.images SELECT ".$newid." as img_id, CONCAT(vorname,nachname) as img_title, img_data, img_type, 1 as img_profil, 0 as img_official, 0 as img_sponsor FROM person.".$type." WHERE id = ".$id;
			mysqli_query($link,$query);
			$newidt = mysqli_fetch_object(mysqli_query($link,"SELECT max(id) + 1 as maxid FROM cs.teilnehmer"))->maxid;
			if($newidt == null) $newidt = 1;
			mysqli_query($link,"INSERT INTO cs.teilnehmer SELECT ".$newidt." as id, vorname, nachname, land, club, license, email, telefon, strasse, ort, plz, ".$newid." as bild, info FROM person.".$type." WHERE id = ".$id);
			mysqli_query($link,"INSERT INTO cs.teilnehmer_bewerb (`teilnehmer`,`bewerb`) VALUES (".$newidt.",1)");
			mysqli_query($link,"INSERT INTO cs.area_access (`area`,`pilot`) VALUES (1,".$newidt.")");
		}
		elseif($type == 'judge'){
			$query = "INSERT INTO cs.images SELECT ".$newid." as img_id, CONCAT(vorname,name) as img_title, img_data, img_type, 0 as img_profil, 1 as img_official, 0 as img_sponsor FROM person.".$type." WHERE id = ".$id;
			mysqli_query($link,$query);
			$newidt = mysqli_fetch_object(mysqli_query($link,"SELECT max(id) + 1 as maxid FROM cs.judge"))->maxid;
			if($newidt == null) $newidt = 1;
			mysqli_query($link,"INSERT INTO cs.judge SELECT ".$newidt." as id, name, vorname, ".$newid." as bild, club, land, license, 0 as pin FROM person.".$type." WHERE id = ".$id);
			mysqli_query($link,"INSERT INTO cs.area_access (`judge`,`area`) VALUES (".$newidt.",1)");
		}
	}
}
?>
