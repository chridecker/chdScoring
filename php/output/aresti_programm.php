<?php
require_once("../host.inc");
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;

$programm = $_GET['programm'];
$query = "SELECT p.* FROM programm p WHERE p.id = ".$programm;
$res_programm = mysqli_fetch_object(mysqli_query($link,$query));?>
<table>
<tr>
<th colspan="3"><?php echo $res_programm->title;?></th></tr>
<tr>
<td colspan="3"><?php echo $res_programm->description;?></td></tr>
<tr>
<?php
$query = "SELECT f.* FROM figur f JOIN figur_programm fp ON (fp.figur = f.id) WHERE fp.programm = ".$programm;
$res = mysqli_query($link,$query);
$count = 0;
while($figur = mysqli_fetch_object($res)){
	if($count == 3){
		echo "</tr><tr>";
		$count=0;
	}
	echo "<td><img class='aresti' src='../operations/load_image_figur.php?figur=".$figur->id."' title='".$figur->name."'></td>";
	$count++;
}?>
</tr>
<tr><td colspan="3" style=" margin-top:0.5cm; font-size:10pt; border-top:1px solid black;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td></tr>
</table>