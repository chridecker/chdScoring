<?php
require_once("host.inc");?>
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();
var id = "1";
var turnier = "";
var klasse = "";
var durchgaenge = "";
var federation = "";
var jury = "";
var org_leiter = "";
var wettkampf_leiter = "";
var veranstalter = "";
var veranstalter_web = "";
var ort = "";
var number = "";
var datum = "";
var end_datum = "";
var info = "";
var score_mode = "";
var judge_pin = "";
var del_on = "";
var edit = "";
var final = "";
var final_durchgang = "";
var final_teilnehmer = "";
var three_panel_final = "";
var end_final = "";
var end_final_durchgang = "";
var end_final_teilnehmer = "";
var airfields = "";

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	if(xmlHttp){
		var file = "operations/stammdaten_edit.php?id=" + id + "&turnier=" + turnier + "&durchgaenge=" + durchgaenge + "&federation=" + federation + "&jury=" + jury + "&org_leiter=" + org_leiter + "&wettkampf_leiter=" + wettkampf_leiter + "&veranstalter=" + veranstalter + "&veranstalter_web=" + veranstalter_web + "&ort=" + ort + "&number=" + number + "&datum=" + datum + "&end_datum=" + end_datum + "&info=" + info + "&score_mode=" + score_mode + "&final=" + final + "&final_durchgang=" + final_durchgang + "&final_teilnehmer=" + final_teilnehmer + "&end_final=" + end_final + "&end_final_durchgang=" + end_final_durchgang + "&end_final_teilnehmer=" + end_final_teilnehmer + "&airfields=" + airfields + "&judge_pin=" + judge_pin + "&three_panel_final=" + three_panel_final + "&del_on=" + del_on + "&edit=" + edit + "&klasse=" + klasse;
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
function handleRequestStateChange(){
	myDiv = document.getElementById('stammdaten');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
		}
	}
}
function speichern(i){
	id = 1;
	turnier = document.getElementById("turnier" + id).value;
	durchgaenge = document.getElementById("durchgaenge" + id).value;
	jury = document.getElementById("jury" + id).value;
	klasse = document.getElementById("klasse" + id).value;
	federation = document.getElementById("federation" + id).value;
	org_leiter = document.getElementById("org_leiter" + id).value;
	wettkampf_leiter = document.getElementById("wettkampf_leiter" + id).value;
	veranstalter = document.getElementById("veranstalter" + id).value;
	veranstalter_web = document.getElementById("veranstalter_web" + id).value;
	ort = document.getElementById("ort" + id).value;
	number = document.getElementById("number" + id).value;
	datum = document.getElementById("datum" + id).value;
	end_datum = document.getElementById("end_datum" + id).value;
	if(document.getElementById("score_mode" + id).checked) score_mode = 1;
	else score_mode = 0;
	if(document.getElementById("judge_pin" + id).checked) judge_pin = 1;
	else judge_pin = 0;
	if(document.getElementById("edit" + id).checked) edit = 1;
	else edit = 0;
	if(document.getElementById("del_on" + id).checked) del_on = 1;
	else del_on = 0;
	if(document.getElementById("three_panel_final" + id).checked) three_panel_final = 1;
	else three_panel_final = 0;
	if(document.getElementById("final" + id).checked) final = 1;
	else final = 0;
	final_teilnehmer = document.getElementById("final_teilnehmer" + id).value;
	final_durchgang = document.getElementById("final_durchgang" + id).value;
	end_final_teilnehmer = document.getElementById("end_final_teilnehmer" + id).value;
	end_final_durchgang = document.getElementById("end_final_durchgang" + id).value;
	if(document.getElementById("end_final" + id).checked) end_final = 1;
	else end_final = 0;
	airfields = document.getElementById("airfields" + id).value;
	
	if(turnier != '' && durchgaenge != '' && number != '')load();
	else alert("Bitte vollständige Eingabe machen");
}
function changeProgramm(durchgang){
	var programm = document.getElementById("prog_round_" + durchgang).value;
	if(xmlHttp2){
		var file = "operations/durchgang_programm.php?durchgang=" + durchgang + "&programm=" + programm;
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
function changeAirfield(durchgang){
	var airfield = document.getElementById("airfield_round_" + durchgang).value;
	if(xmlHttp2){
		var file = "operations/durchgang_airfield.php?durchgang=" + durchgang + "&airfield=" + airfield;
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
function changePanel(durchgang,lmr){
	if(lmr){
		var l = document.getElementById("panel_round_" + durchgang + "_1").value;
		var m = document.getElementById("panel_round_" + durchgang + "_2").value;
		var r = document.getElementById("panel_round_" + durchgang + "_3").value;
		if(xmlHttp2){
			var file = "operations/durchgang_panel.php?durchgang=" + durchgang + "&l=" + l + "&m=" + m +"&r=" + r + "&lmr=" + lmr;
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
	else {
		var panel = document.getElementById("panel_round_" + durchgang).value;
		if(xmlHttp2){
			var file = "operations/durchgang_panel.php?durchgang=" + durchgang + "&panel=" + panel + "&lmr=" + lmr;
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
}

window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}
</script>
<link rel="stylesheet" type="text/css" href="css/stammdaten.css" />
<body onLoad="load();">
<div id="stammdaten"></div>
</body>
