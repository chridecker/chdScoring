<?php
require_once("../host.inc");
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;

if(isset($_GET['bewerb']))$bewerb = $_GET['bewerb'];
else $bewerb = 1;
//Bewerb
$query_bewerb = "SELECT name,number FROM bewerb WHERE id = ".$bewerb;
$res_bewerb = mysqli_fetch_object(mysqli_query($link,$query_bewerb));
if($bewerb != 1){
	$turnier = $res_bewerb->name;
	$turnier_no = $res_bewerb->number;
}
//Create Table & INSERT Preliminaries
$query = "CREATE TEMPORARY TABLE bewerb".$bewerb." SELECT teilnehmer, sum(wert_prom) - min(wert_prom) as prom FROM durchgang WHERE durchgang < ".$final_durchgang." GROUP BY teilnehmer ORDER BY prom DESC, min(wert_prom) DESC"; 
if($result_config->end_finale == 1 ) $query .= " LIMIT ".$final_teilnehmer;
mysqli_query($link,$query);
//Set Prelim to durchgang 1
mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD durchgang int(11) FIRST");
mysqli_query($link,"UPDATE bewerb".$bewerb." SET durchgang = 1");
//Norm Prelim Rounds to AVG of TOP half
$res_count_teilnehmer = mysqli_fetch_object(mysqli_query($link,"SELECT ROUND(count(teilnehmer) * ".$normalizationBaseLimit.") as tBase FROM bewerb".$bewerb." WHERE durchgang = 1"));

$final_teilnehmer_count_base = $res_count_teilnehmer->tBase;


$final_teilnehmer_count_base = round(($final_teilnehmer * $normalizationBaseLimit),0); //round()
if($final_teilnehmer_count_base == 0){$final_teilnehmer_count_base = 1;}

$res_max_prom = mysqli_fetch_object(mysqli_query($link,"SELECT AVG(t1.prom) as rBase FROM (SELECT prom FROM bewerb".$bewerb." WHERE durchgang = 1 ORDER BY prom DESC LIMIT ".$final_teilnehmer_count_base.") as t1"));
mysqli_query($link,"UPDATE bewerb".$bewerb." SET prom = prom / ".$res_max_prom->rBase." * 1000 WHERE durchgang = 1");

