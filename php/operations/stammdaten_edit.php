<?php 
require_once("../host.inc");
if(isset($_GET['id'])){
	$query = "UPDATE stammdaten SET turnier = '".$_GET['turnier']."', durchgaenge = ".$_GET['durchgaenge'].", veranstalter = '".$_GET['veranstalter']."', veranstalter_web = '".$_GET['veranstalter_web']."', federation = '".$_GET['federation']."', jury = ".$_GET['jury'].", ort = '".$_GET['ort']."', number = '".$_GET['number']."', datum = '".$_GET['datum']."', end_datum = '".$_GET['end_datum']."', score_mode = ".$_GET['score_mode'].", finale = ".$_GET['final'].", final_durchgang = ".$_GET['final_durchgang'].", final_teilnehmer = ".$_GET['final_teilnehmer'].", org_leiter = ".$_GET['org_leiter'].", wettkampf_leiter = ".$_GET['wettkampf_leiter'].", end_finale = ".$_GET['end_final'].", end_final_durchgang = ".$_GET['end_final_durchgang'].", end_final_teilnehmer = ".$_GET['end_final_teilnehmer'].", airfields = ".$_GET['airfields'].", judge_pin = ".$_GET['judge_pin'].", three_panel_final = ".$_GET['three_panel_final'].", del_on = ".$_GET['del_on'].", edit = ".$_GET['edit'].", klasse = ".$_GET['klasse']." WHERE id = ".$_GET['id'];
	mysqli_query($link,$query);
	$query = "DELETE FROM durchgang_programm WHERE durchgang > ".$_GET['durchgaenge'];
	mysqli_query($link,$query);
	$query = "DELETE FROM durchgang_airfield WHERE durchgang > ".$_GET['durchgaenge'];
	mysqli_query($link,$query);
	$query = "DELETE FROM durchgang_panel WHERE durchgang > ".$_GET['durchgaenge'];
	mysqli_query($link,$query);
	$query = "SELECT max(durchgang) as max FROM durchgang_programm";
	$obj_max = mysqli_fetch_object(mysqli_query($link,$query));
	if($_GET['durchgaenge'] > $obj_max->max)for($i=($obj_max->max + 1);$i<=$_GET['durchgaenge'];$i++)mysqli_query($link,"INSERT INTO durchgang_programm (`durchgang`,`programm`) VALUES (".$i.",1)");
	$query = "SELECT max(durchgang) as max FROM durchgang_airfield";
	$obj_max = mysqli_fetch_object(mysqli_query($link,$query));
	if($_GET['durchgaenge'] > $obj_max->max)for($i=($obj_max->max + 1);$i<=$_GET['durchgaenge'];$i++)mysqli_query($link,"INSERT INTO durchgang_airfield (`durchgang`,`airfield`) VALUES (".$i.",1)");
	$query = "SELECT max(durchgang) as max FROM durchgang_panel";
	$obj_max = mysqli_fetch_object(mysqli_query($link,$query));
	if($_GET['durchgaenge'] > $obj_max->max)for($i=($obj_max->max + 1);$i<=$_GET['durchgaenge'];$i++)mysqli_query($link,"INSERT INTO durchgang_panel (`durchgang`,`panel`) VALUES (".$i.",1)");
	if($_GET['three_panel_final'] == 0 && $_GET['end_final_durchgang'] != ""){
		if(mysqli_fetch_object(mysqli_query($link,"SELECT count(dp.panel) as panels FROM durchgang_panel dp WHERE dp.durchgang = ".$_GET['end_final_durchgang']))->panels > 1){
			mysqli_query($link,"DELETE FROM durchgang_panel WHERE durchgang >= ".$_GET['end_final_durchgang']);
			for($i=$_GET['end_final_durchgang'];$i<=$_GET['durchgaenge'];$i++)mysqli_query($link,"INSERT INTO durchgang_panel (`durchgang`,`panel`) VALUES (".$i.",1)");
		}
	}
	$query = "UPDATE bewerb SET name = '".$_GET['turnier']."' WHERE id = 1";
	mysqli_query($link,$query);
	
}
$query = "SELECT * FROM stammdaten";
$res = mysqli_query($link,$query);
$obj = mysqli_fetch_object($res);
$durchgaenge = $obj->durchgaenge;
$res_panels = mysqli_fetch_object(mysqli_query($link,"SELECT max(panel) as panels FROM judge_panel"));
?>
<table class="liste">
<tr>
<th colspan="100%" class="header" style="font-size:20pt; background-color:lightblue;"><img src="../bilder/buttons/options.png" height="80" style="margin-right:20px;"/>Preferences</th></tr>
<tr>
<tr class="header">
<th>Title</th></th><th>Location</th><th>Host</th><th>Host-Web</th><th>Federation</th><th>Comp. Number</th><th>Date</th></tr>
<tr>
<td style="border-left:1px solid;"><input type="text" id="turnier<?php echo $obj->id;?>" value="<?php echo $obj->turnier;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input type="text" id="ort<?php echo $obj->id;?>" value="<?php echo $obj->ort;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input type="text" id="veranstalter<?php echo $obj->id;?>" value="<?php echo $obj->veranstalter;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input type="text" id="veranstalter_web<?php echo $obj->id;?>" value="<?php echo $obj->veranstalter_web;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input type="text" id="federation<?php echo $obj->id;?>" value="<?php echo $obj->federation;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input type="text" id="number<?php echo $obj->id;?>" value="<?php echo $obj->number;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input type="date" id="datum<?php echo $obj->id;?>" value="<?php echo $obj->datum;?>" onChange="speichern(<?php echo $obj->id;?>);" /><input type="date" id="end_datum<?php echo $obj->id;?>" value="<?php echo $obj->end_datum;?>" onchange="speichern(<?php echo $obj->id;?>);" /></td>
</tr>
<tr class="header">
<th>Class</th><th>Jury</th><th>Organisation</th><th>Contest Director</th><th>TBL | PIN | Edit | Del ON</th><th>Rounds</th><th>Airflieds</th></tr>
<tr>
<td><select id="klasse<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>)">
<?php
$res_offical = mysqli_query($link,"SELECT * FROM klassen ORDER BY name ASC");
while($official = mysqli_fetch_object($res_offical)){?>
	<option value="<?php echo $official->id;?>"<?php if($official->id == $obj->klasse) echo "selected='selected'";?> >
		<?php echo $official->name;?>
    </option>
    <?php
}?>
</select></td>
<td><select id="jury<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>)">
<?php
$res_offical = mysqli_query($link,"SELECT * FROM official ORDER BY nachname ASC, vorname ASC");
while($official = mysqli_fetch_object($res_offical)){?>
	<option value="<?php echo $official->id;?>"<?php if($official->id == $obj->jury) echo "selected='selected'";?> >
		<?php echo $official->nachname." ".$official->vorname;?>
    </option>
    <?php
}?>
</select></td>
<td><select id="org_leiter<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>)">
<?php
$res_offical = mysqli_query($link,"SELECT * FROM official ORDER BY nachname ASC, vorname ASC");
while($official = mysqli_fetch_object($res_offical)){?>
	<option value="<?php echo $official->id;?>"<?php if($official->id == $obj->org_leiter) echo "selected='selected'";?> >
		<?php echo $official->nachname." ".$official->vorname;?>
    </option>
    <?php
}?>
</select></td>
<td><select id="wettkampf_leiter<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>)">
<?php
$res_offical = mysqli_query($link,"SELECT * FROM official ORDER BY nachname ASC, vorname ASC");
while($official = mysqli_fetch_object($res_offical)){?>
	<option value="<?php echo $official->id;?>"<?php if($official->id == $obj->wettkampf_leiter) echo "selected='selected'";?> >
		<?php echo $official->nachname." ".$official->vorname;?>
    </option>
    <?php
}?>
</select></td>
<td>
	<input type="checkbox" id="score_mode<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>);"<?php if($obj->score_mode == 1) echo " checked='checked'";?>> |
  <input type="checkbox" id="judge_pin<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>);"<?php if($obj->judge_pin == 1) echo " checked='checked'";?>> |
  <input type="checkbox" id="edit<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>);"<?php if($obj->edit == 1) echo " checked='checked'";?>> |
  <input type="checkbox" id="del_on<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>);"<?php if($obj->del_on == 1) echo " checked='checked'";?>>
