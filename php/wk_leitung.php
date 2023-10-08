<?php
require_once("host.inc");
//XML Config File
$file = "config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;?>
<script src="js/wk_leitung.js" language="javascript" type="text/javascript"></script>
<script type="text/javascript">
var airfield = 1;
setInterval(function () {process(airfield);}, 200);
</script>
<link rel="shortcut icon" href="bilder/favicon.ico" type="image/x-icon" />
<link rel="stylesheet" type="text/css" href="css/wk_leitung.css">
<body onLoad="control();">
<div id="all">
<div id="header">
<table class="headline">
<tr>
<th colspan="4" style="text-align:center;" class="title">chdScoring - COMPETITION BOARD</th>
</tr>
<tr class="header">
<th>Settings</th><th>Scoring</th><th>Flight Control</th>
<td class="controlButtons" rowspan="2">
<button title="Keyboard Control" id="keyboard" onClick="changeControl('keyboard');"><img src="bilder/buttons/keyboard.png" height="20"></button>
<button title="Go to FullScreen [F11]" id="fullscreen" onClick="goFullScreen();"><img src="bilder/buttons/fullscreen.png" height="20"></button>
<button id="bprint" title="Show print options [F6]" onClick="box('visible','print/');"><img src="bilder/buttons/print.png" height="40"></button><br>
<button title="Gamepad Control" id="gamepad" onClick="changeControl('gamepad');"><img src="bilder/buttons/gamepad.png" height="20"></button>
<button id="bsearch" title="Search Pilot[F3]" onClick="suche();"><img src="bilder/buttons/search.png" height="40"></button>
<button id="bpublic" title="Show Public Screen options [F10]" onClick="oeffnen('public/');"><img src="bilder/buttons/public.png" height="40"></button>
</td>
</tr>
<tr class="header">
<td>
<button id="bpreferences" title="Preferences [F1]" onClick="oeffnen('stammdaten.php');" value="Pref">
<img src="bilder/buttons/options.png" height="40"></button>
<button id="bpreferences" title="Import Data" onClick="oeffnen('import.php');" value="Import">
<img src="bilder/buttons/import.png" height="40"></button>
<button id="bjudges" title="Judges [F2]" onClick="oeffnen('judges_liste.php');">
<img src="bilder/buttons/judge.png" height="40>"></button>
<button id="bpilots" title="Pilots [F4]" onClick="oeffnen('teilnehmer_eingabe.php');">
<img src="bilder/buttons/pilots.png" height="40"></button>
<button id="bofficals" title="Officials" onClick="oeffnen('officials_eingabe.php');">
<img src="bilder/buttons/vip.png" height="40"></button>
<button id="bscheduleedit" title="Schedule Editor [F7]" onClick="oeffnen('figuren.php');">
<img src="bilder/buttons/schedule.png" height="40"></button>
<button title="Image Administator" onClick="oeffnen('image_admin.php');">
<img src="bilder/buttons/photos.png"></button>
<?php 
if($result_config->del_on == 1){?>
<button title="Earse all data" onClick="clearData();">
<img src="bilder/buttons/erase.png" height="40"></button>
<?php }?>
</td>
<td>
<button id="beditround" title="Edit Roundscoring" onClick="oeffnen('korrektur.php');">
<img src="bilder/buttons/edit_round.png"></button>
<button title="Enter Score" onClick="oeffnen('wertung_erfassen.php');">
<img src="bilder/buttons/enter_score.png"></button>
<button title="Fill Scores"onClick="oeffnen('wertung_fill.php');">
<img src="bilder/buttons/fill_score.png"></button>
<button title="Refilght" onClick="oeffnen('delete.php');">
<img src="bilder/buttons/reset.png"></button>
<button id="bprintroundsheet" title="Print Roundsheet [F8]" onClick="oeffnen('print_durchgang.php');">
<img src="bilder/buttons/print.png"></button>
</td>
<td>
<?php
if($result_durchgang = mysqli_fetch_object(mysqli_query($link,"SELECT max(wl.durchgang) + 1 as durchgang FROM wettkampf_leitung wl WHERE wl.status = 3"))){
	if(!$result_durchgang->durchgang || $result_durchgang->durchgang == 1){?>
    	<button title="Start" onClick="start(1);">
    	<img src="bilder/buttons/start.png"></button>
        <button id="beditstartorder" title="Edit Startorder" onClick="oeffnen('edit_startorder.php?durchgang=1');">
		<img src="bilder/buttons/order.png"></button>

    <?php
	}
	else if($result_durchgang->durchgang >= $end_final_durchgang && $end_finale == 1 && $result_durchgang->durchgang <= $result_config->durchgaenge){?>
    	<button title="Final <?php echo ($result_durchgang->durchgang);?>" onClick="start(<?php echo ($result_durchgang->durchgang);?>);">
    	<img src="bilder/buttons/start.png"></button>
        <button id="beditstartorder" title="Edit Startorder" onClick="oeffnen('edit_startorder.php?durchgang=<?php echo ($result_durchgang->durchgang);?>');">
		<img src="bilder/buttons/order.png"></button>
    <?php
	}
	else if($result_durchgang->durchgang >= $final_durchgang && $finale == 1 && $result_durchgang->durchgang < $end_final_durchgang){?>
    	<button title="Semifinal <?php echo ($result_durchgang->durchgang);?>" onClick="start(<?php echo ($result_durchgang->durchgang);?>);">
    	<img src="bilder/buttons/start.png"></button>
        <button id="beditstartorder" title="Edit Startorder" onClick="oeffnen('edit_startorder.php?durchgang=<?php echo ($result_durchgang->durchgang);?>');">
		<img src="bilder/buttons/order.png"></button>
    <?php
	}
	else {?>
		<button id="beditstartorder" title="Edit Startorder" onClick="oeffnen('edit_startorder.php?durchgang=<?php echo ($result_durchgang->durchgang);?>');"><img src="bilder/buttons/order.png"></button>
	<?php
	}
}
if($airfield_anzahl > 1){
	for($a=1;$a<=$airfield_anzahl;$a++){?>
    	<button title="Control Airfiled <?php echo $a;?>" onClick="loadAirfield(<?php echo $a;?>);">
		<img src="bilder/buttons/a<?php echo $a;?>.png"></button>
        <?php
	}?>
	<button title="Control Airfiled <?php echo $airfield_anzahl + 1;?>" onClick="loadAirfield(<?php echo $airfield_anzahl + 1;?>);">
	<img src="bilder/buttons/a_master.png"><?php echo $airfield_anzahl + 1;?></button>
	<?php
}
else {?>
	<button title="Pre - Flight [F9]" id="vorflieger" onClick="vorflieger(1);">
	<img src="bilder/buttons/vorflieger.png"></button>
    <?php
}?>
</td>
</tr>
</table></div>
<div id="main"></div></div>
<div id="logo"><img src="operations/load_image.php?id=2" height="150"></div>
<div id="footer"><?php echo $system_name." ".$version." &copy; ".$year; ?></div>
<div id="box"><button onClick="box('hidden','print/');" id="closebox">X</button><div id="inbox"></div></div>
</body>