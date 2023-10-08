<?php
require_once("../host.inc");
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;
$count = 0; 
$limit = 9;
$gesamt = ceil($teilnehmer_anzahl / $limit);?>

<table>
<tr class="headline">
<th colspan="4"><?php echo $turnier;?></th></tr>
<tr class="headline">
<td colspan="4"><?php echo $turnier_ort.", ".$turnier_date;?></td></tr>
<tr class="header">
<th colspan="2">Pilot</th><th>Address</th><th>Model Aircraft</th></tr>
<?php
$res_teilnehmer = mysqli_query($link,"SELECT * FROM teilnehmer ORDER BY nachname ASC, vorname ASC");
while($teilnehmer = mysqli_fetch_object($res_teilnehmer)){
	$count++;
	$res_model = mysqli_query($link,"SELECT * FROM model_aircraft WHERE teilnehmer = ".$teilnehmer->id);
	?>
    <tr style="border-top:1px solid black;">
    <td class="profil" rowspan="4"><img src="../operations/load_image.php?id=<?php echo $teilnehmer->bild;?>" /></td>
    <td class="name"><?php echo $teilnehmer->vorname." ".strtoupper($teilnehmer->nachname);?></td>
    <td class="adresse"><?php echo $teilnehmer->strasse;?></td>
    <td rowspan="2">
	<?php 
	if($model = mysqli_fetch_object($res_model)){
		echo $model->name."<br>[".$model->weight."g][".($model->length / 10)."cm][".($model->wingspan / 10)."cm]";
	}
	else echo " ";?>
    </td></tr>
    
    <tr>
    <td><?php echo $teilnehmer->ort;?></td>
    <td><?php echo $teilnehmer->plz." - ".$teilnehmer->ort;?></td></tr>
    
    <tr>
    <td><?php echo strtoupper(mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$teilnehmer->land))->name);?></td>
    <td><?php echo $teilnehmer->telefon;?></td>
    <td rowspan="2">
	<?php 
	if($model = mysqli_fetch_object($res_model)){
		echo $model->name."<br>[".$model->weight."g][".($model->length / 10)."cm][".($model->wingspan / 10)."cm]";
	}
	else echo " ";?>
    </td></tr>
    
    <tr>
    <td><?php echo $teilnehmer->license;?></td>
    <td><?php echo $teilnehmer->email;?></td></tr>
    <?php
	if(($count % $limit) == 0 && ($count != $teilnehmer_anzahl)){?>
    	<tr class="footer">
        <td colspan="2" style="text-align:left;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
        <td colspan="2" style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
        </table>
        <table>
        <tr class="header">
        <th colspan="2">Pilot</th><th>Address</th><th>Model Aircraft</th></tr>
        <?php
	}
}?>
<tr class="footer">
<td colspan="2" style="text-align:left;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
<td colspan="2" style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
</table>
