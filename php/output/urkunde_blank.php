<?php
require_once("../host.inc");
if(isset($_GET["db"])){$link = mysqli_connect($host,$user,$password,$_GET["db"]);}
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;

//$durchgaenge = $result_config->durchgaenge;
if(isset($_GET['bewerb']))$bewerb = $_GET['bewerb'];
else $bewerb = 1;
if(isset($_GET['logo']))$logo = $_GET['logo'];
//Bewerb
$query_bewerb = "SELECT name , number FROM bewerb WHERE id = ".$bewerb;
$res_bewerb = mysqli_fetch_object(mysqli_query($link,$query_bewerb));
if($bewerb != 1){
	$turnier = $res_bewerb->name;
	$turnier_no = $res_bewerb->number;
}

?>
<link rel="stylesheet" type="text/css" href="../css/print_urkunde.css" />

<?php $rank = 1;?>
<?php
$query_teilnehmer = "SELECT t.*, b.teilnehmer FROM teilnehmer_bewerb as b, teilnehmer as t WHERE b.teilnehmer = t.id AND b.bewerb = ".$bewerb." ORDER BY t.nachname ASC";
if($result_teilnehmer = mysqli_query($link,$query_teilnehmer)){
	while($teilnehmer = mysqli_fetch_object($result_teilnehmer)){?>
	<table align='center'>
		<tr><th class="veranstalter" colspan="3"><?php echo $veranstalter;?></th></tr>
		<tr><td><br></td></tr>
		<tr><td colspan="3" class="logo"><img src="../operations/load_image.php?id=<?php echo $result_config->urkunde_id;?>"></td></tr>
		<tr class="headline"><th colspan='3'>URKUNDE</th></tr>
		<tr><td><br></td></tr>
		<?php 
		$query_sub = "SELECT name FROM bewerb WHERE id = ".$bewerb;
		$sub = mysqli_fetch_object(mysqli_query($link,$query_sub));?>
		<tr class="header"><th class="turnier" colspan="3"><?php echo $turnier;?></th></tr>	
		<tr><td><br></td></tr>
		<tr class="ergebnis"><th colspan="3" class="name"><?php echo strtoupper($teilnehmer->nachname)." ".$teilnehmer->vorname;?></th></tr>
		<tr class="ergebnis"><th colspan="3" class="club">errang in der Klasse <?php echo $sub->name; ?></th></tr>
		<tr class="ergebnis"><th colspan="3" class="punkte">mit &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <?php // echo number_format($teilnehmer->gesamt,2,",","");?>
        &permil; Punkten</th></tr>
		<tr class="ergebnis"><th colspan="3" class="platz"><i>den</i> &nbsp;&nbsp;<?php //echo $rank;?> Platz</th></tr>
		<tr><td><br></td></tr>
		<tr class="bild"><td colspan="3" class="gruppenbild"><img src="../operations/load_image.php?id=<?php echo $result_config->gruppe_id;?>"></td></tr>
		<tr><td><br></td></tr>
		<!--<tr class="footer"><td colspan="3" class="spacer"></td></tr>-->
        <tr class="footer">
        <td class="signature"><?php if(isset($_GET['cm']) && $_GET['cm']==true)echo "<img src='../bilder/cm_sign.png'>";?></td>
        <td>&nbsp;</td>
        <td class="signature"><?php if(isset($_GET['org']) && $_GET['org']==true)echo "<img src='../bilder/org_sign.png'>";?></td></tr>
		<tr class="footer"><th class="name"><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT CONCAT(vorname, ' ', nachname) as name FROM official WHERE id = ".$result_config->wettkampf_leiter))->name;?></th><td></td><th class="name"><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT CONCAT(vorname, ' ', nachname) as name FROM official WHERE id = ".$result_config->org_leiter))->name;?></th></tr>
		<tr class="footer"><th class="funktion"><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT club as name FROM official WHERE id = ".$result_config->wettkampf_leiter))->name;?></th><td></td><th class="funktion"><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT club as name FROM official WHERE id = ".$result_config->org_leiter))->name;?></th></tr>
		<tr><td colspan="3" class="ort"><?php echo $turnier_ort;?>, <?php echo date("d.m.Y",strtotime($result_config->end_datum));?></td></tr>
		</table>
		<?php 
		$rank++;
	}
}
?>