//INSERT Semifinal Results
for($durchgang = $final_durchgang;$durchgang<=$durchgaenge;$durchgang++){
	
	$query_teilnehmer_round_bewerb = "SELECT ROUND(count(t.id) * ".$normalizationBaseLimit.") as tBase FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id = ".$bewerb.";";
	$res_teilnehmer_round_bewerb = mysqli_fetch_object(mysqli_query($link,$query_teilnehmer_round_bewerb))->tBase;
	if($res_teilnehmer_round_bewerb == 0){$res_teilnehmer_round_bewerb = 1;}
	$query_round_r_base = "SELECT AVG(av.wert) as rBase FROM (SELECT d.wert_abs as wert FROM durchgang as d, teilnehmer as t, teilnehmer_bewerb as bw, bewerb as b WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY d.wert_abs DESC LIMIT ".$res_teilnehmer_round_bewerb.") as av";
	
	$rBaseVal = mysqli_fetch_object(mysqli_query($link,$query_round_r_base))->rBase;
	
	$query = "INSERT INTO bewerb".$bewerb." SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert_abs) / ".($rBaseVal)." * 1000),2) as prom FROM durchgang as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY t.id ASC;";
	
	mysqli_query($link,$query);
	mysqli_query($link,"UPDATE bewerb".$bewerb." SET durchgang = ".($durchgang - $final_durchgang + 2)." WHERE durchgang = ".$durchgang);
}
$durchgaenge = $durchgaenge - $final_durchgang + 2;
$res_durchgaenge = mysqli_fetch_object(mysqli_query($link,"SELECT max(durchgang) as md FROM bewerb".$bewerb));
$query = "SELECT distinct(teilnehmer) FROM bewerb".$bewerb;
$res = mysqli_query($link,$query);
if($res_durchgaenge->md <= 2){
	while($teilnehmer = mysqli_fetch_object($res)){
		$query_gesamt = "SELECT sum(prom) as gesamt FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
		$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
		$query_result = "INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
		mysqli_query($link,$query_result);
		mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD declined float");
		$query_result = "UPDATE bewerb".$bewerb." SET declined = 0 WHERE durchgang = ".($durchgaenge+1)." AND teilnehmer = ".$teilnehmer->teilnehmer;
		mysqli_query($link,$query_result);
	}
}
else {
	$count = 0;
	while($teilnehmer = mysqli_fetch_object($res)){
		$count++;
		if($count <= $final_teilnehmer){
			$min_final = mysqli_fetch_object(mysqli_query($link,"SELECT min(prom) as streicher FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer));
			$query_gesamt = "SELECT (sum(prom) - ".$min_final->streicher.") as gesamt FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
			$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
			$query_result = "INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
			mysqli_query($link,$query_result);
			mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD declined float");
			$query_gesamt = "SELECT min(prom) as streicher FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
			$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
			$query_result = "UPDATE bewerb".$bewerb." SET declined = ".$res_gesamt->streicher." WHERE durchgang = ".($durchgaenge+1)." AND teilnehmer = ".$teilnehmer->teilnehmer;
			mysqli_query($link,$query_result);
		}
		else {
			$query_gesamt = "SELECT sum(prom) as gesamt FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
			$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
			$query_result = "INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
			mysqli_query($link,$query_result);
			mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD declined float");
			$query_result = "UPDATE bewerb".$bewerb." SET declined = 0 WHERE durchgang = ".($durchgaenge+1)." AND teilnehmer = ".$teilnehmer->teilnehmer;
			mysqli_query($link,$query_result);
		}
	}
}
?>
<link rel="stylesheet" href="../css/print_endergebnis.css">
<body>
<table>
<tr class="kopf">
<td colspan="2" style="padding:0px;"><img src="../operations/load_image.php?id=<?php echo $result_config->fed_id;?>" class="logo"></td>
<th colspan="<?php echo (1 + $durchgaenge);?>" style="text-align:center; font-size:18pt;">
<?php echo $turnier;?><br>
<i style="font-size:10pt; font-style:normal;"><?php echo $turnier_ort.", ".$turnier_date;?><br>
<?php echo $veranstalter;?></i>
</th>
<td colspan="2" style="padding:0px; text-align:right;"><img src="../operations/load_image.php?id=<?php echo $result_config->img_id;?>" class="logo"></td>
</tr>
<tr class="headline">
<th colspan="<?php echo (5 + $durchgaenge);?>"><?php if($end_finale == 1) echo "Semi-";?>Final Results</th></tr>
<tr class="header">
<th colspan="4">Competition No.: <?php echo $turnier_no;?></th>
<th colspan="<?php echo (5 + $durchgaenge - 4);?>"></th>
</tr>
<tr class="header_small">
<th>Rank</th><th>Name</th><th>Club / Nation</th><th>FAI-ID</th>
<?php
for($i=1;$i<=$durchgaenge;$i++){
	echo "<th class='zahl'>";
	if($i == 1) echo "Preliminaries";
	else {
		echo "Final ".($i - 1)."<br>";
		$res_programm = mysqli_fetch_object(mysqli_query($link,"SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".($i + $final_durchgang - 2)));
		echo "<i style='font-size:8pt; font-weight:100;'>(".$res_programm->title.")</i></th>";
	}
}?>
<th class="zahl">Total</th></tr>
<?php
$count = 1;
$limit = 28;
$gesamt = ceil($teilnehmer_anzahl / $limit);
$query_teilnehmer = "SELECT t.*, b.teilnehmer, b.declined, b.prom as gesamt FROM bewerb".$bewerb." as b, teilnehmer as t WHERE b.durchgang = ".($durchgaenge+1)." AND b.teilnehmer = t.id ORDER BY prom DESC, declined DESC";
if($result_teilnehmer = mysqli_query($link,$query_teilnehmer)){
while($teilnehmer = mysqli_fetch_object($result_teilnehmer)){
	$line_trough = false;
	if($res_durchgaenge->md <=1)$line_trough = true;
	?>
	<tr class="<?php if($count % 2 == 0) echo "gerade";else echo "ungerade";?>" <?php if($count == $end_final_teilnehmer && $end_finale == 1) echo "style='border-bottom:solid;'";?>>
    <td><?php echo $count;?></td>
    <td><?php echo strtoupper($teilnehmer->nachname)." ".$teilnehmer->vorname;?></td>
    <td><?php if(isset($_GET['club']))echo $teilnehmer->club."<br>";
	echo mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$teilnehmer->land))->name;?></td>
    <td><?php echo /* mysqli_fetch_object(mysqli_query($link,"SELECT UPPER(short) as short FROM country_images WHERE img_id = ".$teilnehmer->land))->short."-". */ $teilnehmer->license;?></td>
    <?php
	$query_min = "SELECT min(prom) as min FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->id;
	$res_min = mysqli_fetch_object(mysqli_query($link,$query_min));
	for($i=1;$i<=$durchgaenge;$i++){?>
        <?php
		$query_durchgang = "SELECT b.prom, d.wert_abs FROM bewerb".$bewerb." as b, durchgang as d WHERE b.teilnehmer = ".$teilnehmer->id." AND b.durchgang = ".$i." AND d.durchgang = b.durchgang + ".($final_durchgang - 2)." AND d.teilnehmer = ".$teilnehmer->id;
		if($wert = mysqli_fetch_object(mysqli_query($link,$query_durchgang))){?>
        	<td style="text-align:right; <?php if($wert->prom == $res_min->min && !$line_trough && $res_durchgaenge->md > 2 && $count <= $final_teilnehmer){echo "text-decoration:line-through;"; $line_trough = true;}?>">
            <?php
			echo number_format($wert->prom,2,",","");?>&permil;<?php if ($i > 1)echo "<div style='font-size:8pt; font-style:italic;'>".number_format($wert->wert_abs,2,",","")."</div>";
		}
		else echo "<td><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";?>
        </td>
        <?php
	}?>
    <td class="zahl" style="font-weight:bold;"><?php echo number_format($teilnehmer->gesamt,2,",","");?>&permil;</td>
    </tr>
	<?php
	if($count % $limit == 0){?>
        <tr class="footer"><td colspan="<?php echo $durchgaenge+3;?>" style="text-align:left;"><?php echo $system_name." ".$version." &copy;".$year;?></td><td colspan="2" style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
		</table>
        <table>
        <tr class="kopf">
        <td colspan="2" style="padding:0px;"><img src="../operations/load_image.php?id=3&db=<?php echo $database;?>" class="logo"></td>
        <th colspan="<?php echo (1 + $durchgaenge);?>" style="text-align:center; font-size:18pt;">
        <?php echo $turnier;?><br>
        <i style="font-size:10pt; font-style:normal;"><?php echo $turnier_ort.", ".$turnier_date;?><br>
        <?php echo $veranstalter;?></i>
        </th>
        <td colspan="2" style="padding:0px; text-align:right;"><img src="../operations/load_image.php?id=<?php echo $result_config->img_id;?>&db=<?php echo $database;?>" class="logo"></td>
        </tr>
        <tr class="headline">
        <th colspan="<?php echo (5 + $durchgaenge);?>"><?php if($end_finale == 1) echo "Semi-";?>Final Results</th></tr>
        <tr class="header">
        <th colspan="4">Competition No.: <?php echo $turnier_no;?></th>
        <th colspan="<?php echo (5 + $durchgaenge - 4);?>"></th>
        </tr>
       <tr class="header_small">
		<th>Rank</th><th>Name</th><th>Club / Nation</th><th>FAI-ID</th>
		<?php
		for($i=1;$i<=$durchgaenge;$i++){
		echo "<th class='zahl'>";
		if($i == 1) echo "Preliminaries";
		else {
		echo "Final ".($i - 1)."<br>";
		$res_programm = mysqli_fetch_object(mysqli_query($link,"SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".($i + $final_durchgang - 2)));
		echo "<i style='font-size:8pt; font-weight:100;'>(".$res_programm->title.")</i></th>";
	}
}?>
<th class="zahl">Total</th></tr>
        <?php
	}
	$count++;
}
}
?>
        <tr class="footer"><td colspan="<?php echo $durchgaenge+3;?>" style="text-align:left;"><?php echo $system_name." ".$version." &copy;".$year;?></td><td colspan="2" style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
