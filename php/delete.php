<?php
require_once("host.inc");?>


<script type="text/javascript">
var xmlHttp2 = createXmlHttpRequestObject();

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}
function del(){
	var id = document.getElementById("delete").value;
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/delete_wertung.php?delete=" + id,true);
			xmlHttp2.send(null)
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
	location.reload();
}
window.onkeydown = function(e) {
	var key = e.keyCode ? e.keyCode : e.which;
	if(key == 27)window.close();
}

</script>
<link rel="stylesheet" type="text/css" href="css/delete.css" />
<?php
$query = "SELECT t.*, d.durchgang FROM teilnehmer as t, durchgang as d WHERE t.id = d.teilnehmer ORDER BY d.durchgang";
$res = mysqli_query($link,$query);?>
<table>
<tr>
<th colspan="2" class="headline">Reflight</th></tr>
<tr>
<th colspan="2">Participant / Round</th></tr>
<tr>
<td>
<select id="delete">
<?php
while($teilnehmer = mysqli_fetch_object($res)){?>
	<option value="<?php echo str_pad($teilnehmer->id,2,0,STR_PAD_LEFT)."_".$teilnehmer->durchgang;?>">
    <?php echo $teilnehmer->vorname." ".$teilnehmer->nachname;?>
    <?php echo "Durchgang: ".$teilnehmer->durchgang;?>
    </option>
    <?php
}?>
</select></td>
<td>
<input type="button" value="Delete" onclick="del();" /></td></tr>
<tr><td colspan="2" style="text-align:left;"><input type="button" value="Close" onclick="window.close();" /></td></tr>
</table>
	