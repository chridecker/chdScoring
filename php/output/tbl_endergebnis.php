<?php
require_once("../host.inc");
if(isset($_GET['bewerb']))$bewerb = $_GET['bewerb'];
else $bewerb = 1;
//Bewerb
$query_bewerb = "SELECT name FROM bewerb WHERE id = ".$bewerb;
$res_bewerb = mysqli_fetch_object(mysqli_query($link,$query_bewerb));
if($bewerb != 1)$turnier = $res_bewerb->name;


for($durchgang = 1;$durchgang<=$durchgaenge;$durchgang++){
	if($durchgang == 1 )$query = "CREATE TEMPORARY TABLE bewerb".$bewerb." SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert) / (SELECT max(d.wert) FROM tbl_result as d, teilnehmer as t, teilnehmer_bewerb as bw, bewerb as b WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb.") * 1000),2) as prom FROM tbl_result as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY t.id ASC;";
	else $query = "INSERT INTO bewerb".$bewerb." SELECT d.durchgang, t.id as teilnehmer, ROUND(((d.wert) / (SELECT max(d.wert) FROM tbl_result as d, teilnehmer as t, teilnehmer_bewerb as bw, bewerb as b WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb.") * 1000),2) as prom FROM tbl_result as d, teilnehmer as t, bewerb as b, teilnehmer_bewerb as bw WHERE d.durchgang = ".$durchgang." AND d.teilnehmer = t.id AND t.id = bw.teilnehmer AND bw.bewerb = b.id AND b.id =".$bewerb." ORDER BY t.id ASC;";
	$res = mysqli_query($link,$query);
}
$query = "SELECT distinct(teilnehmer) FROM bewerb".$bewerb;
$res = mysqli_query($link,$query);
while($teilnehmer = mysqli_fetch_object($res)){
	$query_gesamt = "SELECT sum(prom) - min(prom) as gesamt FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
	$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
	$query_result = "INSERT INTO bewerb".$bewerb." (`durchgang`,`teilnehmer`,`prom`) VALUES (".($durchgaenge+1).",".$teilnehmer->teilnehmer.",".$res_gesamt->gesamt.")";
	mysqli_query($link,$query_result);
	mysqli_query($link,"ALTER TABLE bewerb".$bewerb." ADD declined float");
	$query_gesamt = "SELECT min(prom) as min FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->teilnehmer.";";
	$res_gesamt = mysqli_fetch_object(mysqli_query($link,$query_gesamt));
	$query_result = "UPDATE bewerb".$bewerb." SET declined = ".$res_gesamt->min." WHERE durchgang = ".($durchgaenge+1)." AND teilnehmer = ".$teilnehmer->teilnehmer;
	mysqli_query($link,$query_result);
}?>

<link rel="stylesheet" type="text/css" href="../css/print_endergebnis.css" />

<body>
<table>
<tr class="kopf">
<th colspan="2" style="text-align:left;"><?php echo $veranstalter;?></th>
<th colspan="<?php echo $durchgaenge;?>" style="text-align:center;"><img width="150px" src="../operations/load_image.php?id=2"></th>
<th colspan="3"><?php echo $turnier_ort;?></th></tr>
<tr class="headline">
<th colspan="<?php echo ($durchgaenge + 5);?>"><?php echo $turnier;?><p>Final Ranking</p></th></tr>
<tr class="header">
<th colspan="4">Competition No.: <?php echo $turnier_no;?><br><?php echo $turnier_date.", ".$turnier_ort;?></th>
<th colspan="<?php echo ($durchgaenge + 1);?>"></th>
</tr>
<tr class="header_small">
<th>Rank</th><th>Name</th><th>Club / Nation</th><th>FAI-License</th>
<?php 
for($d=1;$d<=$durchgaenge;$d++){?>
	<th class="zahl">Round <?php echo $d;?></th>
    <?php
}?>
<th class="zahl">Total</th></tr>

<?php
$count = 1;
$query_teilnehmer = "SELECT t.*, b.teilnehmer, b.declined, b.prom as gesamt FROM bewerb".$bewerb." as b, teilnehmer as t WHERE b.durchgang = ".($durchgaenge+1)." AND b.teilnehmer = t.id ORDER BY prom DESC, declined DESC";
$result_teilnehmer = mysqli_query($link,$query_teilnehmer);
while($teilnehmer = mysqli_fetch_object($result_teilnehmer)){
	$query_max = "SELECT max(durchgang) as max FROM tbl_result WHERE teilnehmer = ".$teilnehmer->id;
	$max = mysqli_fetch_object(mysqli_query($link,$query_max));
	?>
	<tr class="<?php if($count % 2 == 0) echo "gerade";else echo "ungerade";?>">
    <td><?php echo $count;?></td>
    <td><?php echo $teilnehmer->vorname." ".$teilnehmer->nachname;?></td>
    <td><?php echo $teilnehmer->club."<br>".$teilnehmer->land;?></td>
    <td><?php echo $teilnehmer->license;?></td>
    <?php
	$query_min = "SELECT min(prom) as min FROM bewerb".$bewerb." WHERE teilnehmer = ".$teilnehmer->id;
	$res_min = mysqli_fetch_object(mysqli_query($link,$query_min));
	for($i=1;$i<=$durchgaenge;$i++){?>
        <?php
		$query_durchgang = "SELECT b.prom, d.wert FROM bewerb".$bewerb." as b, tbl_result as d WHERE b.durchgang = ".$i." AND b.teilnehmer = ".$teilnehmer->id." AND d.durchgang = ".$i." AND d.teilnehmer = ".$teilnehmer->id;
		if($wert = mysqli_fetch_object(mysqli_query($link,$query_durchgang))){?>
        	<td style="text-align:right; <?php if($wert->prom == $res_min->min && $max->max > 1)echo "text-decoration:line-through;";?>">
            <?php
			echo number_format($wert->prom,2,",","");?>&permil;<br><?php echo "<div style='font-size:8pt; font-style:italic;'>".number_format($wert->wert,2,",","")."</div>";
		}
		else echo "<td>\n";?>
        </td>
        <?php
	}?>
    <td class="zahl" style="font-weight:bold;"><?php echo number_format($teilnehmer->gesamt,2,",","");?></td>
    </tr>
	<?php
	$count++;
}
?>
</table>
</body>
