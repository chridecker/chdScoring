<?php
require_once("../host.inc");
if(isset($_GET['change']) && $_GET['change'] == 1){
	$query = "UPDATE judge SET vorname = '".$_GET['vorname']."', name = '".$_GET['nachname']."', club = '".$_GET['club']."', land = ".$_GET['land'].", license = '".$_GET['license']."', bild = ".$_GET['bild']." WHERE id = ".$_GET['id'];
	mysqli_query($link,$query);
}
if(isset($_GET['id']) && $_GET['id'] != "0"){
$query = "SELECT * FROM judge WHERE id = ".$_GET['id'];
$teilnehmer = mysqli_fetch_object(mysqli_query($link,$query));
}
?>
<style>
body{
	margin:0px;
}
</style>
<table>
<tr class="header">
<th colspan="4">Judge Details</th></tr>
<tr>
<th>#</th><td><input type="text" id="id" value="<?php echo $teilnehmer->id;?>" readonly="readonly"></td>
<td rowspan="5" colspan="2" class="profilbild">
<img src="operations/load_image.php?id=<?php echo $teilnehmer->bild;?>" height="200">
<?php include("load_image_select_official.php");?>
<img src="operations/load_image_country.php?id=<?php echo $teilnehmer->land;?>" height="200" style="margin-left:100px;">
</td></tr>
<tr>
<th>Name</th><td><input type="text" id="nn" value="<?php echo $teilnehmer->name;?>" onchange="speichern();"></td></tr>
<tr>
<th>Surname</th><td><input type="text" id="vn" value="<?php echo $teilnehmer->vorname;?>" onchange="speichern();"></td></tr>
<tr>
<th>License</th><td><input type="text" id="li" value="<?php echo $teilnehmer->license;?>" onchange="speichern();"></td>
</tr>
<tr><td colspan="2"><br /></td>
</tr>
<tr>
<th>Club</th><td><input type="text" id="c" value="<?php echo $teilnehmer->club;?>" onchange="speichern();"></td>
<td colspan="2" class="profilbild">
<button onclick="oeffnen('../image_admin.php');" title="Go to Images"><img src="../bilder/buttons/photos.png" /></button>
<button id="printer" onclick="oeffnen('../print/?file=cards&type=judge&rear=true&id=<?php echo $teilnehmer->id;?>');" title="Print Accredidation"><img src="../bilder/buttons/print.png" /></button>
</td></tr>
<tr>
<th>Nation</th><td>
<select id="l" onChange="speichern(<?php echo $teilnehmer->id;?>);">
<?php
$res_land = mysqli_query($link,"SELECT img_id, name FROM country_images ORDER BY name ASC");
while($obj_land = mysqli_fetch_object($res_land)){
    echo "<option value='".$obj_land->img_id."'";
    if($obj_land->img_id == $teilnehmer->land) echo " selected='selcted'";
    echo ">".$obj_land->name."</option>";
}?>
</select></td>
<td>
<table class="area">
<tr><th style="text-align:center;" colspan="<?php echo mysqli_fetch_object(mysqli_query($link,"SELECT count(id) as cid FROM area"))->cid;?>">Area Access</th></tr>
<tr>
<?php
$res_area = mysqli_query($link,"SELECT * FROM area ORDER BY name ASC");
while($area = mysqli_fetch_object($res_area)){
	$res = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM area_access WHERE area = ".$area->id." AND judge = ".$teilnehmer->id));
	?>
	<td><img onclick="changeAreaAccess(<?php echo $area->id.",".$teilnehmer->id.",'judge'";?>);" src="../operations/load_image_area.php?id=<?php echo $area->id;?>" <?php if($res)echo " style='background-color:lightblue;opacity:1;'";?> /><br /><?php echo strtoupper($area->name);?></td>
    <?php
}?>
</tr>
</table>

</td></tr>
<tr>
<td><button title="Close" onclick="window.close();"><img src="../bilder/buttons/exit.png" /></button>
<button title="Close" onclick="load(<?php echo $teilnehmer->id;?>);"><img src="../bilder/buttons/refresh.png" /></button></td>
<td colspan="6"></td></tr>
</table>