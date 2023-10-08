<?php
require_once("host.inc");
if(isset($_GET['programm']))$programm = $_GET['programm'];
else $programm = 1;
?>

<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();
var programm = <?php echo $programm;?>;
function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	if(xmlHttp){
		try{
			xmlHttp.open("GET","operations/edit_programm.php?programm=" + programm,true);
			xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function speichern(id){
	var name = document.getElementById("f" + id).value;
	var wert = document.getElementById("k" + id).value;
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/edit_programm.php?programm=" + programm + "&id=" + id + "&name=" + name + "&wert=" + wert,true);
			xmlHttp2.send(null);
			load();
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function speicherProgramm(){
	var name = document.getElementById("programm_name").value;
	var description = document.getElementById("programm_desc").value;
	var file = "operations/edit_programm.php?programm=" + programm + "&name=" + name + "&description=" + description;
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET",file,true);
			xmlHttp2.send(null);
			load();
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function handleRequestStateChange(){
	myDiv = document.getElementById('daten');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
		}
	}
}
window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}

</script>
<link rel="stylesheet" type="text/css" href="css/figuren.css">
<body onLoad="load();">
<div id="daten"></div>
</body>