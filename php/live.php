<?php
require_once("host.inc");?>
<table>
<?php
//Teilnehmer Durchgangsansicht
if(isset($airfield))$airfield = $airfield;
else if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
$d = mysqli_fetch_object(mysqli_query($link,"SELECT min(da.durchgang) as durchgang FROM durchgang_airfield da JOIN wettkampf_leitung wl ON (wl.durchgang = da.durchgang) WHERE da.airfield = ".$airfield." AND wl.status <= 1"));
$durchgang = $d->durchgang;
$count_judges = mysqli_fetch_object(mysqli_query($link,"SELECT count(j.id) as judges FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.airfield = ".$airfield." AND da.durchgang = ".$durchgang));
$judges = $count_judges->judges;
$wert_durchgang = 0;
$query_judge = "SELECT t.id, w.durchgang FROM teilnehmer as t, wettkampf_leitung as w WHERE t.id = w.teilnehmer AND w.status = 1 AND w.durchgang =".$durchgang;
$res_programm = mysqli_fetch_object(mysqli_query($link,"SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".$durchgang));
$teilnehmer_load = false;
if($result_judge = mysqli_fetch_object(mysqli_query($link,$query_judge))){
	$teilnehmer_load = true;
	$teilnehmer = $result_judge->id;
	$durchgang = $result_judge->durchgang;
	$query_teilnehmer = "SELECT * FROM teilnehmer WHERE id = ".$teilnehmer;
	$result_teilnehmer = mysqli_fetch_object(mysqli_query($link,$query_teilnehmer));?>
    <tr>
    <td style="text-align:left;">
    <?php
	if($_SERVER['PHP_SELF'] != "/wettkampf.php"){?>
    	<img style="height:150px; margin-right:70px;" src="../operations/load_image.php?id=2"><?php 
	}?>
    <?php
	if($_SERVER['PHP_SELF'] != "/wettkampf.php"){?>
    	<img src="../operations/load_image.php?id=<?php echo $result_teilnehmer->bild;?>" class="profilbild"><?php 
	}?>
    <?php
	if($_SERVER['PHP_SELF'] != "/wettkampf.php"){?>
			<img style="height:120px; margin-left:70px;" src="../operations/load_image_country.php?id=<?php echo $result_teilnehmer->land;?>" class="profilbild"><?php 
	}?>
    </td>
    <th colspan="6" style="text-align:center;">
    <?php echo $result_teilnehmer->id." - ".$result_teilnehmer->vorname." ".$result_teilnehmer->nachname;?>
	<?php
	if($_SERVER['PHP_SELF'] != "/wettkampf.php"){
	$timer_edit = false;
	include("output/timer_calc.php");
    echo "<br><i style='font-size:25pt;'>Round ".$durchgang." (".$res_programm->title.")</i>";
	}?>
    </th>
</tr>

   
	<tr class="header">
	<th>Manoeuvre</th><th>K</th>
    <?php 
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.airfield = ".$airfield." AND da.durchgang = ".$durchgang);
	while($obj_judges = mysqli_fetch_object($res_judges)){?>
    	<th>Judge <?php echo $obj_judges->id;?></th>
        <?php
	}?>

    <th>Gesamt</th></tr>
    
	<?php
	$max_count = 0;
	$min_count = 0;
	$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$durchgang;
	$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
	for($figur=$obj_count_figur->anfang;$figur<=$obj_count_figur->ende;$figur++){
		if(($figur % 2) == 0 )echo "<tr class='gerade'>";
		else echo "<tr>";
		$query_figur = "SELECT f.name as figur, f.wert as k FROM figur as f WHERE f.id = ".$figur;
		$result_figur = mysqli_fetch_object(mysqli_query($link,$query_figur));
		echo "<td class='figur'>".$result_figur->figur."</td><td align='center'>".$result_figur->k."</td>";
		$wertungen = array();
		$count = 0;
		$no = 0;
		$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.airfield = ".$airfield." AND da.durchgang = ".$durchgang);
		while($obj_judges = mysqli_fetch_object($res_judges)){
			$judge = $obj_judges->id;
			$query_min_max = "SELECT min(abs(wert)) as min, max(abs(wert)) as max FROM wertung WHERE figur = ".($figur - $obj_count_figur->anfang +1)." AND durchgang = ".$durchgang." AND teilnehmer = ".$teilnehmer;
			$min_max = mysqli_fetch_object(mysqli_query($link,$query_min_max));
			$min = $min_max->min;
			$max = $min_max->max;
			$query_wert = "SELECT w.wert as wert FROM wertung as w, figur as f WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = ".($figur - $obj_count_figur->anfang + 1)." AND w.judge = ".$judge." AND w.figur = f.id + 1 - ".$obj_count_figur->anfang;
			if($result_wert = mysqli_fetch_object(mysqli_query($link,$query_wert))){
				$wert = abs($result_wert->wert);
				if($wert == $min && $min_count < 1){
					echo "<td align='center' style='text-decoration:line-through;'>";
					$min_count++;
				}
				elseif($wert == $max && $max_count < 1){
					echo "<td align='center' style='text-decoration:line-through;'>";
					$max_count++;
				}
				else echo "<td align='center'>";
				$count++;
				if($result_wert->wert == -1){
					$query_norm = "SELECT count(w.wert) as count FROM wertung as w WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = ".($figur - $obj_count_figur->anfang + 1)."";
					$res_norm = mysqli_fetch_object(mysqli_query($link,$query_norm));
					if($res_norm->count == $judges){
						$query_norm = "SELECT ROUND(avg(wert))as avg FROM wertung WHERE durchgang = ".$durchgang." AND teilnehmer = ".$teilnehmer." AND figur = ".($figur - $obj_count_figur->anfang + 1)." AND wert != -1";
						$res_norm = mysqli_fetch_object(mysqli_query($link,$query_norm));
						mysqli_query($link,"UPDATE wertung SET wert = ".($res_norm->avg*-1)." WHERE durchgang = ".$durchgang." AND teilnehmer = ".$teilnehmer." AND figur = ".($figur - $obj_count_figur->anfang + 1)." AND judge = ".$judge);
					}
					echo "N/O";
				}
				elseif($result_wert->wert < 0){
					echo $wert."*";
				}
				else {
					echo $wert;
				}
			}
			else echo "<td align='center'>&nbsp;";
			echo "</td>";
		}
		echo "<td align='center'>";
		if($count == $judges){
			$query_figurwert = "SELECT (sum(abs(w.wert))-min(abs(w.wert))-max(abs(w.wert)))*f.wert as erg FROM figur as f, wertung as w WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = f.id + 1 - ".$obj_count_figur->anfang." AND f.id = ".$figur;
			$res_figurwert = mysqli_fetch_object(mysqli_query($link,$query_figurwert));
			$wert_durchgang += $res_figurwert->erg;
			echo $res_figurwert->erg;
		}
		else echo "0";
		echo "</td>";
		?></tr><?php
		$min_count = 0;
		$max_count = 0;
	}
	echo "<tr class='gesamt'><th colspan='".($judges+3)."' align='right'>Total: ".$wert_durchgang."</th></tr>";
	?>
	</table>
    <?php
}
elseif($_SERVER['PHP_SELF']=="/live.php") include("output/info.php");
else {
	$file = "config.xml";
	include("output/durchgangswertung.php");
}