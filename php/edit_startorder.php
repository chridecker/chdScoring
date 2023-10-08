<?php
require_once("host.inc");?>

<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var id = 0;
var durchgang = <?php echo $_GET['durchgang'];?>;
var start = 0;
var change = 0;
function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(i){
	if(i != 0){
		id = i;
		start = document.getElementById('start' + i).value;
		change = 1;
	}
	if(xmlHttp){
		var file = "operations/edit_startorder.php?id=" + id + "&start=" + start + "&durchgang=" + durchgang + "&change=" + change;
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
	myDiv = document.getElementById('div');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
			id = 0;
			start = 0;
			change = 0;
		}
	}
}
window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}
</script>
<style type="text/css">
body {
	margin:0px;
}
table {
	width:500px;
}
tr.headline th{
	background-color:lightblue;
	font-size:25pt;
	border-bottom: 1px solid black;
}
th{
	font-size:16pt;
	padding:5px;
}
td {
	padding:2px;
	text-align:center;
	font-size:14pt;
}
td input[type=text]	{
	text-align:center;
	width: 30px;
	font-size:14pt;
	font-weight:bold;
}
tr.grey td{
	background-color:lightgrey;
}
</style>
<body onLoad="load(0);">
<div id="div"></div>
</body>