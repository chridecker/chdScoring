<?php
require_once("../host.inc");
$res_panel = mysqli_query($link,"SELECT DISTINCT(panel) as panel FROM judge_panel ORDER BY panel ASC");
?>
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function judgePanel(judge,panel){
	var box = document.getElementById('judge_panel_' + judge + '_' + panel);
	if(box.checked) var value = 1;
	else var value = 0;
	var file = "judge_panel_edit.php?judge=" + judge + "&panel=" + panel + "&value=" + value;
	if(xmlHttp){
		try{
			xmlHttp.open("GET",file,true);
			xmlHttp.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function addPanel(){
	var file = "judge_panel_edit.php?panel=new";
	if(xmlHttp){
		try{
			xmlHttp.open("GET",file,true);
			xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function randomPanel(){
	var spanel = document.getElementById('spanel').value;
	var epanel = document.getElementById('epanel').value;
	var lmr = document.getElementById('lmr_panel').checked;
	if(spanel != "" && epanel != "" && spanel >0 && epanel > 0 && epanel > spanel)var file = "random_judge_panel.php?spanel=" + spanel + "&epanel=" + epanel + "&lmr=" + lmr;
	if(xmlHttp){
		try{
			xmlHttp.open("GET",file,true);
			xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function handleRequestStateChange(){
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			location.reload();
		}
	}
}
window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}

</script>
<link rel="stylesheet" href="../css/judge_panel.css" />
<table class="liste">
<tr class="header">
<th colspan="100%">Judge Panel Editor</th>
</th>
<tr>
<td></td>
<?php
while($panel = mysqli_fetch_object($res_panel)){
	echo "<th>Panel ".$panel->panel."</th>";
}?>
<th><button onclick="addPanel();">+</button></th>
<th>
</tr>
<?php
$res_judge = mysqli_query($link,"SELECT j.* FROM judge j ORDER BY j.id ASC");
$count = 0;
while($judge = mysqli_fetch_object($res_judge)){
	$count++;
	if(($count % 2) == 0)echo "<tr class='gerade'>";
	else echo "<tr>";
	echo "<th>".$judge->name."</th>";
	$res_panel = mysqli_query($link,"SELECT DISTINCT(panel) as panel FROM judge_panel ORDER BY panel ASC");
	while($panel = mysqli_fetch_object($res_panel)){
		echo "<th><input type='checkbox' id='judge_panel_".$judge->id."_".$panel->panel."' onchange='judgePanel(".$judge->id.",".$panel->panel.");';";
		if($res_judge_panel = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM judge_panel WHERE judge = ".$judge->id." AND panel = ".$panel->panel))) echo " checked='checked'";
		echo "></th>";
	}
	echo "<td></td>";
	echo "</tr>";
}
?>
<tr><td><button title="Close" onclick="window.close();">Close</button></td></tr>
<tr><td colspan="100%">Randomize <input type="text" id="spanel" /> - <input type="text" id="epanel" />
<input type="checkbox" id="lmr_panel" />L-M-R Panel
<button onclick="randomPanel();" title="Random"><img src="../bilder/buttons/random.png" height="20" /></button>
</td>
</td></tr>
</table>
