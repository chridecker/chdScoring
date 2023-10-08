<?php
require_once("../host.inc");
if(isset($_GET["db"])){$link = mysqli_connect($host,$user,$password,$_GET["db"]);}
//TBL - Statistik
if(!isset($durchgang))$durchgang = $_GET['durchgang'];
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = ".$durchgang;
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
$res_panels = mysqli_query($link,"SELECT jp.panel, count(jp.judge) as judges FROM durchgang_panel dp JOIN judge_panel jp ON (jp.panel = dp.panel) WHERE durchgang = ".$durchgang." GROUP BY dp.panel ORDER BY judges ASC, dp.panel ASC");
$panel = 1;
$judges = 0;
$missplace = array();
while($panels = mysqli_fetch_object($res_panels)){
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$judge = $obj_judges->id;
		$missplace[$judge] = 0;
		$judges++;
		mysqli_query($link,"CREATE TEMPORARY TABLE judge".$judge." (teilnehmer int, wert double)");
		$res_teilnehmer = mysqli_query($link,"SELECT DISTINCT(teilnehmer) FROM durchgang WHERE durchgang = ".$durchgang);
		while($obj_teilnehmer = mysqli_fetch_object($res_teilnehmer)){
			$teilnehmer = $obj_teilnehmer->teilnehmer;
			$query = "SELECT sum(abs(w.wert) * f.wert) as wertung FROM wertung as w, figur as f WHERE w.figur = f.id + 1 - ".$obj_count_figur->anfang." AND w.judge = ".$judge." and w.durchgang = ".$durchgang." AND w.teilnehmer = ".$teilnehmer;
			$res = mysqli_query($link,$query);
			while($obj = mysqli_fetch_object($res)){
				mysqli_query($link,"INSERT INTO judge".$judge." (`teilnehmer`,`wert`) VALUES (".$teilnehmer.",".$obj->wertung.")");
			}
		}
		mysqli_query($link,"ALTER TABLE judge".$judge." ADD rank int(11)");
		$res_place = mysqli_query($link,"SELECT teilnehmer FROM judge".$judge." ORDER BY wert DESC");
		$rank = 0;
		while($place = mysqli_fetch_object($res_place)){
			$rank++;
			mysqli_query($link,"UPDATE judge".$judge." SET rank = ".$rank." WHERE teilnehmer = ".$place->teilnehmer);
		}
	}
	//XML Config File
	$file = "../config.xml";
	$xml = simplexml_load_file($file);
	$system_name = $xml->information->name;
	$version = $xml->information->version;
	$year = $xml->information->year;
	?>
    <link rel="stylesheet" href="../css/print_statistik.css" />
	<table class="judge_stats">
	<tr class="headline">
	<th colspan="<?php echo ($judges*3) + 3;?>" style="text-align:left; vertical-align:bottom;">
	<img src="../operations/load_image.php?id=2" style="height:1.3cm;" />
	<?php echo $turnier.": Judge Missplacments Round ".$durchgang;?></th>
	</tr>
	<tr class="header">
	<th colspan="3"><?php echo $turnier_no;?></th>
	<?php
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$i = $obj_judges->id;?>
		<th colspan="3" style="text-align:center;">Judge <?php echo $i;?></th>
		<?php
	}?>
	</tr>
	<tr class="header_small">
	<th>Rank</th><th>ID</th><th>Score</th>
	<?php 
	$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$judge = $obj_judges->id;?>
		<th>J<?php echo $judge;?></th><th>Rnk</th><th>Dev</th>
		<?php
	}?>
	</tr>
	<?php
	$count = 0;
	$limit = 21;
	$teilnehmer_anzahl_real = mysqli_fetch_object(mysqli_query($link,"SELECT count(teilnehmer) as anzahl FROM durchgang WHERE durchgang = ".$durchgang))->anzahl;
	$gesamt = ceil($teilnehmer_anzahl_real / $limit);
	$query_teilnehmer  = "SELECT t.* FROM teilnehmer t JOIN durchgang d ON (d.teilnehmer = t.id) AND d.durchgang = ".$durchgang." ORDER BY d.wert_abs DESC";
	$res_teilnehmer = mysqli_query($link,$query_teilnehmer);
	while($teilnehmer = mysqli_fetch_object($res_teilnehmer)){
		$count++;
		$res_wert = mysqli_query($link,"SELECT wert_abs as wertung FROM durchgang WHERE teilnehmer = ".$teilnehmer->id." AND durchgang = ".$durchgang);
		$wert = mysqli_fetch_object($res_wert);
		?>
		<tr class="content" <?php if($count % 2 == 0)echo " style='background-color:lightgrey;'";?>>
		<td class="content" style="text-align:center;"><?php echo $count;?></td>
        <td class="figur"><?php echo "P".$teilnehmer->id;?></td>
		<td class="figur"><?php echo number_format($wert->wertung,2);?></td>
		<?php
		$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
		while($obj_judges = mysqli_fetch_object($res_judges)){
			$judge = $obj_judges->id;
			$place = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM judge".$judge." WHERE teilnehmer = ".$teilnehmer->id));
			echo "<td style='border-left:1px black solid; text-align:center;'>".$place->wert."</td>";
			echo "<td align='center'>".$place->rank."</td>";
			echo "<td align='center'>".abs($count - $place->rank)."</td>";
			$missplace[$judge] += abs($count - $place->rank);
		}

		?>
		</tr>
		<?php
		if($count % $limit == 0){?>
			<tr class="total"><td colspan="<?php echo $judges*3 + 1;?>"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
            <td colspan="2">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td>
			</tr>
			</table>
			<table class="judge_stats">
			<tr class="headline">
			<th colspan="<?php echo ($judges*3 + 3);?>" style="text-align:left; vertical-align:bottom;">
			<img src="../operations/load_image.php?id=2" style="height:1.5cm;" />
			<?php  echo $turnier.": Judge Missplacments Round ".$durchgang;?></th>
			</tr>
			<tr class="header">
			<th colspan="3"><?php echo $turnier_no;?></th>
			<?php
			$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
			while($obj_judges = mysqli_fetch_object($res_judges)){
				$i = $obj_judges->id;?>
				<th colspan="3" style="text-align:center;">Judge <?php echo $i;?></th>
				<?php
			}?>
			</tr>
			<tr class="header_small">
			<th>Rank</th><th>ID</th><th>Score</th>
			<?php 
			$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
			while($obj_judges = mysqli_fetch_object($res_judges)){
				$judge = $obj_judges->id;?>
				<th>J<?php echo $judge;?></th><th>Rnk</th><th>Dev</th>
				<?php
			}?>
			</tr>
			<?php
		}
	}?>
    <tr>
    <td colspan="3">Missed Places</td>
    <?php
    $res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang." AND jp.panel = ".$panels->panel);
	while($obj_judges = mysqli_fetch_object($res_judges)){
		$judge = $obj_judges->id;?>
        <td colspan="3"><?php echo $missplace[$judge];?></td>
        <?php
	}?>
    </tr>
	<tr class="total">
	<td colspan="<?php echo $judges*3 + 1;?>"><?php echo $system_name." ".$version." &copy; ".$year; ?></td>
	<td colspan="2">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td>
	</tr>
	</table>
    <?php
	mysqli_query($link,"TRUNCATE statistik");
	$panel++;
}
?>
