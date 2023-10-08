<?php
require_once("../host.inc");
?>
<table class="korrektur">
<tr class="name">
<?php
$teilnehmer = $_GET['teilnehmer'];
$durchgang = $_GET['durchgang'];
$query_teilnehmer = "SELECT * FROM teilnehmer WHERE id = ".$teilnehmer;
$result_teilnehmer = mysqli_fetch_object(mysqli_query($link,$query_teilnehmer));
$count_judges = mysqli_fetch_object(mysqli_query($link,"SELECT count(j.id) as judges FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang));
$judges = $count_judges->judges;
?>
<th colspan='<?php echo $judges+2;?>'><?php
echo "#".$result_teilnehmer->id." ".$result_teilnehmer->nachname." ".$result_teilnehmer->vorname;	
echo " - ".$result_teilnehmer->club;
echo " / ".$result_teilnehmer->land;
?></th>
</tr>
<tr class="header">
<th>Manoeuvre (K)</th>
<?php
$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang);
while($obj_judges = mysqli_fetch_object($res_judges)){?>
	<th>Judge <?php echo $obj_judges->id;?></th>
    <?php
}?>
<th>Total</th></tr>
<?php
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$_GET['durchgang'];
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
for($figur=$obj_count_figur->anfang;$figur<=$obj_count_figur->ende;$figur++){
	?>
    <tr <?php if($figur % 2 == 0) echo " class='gerade'";?>><?php
	$query_figur = "SELECT f.name as figur, f.wert as k, f.id FROM figur as f WHERE f.id = ".$figur;
	$result_figur = mysqli_fetch_object(mysqli_query($link,$query_figur));
	echo "<td width='300px'>".($figur-$obj_count_figur->ende+$obj_count_figur->anzahl)." ".$result_figur->figur." (".$result_figur->k.")</td>";
	$wertungen = array();
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$judge = $obj_judges->id;
		echo "<td align='center'>";
		$query_wert = "SELECT abs(w.wert) as wert FROM wertung as w, figur as f WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND f.id = ".$figur." AND w.judge = ".$judge." AND w.figur = f.id + 1 - ".$obj_count_figur->anfang;
		if($result_wert = mysqli_fetch_object(mysqli_query($link,$query_wert))){
			$wertungen[] = $result_wert->wert;
			$name = $teilnehmer."_".$durchgang."_".($figur - $obj_count_figur->anfang +1)."_".$judge;?>
			<input type="text" class="wertung" id="<?php echo $name;?>" value="<?php echo $result_wert->wert;?>" onchange="speichern(<?php echo $teilnehmer;?>,<?php echo $durchgang;?>,<?php echo ($figur - $obj_count_figur->anfang +1);?>,<?php echo $judge;?>,'<?php echo $name;?>');" />
            <?php
		}
		else echo "&nbsp;";
		echo "</td>";
	}
	$query_figurwert = "SELECT (sum(abs(w.wert))-min(abs(w.wert))-max(abs(w.wert)))*f.wert as erg FROM figur as f, wertung as w WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = f.id + 1 - ".$obj_count_figur->anfang." AND f.id = ".$figur;
	$res_figurwert = mysqli_fetch_object(mysqli_query($link,$query_figurwert));
	echo "<td>".$res_figurwert->erg."</td>";
	?>
	</tr><?php
}
$query_druchgangswert = "SELECT wert_abs FROM durchgang WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang;
$res_durchgang = mysqli_fetch_object(mysqli_query($link,$query_druchgangswert));
?>
<tr style="border-top:1px solid;">
<th colspan="<?php echo $judges+1;?>" align="left"><input type="button" value="Close" onclick="window.close();" /></th>
<th><?php echo number_format($res_durchgang->wert_abs,2);?></th></tr>
</tr>