<tr><td><br/></td></tr>
<tr>
<th style="text-align:left;" colspan="2">Contest Director</th></tr>
<?php
$sql = "SELECT * FROM official WHERE club = 'Wettbewerbsleiter' ORDER BY id";
$res = mysqli_query($link,$sql);
while($obj = mysqli_fetch_object($res)){?>
	<tr><td colspan="2"><?php echo $obj->vorname . " " . $obj->nachname; ?></td>
	<td><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$obj->land))->name; ?></td>
	<td><?php echo $obj->license; ?></td></tr>
<?php
}?>
<tr>
<th style="text-align:left;" colspan="2">Jury Members</th></tr>
<?php
$sql = "SELECT * FROM official WHERE club = 'Jury' ORDER BY id";
$res = mysqli_query($link,$sql);
while($obj = mysqli_fetch_object($res)){?>
	<tr><td colspan="2"><?php echo $obj->vorname . " " . $obj->nachname; ?></td>
	<td><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$obj->land))->name; ?></td>
	<td><?php echo $obj->license; ?></td></tr>
<?php
}?>
<tr><td><br/></td></tr>
<tr>
<th style="text-align:left;" colspan="2">Jugdes</th></tr>
<?php
$sql = "SELECT * FROM judge ORDER BY id";
$res = mysqli_query($link,$sql);
while($obj = mysqli_fetch_object($res)){?>
	<tr><td colspan="2"><?php echo $obj->vorname . " " . $obj->name; ?></td>
	<td><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$obj->land))->name; ?></td>
	<td><?php echo $obj->license; ?></td></tr>
<?php
}?>
</table>
</body>