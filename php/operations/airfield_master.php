<?php
//require_once("../host.inc");?>
<style type="text/css">
.airfield{
	float:left;
	width:49%;
	padding:10px;
	<?php
	if($airfield_anzahl > 2)echo"height:50%;";?>
}
.timer{
	float:left;
	width:49%;
	padding-left:10px;
	text-align:center;
	font-size:30pt;
}
</style>
<?php
$timer_edit = false;
for($airfield = 1;$airfield<=2;$airfield++){?>
    <div class="timer" onClick="loadAirfield(<?php echo $airfield;?>);">AIRFIELD <?php echo $airfield;?> - <?php include("output/timer_calc.php");?></div>
    <?php
}
for($airfield = 1;$airfield<=2;$airfield++){?>
	<div class="airfield" onClick="loadAirfield(<?php echo $airfield;?>);"><?php include("live_tbl.php");?></div>
	<?php
}
if($airfield_anzahl > 2){
	for($airfield = 3;$airfield<=$airfield_anzahl;$airfield++){?>
		<div class="timer" onClick="loadAirfield(<?php echo $airfield;?>);">AIRFIELD <?php echo $airfield;?> - <?php include("output/timer_calc.php");?></div>
		<?php
	}
	for($airfield = 3;$airfield<=$airfield_anzahl;$airfield++){?>
		<div class="airfield" onClick="loadAirfield(<?php echo $airfield;?>);"><?php include("live_tbl.php");?></div>
		<?php
	}
}?>