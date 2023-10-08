<?php 
require_once("../host.inc");
$query = "SELECT max(id) as id FROM official";
$res = mysqli_fetch_object(mysqli_query($link,$query));
$id = $res->id + 1;
if(isset($_GET['id']) && $_GET['id'] == "*"){
	$query = "INSERT INTO official (`id`,`vorname`,`nachname`,`club`,`land`,`license`,`bild`) VALUES (".$id.",'".$_GET['vorname']."','".$_GET['nachname']."','".$_GET['club']."','".$_GET['land']."','".$_GET['license']."',1)";
	mysqli_query($link,$query);
}

if(isset($_GET['id']) && $_GET['id']== "del" && isset($_GET['del'])){
	$query = "DELETE FROM official WHERE id = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM area_access WHERE official = ".$_GET['del'];
	mysqli_query($link,$query);
}

if(isset($_GET['id']) &&$_GET['id'] != "*" && $_GET['id'] != "" && $_GET['id'] != "del"){
	$query = "UPDATE official SET vorname = '".$_GET['vorname']."', nachname = '".$_GET['nachname']."', club = '".$_GET['club']."', land = ".$_GET['land'].", license = '".$_GET['license']."' WHERE id = ".$_GET['id'];
	mysqli_query($link,$query);
}
$query = "SELECT * FROM official WHERE id > 0 ORDER BY id ASC";
$res = mysqli_query($link,$query);
?>
<table class="liste">
<tr>
<th colspan="7" class="header"><?php echo $turnier;?></th></tr>
<tr>
<th colspan="7">List of Officials</th></tr>
<tr class="header">
<th colspan="1">#</th><th>Name</th><th>Surname</th><th>Function</th><th>Nation</th><th>License</th>
<th></th></tr>
<?php
$count = 1;
while($obj = mysqli_fetch_object($res)){?>
	<tr <?php if($count % 2 == 0) echo "class='gerade'";?>>
    <td>
	<?php echo $obj->id;?>
    </td>
    <td><input type="text" id="nn<?php echo $obj->id;?>" value="<?php echo $obj->nachname;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
    <td><input type="text" id="vn<?php echo $obj->id;?>" value="<?php echo $obj->vorname;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
    <td><input type="text" id="c<?php echo $obj->id;?>" value="<?php echo $obj->club;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
    <td>
    <select id="l<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>);">
    <?php
	$res_land = mysqli_query($link,"SELECT img_id, name FROM country_images ORDER BY name ASC");
	while($obj_land = mysqli_fetch_object($res_land)){
		echo "<option value='".$obj_land->img_id."'";
		if($obj_land->img_id == $obj->land) echo " selected='selcted'";
		echo ">".$obj_land->name."</option>";
	}?>
    </select>
    </td>
    <td><input type="text" id="li<?php echo $obj->id;?>" value="<?php echo $obj->license;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
    <td style="padding:5px; border-right:1px solid;"><button class="edit" onclick="oeffnen('../officials_detail.php?id=<?php echo $obj->id;?>');"><img src="../bilder/buttons/edit.png" height="14" /></button><input type="button" onclick="del(<?php echo $obj->id;?>);" value="X" /></td>
    <?php }?>
    </tr><?php
	$count++;
?>
<tr <?php if($count % 2 == 0) echo "class='gerade'";?> style="font-style:italic;">
<td style="border-left:1px solid;" colspan="1">[*new*]</td>
<td><input type="text" id="nn*" /></td>
<td><input type="text" id="vn*" /></td>
<td><input type="text" id="c*" /></td>
<td>
<select id="l*">
    <?php
	$res_land = mysqli_query($link,"SELECT img_id, name FROM country_images ORDER BY name ASC");
	while($obj_land = mysqli_fetch_object($res_land)){
		echo "<option value='".$obj_land->img_id."'";
		echo ">".$obj_land->name."</option>";
	}?>
    </select>
</td>
<td><input type="text" id="li*" /></td>
<td style="border-right:1px solid;" colspan="4"><input type="button" value="Save" onclick="speichern('*');" /></td>
</tr>
<tr>
<td colspan="4"><input type="button" value="Close" onclick="window.close();" /></td>
</tr>
</table>
