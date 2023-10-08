<?php
include("../db_connection.inc");
$link = mysqli_connect($host,$user,$password,$database);
$id = $_GET['figur']; 
$query = "SELECT img_data, img_type FROM figuren_images WHERE figur = ".$id;
$res = mysqli_fetch_object(mysqli_query($link,$query));

$ausgabe = "Content-Type: ".$res->img_type;
header($ausgabe);

echo $res->img_data;
?>