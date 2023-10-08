<?php
require_once("../host.inc");


?>
<script type="text/javascript">
function oeffnen(file){
	var win = window.open(file,'_self');
	win.focus();
}

</script>
<link rel="stylesheet" type="text/css" href="../css/judges.css">
<table>
<tr class="headline"><th>Judges Auswahl</th></tr>
<tr class="header"><th>Anmelden</th></tr>
<?php 
$query = "SELECT * FROM judge";
if($res = mysqli_query($link,$query)){
	while($judge = mysqli_fetch_object($res)){?>
		<tr>
		<td><button class='button' onClick="oeffnen('judge.php?id=<?php echo $judge->id;?>');"><?php echo $judge->name." ".$judge->vorname;?></button></td>
		</tr>
		<?php
	} 
}
mysqli_free_result($res);
mysqli_close($link);
?>
</table>