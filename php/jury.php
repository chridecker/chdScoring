<?php
require_once("host.inc");?>
<style type="text/css">
body {
	margin:0px;
	font-size:250pt;
	font-weight:bold;
	text-align:center;
}
button {
	background-color:lightgrey;
}
button img{
	height:300px;
}
</style>
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function process(){
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","jury_load.php",true);
			xmlHttp2.onreadystatechange = function () {handleRequestStateChange();};
			xmlHttp2.send(null)
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function handleRequestStateChange(){
	var myDiv = document.getElementById('main');
	if(xmlHttp2.readyState == 4){
		if(xmlHttp2.status == 200){
			var response = xmlHttp2.responseText;
			myDiv.innerHTML = response;
		}
	}
}


function timer(action,airfield){
	if(xmlHttp){
		try{
			xmlHttp.open("GET","operations/timer.php?action=" + action + "&airfield=" + airfield,true);
			xmlHttp.send(null)
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
setInterval(function(){process();},500);
</script>
<div id="main">

</div>