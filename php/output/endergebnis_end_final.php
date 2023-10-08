<?php
require_once("../host.inc");
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;
$durchgaenge = $result_config->durchgaenge - $durchgaenge;
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
<th colspan="<?php echo (5 + $durchgaenge);?>">Final Results</th></tr>
<tr class="header">
<th colspan="4">Competition No.: <?php echo $turnier_no;?></th>
<th colspan="<?php echo (5 + $durchgaenge - 4);?>"></th>
</tr>
<tr class="header_small">
<th>Rank</th><th>Name</th><th>Club / Nation</th><th>FAI-License</th>
<?php
for($i=1;$i<=$durchgaenge;$i++){
	echo "<th class='zahl'>";
	echo "Final ".$i."<br>";
	$res_programm = mysqli_fetch_object(mysqli_query($link,"SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".($i + $end_final_durchgang - 1)));
	echo "<i style='font-size:8pt; font-weight:100;'>(".$res_programm->title.")</i></th>";
}?>
<th class="zahl" style="border-right:1px solid black;">Total</th></tr>
<?php
$count = 1;
$limit = 26;
$gesamt = ceil($teilnehmer_anzahl / $limit);
$query_teilnehmer = "SELECT t.*, sum(d.wert_prom) - x.streicher as total FROM teilnehmer t JOIN durchgang d ON (d.teilnehmer = t.id) JOIN (SELECT t.id, min(d.wert_prom) as streicher FROM teilnehmer t JOIN durchgang d ON (d.teilnehmer = t.id) WHERE d.durchgang = ".$end_final_durchgang." OR d.durchgang = ".($end_final_durchgang + 2)." GROUP BY t.id) as x ON (x.id = d.teilnehmer) WHERE d.durchgang >= ".$end_final_durchgang." GROUP BY t.id ORDER BY total DESC";
if($result_teilnehmer = mysqli_query($link,$query_teilnehmer)){
while($teilnehmer = mysqli_fetch_object($result_teilnehmer)){
	$query_streicher = "SELECT min(d.wert_prom) as streicher FROM teilnehmer t JOIN durchgang d ON (d.teilnehmer = t.id) WHERE (d.durchgang = ".$end_final_durchgang." OR d.durchgang = ".($end_final_durchgang + 2).") AND t.id = ".$teilnehmer->id;
	$res_streicher = mysqli_fetch_object(mysqli_query($link,$query_streicher));
	$streicher = $res_streicher->streicher;?>
	<tr style="border-right:1px solid black;" class="<?php if($count % 2 == 0) echo "gerade";else echo "ungerade";?>">
    <td><?php echo $count;?></td>
    <td><?php echo strtoupper($teilnehmer->nachname)." ".$teilnehmer->vorname;?></td>
    <td><?php if(isset($_GET['club']))echo $teilnehmer->club."<br>";
	echo mysqli_fetch_object(mysqli_query($link,"SELECT name FROM country_images WHERE img_id = ".$teilnehmer->land))->name;?></td>
    <td><?php echo mysqli_fetch_object(mysqli_query($link,"SELECT UPPER(short) as short FROM country_images WHERE img_id = ".$teilnehmer->land))->short."-".$teilnehmer->license;?></td>
	<?php
	$line_through = true;
	for($i=$end_final_durchgang;$i<=($end_final_durchgang + $durchgaenge - 1);$i++){
		$res = mysqli_query($link,"SELECT * FROM durchgang WHERE teilnehmer = ".$teilnehmer->id." AND durchgang >= ".$i);
		echo "<td style='text-align:right;";
		if($obj = mysqli_fetch_object($res)){
			if($obj->wert_prom == $streicher && $line_through){echo "text-decoration:line-through;"; $line_through = false;}
			echo "'>";
			echo number_format($obj->wert_prom,2,",","")."&permil;";
			echo "<div style='font-size:8pt; font-style:italic;'>".number_format($obj->wert_abs,2)."</div>";
		}
		else echo "'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
		echo "</td>";
	}
	?>
    <td style="border-right:1px solid black;font-weight:bold;" class="zahl"><?php echo number_format($teilnehmer->total,2,",","");?></td>
    </tr>
	<?php
	if($count % $limit == 0){?>
        <tr class="footer"><td colspan="<?php echo $durchgaenge+3;?>" style="text-align:left;"><?php echo $system_name." ".$version." &copy;".$year;?></td><td colspan="2" style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
		</table>
        <table>
        <tr class="kopf">
        <td colspan="2" style="padding:0px;"><img src="../operations/load_image.php?id=3" class="logo"></td>
        <th colspan="<?php echo (1 + $durchgaenge);?>" style="text-align:center; font-size:18pt;">
        <?php echo $turnier;?><br>
        <i style="font-size:10pt; font-style:normal;"><?php echo $turnier_ort.", ".$turnier_date;?><br>
        <?php echo $veranstalter;?></i>
        </th>
        <td colspan="2" style="padding:0px; text-align:right;"><img src="../operations/load_image.php?id=<?php echo $result_config->img_id;?>" class="logo"></td>
        </tr>
        <tr class="headline">
        <th colspan="<?php echo (5 + $durchgaenge);?>">Final Results</th></tr>
        <tr class="header">
        <th colspan="4">Competition No.: <?php echo $turnier_no;?></th>
        <th colspan="<?php echo (5 + $durchgaenge - 4);?>"></th>
        </tr>
        <tr class="header_small">
        <th>Rank</th><th>Name</th><th>Club / Nation</th><th>FAI-License</th>
        <?php
        for($i=1;$i<=$durchgaenge;$i++){
            echo "<th class='zahl'>";
            echo "Final ".$i."<br>";
            $res_programm = mysqli_fetch_object(mysqli_query($link,"SELECT p.title FROM programm as p JOIN durchgang_programm dp ON(dp.programm = p.id) WHERE dp.durchgang = ".($i + $end_final_durchgang - 1)));
            echo "<i style='font-size:8pt; font-weight:100;'>(".$res_programm->title.")</i></th>";
        }?>
        <th class="zahl">Total</th></tr>
        <?php
	}
$count++;
}
}
?>
        <tr class="footer"><td colspan="<?php echo $durchgaenge+3;?>" style="text-align:left;"><?php echo $system_name." ".$version." &copy;".$year;?></td><td colspan="2" style="text-align:right;">Page <?php echo ceil($count/$limit)."/".$gesamt;?></td></tr>
</table>
</body>