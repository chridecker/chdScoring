<?php 
require_once("../host.inc");
if(isset($_GET['id']) && $_GET['id'] != 'del' && $_GET['id'] != '*' && $_GET['id'] != ""){
	$query = "UPDATE bewerb SET name = '".$_GET['name']."', number = '".$_GET['number']."' WHERE id = ".$_GET['id'];
	mysqli_query($link,$query);
	
}
if(isset($_GET['id']) && $_GET['id'] == "*" && $_GET['id'] != ""){
	$query = "SELECT max(id) as max FROM bewerb";
	$max = mysqli_fetch_object(mysqli_query($link,$query));
	$query = "INSERT INTO bewerb (`id`,`name`,`number`) VALUES (".($max->max + 1).",'".$_GET['name']."','".$_GET['number']."')";
	mysqli_query($link,$query);
}
if(isset($_GET['id']) && $_GET['id'] == "del"){
	$query = "DELETE FROM bewerb WHERE id = ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "UPDATE bewerb SET id = id - 1 WHERE id > ".$_GET['del'];
	mysqli_query($link,$query);
	$query = "DELETE FROM teilnehmer_bewerb WHERE bewerb = ".$_GET['del'];
	mysqli_query($link,$query);
}
$query = "SELECT * FROM bewerb";
$res = mysqli_query($link,$query);
?>
<table class="liste">
<tr>
<th colspan="4" class="header">Sub - Events</th></tr>
<tr>
<tr class="header">
<th>ID</th><th>Sub-Event</th><th>Comp. Number</th><th>[X]</th></tr>
<?php
while($obj = mysqli_fetch_object($res)){?>
        <tr<?php if($obj->id % 2 == 0)echo " class='gerade'";?>>
        <td><?php echo $obj->id;?></td>
        <td style="border-left:1px solid;"><input type="text" <?php if($obj->id == 1)echo "readonly='readonly'";?> id="name<?php echo $obj->id;?>" value="<?php echo $obj->name;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
        <td style="border-left:1px solid;"><input type="text" <?php if($obj->id == 1)echo "readonly='readonly'";?> id="number<?php echo $obj->id;?>" value="<?php echo $obj->number;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
        <td><?php if($obj->id != 1){?><input type="button" value="X" onclick="del(<?php echo $obj->id;?>);" /><?php }?></td>
        </tr>
        <?php
}?>
<tr><td>*new</td><td><input type="text" id="name*"></td><td><input type="text" id="number*"></td><td><input type="button" value="Save" onClick="speichern('*');"></td></tr>
<tr><td><input type="button" value="Close" onClick="window.close();"></td></tr>
</table>