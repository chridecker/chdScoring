<?php 
require_once("../host.inc");
$query = "SELECT max(id) as id FROM judge";
$res = mysqli_fetch_object(mysqli_query($link,$query));
$id = $res->id + 1;
if($_GET['id'] == "*"){
	$query = "INSERT INTO judge (`id`,`name`,`vorname`,`license`,`pin`) VALUES (".$id.",'".$_GET['name']."','".$_GET['vorname']."','".$_GET['license']."','".$_GET['pin']."')";
	mysqli_query($link,$query);
	mysqli_query($link,"INSERT INTO area_access (`area`,`judge`) VALUES (3,".$id.")");
}

if($_GET['id']== "del" && isset($_GET['del'])){
	$query = "DELETE FROM judge WHERE id = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM judge_panel WHERE judge = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM area_access WHERE judge = ".$_GET['del'];
	mysqli_query($link,$query);
	//if($_GET['del'] < $id)mysqli_query($link,"UPDATE judge SET id = id -1 WHERE id > ".$_GET['del']."");
}

if($_GET['id'] != "*" && $_GET['id'] != "" && $_GET['id'] != "del"){
	$query = "UPDATE judge SET name = '".$_GET['name']."', vorname = '".$_GET['vorname']."', license = '".$_GET['license']."', pin = '".$_GET['pin']."' WHERE id = ".$_GET['id'];
	mysqli_query($link,$query);
	
}
$query = "SELECT * FROM judge ORDER BY id ASC";
$res = mysqli_query($link,$query);
?>
<table class="liste">
<tr>
<th colspan="5" class="headline">Judges</th>
<th><button title="Edit Panels" style="border:1px black solid; padding:5px; background-color:white;" onclick="oeffnen('operations/judge_panel.php');"><img src="../bilder/buttons/edit_round.png" height="30" /></button></th>
</tr>
<tr class="header">
<th>#</th><th>Name</th><th>Surname</th><th>License</th><th>Pin</th><th>[X]</th></tr>
<?php
$count = 1;
while($obj = mysqli_fetch_object($res)){?>
	<tr <?php if($count % 2 == 0) echo "class='gerade'";?>>
	<td style="border-left:1px solid;"><?php echo $obj->id;?></td>
    <td><input type="text" id="name<?php echo $obj->id;?>" value="<?php echo $obj->name;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
    <td><input type="text" id="vorname<?php echo $obj->id;?>" value="<?php echo $obj->vorname;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
    <td><input type="text" id="license<?php echo $obj->id;?>" value="<?php echo $obj->license;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
    <td><input type="text" id="pin<?php echo $obj->id;?>" value="<?php echo $obj->pin;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
    <td style="padding:5px; border-right:1px solid;">
    <button title="Edit Details" onclick="oeffnen('../judges_detail.php?id=<?php echo $obj->id;?>');"><img src="../bilder/buttons/edit.png" height="20" /></button>
    <button onclick="randomCode(<?php echo $obj->id;?>);" title="Generate Random Pin"><img src="../bilder/buttons/random.png" height="20" /></button>
    <button onclick="del(<?php echo $obj->id;?>);" title="Delete"><img src="../bilder/buttons/delete.png" height="20" /></button></td>
    </tr><?php
	$count++;
}
?>
	<tr <?php if($count % 2 == 0) echo "class='gerade'";?>>
	<td style="border-left:1px solid;">*new</td>
    <td><input type="text" id="name*" /></td>
    <td><input type="text" id="vorname*" /></td>
    <td><input type="text" id="license*" /></td>
    <td><input type="text" id="pin*" /></td>
    <td style="border-right:1px solid;"><input type="button" value="Save" onClick="speichern('*')" /></td>
    </tr>
    <tr>
    <td><input type="button" value="Close" onclick="window.close();" /></td></tr>
</table>
