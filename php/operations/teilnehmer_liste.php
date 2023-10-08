<?php 
require_once("../host.inc");
$query_start = "SELECT count(wl.teilnehmer) as count FROM wettkampf_leitung as wl";
$start = mysqli_fetch_object(mysqli_query($link,$query_start));
$query = "SELECT max(id) as id FROM teilnehmer";
$res = mysqli_fetch_object(mysqli_query($link,$query));
$id = $res->id + 1;
if(isset($_GET['id']) && $_GET['id'] == "*"){
	$query = "INSERT INTO teilnehmer (`id`,`vorname`,`nachname`,`club`,`land`,`license`) VALUES (".$id.",'".$_GET['vorname']."','".$_GET['nachname']."','".$_GET['club']."','".$_GET['land']."','".$_GET['license']."')";
	mysqli_query($link,$query);
	$query = "INSERT INTO teilnehmer_bewerb (`teilnehmer`,`bewerb`) VALUES (".$id.",1)";
	mysqli_query($link,$query);
	mysqli_query($link,"INSERT INTO area_access (`area`,`pilot`) VALUES (1,".$id.")");
	for($i=2;$i<=$subevents;$i++){
		if(isset($_GET['sub'.$i]) && $_GET['sub'.$i] == "true"){
			$query = "INSERT INTO teilnehmer_bewerb (`teilnehmer`,`bewerb`) VALUES (".$id.",".$i.")";
			mysqli_query($link,$query);
		}
	}
}

if(isset($_GET['id']) && $_GET['id']== "del" && isset($_GET['del'])){
	$bildid = mysqli_fetch_object(mysqli_query($link,"SELECT bild FROM teilnehmer WHERE id = ".$_GET['del']))->bild;
	$query = "DELETE FROM images WHERE img_id = ".$bildid;
	if($bildid !=  1)mysqli_query($link,$query);
	$query = "DELETE FROM teilnehmer WHERE id = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM teilnehmer_bewerb WHERE teilnehmer = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM durchgang WHERE teilnehmer = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM endergebnis WHERE teilnehmer = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM tbl_result WHERE teilnehmer = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM wertung WHERE teilnehmer = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM model_aircraft WHERE teilnehmer = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM area_access WHERE pilot = ".$_GET['del'];
	mysqli_query($link,$query);
	
	
	$query = "UPDATE teilnehmer SET id = id - 1 WHERE id > ".$_GET['del'];
	//mysqli_query($link,$query);
	$query = "UPDATE teilnehmer_bewerb SET teilnehmer = teilnehmer - 1 WHERE teilnehmer > ".$_GET['del'];
	//mysqli_query($link,$query);
}

