<?php
require_once("../host.inc");?>
<head>
<meta name="viewport" content="user-scalable=no, initial-scale=1, maximum-scale=1, minimum-scale=1, width=device-width, height=device-height, target-densitydpi=device-dpi" />
<meta name="viewport" content="user-scalable=no,target-densitydpi=device-dpi" />
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();
var xmlHttp3 = createXmlHttpRequestObject();
var xmlHttp4 = createXmlHttpRequestObject();
var judge = <?php echo $_GET['id'];?>;
var judgePad = "numblock"; 
var durchgang;
var editS;
var recent_teilnehmer;
var recent_figur = null;
var wertung = 10;
var soundOutput = false;
var sound;
var speaker;


function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function process(){
	if(xmlHttp){
		try{
			xmlHttp.open("GET","../operations/create_judge_xml.php?judge=" + judge,true);
			xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function loadScoring(){
	if(xmlHttp3){
		try{
			xmlHttp3.open("GET","../operations/judge_" + judgePad + ".php",true);
			xmlHttp3.onreadystatechange = handleRequestStateChangejudgePad;
			xmlHttp3.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function handleRequestStateChange(){
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			var response = xmlHttp.responseXML;
			processResponse(response);
			if (judgePad == "scribe")calc(0);
		}
	}
}
function handleRequestStateChangejudgePad(){
	if(xmlHttp3.readyState == 4){
		if(xmlHttp3.status == 200){
			var response = xmlHttp3.responseText;
			document.getElementById("right").innerHTML = response;
		}
	}
}
function processResponse(response){
	recent_figur = null;
	var scores = response.getElementsByTagName('scores')[0];
	var confirmS = scores.getAttribute('confirm');
	editS = scores.getAttribute('edit');
	document.getElementById("judge_name").innerHTML = "#" + judge + " - " + scores.getAttribute('name');
	durchgang = scores.getAttribute('durchgang');
	var figuren = scores.getElementsByTagName('figuren')[0];
	var anzahl = figuren.getAttribute('anzahl');
	var start = figuren.getAttribute('start');
	var teilnehmer = scores.getElementsByTagName('teilnehmer')[0];
	if(recent_teilnehmer != teilnehmer.getAttribute('id')) clearScores(anzahl);
	recent_teilnehmer = teilnehmer.getAttribute('id');
	for(var i = 0;i < anzahl;i++){
		if(figuren.getElementsByTagName('figur')[i].getAttribute('recent') == "true"){
			recent_figur = figuren.getElementsByTagName('figur')[i];
			document.getElementById("fname" + i).style.backgroundColor = "darkgreen";
		}
		else if((i % 2) != 0) document.getElementById("fname" + i).style.backgroundColor = "lightgrey";
		else document.getElementById("fname" + i).style.backgroundColor = "white";
		document.getElementById("fname" + i).innerHTML = figuren.getElementsByTagName('figur')[i].getAttribute('name');
		document.getElementById("fname" + i).style.maxWidth = "500px";
		if(figuren.getElementsByTagName('figur')[i].getAttribute('score') != null)document.getElementById("fscore" + i).innerHTML = figuren.getElementsByTagName('figur')[i].getAttribute('score');
	}
	document.getElementById("teilnehmer").innerHTML = recent_teilnehmer + " - " + teilnehmer.getAttribute('name');
	if(recent_figur != null){
		document.getElementById("recent_figur").innerHTML = "#" + (recent_figur.getAttribute('id') - start + 1) + " - " + recent_figur.getAttribute('name');
		recent_figur = recent_figur.getAttribute('id') - start + 1;
	}
// Judge Timer Sperre
	if(scores.getAttribute('timer') >= 480)deactivate(" Timer Over ");
	else activate();
	if(recent_figur == null && confirmS == 0){
		deactivate("");
		document.getElementById('recent_figur').innerHTML = "<button class='confirm' onclick='confirmScores();'>CONFIRM SCORES</button>";
	}
	else if(recent_figur == null && confirmS == 1)deactivate("Scores confirmed");
}
function confirmScores(){
	var pin = prompt('Insert Judge Pincode');
	var file = "../operations/confirm_score.php?judge=" + judge + "&pin=" + pin + "&teilnehmer=" + recent_teilnehmer + "&durchgang=" + durchgang;
	if(xmlHttp4){
		try{
			xmlHttp4.open("GET",file,true);
			xmlHttp4.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function clearScores(anzahl){
	for(var i = 0;i < anzahl;i++)document.getElementById("fscore" + i).innerHTML = "";
}
function deactivate(message){
	if(judgePad == 'numblock'){
		for(var i=0;i<=10;i++){
			document.getElementById("p" + i).disabled = true;
			document.getElementById("p" + i).style.backgroundColor = "lightgrey";
		}
		document.getElementById("pno").disabled = true;
		document.getElementById("pno").style.backgroundColor = "lightgrey";
	}
	else if(judgePad == 'scribe'){
		document.getElementById("mnf").disabled = true;
		document.getElementById("me").disabled = true;
		document.getElementById("pe").disabled = true;
		document.getElementById("pnf").disabled = true;
		document.getElementById("n").disabled = true;
		document.getElementById("save").disabled = true;
		document.getElementById("no").disabled = true;	
		document.getElementById("mnf").style.backgroundColor = "lightgrey";
		document.getElementById("me").style.backgroundColor = "lightgrey";
		document.getElementById("pe").style.backgroundColor = "lightgrey";
		document.getElementById("pnf").style.backgroundColor = "lightgrey";
		document.getElementById("n").style.backgroundColor = "lightgrey";
		document.getElementById("save").style.backgroundColor = "lightgrey";
		document.getElementById("no").style.backgroundColor = "lightgrey";
	
	}
	document.getElementById("recent_figur").innerHTML = "--" + message + " --";
}
function activate(){
	if(judgePad == 'numblock'){
		for(var i=0;i<=10;i++){
			document.getElementById("p" + i).disabled = false;
			document.getElementById("p" + i).style.backgroundColor = "lightblue";
		}
		document.getElementById("pno").disabled = false;
		document.getElementById("pno").style.backgroundColor = "lightblue";
	}
	else if(judgePad == 'scribe'){
		document.getElementById("mnf").disabled = false;
		document.getElementById("me").disabled = false;
		document.getElementById("pe").disabled = false;
		document.getElementById("pnf").disabled = false;
		document.getElementById("n").disabled = false;
		document.getElementById("save").disabled = false;
		document.getElementById("no").disabled = false;		
		document.getElementById("mnf").style.backgroundColor = "lightblue";
		document.getElementById("me").style.backgroundColor = "lightblue";
		document.getElementById("pe").style.backgroundColor = "lightblue";
		document.getElementById("pnf").style.backgroundColor = "lightblue";
		document.getElementById("n").style.backgroundColor = "lightblue";
		document.getElementById("save").style.backgroundColor = "lightblue";
		document.getElementById("no").style.backgroundColor = "lightblue";
	}
}
function beep(){
	if(sound == null)sound = document.getElementById('beep');
	if(soundOutput)sound.play();
}
function vibrate(){
	if("vibrate" in navigator){
		navigator.vibrate = navigator.vibrate || navigator.webkitVibrate || navigator.mozVibrate || navigator.msVibrate;
		if(navigator.vibrate)navigator.vibrate(300);
	}
}
function save(wert){
	if(wert == 99) wert = Math.round(wertung);
	var file = "../operations/save_wertung.php?teilnehmer=" + recent_teilnehmer + "&durchgang=" + durchgang + "&figur=" + recent_figur + "&judge=" + judge + "&wert=" + wert;
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET",file,true);
			xmlHttp2.onreadystatechange = function(){beep();vibrate();};
			xmlHttp2.send(null);
			wertung = 10;
			//sprechen(wert);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function calc(value){
	wertung = wertung + value;
	if(wertung < 0) wertung = 0;
	else if (wertung > 10 )wertung = 10;
	document.getElementById("anzeige_wertung").innerHTML = wertung;
	//if(value != 0)sprechen(wertung);
} 
function changePad(){
	if(judgePad == 'numblock'){
		judgePad = 'scribe';
		loadScoring();
		calc(0);
	}
	else if(judgePad == 'scribe'){
		judgePad = 'numblock';
		loadScoring();
	}
	process();
}
function changeS(figur){
	if(!recent_figur && editS == 1){
		var check = confirm('Do you want to change the score from manoeuvre ' + (figur + 1) + '?');
		var file = "../operations/edit_wertung.php?judge=" + judge + "&teilnehmer=" + recent_teilnehmer + "&durchgang=" + durchgang + "&figur=" + (figur + 1);
		if(xmlHttp4 && check){
			try{
				xmlHttp4.open("GET",file,true);
				xmlHttp4.send(null);
			}
			catch(e){
				alert("Can't connect to Server: " + e.toString());
			}
		}
		if(check)document.getElementById("fscore" + figur).innerHTML = "";
	}
}
function sprechen(text){
	if(text == -1) text = "N O";
	if(soundOutput){
		if(speaker == null)speaker = new SpeechSynthesisUtterance();
		speaker.text = text;
		speaker.lang = "de-DE";
		window.speechSynthesis.speak(speaker);
	}
}
function changeSoundOutput(){
	if(soundOutput){
		soundOutput = false;
		document.getElementById('soundmode').style.backgroundColor = "lightgrey";
	}
	else {
		soundOutput = true;
		document.getElementById('soundmode').style.backgroundColor = "lightblue";
	}
}
function showPressedValue(box){
	var oldColor = document.getElementById(box).style.backgroundColor;
	document.getElementById(box).style.backgroundColor = "green";
	setTimeout(function(){resetColor(box,oldColor);},1000);
	
}
function resetColor(box,color){
	document.getElementById(box).style.backgroundColor = color;
}
window.onkeypress = function(e) {
	e.preventDefault();
}
window.onkeyup = function(e) {
	e.preventDefault();
}
window.onkeydown = function (e){
	var key = e.keyCode ? e.keyCode : e.which;
	if(key === 8 || key == 166){
		e.preventDefault();
        e.stopPropagation();
	}
	e.preventDefault();
	if(judgePad == 'scribe'){
		switch(key){
			case 228:
				document.getElementById("mnf").click();
				break;
			case 227:
				document.getElementById("pnf").click();
				break;
			case 177:
				document.getElementById("me").click();
				break;
			case 176:
				document.getElementById("pe").click();
				 break;
			case 13:
				document.getElementById("save").click();
				 break;
			default:
				break;
		}
	}
	else if(judgePad == 'numblock' && key != 144){
		switch(key){
			case 96:
			case 48:
				showPressedValue("p0");
				document.getElementById("p0").click();
				break;
			case 97:
			case 49:
				showPressedValue("p1");
				document.getElementById("p1").click();
				break;
			case 98:
			case 50:
				showPressedValue("p2");
				document.getElementById("p2").click();
				break;
			case 99:
			case 51:
				showPressedValue("p3");
				document.getElementById("p3").click();
				 break;
			case 100:
			case 52:
				showPressedValue("p4");
				document.getElementById("p4").click();
				 break;
			case 101:
			case 53:
				showPressedValue("p5");
				document.getElementById("p5").click();
				 break;
			case 102:
			case 54:
				showPressedValue("p6");
				document.getElementById("p6").click();
				 break;
			case 103:
			case 55:
				showPressedValue("p7");
				document.getElementById("p7").click();
				 break;
			case 104:
			case 56:
				showPressedValue("p8");
				document.getElementById("p8").click();
				 break;
			case 105:
			case 57:
				showPressedValue("p9");
				document.getElementById("p9").click();
				 break;
			case 106:
			case 58:
				showPressedValue("p10");
				document.getElementById("p10").click();
				 break;
			case 111:
				showPressedValue("pno");
				document.getElementById("pno").click();
				 break;
			default:
				//alert(key);
				break;
		}
	}

}
setInterval(function () {process()}, 500);
</script>
<link rel="stylesheet" type="text/css" href="../css/judge.css" />
</head>
<body onLoad="changeSoundOutput();">
<audio src="../bilder/beep.mp3" autostart="false" id="beep"></audio>
<table class="judge">
<tr class="name" >
<th id="judge_name"></th>
<th>
<button class="judgeMode" onClick="changePad();"><img src="../bilder/buttons/change.png" height="20" /></button>
<button class="judgeMode" id="soundmode" onClick="changeSoundOutput();"><img src="../bilder/buttons/sound.png" height="20" /></button>
</th>
</tr>
<tr>
<td class="left">
<table class='wertungen'>
<tr>
<th class='headline'>#</th><th class='headline'>Manoeuvre</th><th class='headline'>Score</th></tr>
<?php
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm JOIN durchgang_programm dp ON dp.programm = p.id WHERE dp.durchgang = (SELECT min(durchgang) FROM wettkampf_leitung)";
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));
for($i=0;$i<$obj_count_figur->anzahl;$i++){
	if((($i + 1) % 2) == 0)echo "<tr class='grey'>";
	else echo "<tr>";
	echo "<td onClick='changeS(".$i.");'>".str_pad(($i + 1),2,0,STR_PAD_LEFT)."</td>";
	echo "<td onClick='changeS(".$i.");' id='fname".$i."' style='max-width:500px;white-space: nowrap; overflow:auto;'></td>";
	echo "<td onClick='changeS(".$i.");' class='score' id='fscore".$i."'></td></tr>";
}
?>
</table>
</td>
<td class="right" id="right">
<table class="panel">
<tr>
<th colspan="3" class="teilnehmer" id="teilnehmer"></th>
</tr>
<tr>
<th colspan="3" class="figur" id="recent_figur"></th></tr>
<tr>
<td><button id="p1" onClick="save(1);">1</button></td>
<td><button id="p2" onClick="save(2);">2</button></td>
<td><button id="p3" onClick="save(3);">3</button></td>
</tr>

<tr align="center">
<td><button id="p4" onClick="save(4);">4</button></td>
<td><button id="p5" onClick="save(5);">5</button></td>
<td><button id="p6" onClick="save(6);">6</button></td>
</tr>
<tr align="center">
<td><button id="p7" onClick="save(7);">7</button></td>
<td><button id="p8" onClick="save(8);">8</button></td>
<td><button id="p9" onClick="save(9);">9</button></td>
</tr>
<tr align="center">
<td><button id="p10" onClick="save(10);">10</button></td>
<td><button id="p0" onClick="save(0);">00</button></td>
<td><button id="pno" onClick="save(-1);">no</button></td>
</tr>
</table>
</td></tr>
</table>
</body>
