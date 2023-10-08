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
var tournament = "";
var id = "";
var searchterm = "";

<?php for($i=2;$i<=$subevents;$i++)echo "var sub".$i." = '';";?>

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	if(xmlHttp){
		var file = "operations/teilnehmer_liste.php?id=" + id + "&vorname=" + vorname + "&nachname=" + nachname + "&club=" + club + "&land=" + land + "&license=" + license + "&tournament="  + tournament + "&search=" + searchterm;;
		file = file + "<?php for($i=2;$i<=$subevents;$i++)echo "&sub".$i."=\" + sub".$i." + \"";?>";
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
	myDiv = document.getElementById('teilnehmer');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
		}
	}
}

function searchTerm(){
	searchterm = document.getElementById("searchterm").value;
	load();
}
function speichern(i){
	id = i;
	vorname = document.getElementById("vn" + id).value;
	nachname = document.getElementById("nn" + id).value;
	club = document.getElementById("c" + id).value;
	land = document.getElementById("l" + id).value;
	license = document.getElementById("li" + id).value;
	tournament = true;
	<?php for($i=2;$i<=$subevents;$i++){
		echo "sub".$i;?> = document.getElementById("sub<?php echo $i."";?>" + i).checked;
		<?php
	}?>
	if(vorname != '' && nachname !='' && club != '' && land != '' && license != '')load();
	else alert("Bitte vollstaendige Eingabe machen");
}
function del(i){
	id = i;
	if(confirm("Möchten Sie den Teilnehmer mit der Startnummer " + id + " wirklich löschen?")){
		if(xmlHttp){
			var file = "operations/teilnehmer_liste.php?id=del&del=" + id;			
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
function randomStarter(){
	if(xmlHttp2){
		var file = "operations/teilnehmer_random.php";	
		try{
			xmlHttp2.open("GET",file,true);
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
	load();
}
function changeID(direction,id){
	if(xmlHttp2){
		var file = "operations/teilnehmer_changeID.php?id=" + id + "&direction=" + direction;	
		try{
			xmlHttp2.open("GET",file,true);
			//xmlHttp2.onreadystatechange = load();
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
	load();
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
<div id="teilnehmer"></div>
</body>