if(isset($_GET['id']) &&$_GET['id'] != "*" && $_GET['id'] != "" && $_GET['id'] != "del"){
	$query = "UPDATE teilnehmer SET vorname = '".$_GET['vorname']."', nachname = '".$_GET['nachname']."', club = '".$_GET['club']."', land = ".$_GET['land'].", license = '".$_GET['license']."' WHERE id = ".$_GET['id'];
	mysqli_query($link,$query);
	$query_sub = "SELECT * FROM bewerb WHERE id != 1";
	$res_sub = mysqli_query($link,$query_sub);
	while($sub = mysqli_fetch_object($res_sub)){
		$query = "SELECT count(teilnehmer) as count FROM teilnehmer_bewerb WHERE bewerb = ".$sub->id." AND teilnehmer = ".$_GET['id'];
		$res_tm = mysqli_fetch_object(mysqli_query($link,$query));
		if($res_tm->count == 0 && $_GET['sub'.$sub->id] == "true")$query_tm = "INSERT INTO teilnehmer_bewerb (`teilnehmer`,`bewerb`) VALUES (".$_GET['id'].",".$sub->id.")";
		if($res_tm->count > 0 && $_GET['sub'.$sub->id] == "false") $query_tm = "DELETE FROM teilnehmer_bewerb WHERE teilnehmer = ".$_GET['id']." AND bewerb = ".$sub->id;
		if(isset($query_tm))mysqli_query($link,$query_tm);
	}
}
if(isset($_GET['search']) && $_GET['search'] != "")$search = " AND nachname LIKE '%".$_GET['search']."%' OR vorname LIKE '%".$_GET['search']."%'";
else $search = "";
$query = "SELECT * FROM teilnehmer WHERE id > 0 ".$search." ORDER BY id ASC";
$res = mysqli_query($link,$query);
?>
<table class="liste">
<tr>
<th colspan="<?php echo (8 + $subevents);?>" class="header"><?php echo $turnier;?></th></tr>
<tr>
<td colspan="<?php echo (8 + $subevents);?>">Search: <input type="text" id="searchterm" onchange="searchTerm();" /></td></tr>
<tr class="header">
<th colspan="3">#</th><th>Name</th><th>Surname</th><th>Club</th><th>Nation</th><th>License</th>
<?php
$query_sub = "SELECT name FROM bewerb WHERE id != 1 ORDER BY id ASC";
$res_sub = mysqli_query($link,$query_sub);
while($sub = mysqli_fetch_object($res_sub)){?>
	<th><?php echo $sub->name;?></th>
    <?php
}?>
<th></th></tr>
<?php
$count = 1;
if($res){
	while($obj = mysqli_fetch_object($res)){?>
		<tr <?php if($count % 2 == 0) echo "class='gerade'";?>>
		<td style="border-left:1px solid; padding:0px;">
		<?php if($obj->id > 1){?>
		<button class="edit" title="Up" onclick="changeID('up',<?php echo $obj->id;?>);">
		<img src="../bilder/buttons/up.png" height="10" /></button>
		<?php }?></td>
		<td>
		<?php if($obj->id < mysqli_fetch_object(mysqli_query($link,"SELECT max(id) as maxid FROM teilnehmer"))->maxid){?>
		<button class="edit" title="Down" onclick="changeID('down',<?php echo $obj->id;?>);">
		<img src="../bilder/buttons/down.png" height="10" /></button>
		<?php }?>	
		</td>
		<td><?php echo $obj->id;?></td>
		<td><?php echo $obj->nachname;?></td>
		<td><?php echo $obj->vorname;?></td>
		<td><?php echo $obj->club;?></td>
		<td>
		<?php
		$res_land = mysqli_query($link,"SELECT img_id, name FROM country_images ORDER BY name ASC");
		while($obj_land = mysqli_fetch_object($res_land)){
			if($obj_land->img_id == $obj->land)echo $obj_land->name;
		}?>
		</td>
		<td><?php echo $obj->license;?></td>
		<?php 
		$query_sub = "SELECT * FROM bewerb WHERE id != 1 ORDER BY id ASC";
		$res_sub = mysqli_query($link,$query_sub);
		while($sub = mysqli_fetch_object($res_sub)){?>
			<td>
			<input type="checkbox" id="sub<?php echo $sub->id.$obj->id;?>"
			<?php
			$query_bewerb = "SELECT tb.bewerb FROM teilnehmer_bewerb as tb WHERE tb.bewerb = ".$sub->id." AND tb.teilnehmer = ".$obj->id;
			$res_bewerb = mysqli_query($link,$query_bewerb);
			if($bewerb = mysqli_fetch_object($res_bewerb))echo " checked";
			?> onChange="speichern(<?php echo $obj->id;?>);"  /></td>
			<?php
		}?>
		<?php if($start->count == 0){?>
		<td style="padding:5px; border-right:1px solid;"><button class="edit" onclick="oeffnen('../teilnehmer_detail.php?id=<?php echo $obj->id;?>');"><img src="../bilder/buttons/edit.png" height="14" /></button><input type="button" onclick="del(<?php echo $obj->id;?>);" value="X" /></td>
		<?php }?>
		</tr><?php
		$count++;
	}
}
?>
<?php if($_SERVER['PHP_SELF'] == "/operations/teilnehmer_liste.php" && $start->count == 0){?>
<tr <?php if($count % 2 == 0) echo "class='gerade'";?> style="font-style:italic;">
<td style="border-left:1px solid;" colspan="3">[*new*]</td>
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
<?php 
$query_sub = "SELECT * FROM bewerb WHERE id != 1 ORDER BY id ASC";
$res_sub = mysqli_query($link,$query_sub);
while($sub = mysqli_fetch_object($res_sub)){?>
	<td><input type="checkbox" id="<?php echo "sub".$sub->id."*";?>" /></td>
    <?php
}?>
<td style="border-right:1px solid;" colspan="4"><input type="button" value="Save" onclick="speichern('*');" /></td>
</tr>
<?php
}?>
<tr>
<td colspan="3"><input type="button" value="Close" onclick="window.close();" /></td>
<?php 
if($start->count == 0){?>
    <td><input type="button" value="Random" onclick="randomStarter();" /></td>
    <?php 
}
?>
</tr>
</table>
