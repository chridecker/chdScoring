<?php
require_once("../host.inc");

//XML Config File
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;

$query = "SELECT f.id as id, f.name as figur, f.wert as k, p.id as pid, p.title as programm FROM figur as f JOIN figur_programm as fp ON (f.id = fp.figur) JOIN programm as p ON (fp.programm = p.id) WHERE p.id = ".$_GET['programm'];
$res = mysqli_query($link,$query);
$obj = mysqli_fetch_object($res);
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm WHERE p.id = ".$obj->pid;
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));

$res = mysqli_query($link,$query);
?>
<table>
<tr class="headline">
<th colspan="3"></th></tr>
<tr class="headline">
<th> # </th><th>Manoeuvre</th><th>K - Value</th></tr>
<?php 
$count = 1;
while($figur = mysqli_fetch_object($res)){
	$id = $figur->id;
	$name = $figur->figur;
	$k = $figur->k;?>
    <tr<?php if($count % 2 == 0)echo " class='gerade'";?>>
    <td><?php echo str_pad(($id - $obj_count_figur->anfang + 1),2,0,STR_PAD_LEFT);?></td>
	<td><?php echo $name;?></td>
    <td style="text-align:center;"><?php echo $k;?></td>
    </tr>
    <?php
	$count++;
}
$query = "SELECT SUM(f.wert) as sumk FROM figur as f JOIN figur_programm as fp ON (f.id = fp.figur) JOIN programm as p ON (fp.programm = p.id) WHERE p.id = ".$_GET['programm'];
$res = mysqli_query($link,$query);
$obj = mysqli_fetch_object($res);?>
<tr><td colspan="2"></td><td><?php echo $obj->sumk;?></td></tr>
<tr><td colspan="3" style="text-align:center; font-size:10pt;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td></tr>

</table>
