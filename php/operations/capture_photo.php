<?php
if(isset($_GET['id']))$id = $_GET['id'];
else $id = 0;
if(isset($_GET['pilot']))$pilot = $_GET['pilot'];
else $pilot= 1;
if(isset($_GET['official']))$official = $_GET['official'];
else $official= 0;
if(isset($_GET['name']))$name = $_GET['name'];
else $name = "";?>
<!-- 640x480 -->
<!-- 960x720 -->
<b style="font-size:20pt; margin-left:400px; margin-bottom:10px;">Name: </b><input style="font-size:20pt; border:1px solid black; background-color:lightgrey; margin-bottom:5px;" type="text" id="title" value="<?php echo $name;?>" /><br />
<button id="snap" style="margin-bottom:5px; margin-left:550px; background-color:white; border:1px solid black;">
<img src="../bilder/buttons/live.png" height="40px;" /></button><br />
<div style="opacity:0.95; position: absolute; height:720; width:260px; background-color:black; top:96px; left:108px;"></div>
<div style="border: solid red; opacity:0.7; position: absolute; height:720; width:503; background-color:none; top:93px; left:364px;"></div>
<div style="opacity:0.95; position: absolute; height:720; width:198px; background-color:black; top:96px; left:871px;"></div>
<video style="margin-left:100px;" id="video" width="960" height="720" autoplay></video>
<canvas id="canvas" width="340" height="480"></canvas>
<script type="text/javascript">
var id = <?php echo $id;?>;
var pilot = <?php echo $pilot;?>;
var official = <?php echo $official;?>;
// Put event listeners into place
window.addEventListener("DOMContentLoaded", function() {
	// Grab elements, create settings, etc.
	var canvas = document.getElementById("canvas"),
		context = canvas.getContext("2d"),
		video = document.getElementById("video");

	// Put video listeners into place
	if(navigator.mediaDevices && navigator.mediaDevices.getUserMedia) { // Standard
		navigator.mediaDevices.getUserMedia({ video : true}).then(function(stream) {
			video.src = window.URL.createObjectURL(stream);
			video.play();
		});
	} else if(navigator.webkitGetUserMedia) { // WebKit-prefixed
		navigator.webkitGetUserMedia(videoObj, function(stream){
			video.src = window.webkitURL.createObjectURL(stream);
			video.play();
		}, errBack);
	} else if(navigator.mozGetUserMedia) { // WebKit-prefixed
		navigator.mozGetUserMedia(videoObj, function(stream){
			video.src = window.URL.createObjectURL(stream);
			video.play();
		}, errBack);
	}

	// Trigger photo take
	document.getElementById("snap").addEventListener("click", function() {
		context.drawImage(video, 170, 0, 960, 720, 0, 0, 960, 720);
		var image = canvas.toDataURL("image/png");
		upload(image);
	});
}, false);

var xmlHttp2 = createXmlHttpRequestObject();

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function upload(data){
	if(xmlHttp2){
		var file = "upload_captured_photo.php";
		var title = document.getElementById('title').value;
		var formData = new FormData();
		formData.append("data",data)
		formData.append("title",title);
		formData.append("id",id);
		formData.append("pilot",pilot);
		formData.append("official",official);
		try{
			//alert(title + " " + id);
			xmlHttp2.open("POST",file,true);
			xmlHttp2.onreadystatechange = handleRequestStateChange;
			xmlHttp2.send(formData);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function handleRequestStateChange(){
	if(xmlHttp2.readyState == 4){
		if(xmlHttp2.status == 200){
			image = null;
			setTimeout(window.close(),500);
			
		}
	}
}
window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
	if(key == 13)document.getElementById('snap').click();
}
</script>
