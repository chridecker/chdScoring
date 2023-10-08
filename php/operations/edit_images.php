<?php
require_once("../host.inc");
if(isset($_GET['img_id']) && isset($_GET['logo'])){
	echo $query = "UPDATE stammdaten SET img_id = ".$_GET['img_id']." WHERE id = ".$_GET['logo'];
	mysqli_query($link,$query);
}
if(isset($_GET['img_id']) && isset($_GET['gruppe'])){
	$query = "UPDATE stammdaten SET gruppe_id = ".$_GET['img_id']." WHERE id = ".$_GET['gruppe'];
	mysqli_query($link,$query);
}
if(isset($_GET['img_id']) && isset($_GET['urkunde'])){
	$query = "UPDATE stammdaten SET urkunde_id = ".$_GET['img_id']." WHERE id = ".$_GET['urkunde'];
	mysqli_query($link,$query);
}
if(isset($_GET['img_id']) && isset($_GET['federation'])){
	$query = "UPDATE stammdaten SET fed_id = ".$_GET['img_id']." WHERE id = ".$_GET['federation'];
	mysqli_query($link,$query);
}
if(isset($_GET['id']) && isset($_GET['title']) && isset($_GET['profile']) && isset($_GET['sponsor']) && $_GET['id'] != "del"){
	$query = "UPDATE images SET img_title = '".$_GET['title']."', img_profil = ".$_GET['profile'].", img_official = ".$_GET['official'].", img_sponsor = ".$_GET['sponsor']." WHERE img_id = ".$_GET['id'];
	mysqli_query($link,$query);
}
else if(isset($_GET['del']) && isset($_GET['id']) && $_GET['id'] == "del"){
	$query = "UPDATE teilnehmer SET bild = 1 WHERE bild =".$_GET['del'];
	mysqli_query($link,$query);
	$query = "UPDATE judge SET bild = 1 WHERE bild =".$_GET['del'];
	mysqli_query($link,$query);
	$query = "UPDATE official SET bild = 1 WHERE bild =".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM images WHERE img_id = ".$_GET['del'];
	mysqli_query($link,$query);
}
$query = "SELECT img_id, img_title, img_profil, img_sponsor, img_official FROM images WHERE img_profil = 0 AND img_official = 0 AND img_sponsor = 0 ORDER BY img_id";
$res = mysqli_query($link,$query);
$max = 7;
?>
<table>
<tr class="headline">
<th>
<button title="Close" onClick="window.close();"><img src="../bilder/buttons/exit.png" /></button>
<button title="Refresh" onClick="load();"><img src="../bilder/buttons/refresh.png" /></button></th>
<th colspan="<?php echo ($max - 2);?>">Images Administator</th>
<th>
<button title="Capture Photo" onclick="oeffnen('https://<?php echo $_SERVER['HTTP_HOST'];?>/operations/capture_photo.php');"><img src="../bilder/buttons/photo.png" /></button>
<button title="Upload Image" onclick="oeffnen('operations/upload.php');"><img src="../bilder/buttons/upload.png" /></button></th></tr>

