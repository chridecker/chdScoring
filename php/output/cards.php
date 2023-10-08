<?php 
require_once("../host.inc");?>
<?php
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;

if(isset($_GET['type']))$type = $_GET['type'];
else $type = "teilnehmer";
if(isset($_GET['rear']))$rear = $_GET['rear'];
else $rear = false;
if(isset($_GET['id']))$id = $_GET['id'];
if(isset($id))$id_string = "WHERE id = ".$id;
else $id_string = "";
if($type == "teilnehmer")$function = "pilot";
else $function = $type;
if($type == "turnier")$turnier =  $_GET['turnier'];

//All Pilots
if($type == "judge")$query_teilnehmer = "SELECT t.*, t.name as nachname FROM ".$type." t ".$id_string." ORDER BY name ASC, vorname ASC";
else $query_teilnehmer = "SELECT t.* FROM ".$type." t ".$id_string." ORDER BY nachname ASC, vorname ASC";
if($result_teilnehmer = mysqli_query($link,$query_teilnehmer)){
	while($teilnehmer = mysqli_fetch_object($result_teilnehmer)){
		$land = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM country_images WHERE img_id = ".$teilnehmer->land));?>
    	<table align='center'>
        <tr class="header">
        <th class="turnier" colspan="4"><?php echo $turnier;?></th></tr>
        <tr class="header">
        <td colspan="1" class="ort"><?php echo $turnier_ort;?></td>
        <td colspan="2" class="logo"><img src="../operations/load_image.php?id=<?php echo $result_config->urkunde_id;?>" /></td>
        <td colspan="1" class="datum"><?php echo $turnier_date;?></td></tr>
        <tr class="header">
        <td colspan="2" rowspan="2" class="photo"><img src="../operations/load_image.php?id=<?php echo $teilnehmer->bild;?>" /></td>
        <td colspan="2" class="flag"><img src="../operations/load_image_country.php?id=<?php echo $teilnehmer->land;?>" /></td></tr>
        <tr class="header">
        <td colspan="2" class="function"><?php echo strtoupper($function);?></td></tr>
        <tr class="main"><td class="name" colspan="4"><?php echo $teilnehmer->vorname." ".strtoupper($teilnehmer->nachname);?></td></tr>
        <tr class="main"><td colspan="4" class="land" ><?php echo strtoupper($land->name);?></td></tr>
        <tr class="main">
        <td class="id" colspan="2"><?php /*echo "&#8470; ".str_pad($teilnehmer->id,3,"0",STR_PAD_LEFT);*/?></td>
        <td colspan="2" rowspan="2" class="barcode">
			<?php /*<img src="../operations/gen_barcode.php?text=<?php echo strtoupper($land->short).$teilnehmer->license;?>&size=40" />
			<br />
			<?php echo strtoupper($land->short).$teilnehmer->license;?>
			 */?>
		</td>
        </tr>
        <tr class="main">
        <td colspan="3"class="club"><?php echo $teilnehmer->club;?></td></tr>
        <tr class="main">
		<th class='area'></th>
		<th class='area'></th>
		<th class='area'></th>
		</tr>
		<tr class="main">
		<td class="area_small">Tested</td>
		<td class="area_small">Tested</td>
		<td class="area_small">Tested</td>
		</tr>
        
        <tr class="footer">
        <td colspan="4" class="version"><?php echo $system_name." ".$version." &copy; ".$year; ?></td></tr>
        </table>
        <?php
	}
}
if($rear){?>
	<table align='center' class='backside'>
	<tr class="rear">
	<td class="turnier" colspan="3"><?php echo $turnier;?></td></tr>
	<tr class="rear">
	<td colspan="1" class="fai_logo"><img src="../operations/load_image.php?id=<?php echo $result_config->fed_id;?>" />
	<br /><?php echo $result_config->federation;?></td>
	<td colspan="1" class="host_logo"><img src="../operations/load_image.php?id=2" />
	<br />http://www.chdscoring.at/</td><td colspan="1" class="host_logo"><img src="../operations/load_image.php?id=<?php echo $result_config->img_id;?>" />
	<br /><?php echo $result_config->veranstalter_web;?></td>
	<tr class="rear">
	<td colspan="4" class="sponsor">
	<?php 
	$res = mysqli_query($link,"SELECT img_id FROM images WHERE img_sponsor = 1 ORDER BY img_title ASC");
	$sponsor_count = 0;
	$sponsor_limit = 3;
	while($sponsor = mysqli_fetch_object($res)){
		$sponsor_count++;
		echo "<img src='../operations/load_image.php?id=".$sponsor->img_id."'>";
		if($sponsor_count % $sponsor_limit == 0)echo "<br>";
	}?>        
	</td></tr>
	<tr class="footer">
	<td colspan="3" class="version"><?php echo $system_name." ".$version." &copy; ".$year; ?></td></tr>
	</table>
	<?php
}
?>
