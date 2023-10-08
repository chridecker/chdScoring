<?php 
require_once("../host.inc");
if(isset($_FILES['programme'])){
	$file =  $_FILES['programme']['tmp_name'];
	$xml = simplexml_load_file($file);
	$image_upload = false;
	if(isset($_POST['aresti']))$image_upload = true;
	foreach($xml->programm as $programm){
		$query = "SELECT min(f.id) as anfang, max(f.id) as ende, count(f.id) as anzahl, p.title as programm FROM figur f JOIN figur_programm fp ON (fp.figur = f.id) JOIN programm p ON (p.id = fp.programm) WHERE p.title = '".$programm['name']."';";
		$res = mysqli_query($link,$query);
		$update = true;
		while($obj= mysqli_fetch_object($res)){
			if($programm['name'] == $obj->programm){
				$update = false;
				foreach($programm->figuren->figur as $figur){
					$query = "UPDATE figur SET name = '".$figur."', wert = ".$figur['k']." WHERE id = ".($figur['id'] + $obj->anfang - 1).";";
					mysqli_query($link,$query);
				}
			}
		}
		if($update){
			$new_id = mysqli_fetch_object(mysqli_query($link,"SELECT max(id) + 1 as neu FROM programm"))->neu;
			$new_figur_id = mysqli_fetch_object(mysqli_query($link,"SELECT max(id) + 1 as neu FROM figur"))->neu;
			mysqli_query($link,"INSERT INTO programm (`id`,`title`,`description`) VALUES (".$new_id.",'".$programm['name']."','".$programm->beschreibung."')");
			foreach($programm->figuren->figur as $figur){
				mysqli_query($link,"INSERT INTO figur (`id`,`name`,`wert`) VALUES (".$new_figur_id.",'".$figur."','".$figur['k']."')");
				mysqli_query($link,"INSERT INTO figur_programm (`figur`,`programm`) VALUES (".$new_figur_id.",".$new_id.")");
				if($image_upload){
					$directory = "../update/aresti";
					$file = $directory."/".$figur['arestifilename'];
					$hndFile = fopen($file, "r");
					$image_type = image_type_to_mime_type(exif_imagetype($file));
					$data = addslashes(fread($hndFile, filesize($file)));
					mysqli_query($link,"INSERT INTO figuren_images (figur,img_data,img_type) VALUES (".$new_figur_id.",'".$data."','".$image_type."')");
				}
				$new_figur_id++;
			}
		}
	}
}?>
<link rel="stylesheet" type="text/css" href="../css/edit_images.css" />
<table style="width:800px;;">
<tr class="headline">
<th colspan="2">Update Schedules</th></tr>
<form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post" enctype="multipart/form-data">
<tr class="images">
<td>XML Update File <input type="file" name="programme" /></td>
<td>Upload Aresti (Extract Files to [update/aresti/] directory)<input type="checkbox" name="aresti" /></td></tr>
<tr class="images">
<th><input type="submit" value="Update" /></th><th><input type="button" value="Close" onclick="window.close();" /></th></tr>
</form>
</table>