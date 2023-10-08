<?php 
require_once("../host.inc");
if(isset($_GET['file'])) $file = $_GET['file'];
else $file = "guest";
if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
?>
<link rel="stylesheet" type="text/css" href="../css/info.css" />
<script type="text/javascript">
var x = new Array();
<?php
$res = mysqli_query($link,"SELECT img_id FROM images WHERE img_sponsor = 1 ORDER BY img_id ASC");
while($id = mysqli_fetch_object($res)){?>
	x.push('<?php echo $id->img_id;?>');
	<?php
}?>
var y = 0;
var startLimit = 0;
function rotate(){
	startLimit++;
	if(startLimit == 3){startLimit = 0;}
	var img = document.getElementById("info_img");
	if(img != null){
		//img.src="../operations/load_image.php?id=" + x[y];
		y++;
		if(y == x.length)y = 0;
	}
}
setInterval(rotate,8000);
var xmlHttp = createXmlHttpRequestObject();
function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}
function load(){
	if(xmlHttp){
		var file = "<?php if($file == "live" || $file == "live_tbl"){ echo "../".$file;}
							else if($file != "guest"){echo "../output/".$file;} 
							else {echo $file;}?>.php?airfield=<?php echo $airfield;?>&info_img=" + x[y] + "&startLimit=" + startLimit + "&rowLimit=10";
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
	myDiv = document.getElementById('live');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
		}
	}
}
function oeffnen(datei){
	var win = window.open(datei,"_blank");
	win.focus();
}
setInterval(function () {load()}, 200);
</script>
<link rel="stylesheet" type="text/css" href="../css/public_<?php echo $file; ?>.css">
<link rel="stylesheet" type="text/css" href="../css/public_info.css">
<body onLoad="load();">
<div id="live"></div>
</body>
