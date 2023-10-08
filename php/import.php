<?php
require_once("host.inc");
$link2 = mysqli_connect($host,$user,$password);

if(isset($_GET['type']))$type = $_GET['type'];
else $type = "pilot";
?>
<head>
<script type="text/javascript">
var xmlHttp = createXmlHttpRequestObject();
var imports;
var type = "<?php echo $type;?>";
var loc = window.location.href;

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function load(){
	var file = "operations/import.php";
	var formData = new FormData();
	formData.append("type",type);
	formData.append("person",imports);
	if(xmlHttp){
		try{
			xmlHttp.open("POST",file,true);
			xmlHttp.send(formData);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function getPerson(){
	imports =  new Array();
	var person = document.getElementsByName('person');
	for(var i = 0;i < person.length;i++){
		if(person[i].checked)imports.push(person[i].value);
	}
	load();
}
function change(person){
	window.location.href = loc + "?type=" + person;
}
	
</script>
<style type="text/css">
body {
	margin:0px;
}
table {
	font-family:Arial, Helvetica, sans-serif;
	border-collapse:collapse;
	-webkit-print-color-adjust: exact;
	border-left: 1px solid black;
	border-right: 1px solid black;
	border-top: 1px solid black;
	border-bottom: 1px solid black;
	width:800px;
}
.headline th {
	font-size:20pt;
	background-color:lightblue;
	padding:10px;
	border-bottom:double black;
}
.header td {
	text-align:center;
	padding:10px;
}
.header td button {
	background-color:lightgrey;
	border:1px solid black;
	height:50px;
}
.header td button:hover {
	background-color:lightblue;
}
.header td button img {
	height:40px;
}
.header th {
	font-size:14pt;
	background-color:lightblue;
	border-bottom:1px solid black;
}
td {
	text-align:center;
	border-bottom:1px solid black;
}
td img {
	max-height:150px;
}
</style>
</head>
<body>
<table>
<tr class="headline">
<th colspan="5">Import Data</th></tr>
<tr class="header">
<td><button title="Import" onClick="getPerson();"><img src="bilder/buttons/import.png" /></button></td>
<td colspan="2"><button title="Pilots" onClick="change('pilot');"><img src="bilder/buttons/pilots.png" /></button></td>
<td colspan="2"><button title="Judges" onClick="change('judge');"><img src="bilder/buttons/judge.png" /></button></td></tr>
<tr class="header">
<th></th><th></th><th>Name</th><th>Surname</th><th>License</th></tr>
<?php
if($type == "pilot")$query = "SELECT * FROM person.".$type." ORDER BY nachname ASC, vorname ASC";
elseif($type == "judge")$query = "SELECT id, vorname, name as nachname, license FROM person.".$type." ORDER BY nachname ASC, vorname ASC";
$res = mysqli_query($link2,$query);
while($person = mysqli_fetch_object($res)){?>
	<tr>
    <td><img src="operations/load_image_person.php?id=<?php echo $person->id;?>&type=<?php echo $type;?>" /></td>
    <td><input type="checkbox" name="person" value="<?php echo $person->id;?>" /></td>
	<td><?php echo strtoupper($person->nachname);?></td>
    <td><?php echo $person->vorname;?></td>
    <td><?php echo $person->license;?></td></tr>
    <?php
}?>
</table>
</body>