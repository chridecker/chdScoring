<?php
require_once("../host.inc");
$img = $_POST['data'];
$img = str_replace('data:image/png;base64,', '', $img);
$img = str_replace(' ', '+', $img);
$data = base64_decode($img);
$data = addslashes($data);
$title = $_POST['title'];
$type = "image/png";
$obj = mysqli_fetch_object(mysqli_query($link,"SELECT max(img_id) as id FROM images"));
$query = "INSERT INTO images (`img_id`,`img_title`,`img_data`,`img_type`,`img_profil`,`img_official`) VALUES (".($obj->id + 1).",'".$title."','".$data."','".$type."',".$_POST['pilot'].",".$_POST['official'].")";
mysqli_query($link,$query);
if($_POST['id'] != 0 && $_POST['pilot'] == 1)mysqli_query($link,"UPDATE teilnehmer SET bild = ".($obj->id + 1)." WHERE id = ".$_POST['id']);
elseif($_POST['id'] != 0 && $_POST['official'] == 1)mysqli_query($link,"UPDATE official SET bild = ".($obj->id + 1)." WHERE id = ".$_POST['id']);

