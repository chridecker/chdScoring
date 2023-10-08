<?php
include("../db_connection.inc");
$link = mysqli_connect($host,$user,$password,"person");
$id = $_GET['id']; 
if(isset($_GET['type']))$type = $_GET['type'];
else $type = "pilot";
$query = "SELECT img_data, img_type FROM ".$type." WHERE id = ".$id;
$res = mysqli_fetch_object(mysqli_query($link,$query));

$ausgabe = "Content-Type: ".$res->img_type;
header($ausgabe);

echo $res->img_data;
?>