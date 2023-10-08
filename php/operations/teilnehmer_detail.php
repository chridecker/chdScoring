<?php
require_once("../host.inc");
if(isset($_GET['change']) && $_GET['change'] == 1){
	$query = "UPDATE teilnehmer SET vorname = '".$_GET['vorname']."', nachname = '".$_GET['nachname']."', club = '".$_GET['club']."', land = ".$_GET['land'].", license = '".$_GET['license']."', email = '".$_GET['email']."', telefon = '".$_GET['telefon']."', strasse = '".$_GET['strasse']."', plz = '".$_GET['plz']."', ort = '".$_GET['ort']."', bild = ".$_GET['bild'].", info='".$_GET['info']."' WHERE id = ".$_GET['id'];
	mysqli_query($link,$query);
}
if(isset($_GET['id']) && $_GET['id'] != "0"){
$query = "SELECT * FROM teilnehmer WHERE id = ".$_GET['id'];
$teilnehmer = mysqli_fetch_object(mysqli_query($link,$query));
}
//else if($_GET['id'] == "0") echo "OK";
?>
<style>
body{
	margin:0px;
}
</style>
<table>
<tr class="header">
<th colspan="4">Participant Details</th></tr>
<tr>
<th>#</th><td><input type="text" id="id" value="<?php echo $teilnehmer->id;?>" readonly="readonly"></td>
<td rowspan="5" colspan="2" class="profilbild">
<img src="operations/load_image.php?id=<?php echo $teilnehmer->bild;?>" height="200">
<?php include("load_image_select.php");?>
<img src="operations/load_image_country.php?id=<?php echo $teilnehmer->land;?>" height="200" style="margin-left:100px;">
</td></tr>
<tr>
<th>Name</th><td><input type="text" id="nn" value="<?php echo $teilnehmer->nachname;?>" onchange="speichern();"></td></tr>
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
<button onclick="oeffnen('https://<?php echo $_SERVER['HTTP_HOST'];?>/operations/capture_photo.php<?php echo "?name=".$teilnehmer->nachname.$teilnehmer->vorname."&id=".$teilnehmer->id."&pilot=1";?>');" title="Go to Images"><img src="../bilder/buttons/photo.png" /></button>
<button id="printer" onclick="oeffnen('../print/?file=cards&type=teilnehmer&id=<?php echo $teilnehmer->id;?>');" title="Print Accredidation"><img src="../bilder/buttons/print.png" /></button></td></tr>
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
</select>
</td><th>Phone</th><td><input type="text" id="telefon" value="<?php echo $teilnehmer->telefon;?>" onchange="speichern();"></td></tr>
<tr>
<th>Street</th><td><input type="text" id="strasse" value="<?php echo $teilnehmer->strasse;?>" onchange="speichern();"></td><th>Email</th><td><input type="text" id="email" value="<?php echo $teilnehmer->email;?>" onchange="speichern();"></td></tr>
<tr>
<th>PLZ</th><td><input type="text" id="plz" value="<?php echo $teilnehmer->plz;?>" onchange="speichern();"></td><th>Location</th><td><input type="text" id="ort" value="<?php echo $teilnehmer->ort;?>" onchange="speichern();"></td></tr>
<tr><th colspan="2">Detail Information</th><th colspan="2"></th></tr>
<tr><td colspan="2"><textarea id="info" onchange="speichern();" style="width:99%; height:150px;"><?php echo $teilnehmer->info;?></textarea></td>
<td colspan="2">
<?php 
$res_class = mysqli_query($link,"SELECT * FROM klassen WHERE id = ".$result_config->klasse);
$class = mysqli_fetch_object($res_class);?>
<table class="aircraft">
<tr><th></th><th>Model Aircraft<br /><i>[<?php echo $class->max_aircraft;?>]</i></th><th>Wingspan<br /><i>[<?php echo $class->max_breite;?>]</i></th><th>Overall Length<br /><i>[<?php echo $class->max_laenge;?>]</i></th><th>Weight<br /><i>[<?php echo $class->max_gewicht;?>]</i></th><th>Propulsion circuit<br />&nbsp;</th><th>Ident<br />&nbsp;</th><th></th></tr>
<?php
$query_ma = "SELECT * FROM model_aircraft WHERE teilnehmer = ".$teilnehmer->id;
$res_ma = mysqli_query($link,$query_ma);
$count = 0;
while($aircraft = mysqli_fetch_object($res_ma)){
	$count++;
	echo "<tr>";
	echo "<td><img src='../bilder/buttons/airfield.png' style='transform:rotate(270deg);border:1px solid black;height:25px;";
	if($aircraft->weight <= $class->max_gewicht && $aircraft->length <= $class->max_laenge && $aircraft->wingspan <= $class->max_breite && $aircraft->ident_mark == 1 && $count <= $class->max_aircraft)echo " background-color:green;'>";
	else echo " background-color:red;'>";
	echo "</td>";
	echo "<td><input onchange='modelAircraft(".$aircraft->id.");' type='text' id='ma_name_".$aircraft->id."' value='".$aircraft->name."'></td>";
	echo "<td><input onchange='modelAircraft(".$aircraft->id.");' type='text' id='ma_wingspan_".$aircraft->id."' value='".$aircraft->wingspan."'></td>";
	echo "<td><input onchange='modelAircraft(".$aircraft->id.");' type='text' id='ma_length_".$aircraft->id."' value='".$aircraft->length."'></td>";
	echo "<td><input onchange='modelAircraft(".$aircraft->id.");' type='text' id='ma_weight_".$aircraft->id."' value='".$aircraft->weight."'></td>";
	echo "<td><input onchange='modelAircraft(".$aircraft->id.");' type='text' id='ma_prop_".$aircraft->id."' value='".$aircraft->prop_circuit."'></td>";
	echo "<td><input onchange='modelAircraft(".$aircraft->id.");' type='checkbox' id='ma_ident_".$aircraft->id."'";
	if($aircraft->ident_mark == 1) echo " checked='checked'";
	echo "'></td>";
	echo "<td><button title='Delete' onclick='deleteModelAircraft(".$aircraft->id.");'><img src='../bilder/buttons/aircraft_delete.png' height='20'></button></td>";
	echo "</tr>";
}?>
<tr><td><button onclick="addModelAircraft();" type="button" title="Add Model Aircraft"><img src='../bilder/buttons/aircraft_add.png' height='20'></button></td><td colspan="7"></td></tr>
</table>
<br />
<table class="area">
<tr><th style="text-align:center;" colspan="<?php echo mysqli_fetch_object(mysqli_query($link,"SELECT count(id) as cid FROM area"))->cid;?>">Area Access</th></tr>
<tr>
<?php
$res_area = mysqli_query($link,"SELECT * FROM area ORDER BY name ASC");
while($area = mysqli_fetch_object($res_area)){
	$res = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM area_access WHERE area = ".$area->id." AND pilot = ".$teilnehmer->id));
	?>
	<td><img onclick="changeAreaAccess(<?php echo $area->id.",".$teilnehmer->id.",'pilot'";?>);" src="../operations/load_image_area.php?id=<?php echo $area->id;?>" <?php if($res)echo " style='background-color:lightblue;opacity:1;'";?> /><br /><?php echo strtoupper($area->name);?></td>
    <?php
}?>
</tr>
</table>
</td>
</tr>
<tr>
<td><button title="Close" onclick="window.close();"><img src="../bilder/buttons/exit.png" /></button>
<button title="Close" onclick="load(<?php echo $teilnehmer->id;?>);"><img src="../bilder/buttons/refresh.png" /></button></td>
<td colspan="6"></td></tr>
</table>