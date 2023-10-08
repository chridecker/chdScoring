<?php
include("../db_connection.inc");
//$database = "cs_westpokal";
if(isset($_GET['db'])){$database=$_GET['db'];}
$link = mysqli_connect($host,$user,$password,$database);
$id = $_GET['id']; 
$query = "SELECT img_data, img_type FROM images WHERE img_id = ".$id;
$res = mysqli_fetch_object(mysqli_query($link,$query));

$ausgabe = "Content-Type: ".$res->img_type;
header($ausgabe);

echo $res->img_data;
?>