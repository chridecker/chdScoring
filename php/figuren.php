<?php
require_once("host.inc");
?>
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();

<?php
$query_programm = "SELECT min(id) as min FROM programm ORDER BY id ASC";
$res_programm = mysqli_query($link,$query_programm);
if($obj_programm = mysqli_fetch_object($res_programm))echo "var programm = ".$obj_programm->min.";";
?>

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	var file = "operations/figuren_liste.php?programm=" + programm;
	if(xmlHttp){
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
function deleteProgramm(){
	var file = "operations/add_delete_programm.php?programm=" + programm;
	var check = confirm("Do really want to delete the chosen program?");
	//if(programm <= 9){check = false; alert("This is a standart programm and can not be deleted");}
	if(xmlHttp2 && check){
		try{
			xmlHttp2.open("GET",file,true);
			xmlHttp2.send(null);
			window.location.reload();
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function addProgramm(){
	var anzahl = prompt("How many manoeuvres should the new program contain?");
	var check = confirm("Do really want to add a new custom program with " + anzahl + " manoeuvres?");
	var file = "operations/add_delete_programm.php?figuren=" + anzahl;
	if(xmlHttp2 && check){
		try{
			xmlHttp2.open("GET",file,true);
			xmlHttp2.send(null);
			window.location.reload();
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}


function loadProgramm(){
	programm = document.getElementById("programm").value;
	load();
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
function openEditor(){
	
		var win = window.open("programm_editor.php?programm=" + programm,"_blank");
		win.focus();
}
function printProgramm(){
	var win = window.open("output/programm.php?programm=" + programm,"_blank");
	win.print();
	win.focus();
}
function update(){
	var win = window.open("operations/update_programm.php","_blank");
	win.focus();
}
function printAresti(){
	var win = window.open("print/?file=aresti_programm&programm=" + programm,"_blank");
	win.print();
	win.focus();
}

window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}

</script>
<link rel="stylesheet" type="text/css" href="css/figuren.css">
<body onLoad="load();">
<table>
<tr class="header">
<th colspan="3">Flight-Program Editor</th></tr>
<tr class="headline">
<th><input type="button" onClick="window.close();" value="Close"></th>
<th>
<select id="programm" onChange="loadProgramm();">
<?php
$query_programm = "SELECT * FROM programm ORDER BY id ASC";
$res_programm = mysqli_query($link,$query_programm);
while($obj_programm = mysqli_fetch_object($res_programm))echo "<option value='".$obj_programm->id."'>".$obj_programm->title." (".$obj_programm->description.")";
?>
</select></th>
<th>
<button title="Print Aresti" onClick="printAresti();"><img src="bilder/buttons/aresti.png" height="15"></button>
<button title="Print Schedule" onClick="printProgramm();"><img src="bilder/buttons/print.png" height="15"></button>
<button title="Edit Schedule" onClick="openEditor();"><img src="bilder/buttons/edit.png" height="15"></button>
<button title="Add new Schedule" onClick="addProgramm();"><img src="bilder/buttons/new.png" height="15"></button>
<button title="Delete Schedule" onClick="deleteProgramm();"><img src="bilder/buttons/delete.png" height="15"></button>
<button title="Update" onClick="update();"><img src="bilder/buttons/update.png" height="15"></button></th>
</tr>
</table>
<div id="daten"></div>
</body>