<?php
require_once("../host.inc");
$programm = $_GET['programm'];

//Bearbeitung
if(isset($_GET['id']) && isset($_GET['name']) && isset($_GET['wert']) && isset($_GET['programm'])){
	$query = "UPDATE figur SET name = '".$_GET['name']."', wert = ".$_GET['wert']." WHERE id = ".$_GET['id'];
	mysqli_query($link,$query);
}
if(isset($_GET['programm']) && isset($_GET['name']) && isset($_GET['description'])){
	$query = "UPDATE programm SET title = '".$_GET['name']."', description = '".$_GET['description']."' WHERE id = ".$_GET['programm'];
	mysqli_query($link,$query);
}



$query = "SELECT f.id as id, f.name as figur, f.wert as k, p.id as pid, p.title as programm, p.description FROM figur as f JOIN figur_programm as fp ON (f.id = fp.figur) JOIN programm as p ON (fp.programm = p.id) WHERE p.id = ".$programm;
$res = mysqli_query($link,$query);
$obj = mysqli_fetch_object($res);
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm WHERE p.id = ".$obj->pid;
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
$res = mysqli_query($link,$query);
?>
<table>
<tr class="header">
<th colspan="3"><input onchange="speicherProgramm();" id="programm_name" class="name" type="text" value="<?php echo $obj->programm;?>" /></th></tr>
<tr class="headline">
<th><input type="button" onClick="window.close();" value="Close"></th>
<th colspan="3"><input onchange="speicherProgramm();" id="programm_desc" class="name" type="text" value="<?php echo $obj->description;?>" /></th></tr>
<tr class="headline">
<th> # </th><th>Description</th><th>Value</th></tr>
<?php 
$count = 1;
while($figur = mysqli_fetch_object($res)){
	$id = $figur->id;
	$name = $figur->figur;
	$k = $figur->k;?>
    <tr<?php if($count % 2 == 0)echo " class='gerade'";?>>
    <td><?php echo str_pad(($id - $obj_count_figur->anfang + 1),2,0,STR_PAD_LEFT);?></td>
	<td><input id="<?php echo "f".$id;?>" class="name" type="text" value="<?php echo $name;?>" onChange="speichern(<?php echo $id;?>);"></td>
    <td><input id="<?php echo "k".$id;?>"class="wert" type="text" value="<?php echo $k;?>" onChange="speichern(<?php echo $id;?>);"></td>
    </tr>
    <?php
	$count++;
}
$query = "SELECT SUM(f.wert) as sumk FROM figur as f JOIN figur_programm as fp ON (f.id = fp.figur) JOIN programm as p ON (fp.programm = p.id) WHERE p.id = ".$_GET['programm'];
$res = mysqli_query($link,$query);
$obj = mysqli_fetch_object($res);?>
<tr><td colspan="2"></td><td><?php echo $obj->sumk;?></td></tr>
</table>
