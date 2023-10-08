<?php
require_once("host.inc");

?>
<link rel="stylesheet" type="text/css" href="css/edit_images.css">
<script type="text/javascript">

var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();

var title = "";
var id = "";
var profile = false;
var official = false;
var sponsor = false;

var form;
var file;

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	if(xmlHttp){
		var file = "operations/edit_images.php?id=" + id + "&title=" + title + "&profile=" + profile + "&sponsor=" + sponsor + "&official=" + official;
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
function changeLogo(img_id){
	if(xmlHttp2){
		var file = "operations/edit_images.php?img_id=" + img_id + "&logo=1";
		try{
			xmlHttp2.open("GET",file,true);
			//xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
		load();
	}
}
function changeUrkunde(img_id){
	if(xmlHttp2){
		var file = "operations/edit_images.php?img_id=" + img_id + "&urkunde=1";
		try{
			xmlHttp2.open("GET",file,true);
			//xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
		
	}
}
function changeFed(img_id){
	if(xmlHttp2){
		var file = "operations/edit_images.php?img_id=" + img_id + "&federation=1";
		try{
			xmlHttp2.open("GET",file,true);
			//xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
		load();
	}
}
function changeGruppe(img_id){
	if(xmlHttp2){
		var file = "operations/edit_images.php?img_id=" + img_id + "&gruppe=1";;
		try{
			xmlHttp2.open("GET",file,true);
			//xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
		load();
	}
}

function handleRequestStateChange(){
	myDiv = document.getElementById('images');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
		}
	}
}


function speichern(i){
	id = i;
	title = document.getElementById("title" + id).value;
	profile = document.getElementById("profile" + id).checked;
	official = document.getElementById("official" + id).checked;
	sponsor = document.getElementById("sponsor" + id).checked;
	if(profile)profile = 1;
	else profile = 0;
	if(official)official = 1;
	else official = 0;
	if(sponsor)sponsor = 1;
	else sponsor = 0;
	if(title != '')load();
	else alert("Bitte vollstaendige Eingabe machen");
}

function del(i){
	id = i;
	if(confirm("Möchten Sie den Teilnehmer mit der Startnummer " + id + " wirklich löschen?")){
		if(xmlHttp){
			var file = "operations/edit_images.php?id=del&del=" + id;			
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

function oeffnen(datei){
	var win = window.open(datei,"_blank");
	win.focus();
}
window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}
</script>
<body onLoad="load();">
<div id="images"></div>
</body>