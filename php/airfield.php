<?php
require_once("host.inc");
//XML Config File
$file = "config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;
if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
?>
<script src="js/wk_leitung.js" language="javascript" type="text/javascript"></script>
<script type="text/javascript">
var airfield = <?php echo $airfield;?>;
setInterval(function () {process(airfield);}, 200);
</script>
<link rel="stylesheet" type="text/css" href="css/wk_leitung.css">
<body onLoad="control();">
<div id="all">
<div id="header">
<table class="headline">
<tr>
<th colspan="4" style="text-align:center;" class="title">chdScoring - AIRFIELD <?php echo $airfield;?></th></tr>
<tr class="header">
<td>
<button title="Pre - Flight [F9]" id="vorflieger" onClick="vorflieger(<?php echo $airfield;?>);">
<img src="bilder/buttons/vorflieger.png"></button>
<button id="beditround" title="Edit Roundscoring" onClick="oeffnen('korrektur.php');">
<img src="bilder/buttons/edit_round.png"></button>
<button title="Enter Score" onClick="oeffnen('wertung_erfassen.php');">
<img src="bilder/buttons/enter_score.png"></button>
<button title="Fill Scores"onClick="oeffnen('wertung_fill.php');">
<img src="bilder/buttons/fill_score.png"></button>
<button title="Refilght" onClick="oeffnen('delete.php');">
<img src="bilder/buttons/reset.png"></button>
<button id="bprintroundsheet" title="Print Roundsheet [F8]" onClick="oeffnen('print_durchgang.php');">
<img src="bilder/buttons/print.png"></button></td>
<td class="controlButtons">
<button title="Keyboard Control" id="keyboard" onClick="changeControl('keyboard');"><img src="bilder/buttons/keyboard.png" height="20"></button>
<button title="Gamepad Control" id="gamepad" onClick="changeControl('gamepad');"><img src="bilder/buttons/gamepad.png" height="20"></button>
<button title="FullScreen [F11]" id="fullscreen" onClick="goFullScreen();"><img src="bilder/buttons/fullscreen.png" height="20"></button>
<button id="bprint" title="Show print options [F6]" onClick="oeffnen('print/');"><img src="bilder/buttons/print.png" height="40"></button>
<button id="bpublic" title="Show Public Screen options [F10]" onClick="oeffnen('public/');"><img src="bilder/buttons/public.png" height="40"></button></td>
<td>
<?php
/*
if($result_durchgang = mysqli_fetch_object(mysqli_query($link,"SELECT max(durchgang) as durchgang FROM durchgang"))){?>
    <button title="Start Round <?php echo ($result_durchgang->durchgang+1);?>" onClick="start(<?php echo ($result_durchgang->durchgang+1);?>);">
    <img src="bilder/buttons/start.png"></button>
    <?php
}*/
?></td></tr>
<?php /*
<tr><th colspan="5" style="text-align:center;" class="turnier"><?php echo $turnier." / ". $turnier_date." (".$turnier_ort.")"; ?></th></tr>
*/?>
</table></div>
<div id="main"></div></div>
<div id="logo"><img src="operations/load_image.php?id=2" height="150"></div>
<div id="footer"><?php echo $system_name." ".$version." &copy; ".$year; ?></div>
<div id="box">
<button onClick="box('hidden');" id="closebox">X</button>
<div id="inbox">Test</div></div>
</body>