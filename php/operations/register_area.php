<?php
require_once("../host.inc");
if(array_key_exists('img',$_FILES)){
	$name = $_POST['name'];
	$description = $_POST['description'];
	$tmpfile =  $_FILES['img']['tmp_name'];
	$type = $_FILES['img']['type'];
	$hndFile = fopen($tmpfile, "r");
	$data = addslashes(fread($hndFile, filesize($tmpfile)));
	$obj = mysqli_fetch_object(mysqli_query($link,"SELECT max(id) as id FROM area"));
	$query = "INSERT INTO area (`id`,`name`,`description`,`img_type`,`img_data`) VALUES (".($obj->id + 1).",'".$name."','".$description."','".$type."','".$data."')";
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
<th colspan="2">Enter Access Area</th></tr>
<form method="post" action="<?php echo $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
<tr><td>Name</td><td><input type="text" name="name" /></td></tr>
<tr><td>Description</td><td><input type="text" name="description" /></td></tr>
<tr class="images">
<td>Bild</td><td><input type="file" name="img" /></td></tr>
<tr class="images">
<th><br /><input type="button" value="Close" onclick="window.close();" /></th><th><input type="submit" value="Save" /></th></tr>
</form>
</table>