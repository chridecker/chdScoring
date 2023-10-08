<?php 
require_once("../host.inc");?>
<link rel="stylesheet" type="text/css" href="../css/print_urkunde.css" />
<?php
$durchgaenge = mysqli_fetch_object(mysqli_query($link,"SELECT max(durchgang) as durchgang FROM durchgang"))->durchgang;
if(isset($_GET['bewerb']))$bewerb = $_GET['bewerb'];
else $bewerb = 1;
//Bewerb
$query_bewerb = "SELECT name FROM bewerb WHERE id = ".$bewerb;
$res_bewerb = mysqli_fetch_object(mysqli_query($link,$query_bewerb));
if($bewerb > 1)$turnier = $res_bewerb->name;
for($durchgang = 1;$durchgang<=$durchgaenge;$durchgang++){
	$durchgang;
	if($durchgang == 1 )$query = "CREATE TEMPORARY TABLE bewerb".$bewerb." SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert_abs) / (SELECT max(d.wert_abs) FROM durchgang as d, teilnehmer as t, teilnehmer_bewerb as bw, bewerb as b WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb.") * 1000),2) as prom FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY t.id ASC;";
	else $query = "INSERT INTO bewerb".$bewerb." SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert_abs) / (SELECT max(d.wert_abs) FROM durchgang as d, teilnehmer as t, teilnehmer_bewerb as bw, bewerb as b WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb.") * 1000),2) as prom FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY t.id ASC;";
	$res = mysqli_query($link,$query);
}

//$res = mysqli_query($link,"SELECT teilnehmer, sum(prom) - min(prom) as total FROM bewerb".$bewerb." GROUP BY teilnehmer");
//while($teil = mysqli_fetch_object($res))echo $teil->teilnehmer." ".$teil->total."<br>";
//exit;

$res_teilnehmer_durchgaenge = mysqli_query($link,"SELECT d.teilnehmer, count(d.durchgang) as anzahl_durchgang FROM durchgang d GROUP BY d.teilnehmer ORDER BY anzahl_durchgang");
while($teiln = mysqli_fetch_object($res_teilnehmer_durchgaenge)){
	if($teiln->anzahl_durchgang >= $end_final_durchgang && $end_finale == 1){
		//End Finale Urkunden
		$decline = mysqli_fetch_object(mysqli_query($link,"SELECT min(prom) as decline FROM bewerb".$bewerb." WHERE (durchgang = ".$end_final_durchgang." OR durchgang = ".($end_final_durchgang + 2).") AND teilnehmer = ".$teiln->teilnehmer))->decline;
		$gesamt = mysqli_fetch_object(mysqli_query($link,"SELECT sum(prom) - ".$decline." as gesamt FROM bewerb".$bewerb." WHERE durchgang >= ".$end_final_durchgang." AND teilnehmer = ".$teiln->teilnehmer))->gesamt;
		mysqli_query($link,"INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge + 1).",".$teiln->teilnehmer.",".($gesamt * 100).")");
	}
	else if($teiln->anzahl_durchgang >= $final_durchgang && $finale == 1){
		//Semi Final Urkunden
		$max_prelim = mysqli_fetch_object(mysqli_query($link,"SELECT sum(prom) - min(prom) as gesamt FROM bewerb".$bewerb." WHERE durchgang < ".$final_durchgang." GROUP BY teilnehmer ORDER BY gesamt DESC LIMIT 1"))->gesamt;
		$prelim = mysqli_fetch_object(mysqli_query($link,"SELECT (sum(prom) - min(prom)) / ".$max_prelim." * 1000 as prelim FROM bewerb".$bewerb." WHERE durchgang < ".$final_durchgang." AND teilnehmer = ".$teiln->teilnehmer))->prelim;
		mysqli_query($link,"INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge + 1).",".$teiln->teilnehmer.",".round($prelim,2).")");
		$gesamt = mysqli_fetch_object(mysqli_query($link,"SELECT sum(prom) - min(prom) as gesamt FROM bewerb".$bewerb." WHERE durchgang >= ".$final_durchgang." AND teilnehmer = ".$teiln->teilnehmer))->gesamt;
		mysqli_query($link,"UPDATE bewerb".$bewerb." SET prom = ".$gesamt." WHERE durchgang = ".($durchgaenge + 1)." AND teilnehmer = ".$teiln->teilnehmer);
	}
	else {
		$max_prelim = mysqli_fetch_object(mysqli_query($link,"SELECT sum(prom) - min(prom) as gesamt FROM bewerb".$bewerb." WHERE durchgang < ".$final_durchgang." GROUP BY teilnehmer ORDER BY gesamt DESC LIMIT 1"))->gesamt;
		$prelim = mysqli_fetch_object(mysqli_query($link,"SELECT (sum(prom) - min(prom)) / ".$max_prelim." * 1000 as prelim FROM bewerb".$bewerb." WHERE durchgang < ".$final_durchgang." AND teilnehmer = ".$teiln->teilnehmer))->prelim;
		mysqli_query($link,"INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge + 1).",".$teiln->teilnehmer.",".$prelim.")");
	}
}
$query_teilnehmer = "SELECT t.*, b.prom as gesamt FROM bewerb".$bewerb." b JOIN teilnehmer t ON (t.id = b.teilnehmer) WHERE b.durchgang = ".($durchgaenge + 1)." ORDER BY prom DESC";
if($result_teilnehmer = mysqli_query($link,$query_teilnehmer)){
	$rank = 1;
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
		<tr class="ergebnis"><th colspan="3" class="name"><?php echo $_GET['name'];?></th></tr>
		<tr class="ergebnis"><th colspan="3" class="club">errang in der Klasse F3A</th></tr>
		<tr class="ergebnis"><th colspan="3" class="punkte">mit <?php echo $_GET['punkte']; ?>
        &permil; Punkten</th></tr>
		<tr class="ergebnis"><th colspan="3" class="platz"><i>den</i> <?php echo $_GET['platz'];?>. Platz</th></tr>
		
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
		<?php $rank++;
		break;
	}
}
