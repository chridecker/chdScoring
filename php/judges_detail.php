<?php
require_once("host.inc");?>
<link rel="stylesheet" type="text/css" href="css/teilnehmer_liste.css">
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();
var xmlHttp4 = createXmlHttpRequestObject();

var vorname = "";
var nachname = "";
var club = "";
var land = "";
var license = "";
var bild = "";
var id = "";
var change = 0;

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(i){
	id = i;
	if(xmlHttp){
		var file = "operations/judges_detail.php?id=" + id + "&vorname=" + vorname + "&nachname=" + nachname + "&club=" + club + "&land=" + land + "&license=" + license + "&bild=" + bild + "&change=" + change;
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
	myDiv = document.getElementById('judge');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
			change = 0;
		}
	}
}

function changeAreaAccess(area,id,type){
	if(xmlHttp4){
		var file = "operations/edit_area_access.php?type=" + type + "&id=" + id + "&area=" + area;
		//alert(file);
		try{
			xmlHttp4.open("GET",file,true);
			//xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp4.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
		load(id);
	}
}

function speichern(){
	vorname = document.getElementById("vn").value;
	nachname = document.getElementById("nn").value;
	club = document.getElementById("c").value;
	land = document.getElementById("l").value;
	license = document.getElementById("li").value;
	bild = document.getElementById("bild").value;
	if(vorname != '' && nachname !='' && club != '' && land != '' && license != ''){
		change = 1;
		load(id);
		change = 0;
	}
	else alert("Bitte vollstaendige Eingabe machen");
}

function oeffnen(datei){
	var win = window.open(datei,"_blank");
	win.focus();
}

window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
	if(e.ctrlKey && key === 80)document.getElementById('printer').click();
}

</script>
<link rel="stylesheet" type="text/css" href="css/teilnehmer_detail.css">
<?php 
if(isset($_GET['id'])) $id = $_GET['id'];
else $id = "0";
if(strlen($id) > 3)$id = getIdFromLic($id,$link);
if($id == 0)exit;
?>
<body onLoad="load(<?php echo $id;?>);">
<div id="judge"></div>
</body>