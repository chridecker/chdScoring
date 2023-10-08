<?php
require_once("host.inc");
?>
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	if(xmlHttp){
		var teilnehmer = document.getElementById("teilnehmer").value;
		var durchgang = document.getElementById("durchgang").value;
		try{
			xmlHttp.open("GET","operations/korrektur_daten.php?teilnehmer=" + teilnehmer + "&durchgang=" + durchgang,true);
			xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function speichern(teilnehmer,durchgang,figur,judge,id){
	var wert = document.getElementById(id).value;
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/korrektur_speichern.php?teilnehmer=" + teilnehmer + "&durchgang=" + durchgang + "&figur=" + figur + "&judge=" + judge + "&wert=" + wert,true);
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
<link rel="stylesheet" type="text/css" href="css/korrektur.css">
<body onLoad="load()">
<table class="select">
<tr class="headline"><th>Edit Scorings</th></tr>
<tr class="header">
<th>
<select id="teilnehmer" onChange="load();">
<?php 
$query_teilnehmer = "SELECT * FROM teilnehmer ORDER BY id";
$res_teilnehmer = mysqli_query($link,$query_teilnehmer);
while($teilnehmer = mysqli_fetch_object($res_teilnehmer)){?>
	<option value="<?php echo $teilnehmer->id;?>"><?php echo $teilnehmer->id." - ".$teilnehmer->vorname." ".$teilnehmer->nachname;?></option>
    <?php
}?>
</select>
<select id="durchgang" onChange="load();">
<?php 
for($durchgang = 1;$durchgang<=$durchgaenge;$durchgang++){?>
	<option value="<?php echo $durchgang;?>">Round <?php echo $durchgang;?></option>
    <?php
}?>
</select>
</th></tr>
</table>
<div id="daten"></div>
</body>
