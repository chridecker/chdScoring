<?php
require_once("../host.inc");?>
<style type="text/css">
.printer {
	width:200px;
	padding:5px;
	margin:10px;
}
	
</style>
<body class="printer">
<h1 style="background-color:lightblue; padding:10px;"><img src="../bilder/buttons/print.png" height="50" /> Print Options</h1>
<h2 style="background-color:lightgrey; padding:10px;">Gernal Printforms</h2>
<input class="printer" type="button" value="Accredidations" onClick="printer('?file=cards');">
<input class="printer" type="button" value="Barcode List" onClick="printer('?file=list');">
<input class="printer" type="button" value="Pilot List sm" onClick="printer('?file=list_small');">
<input class="printer" type="button" value="Pilot List" onClick="printer('?file=pilotlist');">
<input class="printer" type="button" value="Startorder" onClick="printer('?file=starterliste');">
<input class="printer" type="button" value="Timetable" onClick="printer('?file=timetable');">
<br />
<h2 style="background-color:lightgrey; padding:10px;">Judge Scorecards</h2>
<?php
$res = mysqli_query($link,"SELECT distinct(durchgang) as d FROM wettkampf_leitung WHERE status = 0 ORDER BY durchgang ASC");
while($d_sc = mysqli_fetch_object($res)){?>
<input class="printer" type="button" value="SC-Round <?php echo $d_sc->d;?>" onClick="printer('?file=programm&round=<?php echo $d_sc->d;?>');">
<?php } ?><br />
<h2 style="background-color:lightgrey; padding:10px;">Round Results</h2>
<?php
for($i=1;$i<=$result_config->durchgaenge;$i++){?>
<input class="printer" type="button" value="Round <?php echo $i;?>" onClick="printer('?file=durchgangswertung&durchgang=<?php echo $i;?>');">
<?php } ?><br />
<h2 style="background-color:lightgrey; padding:10px;">Overall Results</h2>
<?php if($finale == 1){?>
	<input class="printer" type="button" value="Preliminary Results" onClick="printer('?file=endergebnis_preliminary');">
    <input class="printer" type="button" value="<?php if($end_finale == 1)echo "Semi-";?>Final Results" onClick="printer('?file=endergebnis_final');">
    <?php 
	if($end_finale == 1){?>
    	<input class="printer" type="button" value="Final Results" onClick="printer('?file=endergebnis_end_final');">
        <?php
	}?>
 
    <?php
}
else {?>
	<input class="printer" type="button" value="Results" onClick="printer('?file=endergebnis');">
    <?php
}?>
<input class="printer" type="button" value="Certifcates" onClick="printer('?file=urkunde');">
<input class="printer" type="button" value="Technical Sheet" onClick="printer('?file=overview');">
<h2 style="background-color:lightgrey; padding:10px;">TBL Statistik</h2>
<?php
$query = "SELECT distinct(durchgang) FROM tbl_result ORDER BY durchgang ASC";
$res = mysqli_query($link,$query);
while($obj = mysqli_fetch_object($res)){?>
	<input class="printer" type="button" value="Statistics R<?php echo $obj->durchgang;?>" onClick="printer('?file=statistik&durchgang=<?php echo $obj->durchgang;?>');">
    <?php
}?>
<h2 style="background-color:lightgrey; padding:10px;">Judge Placements</h2>
<?php
$query = "SELECT distinct(durchgang) FROM tbl_result ORDER BY durchgang ASC";
$res = mysqli_query($link,$query);
while($obj = mysqli_fetch_object($res)){?>
	<input class="printer" type="button" value="Placment R<?php echo $obj->durchgang;?>" onClick="printer('?file=judge_placment&durchgang=<?php echo $obj->durchgang;?>');">
    <?php
}?>

</body>