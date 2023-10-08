<?php
require_once("host.inc");

?>
<link rel="stylesheet" type="text/css" href="css/subevent.css">
<script type="text/javascript">

var xmlHttp = createXmlHttpRequestObject();
var id = "";
var name ="";
var number ="";

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	if(xmlHttp){
		var file = "operations/subevent_edit.php?id=" + id + "&name=" + name + "&number=" + number;
		
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
	myDiv = document.getElementById('subevent');
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
	number = document.getElementById("number" + id).value;
	if(name	 != '' )load();
	else alert("Bitte vollständige Eingabe machen");
}
function del(i){
	id = "del";
	if(xmlHttp){
		var file = "operations/subevent_edit.php?id=" + id + "&del=" + i;
		//alert(file);
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
	

window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}
	
</script>
<body onLoad="load();">
<div id="subevent"></div>
</body>