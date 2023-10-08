<?php
require_once("../host.inc"); // Systemanbindung
// Airfield
if(isset($airfield));
else if(isset($_GET['airfield']))$airfield = $_GET['airfield']; // Define current Airfield
else $airfield = 1;
// Load current Round
if($res_durchgang = mysqli_fetch_object(mysqli_query($link,"SELECT wl.durchgang as durchgang FROM wettkampf_leitung wl JOIN durchgang_airfield da ON (da.durchgang = wl.durchgang) WHERE da.airfield = ".$airfield." AND wl.status = 1"))){
	$durchgang = $res_durchgang->durchgang;
	$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$durchgang;
	$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
	$query = "SELECT f.id, x.teilnehmer as teilnehmer FROM figur f JOIN (SELECT t.id as teilnehmer, wl.durchgang, w.figur, count(w.judge) as anzahl FROM teilnehmer as t JOIN wettkampf_leitung wl ON (wl.teilnehmer = t.id) JOIN wertung w ON (w.teilnehmer = t.id AND wl.durchgang = w.durchgang) WHERE wl.status = 1 AND wl.durchgang = ".$durchgang;
	if($durchgang >= $end_final_durchgang && $result_config->three_panel_final == 1) $query .= " AND w.wert <> 0";
	$query .= " GROUP by w.figur) as x ON(x.figur = f.id - ".$obj_count_figur->anfang." + 1) WHERE x.anzahl >= 1 ORDER BY f.id DESC LIMIT 1";
	$res = mysqli_query($link,$query);
	if($obj = mysqli_fetch_object($res)){
		$figur = $obj->id;
		$teilnehmer = $obj->teilnehmer;
		}
	else {
		$figur = $obj_count_figur->anfang - 1;
		$res_teilnehmer = mysqli_fetch_object(mysqli_query($link,"SELECT teilnehmer FROM wettkampf_leitung WHERE status = 1"));
		$teilnehmer = $res_teilnehmer->teilnehmer;
	}
	$query = "SELECT * FROM teilnehmer t WHERE id  = ".$teilnehmer;
	$res_teilnehmer = mysqli_fetch_object(mysqli_query($link,$query));
	if(($figur - $obj_count_figur->anfang + 1) == $obj_count_figur->anzahl)$query = "SELECT * FROM figur f JOIN figur_programm fp  ON (f.id = fp.figur) JOIN durchgang_programm dp ON (dp.programm = fp.programm) JOIN programm p ON (fp.programm = p.id) WHERE f.id  = ".($figur);
	else $query = "SELECT * FROM figur f JOIN figur_programm fp  ON (f.id = fp.figur) JOIN durchgang_programm dp ON (dp.programm = fp.programm) JOIN programm p ON (fp.programm = p.id) WHERE f.id  = ".($figur + 1);
	$res_figur = mysqli_fetch_object(mysqli_query($link,$query));
	$query = "SELECT * FROM figur f JOIN figur_programm fp  ON (f.id = fp.figur) JOIN durchgang_programm dp ON (dp.programm = fp.programm) JOIN programm p ON (fp.programm = p.id) WHERE f.id  = ".$figur;
	$res_figur_prev = mysqli_fetch_object(mysqli_query($link,$query));?>
	<table>
    <tr class="aresti">
    <td colspan="<?php echo ceil($judges/2);?>"><img src="../operations/load_image_figur.php?figur=<?php echo $res_figur->figur;?>" /></td>
    <th colspan="<?php echo floor($judges/2);?>"><?php echo $res_figur->name;?></th>
    </tr>
    <tr>
    <th class="teilnehmer" >
    <img src="../operations/load_image.php?id=<?php echo $res_teilnehmer->bild;?>" />
    </th>
    <th class="teilnehmer" colspan="<?php echo ($judges - 2);?>" style="background: rgba(173,216,230,1) #url(../operations/load_image_country.php?id=<?php echo $res_teilnehmer->land;?>) no-repeat; background-position:bottom; background-size:200px;" >
	<?php 
	if(strlen($res_teilnehmer->vorname." ".$res_teilnehmer->nachname)> 20) echo substr($res_teilnehmer->vorname,0,1).". ".$res_teilnehmer->nachname;
	else echo $res_teilnehmer->vorname." ".$res_teilnehmer->nachname;
	?></th>
    <th class="timer">
	<?php $timer_edit = false; include("../output/timer_calc.php");?>
    <?php echo "<br><i style='font-size:40pt;'>Round ".$durchgang." (".$res_figur->title.")</i>";?> 
    </th>
    </tr>
    
    <?php
	//$link = mysqli_connect("localhost","root","","cs");
	$res_judges = mysqli_fetch_object(mysqli_query($link,"SELECT count(j.id) as judges FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.airfield = ".$airfield." AND da.durchgang = ".$durchgang));
	$judges = $res_judges->judges;?>
   	<tr class="figur"><th colspan="<?php echo $judges;?>">
    <?php
	if(($res_figur->figur - $obj_count_figur->anfang + 1) != 1)echo "&#8470; ".($res_figur_prev->figur - $obj_count_figur->anfang + 1)." - ".$res_figur_prev->name." [K=".$res_figur_prev->wert."]";
	else echo "<br>"; ?>
    </th></tr>
	<tr class="wertungen">
    <td colspan="<?php echo $judges;?>">
	<?php
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.airfield = ".$airfield." AND da.durchgang = ".$durchgang);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$i = $obj_judges->id;
		echo "<div class='wertung'";
		if(($figur - $obj_count_figur->anfang + 1) % 2 != 0 && $result_config->three_panel_final == 1)echo " style='margin-left:6%;'";
		else if($judges >= 10 && $result_config->three_panel_final != 1)echo " style='margin-left:6%;'";
		echo ">";
		$query = "SELECT w.wert FROM wertung w JOIN judge j ON (w.judge = j.id) WHERE j.id = ".$i." AND w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = ".($figur - $obj_count_figur->anfang + 1);
		if($obj = mysqli_fetch_object(mysqli_query($link,$query))){
			if($obj->wert == -1)echo "no";
			else if($durchgang == $end_final_durchgang && $result_config->three_panel_final == 1 && $obj->wert == 0);
			else echo abs($obj->wert);
			if($obj->wert < -1) echo "*";
		}
		else echo "&nbsp;";
		echo "</div>";
	}?>
    </td>
    </tr>

    </table>
    <?php
}
else include("info.php");
?>