</td>
<td>
<input type="text" id="durchgaenge<?php echo $obj->id;?>" value="<?php echo $obj->durchgaenge;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input type="text" id="airfields<?php echo $obj->id;?>" value="<?php echo $obj->airfields;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
</tr>
<tr class="header">
<th>Semifinal</th><th>Semifinal - Round</th><th>Starters in Semifinal</th><th>Final</th><th>3 Panels</th><th>Final - Round</th><th>Starters in Final</th></tr>
<tr>
<td>
	<input type="checkbox" id="final<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>);" <?php if($obj->finale == 1)echo " checked='checked'";?>>
</td>
<td><input <?php if($obj->finale == 0) echo " readonly='readonly'";?> type="text" id="final_durchgang<?php echo $obj->id;?>" value="<?php echo $obj->final_durchgang;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input <?php if($obj->finale == 0) echo " readonly='readonly'";?> type="text" id="final_teilnehmer<?php echo $obj->id;?>" value="<?php echo $obj->final_teilnehmer;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input type="checkbox" id="end_final<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>);"<?php if($obj->end_finale == 1)echo " checked='checked'";?>></td>
<td><input type="checkbox" id="three_panel_final<?php echo $obj->id;?>" onChange="speichern(<?php echo $obj->id;?>);"<?php if($obj->three_panel_final == 1)echo " checked='checked'";?>></td>
<td><input <?php if($obj->end_finale == 0) echo " readonly='readonly'";?> type="text" id="end_final_durchgang<?php echo $obj->id;?>" value="<?php echo $obj->end_final_durchgang;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
<td><input <?php if($obj->end_finale == 0) echo " readonly='readonly'";?> type="text" id="end_final_teilnehmer<?php echo $obj->id;?>" value="<?php echo $obj->end_final_teilnehmer;?>" onChange="speichern(<?php echo $obj->id;?>);" /></td>
</tr>

