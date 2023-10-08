<?php
require_once("../host.inc");
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;
$count = 0; 
$limit = 30;
$gesamt = ceil($teilnehmer_anzahl / $limit);?>
<style type="text/css">
body {
	margin:0cm;
}
table {
	width:20cm;
	max-height:29cm;
	border-collapse:collapse;
	-webkit-print-color-adjust: exact;
	font-family:Arial, Helvetica, sans-serif;
}
.headline th{
	font-size:20pt;
	text-align:center;
	background-color:white;
	border-bottom:solid 1px grey;
}
.headline td{
	font-size:10pt;
	text-align:center;
	background-color:white;
	border-bottom:solid 1px black;
}
tr.header th {
	background-color:lightblue;
	font-size:14pt;
	text-align:left;
	padding:0.1cm;
	border-bottom:solid black;
}

td{
	font-size:12pt;
	padding:0.2cm;
}
.grey td {
	background-color:lightgrey;
}
.footer td {
	font-size:10pt;
	border-top:1px solid black;
}
@media print {
	table{
		page-break-after:always;
	}
}
</style>
<table>
<tr class="headline">
<th colspan="5"><?php echo $turnier;?></th></tr>
<tr class="headline">
<td colspan="5"><?php echo $turnier_ort.", ".$turnier_date;?></td></tr>
<tr class="header">
<th>Name</th><th>Vorname</th><th>License</th><th>Club</th><th>Land</th></tr>
<?php
$res_teilnehmer = mysqli_query($link,"SELECT * FROM teilnehmer ORDER BY nachname ASC, vorname ASC");
while($teilnehmer = mysqli_fetch_object($res_teilnehmer)){
	$count++;?>
    <tr <?php if($count % 2 == 0)echo"class='grey'";?> style="#border-top:1px solid black;">
    <td><?php echo strtoupper($teilnehmer->nachname);?></td>
    <td><?php echo $teilnehmer->vorname;?></td>
	<td><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT UPPER(short) as code FROM country_images WHERE img_id = ".($teilnehmer->land)))->code;?>-<?php echo $teilnehmer->license;?></td>
    <td><?php echo ($teilnehmer->club);?></td>
    <td><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".($teilnehmer->land)))->name;?></td>
    </tr>
    <?php
	if(($count % $limit) == 0){?>
    	<tr class="footer">
        <td colspan="3" style="text-align:left;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
        <td colspan="2" style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
        </table>
        <table>
        <tr class="header">
        <th>Name</th><th>Vorname</th><th>License</th><th>Club</th><th>Land</th></tr>
        <?php
	}
}?>
<tr>
<th colspan="5" style="text-align:right;padding-right:0.8cm; border-top:1px solid black;">Total: <?php echo mysqli_fetch_object(mysqli_query($link,"SELECT count(id) as gesamt FROM teilnehmer"))->gesamt;?></th></tr>
<tr class="footer">
<td colspan="3" style="text-align:left;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
<td colspan="2" style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
</table>
