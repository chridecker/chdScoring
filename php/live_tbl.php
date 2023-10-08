<?php
require_once("host.inc");?>
<table>
<?php
//Teilnehmer Durchgangsansicht
if(isset($airfield));
else if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
$d = mysqli_fetch_object(mysqli_query($link,"SELECT min(da.durchgang) as durchgang FROM durchgang_airfield da JOIN wettkampf_leitung wl ON (wl.durchgang = da.durchgang) WHERE da.airfield = ".$airfield." AND wl.status <= 1"));
if($round = $d->durchgang){
	$count_judges = mysqli_fetch_object(mysqli_query($link,"SELECT count(j.id) as judges FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.airfield = ".$airfield." AND da.durchgang = ".$round));
	$judges = $count_judges->judges;
	$wert_durchgang = 0;
	$query_judge = "SELECT t.id, w.durchgang FROM teilnehmer as t, wettkampf_leitung as w WHERE t.id = w.teilnehmer AND w.status = 1 AND w.durchgang =".$round;
	$res_programm = mysqli_fetch_object(mysqli_query($link,"SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".$round));
	$teilnehmer_load = false;
	if($result_judge = mysqli_fetch_object(mysqli_query($link,$query_judge))){
		$durchgang = $round;
		$teilnehmer_load = true;
		$teilnehmer = $result_judge->id;
		$query_teilnehmer = "SELECT * FROM teilnehmer WHERE id = ".$teilnehmer;
		$result_teilnehmer = mysqli_fetch_object(mysqli_query($link,$query_teilnehmer));
		if($durchgang >= $end_final_durchgang && $result_config->three_panel_final) $col = 5;
		else $col = 2;
		?>
		<tr>
		<td style="text-align:left;" colspan="<?php echo $col;?>">
		<?php
		if($_SERVER['PHP_SELF'] != "/wettkampf.php"){?>
			<img src="../operations/load_image.php?id=<?php echo $result_teilnehmer->bild;?>" class="profilbild"><?php 
		}?>
		<?php
		if($_SERVER['PHP_SELF'] != "/wettkampf.php"){?>
			<img class="flagge" src="../operations/load_image_country.php?id=<?php echo $result_teilnehmer->land;?>"><?php 
		}?>
		</td>
		<th colspan="<?php echo $judges;?>" style="text-align:center;" class="teilnehmer">
		<?php echo "&#8470; ".$result_teilnehmer->id." - ".$result_teilnehmer->vorname." ".$result_teilnehmer->nachname;?>
		<?php
		if($_SERVER['PHP_SELF'] != "/wettkampf.php"){
		$timer_edit = false;
		include("output/timer_calc.php");
		echo "<br><i style='font-size:40pt;'>Round ".$durchgang." (".$res_programm->title.")</i>";
		}?>
		</th>
	</tr>
	
		<tr class="header">
		<th>Manoeuvre</th><th>K</th>
		<?php 
		$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.airfield = ".$airfield." AND da.durchgang = ".$durchgang);
		$link = mysqli_connect("localhost",$user,$password,$database);
		$res_judges = mysqli_query($link,"SELECT j.* FROM judge j");
		while($obj_judges = mysqli_fetch_object($res_judges)){?>
			<th class="judges">Judge <?php echo $obj_judges->id;?></th>
			<?php
		}?>
		</tr>
		
		<?php
		$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$durchgang;
		$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
		for($figur=$obj_count_figur->anfang;$figur<=$obj_count_figur->ende;$figur++){
			if(($figur % 2) == 0 )echo "<tr class='gerade'>";
			else echo "<tr>";
			$query_figur = "SELECT f.name as figur, f.wert as k FROM figur as f WHERE f.id = ".$figur;
			$result_figur = mysqli_fetch_object(mysqli_query($link,$query_figur));
			echo "<td class='figur'";
			if($durchgang >= $end_final_durchgang && $result_config->three_panel_final) echo " style='max-width:400px;'";
			echo ">".($figur-$obj_count_figur->anfang+1)."-".$result_figur->figur."</td><td align='center'>".$result_figur->k."</td>";
			$wertungen = array();
			$count = 0;
			$no = 0;
			$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.airfield = ".$airfield." AND da.durchgang = ".$durchgang);
			while($obj_judges = mysqli_fetch_object($res_judges)){
			$judge = $obj_judges->id;
				$query_wert = "SELECT w.wert as wert FROM wertung as w, figur as f WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = ".($figur - $obj_count_figur->anfang + 1)." AND w.judge = ".$judge." AND w.figur = f.id + 1 - ".$obj_count_figur->anfang;
				if($result_wert = mysqli_fetch_object(mysqli_query($link,$query_wert))){
					$wert = abs($result_wert->wert);
					echo "<td align='center'>";
					$count++;
					if($result_wert->wert == -1){
						$query_norm = "SELECT count(w.wert) as count FROM wertung as w WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.figur = ".($figur - $obj_count_figur->anfang + 1)."";
						$res_norm = mysqli_fetch_object(mysqli_query($link,$query_norm));
						if($res_norm->count == $judges){
							$query_norm = "SELECT ROUND(abs(avg(wert)))as avg FROM wertung WHERE durchgang = ".$durchgang." AND teilnehmer = ".$teilnehmer." AND figur = ".($figur - $obj_count_figur->anfang + 1)." AND wert != -1";
							 if($durchgang >= $end_final_durchgang && $result_config->three_panel_final == 1)$query_norm .= " AND wert != 0";
							$res_norm = mysqli_fetch_object(mysqli_query($link,$query_norm));
							mysqli_query($link,"UPDATE wertung SET wert = ".($res_norm->avg*-1)." WHERE durchgang = ".$durchgang." AND teilnehmer = ".$teilnehmer." AND figur = ".($figur - $obj_count_figur->anfang + 1)." AND judge = ".$judge);
						}
						echo "N/O";
					}
					elseif($result_wert->wert < 0){
						echo number_format($wert*$result_figur->k)."(".$wert."*)";
					}
					elseif($result_wert->wert == 0 && $durchgang >= $end_final_durchgang && $result_config->three_panel_final == 1){
						echo " ";
					}
					else {
						if($wert < 1 && $wert >=0){
							echo "<i style='background-color: red;'>";
						}
						echo number_format($wert*$result_figur->k)."(".$wert.")";
						if($wert <= 0.5 && $wert >=0){
							echo "</i>";
						}
					}
				}
				else echo "<td align='center'>&nbsp;";
				echo "</td>";
			}
			?></tr><?php
			$min_count = 0;
			$max_count = 0;
		}
		if($durchgang >= $end_final_durchgang && $result_config->three_panel_final == 1){
			$panel_wert = array();
			$res_panels = mysqli_query($link,"SELECT jp.panel, count(jp.judge) as judges FROM judge_panel jp JOIN durchgang_panel dp ON(dp.panel = jp.panel) WHERE dp.durchgang = ".$durchgang." GROUP BY jp.panel ORDER BY panel ASC");
			while($obj_panels = mysqli_fetch_object($res_panels)){
				$res_j = mysqli_query($link,"SELECT judge FROM judge_panel WHERE panel = ".$obj_panels->panel);
				$wert_panel = 0;
				while($obj_j = mysqli_fetch_object($res_j)){
					for($figur=$obj_count_figur->anfang;$figur<=$obj_count_figur->ende;$figur++){
						$query_f = "SELECT (f.wert * ABS(w.wert)) as wertung FROM wertung as w, figur as f WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.judge = ".$obj_j->judge." AND f.id = ".$figur." AND w.figur = f.id + 1 - ".($obj_count_figur->anfang);
						if($obj_f = mysqli_fetch_object(mysqli_query($link,$query_f)))$wert_panel += $obj_f->wertung;
						
					}
				}
				array_push($panel_wert,($wert_panel / $obj_panels->judges));
			}
			echo "<tr><th colspan='2'></th>";
			$wert_durchgang = 0;
			for($i=0;$i<count($panel_wert);$i++){
				echo "<th colspan='6' class='gesamt' style='text-align:center; padding:5px; border-top:1px solid;'>".$panel_wert[$i]."</th>";
				$wert_durchgang += $panel_wert[$i];
			}
			echo "</tr>";
		}
		else {
			echo "<tr><td colspan='2'style='border-top:1px solid;'></td>";
			$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.airfield = ".$airfield." AND da.durchgang = ".$durchgang);
			while($obj_judges = mysqli_fetch_object($res_judges)){
				$i = $obj_judges->id;
				$endwert = 0;
				$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$durchgang;
				$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
				for($figur=$obj_count_figur->anfang;$figur<=$obj_count_figur->ende;$figur++){
					$query = "SELECT (f.wert * ABS(w.wert)) as wertung FROM wertung as w, figur as f WHERE w.teilnehmer = ".$teilnehmer." AND w.durchgang = ".$durchgang." AND w.judge = ".$i." AND f.id = ".$figur." AND w.figur = f.id + 1 - ".($obj_count_figur->anfang);
					if($res = mysqli_fetch_object(mysqli_query($link,$query)))$endwert += $res->wertung;
				}
				echo "<th class='gesamt' style='text-align: center; border-top:1px solid;'>".$endwert."</th>";
				$wert_durchgang += $endwert;
			}
			echo "</tr>";
			$wert_durchgang = $wert_durchgang / $judges;
		}
		echo "<tr class='total'><th colspan='".($judges+2)."' style='text-align:right;'>Total: ".number_format($wert_durchgang,2)."</th></tr>";
		?>
		</table>
		<?php
	}
	elseif($_SERVER['PHP_SELF']=="/live_tbl.php") include("output/info.php");
	else {
		$file = "config.xml";
		include("output/durchgangswertung.php");
	}
}
elseif (isset($durchgang)){
	$file = "config.xml";
	include("output/durchgangswertung.php");
	$teilnehmer_load = false;
}
elseif($_SERVER['PHP_SELF']=="/live_tbl.php") include("output/info.php");
//mysqli_free_result($res_judges);
//mysqli_close($link);

?>
