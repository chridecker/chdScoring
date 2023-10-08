<?php
if($_SERVER['PHP_SELF'] == "/output/timer_calc.php")require_once("../host.inc");
if(isset($airfield));
else if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
$timelimit = mysqli_fetch_object(mysqli_query($link,"SELECT zeit FROM klassen WHERE id = ".$result_config->klasse))->zeit;
$query = "SELECT t.id, wk.durchgang FROM teilnehmer t JOIN wettkampf_leitung wk ON(t.id = wk.teilnehmer) JOIN durchgang_airfield da ON (da.durchgang = wk.durchgang) WHERE wk.status = 1 AND da.airfield = ".$airfield;
if($res_teilnehmer = mysqli_fetch_object(mysqli_query($link,$query)))$query_timer = "SELECT start_time as start FROM wettkampf_leitung wl JOIN durchgang_airfield da ON (wl.durchgang = da.durchgang) WHERE wl.teilnehmer = ".$res_teilnehmer->id." AND wl.status = 1 AND da.airfield = ".$airfield;
if($_SERVER['PHP_SELF'] == "/output/timer_calc.php"){
	$timer_edit = false;
	$query = "SELECT t.*, wk.durchgang FROM teilnehmer t JOIN wettkampf_leitung wk ON(t.id = wk.teilnehmer) JOIN durchgang_airfield da ON (da.durchgang = wk.durchgang) WHERE wk.status = 1 AND da.airfield = ".$airfield;
	if($res = mysqli_fetch_object(mysqli_query($link,$query))){
		echo "<div id='teilnehmer'>";
		echo "&#8470;".$res->id." ".substr($res->vorname,0,1).". ".$res->nachname;
		//echo "</div>";
		echo "<br>Round ".$res->durchgang."</div>";
	}
	else {
		echo "STAND BY<br>";
		if(!$timer_edit)echo "<img class='timer_point' src='../bilder/buttons/point_red.png' style='padding-right:5px;'></div>";
	}
		
}
if(isset($query_timer)){
	$timer = mysqli_fetch_object(mysqli_query($link,$query_timer));
	$diff =  strtotime(date("H:i:s")) - strtotime($timer->start);
	if(floor($diff/60) > $timelimit && $timer->start == "00:00:00"){
		if(!$timer_edit)echo "<img class='timer_point' src='../bilder/buttons/point_yellow.png' style='padding-right:5px;'>";
		echo str_pad($timelimit,2,"0",STR_PAD_LEFT).":00";//"08:00";
		if($timer_edit){?>
            <br /><button id="bstarttimer" class="durchgang" title="Start Timer" onclick="timer('start',<?php echo $airfield;?>);">
            <img src="../bilder/buttons/start_timer.png" /></button>
            <?php
		}
	}
	elseif(floor($diff/60)> ($timelimit - 1) || floor($diff/60)<0){
		if(!$timer_edit)echo "<img class='timer_point' src='../bilder/buttons/point_red.png' style='padding-right:5px;'></div>";
		echo "00:00";
		if($timer_edit){?>
            <br /><button id="bresettimer" class="durchgang" title="Reset Timer" onclick="timer('stop',<?php echo $airfield;?>);">
            <img src="../bilder/buttons/reset_timer.png" /></button>
            <?php
		}
	}
	elseif(floor($diff/60) == ($timelimit - 1) && ($diff % 60) >= 30){
		if(!$timer_edit)echo "<img class='timer_point' src='../bilder/buttons/point_green.png' style='padding-right:5px;'></div>";
		echo str_pad(($timelimit ) - floor($diff/60),2,"0",STR_PAD_LEFT).":".str_pad(59 - ($diff % 60),2,"0",STR_PAD_LEFT);
		if($timer_edit){?>
            <br /><button id="bstoptimer" class="durchgang" title="Stop Timer" onclick="timer('stop',<?php echo $airfield;?>);">
            <img src="../bilder/buttons/stop_timer.png" /></button>
            <?php
		}
	}
	else{
		if(!$timer_edit)echo "<img class='timer_point' src='../bilder/buttons/point_green.png' style='padding-right:5px;'></div>";
		echo str_pad(($timelimit - 1) - floor($diff/60),2,"0",STR_PAD_LEFT).":".str_pad(59 - ($diff % 60),2,"0",STR_PAD_LEFT);
		if($timer_edit){?>
			<br /><button id="bstoptimer" class="durchgang" title="Stop Timer" onclick="timer('stop',<?php echo $airfield;?>);">
            <img src="../bilder/buttons/stop_timer.png" /></button>
    		<?php
		}
	}
}
//mysqli_free_result($res);
mysqli_close($link);
?>