<tr>
<?php 
for($i=1;$i<=$durchgaenge;$i++){
	echo "<th>Round ".$i."</th>";
}?>
</tr>
<tr>
<?php
for($i=1;$i<=$durchgaenge;$i++){
	$query_durchgang = "SELECT programm FROM durchgang_programm WHERE durchgang = ".$i;
	$res_durchgang = mysqli_query($link,$query_durchgang);
	$obj_durchgang = mysqli_fetch_object($res_durchgang);
	$query_programm = "SELECT * FROM programm ORDER BY title ASC";
	$res_programm = mysqli_query($link,$query_programm);
	echo "<td><select id='prog_round_".$i."' onchange='changeProgramm(".$i.");'>";
	while($obj_programm = mysqli_fetch_object($res_programm)){
		echo "<option value='".$obj_programm->id."'";
		if($obj_programm->id == $obj_durchgang->programm) echo " selected='selected'";
		echo ">".$obj_programm->title."</option>";
	}
	echo "</select></td>";
}
?>
</tr>
<tr>
<?php
for($i=1;$i<=$durchgaenge;$i++){
	$query_durchgang = "SELECT airfield FROM durchgang_airfield WHERE durchgang = ".$i;
	$res_durchgang = mysqli_query($link,$query_durchgang);
	$obj_durchgang = mysqli_fetch_object($res_durchgang);
	echo "<td><select id='airfield_round_".$i."' onchange='changeAirfield(".$i.");'>";
	for($a=1;$a<=$obj->airfields;$a++){
		echo "<option value='".$a."'";
		if($a == $obj_durchgang->airfield) echo " selected='selected'";
		echo ">Airfield ".$a."</option>";
	}
	echo "</select></td>";
}
?>
</tr>
<tr>
<?php
for($i=1;$i<=$durchgaenge;$i++){
	if($obj->three_panel_final == 1 && $i >= $end_final_durchgang){
		$res_durch = mysqli_query($link,"SELECT panel FROM durchgang_panel WHERE durchgang = ".$i);
		$lmr_panels = array();
		while($obj_durchgang = mysqli_fetch_object($res_durch)){
			array_push($lmr_panels,$obj_durchgang->panel);
		}
		echo "<td colspan='2'>";
		echo "Left - Right - Middle<br>";
		for($r=1;$r<=3;$r++){
			echo "<select id='panel_round_".$i."_".$r."' onchange='changePanel(".$i.",true);'>";
			$res_p = mysqli_query($link,"SELECT jp.panel, count(jp.judge) as judges FROM judge_panel jp GROUP BY panel ORDER BY panel ASC"); 
			while($obj_p = mysqli_fetch_object($res_p)){
				echo "<option value='".$obj_p->panel."'";
				if(isset($lmr_panels[$r-1]))if($obj_p->panel == $lmr_panels[$r - 1]) echo " selected='selected'";
				echo ">J-Panel ".$obj_p->panel."(".$obj_p->judges.")</option>";
			}
			echo "</select>";
		}
		$i++;
		echo "</td>";
	}
	else {
		$obj_durchgang = mysqli_fetch_object(mysqli_query($link,"SELECT panel FROM durchgang_panel  WHERE durchgang = ".$i));
		echo "<td><select id='panel_round_".$i."' onchange='changePanel(".$i.",false);'>";
		$res_p = mysqli_query($link,"SELECT jp.panel, count(jp.judge) as judges FROM judge_panel jp GROUP BY panel ORDER BY panel ASC"); 
		while($obj_p = mysqli_fetch_object($res_p)){
			echo "<option value='".$obj_p->panel."'";
			if($obj_p->panel == $obj_durchgang->panel) echo " selected='selected'";
			echo ">J-Panel ".$obj_p->panel." (".$obj_p->judges.")</option>";
		}
		echo "</select></td>";
	}
}
?>
</tr>
<tr><th colspan="100%"></th></tr>
<tr><td><input type="button" value="Close" onClick="window.close();"></td><td><input type="button" onclick="window.open('subevent.php','_blank');" value="Sub Event" /></td></tr>
</table>
