<?php
require_once("host.inc");?>
<link rel="stylesheet" type="text/css" href="css/teilnehmer_liste.css">
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();
var xmlHttp3 = createXmlHttpRequestObject();
var xmlHttp4 = createXmlHttpRequestObject();

var vorname = "";
var nachname = "";
var club = "";
var land = "";
var license = "";
var email = "";
var telefon = "";
var strasse = "";
var plz = "";
var ort = "";
var bild = "";
var id = <?php echo $_GET['id'];?>;
var info = "";
var change = 0;

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(i){
	if(id == 0)var n = "_new";
	else var n = "";
	id = i;
	if(xmlHttp){
		var file = "operations/teilnehmer_detail" + n + ".php?id=" + id + "&vorname=" + vorname + "&nachname=" + nachname + "&club=" + club + "&land=" + land + "&license=" + license + "&email="  + email + "&telefon=" + telefon + "&plz=" + plz + "&ort=" + ort + "&strasse=" + strasse + "&bild=" + bild + "&info=" + info + "&change=" + change;
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
function modelAircraft(ma_id){
	var name = document.getElementById("ma_name_" + ma_id).value;
	var wingspan = document.getElementById("ma_wingspan_" + ma_id).value;
	var length = document.getElementById("ma_length_" + ma_id).value;
	var weight = document.getElementById("ma_weight_" + ma_id).value;
	var prop = document.getElementById("ma_prop_" + ma_id).value;
	var ident = document.getElementById("ma_ident_" + ma_id).checked;
	if(ident)ident = 1;
	else ident = 0;
	if(xmlHttp3){
		var file = "operations/edit_aircraft.php?teilnehmer=" + id + "&id=" + ma_id + "&name=" + name + "&wingspan=" + wingspan + "&length=" + length + "&weight=" + weight + "&prop=" + prop + "&ident=" + ident + "&change=1";
		try{
			xmlHttp3.open("GET",file,true);
			//xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp3.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
		speichern();
	}
}
function addModelAircraft(){
	if(xmlHttp3){
		var file = "operations/edit_aircraft.php?teilnehmer=" + id + "&new=1";
		try{
			xmlHttp3.open("GET",file,true);
			//xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp3.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
		speichern();
	}
}
function deleteModelAircraft(ma){
	var check = confirm("Do you really want to delete this model aircraft?");
	if(xmlHttp3 && check){
		var file = "operations/edit_aircraft.php?id=" + ma + "&delete=1";
		try{
			xmlHttp3.open("GET",file,true);
			//xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp3.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
		speichern();
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
	
function handleRequestStateChange(){
	myDiv = document.getElementById('teilnehmer');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			if(response == "SAVED")load(id);
			else myDiv.innerHTML = response;
			change = 0;
		}
	}
}


function speichern(){
	vorname = document.getElementById("vn").value;
	nachname = document.getElementById("nn").value;
	club = document.getElementById("c").value;
	land = document.getElementById("l").value;
	license = document.getElementById("li").value;
	email = document.getElementById("email").value;
	telefon = document.getElementById("telefon").value;
	strasse = document.getElementById("strasse").value;
	ort = document.getElementById("ort").value;
	plz = document.getElementById("plz").value;
	bild = document.getElementById("bild").value;
	info = document.getElementById("info").value;
	if(vorname != '' && nachname !='' && club != '' && land != '' && license != ''){
		change = 1;
		if(id == 0){
			bild = 1;
			load(document.getElementById('id').value);
		}
		else load(id);
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
else $id = "*";
if(strlen($id) > 3)$id = getIdFromLic($id,$link);
if($id == "*")exit;
?>
<body onLoad="load(<?php echo $id;?>);">
<div id="teilnehmer"></div>
</body>