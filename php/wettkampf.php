<?php
require_once("host.inc");
if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
if($airfield > $airfield_anzahl){
	include("operations/airfield_master.php");
	exit;
}
$query_check = "SELECT count(distinct(wl.teilnehmer)) as anzahl FROM wettkampf_leitung wl JOIN durchgang_airfield da ON (da.durchgang = wl.durchgang) WHERE da.airfield = ".$airfield." AND status <= 2";
$result_check = mysqli_fetch_object(mysqli_query($link,$query_check));
$teilnehmer_anzahl =  $result_check->anzahl;
if($result_check->anzahl > 0){
	$query_check = "SELECT min(wl.durchgang) as durchgang FROM wettkampf_leitung wl JOIN durchgang_airfield da ON (da.durchgang = wl.durchgang) WHERE da.airfield = ".$airfield." AND status <= 1";
	$result_check = mysqli_fetch_object(mysqli_query($link,$query_check));
	$durchgang = $result_check->durchgang;?>
	<div id="teilnehmer_offen">
		<?php
        $check = 0;
        $teilnehmer_count = 0;?>
        <table class="liste">
        <tr class="headline"><th colspan="3">Pending Pilots R<?php echo $durchgang;?></th></tr>
        <tr class="header"><th style="height:20pt;">&nbsp;#&nbsp;</th><th>Name</th><th>>>></th></tr>
        <tr><td colspan="100%"></td></tr>
        <?php
        $query_startliste = "SELECT t.*, w.status FROM teilnehmer as t, wettkampf_leitung as w WHERE w.teilnehmer = t.id AND w.durchgang = ".$durchgang." AND w.status = 0 ORDER BY w.start ASC";
        if($res = mysqli_query($link,$query_startliste)){
            while($result_startliste = mysqli_fetch_object($res)){
                $teilnehmer_count++;?>
                <tr <?php if($teilnehmer_count % 2 == 0)echo " class='gerade'";?>>
                <td><?php echo $result_startliste->id;?></td>
                <td><?php echo $result_startliste->vorname." ".$result_startliste->nachname;?></td>
                <td><input  type="button" value=">>" onclick="ladeTeilnehmer('<?php echo $result_startliste->id;?>','<?php echo $durchgang;?>','<?php echo $airfield;?>');" id="b<?php echo $teilnehmer_count;?>" /></td>
                </tr>
                <?php
            }
        }?>
        </table>
	</div>
    <?php 
	$res_check_finish = mysqli_fetch_object(mysqli_query($link,"SELECT count(wl.teilnehmer) as teilnehmer, wl.durchgang FROM wettkampf_leitung as wl JOIN durchgang_airfield da ON (da.durchgang = wl.durchgang) WHERE wl.status = 2 AND da.airfield = ".$airfield));
	if($res_check_finish->teilnehmer == $teilnehmer_anzahl)$durchgang = $res_check_finish->durchgang;
	?>
	<div id="teilnehmer_aktuell">
    <table class="live">
    <tr class="headline">
    <th><?php echo "Airfield ".$airfield;?></th></tr>
    <tr><td>
    <?php 
	if($score_mode == 1) include("live_tbl.php");
	else include("live.php");?>
    </td></tr>
    </table>
	</div>
	
	<div id="buttons">
    <div id="timer">
    <p class="header">Timer</p>
	<?php 
	$timer_edit = true;
	include("output/timer_calc.php");?>
    </div>
    	<?php
		$score_confirmed = true;
		if($judge_pin == 1 && $teilnehmer_load){
			$res_judges = mysqli_query($link,"SELECT j.* FROM judge j JOIN judge_panel jp ON (jp.judge = j.id) JOIN durchgang_panel dp ON (dp.panel = jp.panel) JOIN durchgang_airfield da ON (da.durchgang = dp.durchgang) WHERE da.durchgang = ".$durchgang);
			while($obj_judges = mysqli_fetch_object($res_judges)){
				$sc = true;
				if($judge_pin == 1){
					if(!mysqli_fetch_object(mysqli_query($link,"SELECT confirm FROM judge_log WHERE teilnehmer = ".$teilnehmer." AND durchgang = ".$durchgang." AND judge = ".$obj_judges->id))){$score_confirmed = false; $sc = false;}
				}
				echo "<i style='margin:5px;padding:3px;";
				if($sc) echo " background-color:green;";
				echo "'>".$obj_judges->id."</i>";
			}
			echo "<br><br>";
		}
		if($teilnehmer_load && $teilnehmer_count >= 0 && $score_confirmed){?>
			<button class="durchgang" id="bsave" title="Save Flight" onClick="save(<?php echo $teilnehmer;?>,<?php echo $durchgang;?>,<?php echo $wert_durchgang;?>);"><img src="bilder/buttons/save.png" /></button>
            <?php 
		}
		elseif($teilnehmer_count > 0 && $res_check_finish->teilnehmer != $teilnehmer_anzahl && $score_confirmed) {?>
        	<button onclick="document.getElementById('b1').click();" class="durchgang" title="Nächster Teilnehmer"><img src="bilder/buttons/start.png"></button>
            <?php
		}
echo $res_check_finish->teilnehmer . " - " . $teilnehmer_anzahl;
		if($res_check_finish->teilnehmer  == $teilnehmer_anzahl){?>
			<button class="durchgang" title="End Round <?php echo $durchgang;?>" onClick="save_durchgang(<?php echo $durchgang;?>);"><img src="bilder/buttons/save_round.png" /><?php echo $durchgang;?></button>
            <?php
		}
		if(isset($query_timer)){?>
			<button title="Break" onClick="pause('<?php echo $airfield;?>');" class="durchgang"><img src="bilder/buttons/pause.png" /></button>
            <?php
		}?>
	</div>

    <?php
	
}
else {
	
	?>
	<div id="teilnehmer_aktuell" style="background-position:center; #background-image:url(operations/load_image.php?id=2); background-repeat:no-repeat; background-size:contain; opacity:0.7;"></div>
    <?php
}