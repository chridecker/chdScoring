<?php
require_once("../host.inc");
//TBL - Statistik
if(isset($_GET["db"])){$link = mysqli_connect($host,$user,$password,$_GET["db"]);}
if(!isset($durchgang))$durchgang = $_GET['durchgang'];
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$durchgang;
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
$res_panels = mysqli_query($link,"SELECT jp.panel, count(jp.judge) as judges FROM durchgang_panel dp JOIN judge_panel jp ON (jp.panel = dp.panel) WHERE durchgang = ".$durchgang." GROUP BY dp.panel ORDER BY judges ASC, dp.panel ASC");
$panel = 1;
while($panels = mysqli_fetch_object($res_panels)){
	mysqli_query($link,"CREATE TEMPORARY TABLE statistik (judge int, teilnehmer int, wertung double)");
	$res_teilnehmer = mysqli_query($link,"SELECT DISTINCT(teilnehmer) FROM durchgang WHERE durchgang = ".$durchgang);
	$judges = $panels->judges;
	$teilnehmer_anzahl_real = 0;
	while($obj_teilnehmer = mysqli_fetch_object($res_teilnehmer)){
		$teilnehmer_anzahl_real++;
		$teilnehmer = $obj_teilnehmer->teilnehmer;
		$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
		while($obj_judges = mysqli_fetch_object($res_judges)){
			$judge = $obj_judges->id;
			$query = "SELECT sum(abs(w.wert) * f.wert) as wertung FROM wertung as w, figur as f WHERE w.figur = f.id + 1 - ".$obj_count_figur->anfang." AND w.judge = ".$judge." and w.durchgang = ".$durchgang." AND w.teilnehmer = ".$teilnehmer;
			$res = mysqli_query($link,$query);
			while($obj = mysqli_fetch_object($res)){
				mysqli_query($link,"INSERT INTO statistik (`judge`,`teilnehmer`,`wertung`) VALUES (".$judge.",".$teilnehmer.",".$obj->wertung.")");
			}
		}
	}

	$res_all_teilnehmer = mysqli_query($link,"SELECT id FROM teilnehmer ORDER BY id ASC");
	//for($t=1;$t<=$teilnehmer_anzahl;$t++){
	while($obj_all_teilnehmer = mysqli_fetch_object($res_all_teilnehmer)){
		$t=$obj_all_teilnehmer->id;
		$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
		while($obj_judges = mysqli_fetch_object($res_judges)){
			$j = $obj_judges->id;
			$tbl_log[$t][$j]=0;
		}
	}
	$res = mysqli_fetch_object(mysqli_query($link,"SELECT avg(wertung) as avg, STDDEV_SAMP(wertung) as std FROM statistik"));
	$avg = $res->avg;
	$std = $res->std;
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$judge = $obj_judges->id;
		$res = mysqli_query($link,"SELECT avg(wertung) as avg, STDDEV_SAMP(wertung) as std FROM statistik WHERE judge = ".$judge);
			while($obj = mysqli_fetch_object($res)){
				mysqli_query($link,"UPDATE statistik SET wertung = wertung - ".($obj->avg - $avg)." WHERE judge = ".$judge);
				mysqli_query($link,"UPDATE statistik SET wertung = wertung - (wertung - ".$avg." - (wertung - ".$avg.") * ".($std/$obj->std).") WHERE judge = ".$judge);
			}
	}
	
	$res_teilnehmer = mysqli_query($link,"SELECT DISTINCT(teilnehmer) FROM statistik");
	while($obj_teilnehmer = mysqli_fetch_object($res_teilnehmer)){
		$teilnehmer = $obj_teilnehmer->teilnehmer;
		$wert = mysqli_fetch_object(mysqli_query($link,"SELECT AVG(wertung) as avg, STDDEV_SAMP(wertung) as std FROM statistik WHERE teilnehmer = ".$teilnehmer));
		$avg = $wert->avg;
		$std = $wert->std;
		$new_avg = 0;
		$new_std= 0;
		$tbl_count = -1;
		while($avg != $new_avg && $std != $new_std){
			if($new_avg != 0 && $new_std != 0){
				$avg = $new_avg;
				$std = $new_std;
			}
			$low = $avg - 1.645 * $std;
			$high = $avg + 1.645 * $std;
			$intervall = mysqli_fetch_object(mysqli_query($link,"SELECT AVG(wertung) as avg, STDDEV_SAMP(wertung) as std FROM statistik WHERE teilnehmer = ".$teilnehmer." AND wertung <= ".$high." AND wertung >= ".$low));
			$new_avg = $intervall->avg;
			$new_std = $intervall->std;
			$tbl_count++;
		}
		if($tbl_count > 0){
			$query_tbl = "SELECT judge FROM statistik WHERE teilnehmer = ".$teilnehmer." AND (wertung < ".$low." OR wertung > ".$high.")";
			$res_tbl = mysqli_query($link,$query_tbl);
			while($obj_tbl = mysqli_fetch_object($res_tbl))$tbl_log[$teilnehmer][$obj_tbl->judge] = 1;
		}
	}
	
	//XML Config File
	$file = "../config.xml";
	$xml = simplexml_load_file($file);
	$system_name = $xml->information->name;
	$version = $xml->information->version;
	$year = $xml->information->year;
	?>
	<table class="judge_stats">
	<tr class="headline">
	<th colspan="<?php echo $judges*2 + 3;?>" style="text-align:left; vertical-align:bottom;">
	<img src="../operations/load_image.php?id=2" style="height:1.3cm;" />
	<?php echo $turnier.": Judge Statistic Round ".$durchgang;?></th>
	</tr>
	<tr class="header">
	<th colspan="3"><?php echo $turnier_no;?></th>
	<?php
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$i = $obj_judges->id;?>
		<th colspan="2" style="text-align:center;">Judge <?php echo $i;?></th>
		<?php
	}?>
	</tr>
	<tr class="header_small">
	<th> # </th><th>Participant</th><th>AVG</th>
	<?php 
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$judge = $obj_judges->id;?>
		<th>Score</th><th>Dev</th>
		<?php
	}?>
	</tr>
	<?php
	$count = 0;
	$limit = 21;
	$gesamt = ceil($teilnehmer_anzahl_real / $limit);
	$query_teilnehmer  = "SELECT t.* FROM teilnehmer t JOIN durchgang d ON (d.teilnehmer = t.id) AND d.durchgang = ".$durchgang." ORDER BY d.wert_abs DESC";
	$res_teilnehmer = mysqli_query($link,$query_teilnehmer);
	while($teilnehmer = mysqli_fetch_object($res_teilnehmer)){
		$count++;
		$res_wert = mysqli_query($link,"SELECT wert_abs as wertung FROM durchgang WHERE teilnehmer = ".$teilnehmer->id." AND durchgang = ".$durchgang);
		$wert = mysqli_fetch_object($res_wert);
		?>
		<tr class="content" <?php if($count % 2 == 0)echo " style='background-color:lightgrey;'";?>>
		<td class="figur" style="text-align:center;"><?php echo $teilnehmer->id;?></td><td class="figur"><?php echo $teilnehmer->vorname." ".$teilnehmer->nachname;?></td>
		<td class="judge"><?php echo number_format($wert->wertung,2);?></td>
		<?php
		$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
		while($obj_judges = mysqli_fetch_object($res_judges)){
			$i = $obj_judges->id;
			$judge_wert = 0;    	
			$res_judge_wertung = mysqli_query($link,"SELECT wertung FROM statistik WHERE teilnehmer = ".$teilnehmer->id." AND judge = ".$i);
			$obj_judge_wertung = mysqli_fetch_object($res_judge_wertung);
			?>
			<td class="judge" <?php if($tbl_log[$teilnehmer->id][$i] == 1 && $score_mode == 1) echo "style='background-color:grey; text-decoration:line-through;'";?>><?php echo number_format($obj_judge_wertung->wertung,0);?></td>
			<td class="judge"><?php echo number_format(- $wert->wertung + $obj_judge_wertung->wertung,2);?></td>
			<?php
		}?>
		</tr>
		<?php
		if($count % $limit == 0){?>
			<tr class="total"><td colspan="<?php echo $judges*2 + 1;?>"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
			<td colspan="2">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td>
			</tr>
			</table>
			<table class="judge_stats">
			<tr class="headline">
			<th colspan="<?php echo $judges*2 + 3;?>" style="text-align:left; vertical-align:bottom;">
			<img src="../operations/load_image.php?id=2" style="height:1.5cm;" />
			<?php echo $turnier.": Judge Statistic Round ".$durchgang;?></th>
			</tr>
			<tr class="header">
			<th colspan="3"><?php echo $turnier_no." / " .$turnier_date.", ".$turnier_ort;?></th>
			<?php
			$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
			while($obj_judges = mysqli_fetch_object($res_judges)){
				$i = $obj_judges->id;?>
				<th colspan="2" style="text-align:center;">Judge <?php echo $i;?></th>
				<?php
			}?>
			</tr>
			<tr class="header_small">
			<th> # </th><th>Participant</th><th>AVG</th>
			<?php 
			$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
			while($obj_judges = mysqli_fetch_object($res_judges)){
				$judge = $obj_judges->id;?>
				<th>Score</th><th>Dev</th>
				<?php
			}?>
			</tr>
			<?php
		}
	}?>
	<tr style="border-top:double;">
	<td colspan="3" style="text-align:right; font-weight:bold; border-left:1px solid black; border-right:black solid 1px;">Scores Accepted</td>
	<?php 
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$j = $obj_judges->id;
		$discard =$teilnehmer_anzahl_real;
		$res_all_teilnehmer = mysqli_query($link,"SELECT id FROM teilnehmer ORDER BY id ASC");
		//for($t=1;$t<=$teilnehmer_anzahl;$t++){
		while($obj_all_teilnehmer = mysqli_fetch_object($res_all_teilnehmer)){
			$i=$obj_all_teilnehmer->id;
			if($tbl_log[$i][$j] == 1)$discard--;
		}
		echo "<td colspan='2' style='text-align:right; border-right:1px solid black;'>".number_format($discard/$teilnehmer_anzahl_real*100,2)."%</td>";
	}?>
	</tr>
	<tr class="total">
	<td colspan="<?php echo $judges*2 + 1;?>"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
	<td colspan="2">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td>
	</tr>
	</table>
    <?php
	mysqli_query($link,"TRUNCATE statistik");
	$panel++;
	unset($tbl_log);
}
?>
