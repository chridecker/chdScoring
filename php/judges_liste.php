<?php
require_once("host.inc");

?>
<link rel="stylesheet" type="text/css" href="css/judges_liste.css">
<script type="text/javascript">

var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();
var id = "";
var name = "";
var vorname = "";
var license = "";
var pin = "";
function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	if(xmlHttp){
		var file = "operations/judges_edit.php?id=" + id + "&name=" + name + "&vorname=" + vorname + "&license=" + license + "&pin=" + pin;
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
	myDiv = document.getElementById('judges');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
		}
	}
}
function speichern(i){
	id = i;
	name = document.getElementById("name" + id).value;
	vorname = document.getElementById("vorname" + id).value;
	license = document.getElementById("license" + id).value;
	pin = document.getElementById("pin" + id).value;
	if(name	 != '' )load();
	else alert("Bitte vollständige Eingabe machen");
	if(i == '*') i = "";
}
function del(i){
	id = i;
	if(confirm("Möchten Sie den Judge Nr." + id + " wirklich löschen?")){
		if(xmlHttp){
			var file = "operations/judges_edit.php?id=del&del=" + id;			
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
}
function randomCode(judge){
	if(xmlHttp2){
		var file = "operations/judge_code.php?judge=" + judge;
		try{
			xmlHttp2.open("GET",file,true);
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
	load();
}
window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}
function oeffnen(datei){
	var win = window.open(datei,"_blank");
	win.focus();
}

</script>
<body onLoad="load();">
<div id="judges"></div>
</body>
