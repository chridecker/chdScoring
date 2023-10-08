<?php
require_once("host.inc");
$durchgaenge = $result_config->durchgaenge;
?>
<script type="text/javascript">
var xmlHttp2 = createXmlHttpRequestObject();

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function speichern(){
	var teilnehmer = document.getElementById("teilnehmer").value;
	var durchgang = document.getElementById("durchgang").value;
	var start = document.getElementById("start").value;
	var ende = document.getElementById("ende").value;
	var wert = document.getElementById("wert").value;
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/fill.php?teilnehmer=" + teilnehmer + "&durchgang=" + durchgang + "&start=" + start + "&ende=" + ende + "&wertung=" + wert,true);
			xmlHttp2.send(null);
			alert("Wertung eingetragen");
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function checkStartEnde(){
	var start = document.getElementById("start").value;
	var ende = document.getElementById("ende").value;
	if(ende.value < start.value)alert("Ende muss mind. gleich wie Start sein");
}

window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}

</script>
<link rel="stylesheet" type="text/css" href="css/wertung_erfassen.css">
<table>
<tr>
<th colspan="5" class="headline">Fill Scores</th></tr>
<tr>
<th>Participant</th><th>Round</th><th>From</th><th>To</th><th>Score</th>
</tr>
<tr>
<td>
<select id="teilnehmer">
<?php 
$query_teilnehmer = "SELECT * FROM teilnehmer";
$res_teilnehmer = mysqli_query($link,$query_teilnehmer);
while($teilnehmer = mysqli_fetch_object($res_teilnehmer)){?>
	<option value="<?php echo $teilnehmer->id;?>"><?php echo $teilnehmer->vorname." ".$teilnehmer->nachname;?></option>
    <?php
}?>
</select></td>
<td>
<select id="durchgang">
<?php 
for($durchgang = 1;$durchgang<=$durchgaenge;$durchgang++){?>
	<option value="<?php echo $durchgang;?>">Round <?php echo $durchgang;?></option>
    <?php
}?>
</select>
</td>
<td>
<select id="start" onchange="checkStartEnde();">
<?php
for($i=1;$i<=17;$i++){?>
	<option value="<?php echo $i;?>"><?php echo "Manoeuvre ".$i;?></option>
    <?php
}?>
</select></td>
<td>
<select id="ende" onchange="checkStartEnde();">
<?php
for($i=1;$i<=17;$i++){?>
	<option <?php if($i == 17) echo "selected";?> value="<?php echo $i;?>"><?php echo "Manoeuvre ".$i;?></option>
    <?php
}?>
</select>
</td>
<td>
<select id="wert">
<option value="0">0</option><option value="1">1</option><option value="2">2</option>
<option value="3">3</option><option value="4">4</option><option value="5">5</option>
<option value="6">6</option><option value="7">7</option><option value="8">8</option>
<option value="9">9</option><option value="10">10</option><option value="-1">N/O</option>
</select>
</td>
</tr>
<tr>
<td colspan="3" style="text-align:left;"><input type="button" value="Close" onClick="window.close();"></td>
<td colspan="3" style="text-align:right;"><input type="button" value="Save" onClick="speichern();"></td></tr>
</table>
