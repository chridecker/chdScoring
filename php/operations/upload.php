<?php
require_once("../host.inc");
if(array_key_exists('img',$_FILES)){
	$title = $_POST['title'];
	if(isset($_POST['profile']))$profil = 1;
	else $profil = 0;
	if(isset($_POST['sponsor']))$sponsor = 1;
	else $sponsor = 0;
	if(isset($_POST['official']))$official = 1;
	else $official = 0;
	$tmpfile =  $_FILES['img']['tmp_name'];
	$type = $_FILES['img']['type'];
	$hndFile = fopen($tmpfile, "r");
	$data = addslashes(fread($hndFile, filesize($tmpfile)));
	$obj = mysqli_fetch_object(mysqli_query($link,"SELECT max(img_id) as id FROM images"));
	$query = "INSERT INTO images (img_id,img_title,img_data,img_type,img_profil,img_sponsor,img_official) VALUES (".($obj->id + 1).",'".$title."','".$data."','".$type."',".$profil.",".$sponsor.",".$official.")";
	mysqli_query($link,$query);
}
?>
<script type="text/javascript">
window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}


</script>
<link rel="stylesheet" type="text/css" href="../css/edit_images.css" />
<table style="width:500px;;">
<tr class="headline">
<th colspan="2">Upload Images</th></tr>
<form method="post" action="<?php echo $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
<tr><td>Title</td><td><input type="text" name="title" /></td></tr>
<tr><td>Pilot</td><td><input type="checkbox" name="profile" /></td></tr>
<tr><td>Official</td><td><input type="checkbox" name="official" /></td></tr>
<tr><td>Sponsor</td><td><input type="checkbox" name="sponsor" /></td></tr>
<tr class="images">
<td>Bild</td><td><input type="file" name="img" /></td></tr>
<tr class="images">
<th><br /><input type="button" value="Close" onclick="window.close();" /></th><th><input type="submit" value="Save" /></th></tr>
</form>
</table>