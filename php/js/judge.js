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
	document.getElementById("judge_name").innerHTML = scores.getAttribute('name');
	durchgang = scores.getAttribute('durchgang');
	var figuren = scores.getElementsByTagName('figuren')[0];
	var anzahl = figuren.getAttribute('anzahl');
	var start = figuren.getAttribute('start');
	var teilnehmer = scores.getElementsByTagName('teilnehmer')[0];
	if(recent_teilnehmer != teilnehmer.getAttribute('id')) clearScores(anzahl);
	recent_teilnehmer = teilnehmer.getAttribute('id');
	for(var i = 0;i < anzahl;i++){
		if(figuren.getElementsByTagName('figur')[i].getAttribute('recent') == "true")recent_figur = figuren.getElementsByTagName('figur')[i];
		document.getElementById("fname" + i).innerHTML = figuren.getElementsByTagName('figur')[i].getAttribute('name');
		document.getElementById("fname" + i).style.maxWidth = "500px";
		if(figuren.getElementsByTagName('figur')[i].getAttribute('score') != null)document.getElementById("fscore" + i).innerHTML = figuren.getElementsByTagName('figur')[i].getAttribute('score');
	}
	document.getElementById("teilnehmer").innerHTML = recent_teilnehmer + " - " + teilnehmer.getAttribute('name');
	if(recent_figur != null){
		document.getElementById("recent_figur").innerHTML = "#" + (recent_figur.getAttribute('id') - start + 1) + " - " + recent_figur.getAttribute('name');
		recent_figur = recent_figur.getAttribute('id') - start + 1;
	}
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

function save(wert){
	if(wert == 99) wert = Math.round(wertung);
	var file = "../operations/save_wertung.php?teilnehmer=" + recent_teilnehmer + "&durchgang=" + durchgang + "&figur=" + recent_figur + "&judge=" + judge + "&wert=" + wert;
	//sprechen(wert + "gespeichert");
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET",file,true);
			xmlHttp2.send(null);
			wertung = 10;
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
	if(value != 0)sprechen(wertung);
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

function sprechen(text){
	if(text == -1) text = "N O";
	if(soundOutput){
		var speaker = new SpeechSynthesisUtterance();
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
		sprechen("Ton an");
		document.getElementById('soundmode').style.backgroundColor = "lightblue";
	}
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
	else if(judgePad == 'numblock'){
		switch(key){
			case 96:
			case 48:
				document.getElementById("p0").click();
				break;
			case 97:
			case 49:
				document.getElementById("p1").click();
				break;
			case 98:
			case 50:
				document.getElementById("p2").click();
				break;
			case 99:
			case 51:
				document.getElementById("p3").click();
				 break;
			case 100:
			case 52:
				document.getElementById("p4").click();
				 break;
			case 101:
			case 53:
				document.getElementById("p5").click();
				 break;
			case 102:
			case 54:
				document.getElementById("p6").click();
				 break;
			case 103:
			case 55:
				document.getElementById("p7").click();
				 break;
			case 104:
			case 56:
				document.getElementById("p8").click();
				 break;
			case 105:
			case 57:
				document.getElementById("p9").click();
				 break;
			case 106:
			case 58:
				document.getElementById("p10").click();
				 break;
			case 111:
				document.getElementById("no").click();
				 break;
			default:
				break;
		}
	}

}