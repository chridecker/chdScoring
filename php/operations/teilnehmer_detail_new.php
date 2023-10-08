<?php
require_once("../host.inc");
if(isset($_GET['change']) && $_GET['change'] == 1){
	$query = "INSERT INTO teilnehmer (`id`,`vorname`,`nachname`,`club`,`land`,`license`,`email`,`telefon`,`strasse`,`plz`,`ort`,`bild`,`info`) VALUES (".$_GET['id'].",'".$_GET['vorname']."','".$_GET['nachname']."','".$_GET['club']."',".$_GET['land'].",'".$_GET['license']."','".$_GET['email']."','".$_GET['telefon']."','".$_GET['strasse']."','".$_GET['plz']."','".$_GET['ort']."',".$_GET['bild'].",'".$_GET['info']."')";
	mysqli_query($link,$query);
	mysqli_query($link,"INSERT INTO teilnehmer_bewerb (`teilnehmer`,`bewerb`) VALUES (".$_GET['id'].",1)");
	mysqli_query($link,"INSERT INTO area_access (`area`,`pilot`) VALUES (1,".$_GET['id'].")");
	echo "SAVED";
	exit;
}
$id = mysqli_fetch_object(mysqli_query($link,"SELECT max(id) + 1 as id FROM teilnehmer"))->id;
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
<th>#</th><td><input type="text" id="id" value="<?php echo $id;?>" readonly="readonly"></td>
<td rowspan="5" colspan="2" class="profilbild">
<img src="operations/load_image.php?id=1" height="200">
<?php include("load_image_select.php");?>
<img src="operations/load_image_country.php?id=13" height="200" style="margin-left:100px;">
</td></tr>
<tr>
<th>Name</th><td><input type="text" id="nn" value="" ></td></tr>
<tr>
<th>Surname</th><td><input type="text" id="vn" value="" ></td></tr>
<tr>
<th>License</th><td><input type="text" id="li" value="" ></td>
</tr>
<tr><td colspan="2"><br /></td>
</tr>
<tr>
<th>Club</th><td><input type="text" id="c" value="" ></td>
<td colspan="2" class="profilbild"></td>
<tr>
<th>Nation</th><td>
<select id="l">
<?php
$res_land = mysqli_query($link,"SELECT img_id, name FROM country_images ORDER BY name ASC");
while($obj_land = mysqli_fetch_object($res_land)){
    echo "<option value='".$obj_land->img_id."'";
    if($obj_land->img_id == 13) echo " selected='selcted'";
    echo ">".$obj_land->name."</option>";
}?>
</select>
</td><th>Phone</th><td><input type="text" id="telefon" value="" ></td></tr>
<tr>
<th>Street</th><td><input type="text" id="strasse" value="" ></td><th>Email</th><td><input type="text" id="email" value="" ></td></tr>
<tr>
<th>PLZ</th><td><input type="text" id="plz" value="" ></td><th>Location</th><td><input type="text" id="ort" value="" ></td></tr>
<tr><th colspan="2">Detail Information</th><th colspan="2"></th></tr>
<tr><td colspan="2"><textarea id="info"  style="width:99%; height:150px;"></textarea></td>
<td colspan="2">
<?php 
/*
$res_class = mysqli_query($link,"SELECT * FROM klassen WHERE id = ".$result_config->klasse);
$class = mysqli_fetch_object($res_class);?>
<table class="aircraft">
<tr><th></th><th>Model Aircraft<br /><i>[<?php echo $class->max_aircraft;?>]</i></th><th>Wingspan<br /><i>[<?php echo $class->max_breite;?>]</i></th><th>Overall Length<br /><i>[<?php echo $class->max_laenge;?>]</i></th><th>Weight<br /><i>[<?php echo $class->max_gewicht;?>]</i></th><th>Propulsion circuit<br />&nbsp;</th><th>Ident<br />&nbsp;</th><th></th></tr>
<?php
$query_ma = "SELECT * FROM model_aircraft WHERE teilnehmer = ".$id;
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
*/?>
</td>
</tr>
<tr>
<td>
<button title="Close" onclick="speichern();"><img src="../bilder/buttons/save.png" /></button>
<button title="Close" onclick="window.close();"><img src="../bilder/buttons/exit.png" /></button>
</td>
<td colspan="6"></td></tr>
</table>