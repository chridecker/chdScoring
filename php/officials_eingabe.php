<?php
require_once("host.inc");

?>
<link rel="stylesheet" type="text/css" href="css/teilnehmer_liste.css">
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();
var vorname = "";
var nachname = "";
var club = "";
var land = "";
var license = "";
var id = "";

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	if(xmlHttp){
		var file = "operations/officials_liste.php?id=" + id + "&vorname=" + vorname + "&nachname=" + nachname + "&club=" + club + "&land=" + land + "&license=" + license;
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
	myDiv = document.getElementById('officials');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
		}
	}
}


function speichern(i){
	id = i;
	vorname = document.getElementById("vn" + id).value;
	nachname = document.getElementById("nn" + id).value;
	club = document.getElementById("c" + id).value;
	land = document.getElementById("l" + id).value;
	license = document.getElementById("li" + id).value;
	if(vorname != '' && nachname !='' && club != '' && land != '' && license != '')load();
	else alert("Bitte vollstaendige Eingabe machen");
}
function del(i){
	id = i;
	if(confirm("Do you want to delete the offical?")){
		if(xmlHttp){
			var file = "operations/officials_liste.php?id=del&del=" + id;			
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
<div id="officials"></div>
</body>