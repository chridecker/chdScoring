<?php
require_once("host.inc");
if(isset($_GET["db"])){$link = mysqli_connect($host,$user,$password,$_GET["db"]);}
$durchgaenge = $result_config->durchgaenge;
?>
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var teilnehmer = 0;
var durchgang = 0;
function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}
function load(){
	if(teilnehmer == 0){
		var t = document.getElementById("teilnehmer");
		teilnehmer = t.options[t.selectedIndex].value;
		var d = document.getElementById("durchgang");
		durchgang = d.options[d.selectedIndex].value;
	}
	if(xmlHttp){
		try{
			xmlHttp.open("GET","operations/print.php?teilnehmer=" + teilnehmer + "&durchgang=" + durchgang,true);
			xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp.send(null);
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
			teilnehmer = 0;
			durchgang = 0;
		}
	}
}
function printPage(){
	document.getElementById("teilnehmer").style.display="none";
	document.getElementById("durchgang").style.display="none";
	document.getElementById("printer").style.display="none";
	window.print();
}
window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(e.ctrlKey && key === 80)document.getElementById('printer').click();
	if(key == 27)window.close();
	if(key == 114){
		e.preventDefault();
		teilnehmer = prompt("Teilnehmer License");
		durchgang = prompt("Durchgang");
		load();
		//printPage();
	}
}

</script>
<link rel="stylesheet" type="text/css" href="css/print.css" />

<body onLoad="load();">
<select id="teilnehmer" onChange="load();">
<?php 
$query_teilnehmer = "SELECT * FROM teilnehmer";
$res_teilnehmer = mysqli_query($link,$query_teilnehmer);
while($teilnehmer = mysqli_fetch_object($res_teilnehmer)){?>
	<option value="<?php echo $teilnehmer->id; ?>"> <?php echo $teilnehmer->id." - ". $teilnehmer->vorname." ".$teilnehmer->nachname;?></option>
    <?php
}?>
</select>
<select id="durchgang" onChange="load();">
<?php 
for($durchgang = 1;$durchgang<=$durchgaenge;$durchgang++){?>
	<option value="<?php echo $durchgang;?>"><?php echo $durchgang;?></option>
    <?php
}?>
</select>
<input type="button" value="Drucken" onClick="printPage();" id="printer">
<div id="daten"></div>