<tr class="header"><th colspan="100">Logos</th></tr>
<?php
$count = 1;
while($obj = mysqli_fetch_object($res)){
    if(($count % ($max + 1)) == 0){ $count = 1;?></tr><?php }
    if($count == 1){?><tr class='images'><?php }?>
	<td>
	<img src="operations/load_image.php?id=<?php echo $obj->img_id;?>&db=<?php echo $database;?>" height="60" title="<?php echo $obj->img_id;?>" /><br />
	<input style="width:100px;" type="text" value="<?php echo $obj->img_title;?>" id="<?php echo "title".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />
    <button style="background-color:lightblue;"title="Delete" onclick="del(<?php echo $obj->img_id;?>);"><img style="height:10px;" src="../bilder/buttons/delete.png" /></button>
    <br />
    <input type="checkbox" <?php if($obj->img_profil)echo "checked='checked'";?> id="<?php echo "profile".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />Pilot|
    <input type="checkbox" <?php if($obj->img_official)echo "checked='checked'";?> id="<?php echo "official".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />Official|
    <input type="checkbox" <?php if($obj->img_sponsor)echo "checked='checked'";?> id="<?php echo "sponsor".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />Sponsor|
    <input type="checkbox" <?php if($result_config->img_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "logo".$obj->img_id;?>" onChange="changeLogo(<?php echo $obj->img_id; ?>);" />Logo
    <br />
    <input type="checkbox" <?php if($result_config->gruppe_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "Group".$obj->img_id;?>" onChange="changeGruppe(<?php echo $obj->img_id; ?>);" />Group|
    <input type="checkbox" <?php if($result_config->urkunde_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "Cert".$obj->img_id;?>" onChange="changeUrkunde(<?php echo $obj->img_id; ?>);" />Cert|
    <input type="checkbox" <?php if($result_config->fed_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "Fed".$obj->img_id;?>" onChange="changeFed(<?php echo $obj->img_id; ?>);" />Fed
	
	</td>
    <?php
	$count++;
}
if($count != 1) echo "</tr>";
?>
</tr>
</table>
<?php
$query = "SELECT img_id, img_title, img_profil, img_official, img_sponsor FROM images WHERE img_profil = 1 OR img_official = 1 ORDER BY img_id";
$res = mysqli_query($link,$query);?>
<table>
<tr class="header">
<th colspan="100">Portraits</th>
</tr>
<?php
$count = 1;
while($obj = mysqli_fetch_object($res)){
    if(($count % ($max + 1)) == 0){ $count = 1;?></tr><?php }
    if($count == 1){?><tr class='images'><?php }?>
	<td>
	<img src="operations/load_image.php?id=<?php echo $obj->img_id;?>" height="60" title="<?php echo $obj->img_id;?>" /><br />
	<input style="width:100px;" type="text" value="<?php echo $obj->img_title;?>" id="<?php echo "title".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />
    <button style="background-color:lightblue;"title="Delete" onclick="del(<?php echo $obj->img_id;?>);"><img style="height:10px;" src="../bilder/buttons/delete.png" /></button>
    <br />
    <input type="checkbox" <?php if($obj->img_profil)echo "checked='checked'";?> id="<?php echo "profile".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />Pilot|
    <input type="checkbox" <?php if($obj->img_official)echo "checked='checked'";?> id="<?php echo "official".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />Official|
    <input type="checkbox" <?php if($obj->img_sponsor)echo "checked='checked'";?> id="<?php echo "sponsor".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />Sponsor|
    <input type="checkbox" <?php if($result_config->img_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "logo".$obj->img_id;?>" onChange="changeLogo(<?php echo $obj->img_id; ?>);" />Logo
    <br />
    <input type="checkbox" <?php if($result_config->gruppe_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "Group".$obj->img_id;?>" onChange="changeGruppe(<?php echo $obj->img_id; ?>);" />Group|
    <input type="checkbox" <?php if($result_config->urkunde_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "Cert".$obj->img_id;?>" onChange="changeUrkunde(<?php echo $obj->img_id; ?>);" />Cert|
    <input type="checkbox" <?php if($result_config->fed_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "Fed".$obj->img_id;?>" onChange="changeFed(<?php echo $obj->img_id; ?>);" />Fed
	
	</td>
    <?php
	$count++;
}
if($count != 1) echo "</tr>";
?>
</tr>
</table>
<?php
$query = "SELECT img_id, img_title, img_profil, img_official, img_sponsor FROM images WHERE img_sponsor = 1 ORDER BY img_id";
$res = mysqli_query($link,$query);?>
<table>
<tr class="header">
<th colspan="100">Sponsors</th>
</tr>
<?php
$count = 1;
while($obj = mysqli_fetch_object($res)){
    if(($count % ($max + 1)) == 0){ $count = 1;?></tr><?php }
    if($count == 1){?><tr class='images'><?php }?>
	<td>
	<img src="operations/load_image.php?id=<?php echo $obj->img_id;?>" height="60" title="<?php echo $obj->img_id;?>" /><br />
	<input style="width:100px;" type="text" value="<?php echo $obj->img_title;?>" id="<?php echo "title".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />
    <button style="background-color:lightblue;"title="Delete" onclick="del(<?php echo $obj->img_id;?>);"><img style="height:10px;" src="../bilder/buttons/delete.png" /></button>
    <br />
    <input type="checkbox" <?php if($obj->img_profil)echo "checked='checked'";?> id="<?php echo "profile".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />Pilot|
    <input type="checkbox" <?php if($obj->img_official)echo "checked='checked'";?> id="<?php echo "official".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />Official|
    <input type="checkbox" <?php if($obj->img_sponsor)echo "checked='checked'";?> id="<?php echo "sponsor".$obj->img_id;?>" onChange="speichern(<?php echo $obj->img_id; ?>);" />Sponsor|
    <input type="checkbox" <?php if($result_config->img_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "logo".$obj->img_id;?>" onChange="changeLogo(<?php echo $obj->img_id; ?>);" />Logo
    <br />
    <input type="checkbox" <?php if($result_config->gruppe_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "Group".$obj->img_id;?>" onChange="changeGruppe(<?php echo $obj->img_id; ?>);" />Group|
    <input type="checkbox" <?php if($result_config->urkunde_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "Cert".$obj->img_id;?>" onChange="changeUrkunde(<?php echo $obj->img_id; ?>);" />Cert|
    <input type="checkbox" <?php if($result_config->fed_id == $obj->img_id)echo "checked='checked'";?> id="<?php echo "Fed".$obj->img_id;?>" onChange="changeFed(<?php echo $obj->img_id; ?>);" />Fed
	
	</td>
    <?php
	$count++;
}
if($count != 1) echo "</tr>";
?>
</tr>
</table>