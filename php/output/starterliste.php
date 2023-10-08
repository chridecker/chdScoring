<?php
require_once("../host.inc");
if(isset($_GET["db"])){$link = mysqli_connect($host,$user,$password,$_GET["db"]);}
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;

if(isset($_GET['durchgang']))$durchgang = $_GET['durchgang'];
else {
	$obj = mysqli_fetch_object(mysqli_query($link,"SELECT min(durchgang) as durchgang FROM wettkampf_leitung"));
	$durchgang = $obj->durchgang;
}
?>
<link rel="stylesheet" type="text/css" href="../css/print_starterliste.css">
<table class="liste">
<tr>
<th colspan="6" class="header">
<img src="../operations/load_image.php?id=2" />
<?php echo "Start Sequence - ".$turnier;?>
</td>
<tr>
<th colspan="6" style="padding:0px;">Round <?php echo $durchgang;?></th></tr>
<tr class="header">
<th>#</th><th>Name</th><th>Surname</th>
<!-- <th>Club</th> -->
<th>Nation</th><th>License</th></tr>
<?php
$res = mysqli_query($link,"SELECT t.*, wl.durchgang FROM teilnehmer as t, wettkampf_leitung as wl WHERE wl.teilnehmer = t.id AND wl.durchgang = ".$durchgang." AND start > 0 ORDER BY wl.start ASC");
$count = 1;
$limit = 30;
$gesamt = ceil($teilnehmer_anzahl / $limit);
while($obj = mysqli_fetch_object($res)){?>
	<tr <?php if($count % 2 == 0) echo "class='gerade'";?>>
    <td><?php echo $obj->id;?></td>
    <td><?php echo $obj->nachname;?></td>
    <td><?php echo $obj->vorname;?></td>
    <!-- <td><?php echo $obj->club;?></td> -->
    <td><?php 
	echo mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$obj->land))->name;?></td>
    <td><?php 
	
	echo (strtoupper(mysqli_fetch_object(mysqli_query($link,"SELECT short FROM country_images WHERE img_id = ".$obj->land))->short))." - ".$obj->license;?></td>
    </tr>
    <?php 
	if($count % $limit == 0){?>
    	<tr class="footer"><td colspan="5" style="text-align:center; border:none;"><?php echo $system_name." ".$version." &copy;".$year;?></td><td style="text-align:right; border:none;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
		</table>
		<table class="liste">
		<tr>
		<th colspan="6" class="header">
		<img src="../operations/load_image.php?id=2" height="70" />
		<?php echo "Start Sequence - ".$turnier;?>
		</td>
		<tr>
        <th colspan="6" style="padding:0px;">Round <?php echo $durchgang;?></th></tr>
		<tr class="header">
		<th>#</th><th>Name</th><th>Surname</th>
		<!-- <th>Club</th> -->
		<th>Nation</th><th>License</th></tr>
		<?php
	}
	$count++;
}?>
<tr class="footer"><td colspan="5" style="text-align:center; border:none;"><?php echo $system_name." ".$version." &copy;".$year;?></td><td style="text-align:right; border:none;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
</table>
