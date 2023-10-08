<?php
if(!isset($file))$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;
if($_SERVER['PHP_SELF'] == "/output/point_statistik.php")require_once("../host.inc");
//Durchgangswertung anzeigen
if(isset($durchgang));
else if(isset($_GET['durchgang']))$durchgang = $_GET['durchgang'];
if($durchgang < 1) {$durchgang = 1;}

$judgeNames;
$judgePointDict = array();

$query_judges = "SELECT t3.* FROM durchgang_panel t1 INNER JOIN judge_panel t2 ON (t1.panel = t2.panel) INNER JOIN judge t3 ON (t3.id = t2.judge) WHERE t1.durchgang = ".$durchgang.";";
$res_judges = mysqli_query($link,$query_judges);
while($judge = mysqli_fetch_object($res_judges)){
	$id = $judge->id;
	if(!array_key_exists($id,$judgePointDict)){
		//$judgePointDict[$id] = array(21);
		$judgePointDict[$id]["NO"] = 0;
		for($i=0;$i<=10;$i+=0.5){
			$judgePointDict[$id][strval($i)] = 0;
		}
	}
	$judgeNames[$id] = $judge->name." ".$judge->vorname;
	
	//echo $judge->name." ".$judge->vorname."<br/>";
	$query_judge_no_points = "SELECT wert, COUNT(wert) as anzahl FROM wertung WHERE judge = ".$judge->id." AND durchgang = ".$durchgang." AND wert < 0;";
	$res_judge_no_points = mysqli_query($link,$query_judge_no_points);
	if($noPoins = mysqli_fetch_object($res_judge_no_points)){
		$judgePointDict[$id]["NO"] = $noPoins->anzahl;
		//echo $points->anzahl;
	}
	
	$query_judge_points = "SELECT wert, COUNT(wert) as anzahl FROM wertung WHERE judge = ".$judge->id." AND durchgang = ".$durchgang." GROUP BY wert;";
	$res_judge_points = mysqli_query($link,$query_judge_points);
	while($points = mysqli_fetch_object($res_judge_points)){
		$judgePointDict[$id][strval($points->wert)] = $points->anzahl;
		 //echo $id."- >".strval($points->wert)."->".$points->anzahl."  [".$judgePointDict[$id][strval($points->wert)]."]<br/>";
	}
	
}

$keys = array_keys($judgePointDict[1]);
$keysJudges = array_keys($judgeNames);
?>

<?php
/* $canvas = imagecreatetruecoloro(800,600);
$pink = imagecolorallocate($canvas, 255, 105, 180);

imagerectangle($canvas, 50, 50, 150, 150, $pink);

header("Content-Type: image/jpeg");
imagejpeg($canvas);
imagedestroy($canvas); */
?>


<table>
<thead><tr><th>Value</th>
<?php
for($i = 1; $i<=count($judgeNames);$i++){?>
	<th><?php echo $judgeNames[$i]; ?>
	<?php
}?>
<th>Total</th>
</tr></thead>
<tbody>
<?php
foreach($keys as $key){?>
	<tr>
	<td><?php echo $key; ?></td>
	<?php
	$sum = 0;
	foreach($keysJudges as $judgeId){?>
		<td>
		<?php 
		if(array_key_exists($key,$judgePointDict[$judgeId])){
			$sum += $judgePointDict[$judgeId][strval($key)];
			echo $judgePointDict[$judgeId][strval($key)];
			// echo $judgePointDict[1]["7.5"];
		}
		else {
			echo "-";
		}
			
		?></td>
		<?php
	}
	?>
	<td><?php echo $sum; ?></td>
	</tr>
	<?php
}

?>
</tbody>
</table>
