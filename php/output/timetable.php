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
	text-align:left;
}
th{
	font-size:12pt;
	padding:0.2cm;
	text-align:left;
}
.grey td, .grey th {
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
<?php
//Loading Variables
$durchgaenge = 3;
$durchgaenge_start = 1;

$teilnehmer_anzahl = $teilnehmer_anzahl;
// Setting Params
$breaks_in_round = 1;
$break_small = 5;
$break = 10;
$lunch_after_round = 2;
$lunch=60;
$starttime = "07:35";
if(isset($_GET['starttime']))$starttime = $_GET['starttime'];
$time = mysqli_fetch_object(mysqli_query($link,"SELECT zeit FROM klassen WHERE id = ".$result_config->klasse))->zeit;
$time_between = 1;
$start = strtotime($result_config->datum." ".$starttime); //strtotime("2016-07-16 08:00");
?>
<table>
<tr class="headline">
<th colspan="2"><?php echo $turnier;?></th></tr>
<tr class="headline">
<th colspan="2">Timetable</th></tr>
<tr class="headline">
<td colspan="2"><?php echo $turnier_ort.", ".$turnier_date;?></td></tr>
<tr class="header">
<th>Approx. Time</th><th>&#8470;</th></tr>
<?php
$count = 1;
echo "<tr><td>".date("H:i",$start)."</td><td>Pilots Meeting</td></tr>";
$start += 15 * 60;
$count++;
if($count % 2 == 0)echo "<tr class='grey'>";
else echo "<tr>";
echo "<td>".date("H:i",$start)."</td><td>Vorflieger 00</td></tr>";
$start += ($time + $time_between + 5) * 60;
$rounds = $durchgaenge_start-1;
for($d=$durchgaenge_start;$d<=$durchgaenge;$d++){
	$breaks = 0;
	$rounds++;

	$count++;
	if(($count % 2) == 0)echo "<tr class='grey' style='border-top:solid black;'>";
	else echo "<tr style=' background-color:white; border-top:solid black;'>";
	echo "<th>".date("H:i",$start)."</th>";
	echo "<th colspan='1'>ROUND ".$d." START</th></tr>";

	for($i=1;$i<=$teilnehmer_anzahl;$i++){
		$count++;
		if($count % 2 == 0)echo "<tr class='grey'>";
		else echo "<tr>";
		echo "<td>".date("H:i",$start)."</td>";
		echo "<td>&#8470; ";
		echo mysqli_fetch_object(mysqli_query($link,"SELECT CONCAT(t.id,' ',t.vorname,' ',UPPER(t.nachname)) as name FROM teilnehmer t JOIN wettkampf_leitung wl ON (wl.teilnehmer = t.id) WHERE wl.durchgang = ".$rounds." AND wl.start = ".$i))->name;
		echo "</td></tr>";
		$start += ($time + $time_between) * 60;
		if($i % round($teilnehmer_anzahl / ($breaks_in_round + 1)) == 0 && $breaks < $breaks_in_round){
			$breaks++;
			$count++;
			if($count % 2 == 0)echo "<tr class='grey' style='border-top:1px solid black;'>";
			else echo "<tr style='border-top: 1px solid black;'>";
			echo "<td>".date("H:i",$start)."</td>";
			echo "<td colspan='1'>BREAK [".$break_small."]</td></tr>";
			$start += ($break_small*60);
		}
	}
	$count++;
	if(($count % 2) == 0)echo "<tr class='grey' style='border-top:solid black;'>";
	else echo "<tr style=' background-color:white; border-top:solid black;'>";
	echo "<th>".date("H:i",$start)."</th>";
	echo "<th colspan='1'>ROUND ".$d." END</th></tr>";

	if($lunch_after_round == $d){
		if(($count % 2) == 0)echo "<tr class='grey' style='border-top:solid black;'>";
		else echo "<tr style=' background-color:white; border-top:solid black;'>";
		echo "<th>".date("H:i",$start)."</th>";
		echo "<th colspan='1'>LUNCH BREAK [".$lunch."]</th></tr>";
		$start += ($lunch) * 60;
	}

	else if($rounds < $durchgaenge){
		$count++;
		if($count % 2 == 0)echo "<tr class='grey'>";
		else echo "<tr>";
		echo "<td>".date("H:i",$start)."</td>";
		echo "<td colspan='1'>BREAK [".$break."]</td></tr>";
		$start += $break * 60;
	}?>
    <tr>
    <tr class="footer">
    <td colspan="1" style="text-align:left;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
    <td colspan="1" style="text-align:right;">Page <?php echo $rounds."/".$durchgaenge;?></td></tr>
    </table>
	<?php
	if($rounds < $durchgaenge){?>
    <table>
    <tr class="headline">
    <th colspan="2">Timetable</th></tr>
    <tr class="header">
	<th>Approx. Time</th><th>&#8470;</th></tr>
	<?php 
	}
}?>
<tr>
<tr class="footer">
<td colspan="1" style="text-align:left;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
<td colspan="1" style="text-align:right;">Page <?php echo $rounds."/".$durchgaenge;?></td></tr>
</table>