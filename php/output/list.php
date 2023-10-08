<?php
require_once("../host.inc");

$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;

if(isset($_GET['type']))$type = $_GET['type'];
else $type = "teilnehmer";?>

<table>
<tr class="headline">
<th colspan="4"><?php echo $turnier; ?></th></tr>
<tr class="headline">
<td colspan="4"><?php echo $turnier_ort.", ".$turnier_date; ?></td></tr>
<tr class="header">
<th></th><th>Name</th><th>Country / Club</th><th>License</th></tr>
<?php
$count = 0;
$limit = 10;
$gesamt = ceil($teilnehmer_anzahl / $limit);
if($type == "judge")$res = mysqli_query($link,"SELECT *, name as nachname FROM judge ORDER BY nachname ASC, vorname ASC");
else $res = mysqli_query($link,"SELECT * FROM ".$type." ORDER BY nachname ASC, vorname ASC");
while($teilnehmer = mysqli_fetch_object($res)){
	$count++;?>
    <tr <?php if($count % 1 == 0)echo "class='grey'";?>>
    <td><img class="profil" src="../operations/load_image.php?id=<?php echo $teilnehmer->bild;?>" /></td>
    <td><?php echo strtoupper($teilnehmer->nachname);?><br /><?php echo $teilnehmer->vorname;?></td>
    <td><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$teilnehmer->land))->name;?><br /><?php echo $teilnehmer->club;?></td>
    <td style="text-align:right; padding-right: 0cm; font-size:10pt; letter-spacing:7px; background-color:white;"><img src="../operations/gen_barcode.php?text=<?php echo mysqli_fetch_object(mysqli_query($link,"SELECT UPPER(short) as name FROM country_images WHERE img_id = ".$teilnehmer->land))->name.$teilnehmer->license;?>&size=40" />
    <?php echo mysqli_fetch_object(mysqli_query($link,"SELECT UPPER(short) as name FROM country_images WHERE img_id = ".$teilnehmer->land))->name.$teilnehmer->license;?>
    </td>
    </tr>
	<?php
    if($count % $limit == 0){?>
        <tr class="footer"><td colspan="3" style="text-align:left;"><?php echo $system_name." ".$version." &copy;".$year;?></td><td style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
		</table>
        <table>
        <tr class="header">
        <th></th><th>Name</th><th>Country / Club</th><th>License</th></tr>
		<?php
		}
}?>
<tr class="footer"><td colspan="3" style="text-align:left;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td><td style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
</table>
