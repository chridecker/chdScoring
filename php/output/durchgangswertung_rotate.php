<?php
if(!isset($file))$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;
if($_SERVER['PHP_SELF'] == "/output/durchgangswertung_rotate.php")require_once("../host.inc");
//Durchgangswertung anzeigen
if(isset($_GET['durchgang']))$durchgang = $_GET['durchgang'];
else if(isset($_GET['airfield'])){
	$query = "SELECT min(wl.durchgang) as durchgang FROM wettkampf_leitung wl JOIN durchgang_airfield da ON (da.durchgang = wl.durchgang) WHERE da.airfield = ".$_GET['airfield']." AND wl.status <= 2";
	$max = mysqli_fetch_object(mysqli_query($link,$query));
	if($max->durchgang == 0)$durchgang = $_GET['airfield'];
	else {
		$durchgang = $max->durchgang;
		$query_anzahl = "SELECT count(wl.teilnehmer) as anzahl FROM wettkampf_leitung wl WHERE wl.durchgang = ".$durchgang." AND wl.status > 2";
		//if(mysqli_fetch_object(mysqli_query($link,$query_anzahl))->anzahl == 0)$durchgang--;
	}
}
if($durchgang < 1) $durchgang = 1;
$rowLimit = 15;
if(isset($_GET['rowLimit'])) $rowLimit = $_GET['rowLimit'];
$startLimit = 0;
if(isset($_GET['startLimit'])) {$startLimit = $_GET['startLimit']; }
$startIndex = $startLimit * $rowLimit;
$query_durchgang = "SELECT t.*, concat(t.vorname,' ',t.nachname) as name, d.wert_abs as abs, d.wert_prom as prom FROM teilnehmer as t, durchgang as d WHERE t.id  = d.teilnehmer AND d.durchgang = ".$durchgang." ORDER BY d.wert_abs DESC LIMIT ".$startIndex.", ".$rowLimit;
$res = mysqli_query($link,$query_durchgang);
?>
<body>
<table>
<tr class="headline">
<?php 
$query_prog = "SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".$durchgang;
$res_prog = mysqli_query($link,$query_prog);
$res_programm = mysqli_fetch_object($res_prog);?>
<th colspan="4" style="text-align:left; vertical-align:bottom;"><?php echo "Round ".$durchgang;?>(<i class="programm"><?php echo $res_programm->title;?></i>)</th>
<th colspan="2" style="background-color:none;"><?php if($_SERVER['PHP_SELF']=="/output/durchgangswertung.php" || $_SERVER['PHP_SELF'] == '/print/index.php'){?>
	<?php }?></th>
</tr>
<tr class="header">
<th>P.</th><th>Name</th><th>Nation</th><th>FAI-Lic</th><th>Score</th></tr>
<?php
$count = 0;
while($result_durchgang = mysqli_fetch_object($res)){
	$count++;
    if(($count % 2) == 0 )echo "<tr class='gerade'>";
	else echo "<tr class='ungerade'>";?>
    <td><?php echo ($count + $startIndex);?></td>
    <td><?php echo $result_durchgang->name;?></td>
    <td><?php //echo $result_durchgang->club." / ";
	echo mysqli_fetch_object(mysqli_query($link,"SELECT UPPER(short) as short FROM country_images WHERE img_id = ".$result_durchgang->land))->short;
	//echo "<img src='../operations/load_image_country.php?id=".$result_durchgang->land."' height='30'>";?></td>
    <td><?php echo $result_durchgang->license;?></td>
    <td><?php if($result_durchgang->prom == 0)echo $result_durchgang->abs;
	 	else echo number_format($result_durchgang->prom,2,',','')."<div class='wert_abs'>(".number_format($result_durchgang->abs,2).")</div>";?></td></tr><?php
}
$check = 1;
if($_SERVER['PHP_SELF'] == "/print/index.php"){?>
<tr>
<td style="border-left:1px solid; border-right:1px solid; text-align:center;" colspan="5"><?php echo $system_name." ".$version." &copy; ".$year;?></td></tr><?php }?>
</table>
</body>
<?php
mysqli_free_result($res);
//mysqli_close($link);?>