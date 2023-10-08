<?php
require_once("../host.inc");
if(array_key_exists('img',$_FILES)){
	$tmpfile =  $_FILES['img']['tmp_name'];
	$type = $_FILES['img']['type'];
	if($tmpfile != "" && $type != ""){
		$hndFile = fopen($tmpfile, "r");
		$data = addslashes(fread($hndFile, filesize($tmpfile)));
		$query = "INSERT INTO figuren_images (figur,img_data,img_type) VALUES (".$_POST['figur'].",'".$data."','".$type."')";
		if(mysqli_query($link,$query))echo "Uploaded Successfully";
	}
}
?>
<link rel="stylesheet" type="text/css" href="../css/edit_images.css" />
<table style="width:800px;;">
<tr class="headline">
<th colspan="2">Upload Manoeuvre Images</th></tr>
<form method="post" action="<?php echo $_SERVER['PHP_SELF'];?>" enctype="multipart/form-data">
<tr class="images">
<td>Bild</td><td><input type="file" name="img" /></td></tr>
<tr class="images">
<td>Figur</td><td>
<select name="figur" style="width:300px;">
<?php
$query = "SELECT p.title, p.id FROM programm p ORDER BY p.title ASC";
$res_programm = mysqli_query($link,$query);
while($programm = mysqli_fetch_object($res_programm)){
	echo "<optgroup label='".$programm->title."'>";
	$query_figur = "SELECT f.id, f.name FROM figur f JOIN figur_programm fp ON (f.id = fp.figur) WHERE fp.programm = ".$programm->id." AND f.id NOT IN (SELECT figur FROM figuren_images );";
	$res = mysqli_query($link,$query_figur);
	while($figur = mysqli_fetch_object($res)){
		echo "<option value='".$figur->id."'>".$figur->name."</option>";
	}
	echo "</optgroup>";
}
?>
</select>
</td></tr>
<tr class="images">
<th><br /><input type="button" value="Close" onclick="window.close();" /></th><th><input type="submit" value="Save" /></th></tr>
</form>
</table>