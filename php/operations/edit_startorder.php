<?php
require_once("..//host.inc");
if(isset($_GET['durchgang']))$durchgang = $_GET['durchgang'];
else $durchgang = 1;
if($_GET['change'] == 1){
	$query = "UPDATE wettkampf_leitung wl SET wl.start = ".$_GET['start']." WHERE wl.teilnehmer = ".$_GET['id']." AND wl.durchgang = ".$durchgang;
	mysqli_query($link,$query);
}
$res_start = mysqli_query($link,"SELECT * FROM wettkampf_leitung wl JOIN teilnehmer t ON (wl.teilnehmer = t.id) WHERE wl.durchgang = ".$durchgang." ORDER BY wl.start ASC");?>
<table>
<tr class="headline">
<th colspan="3">Edit Startorder Round <?php echo $durchgang;?></th></tr>
<tr>
<th>Startnr</th><th>ID</th><th>Participant</th></tr>
<?php
$count = 0;
while($teilnehmer = mysqli_fetch_object($res_start)){
	$count++;?>
	<tr<?php if(($count % 2) == 0)echo " class='grey'";?>>
	<td><input type='text' onchange='load(<?php echo $teilnehmer->id;?>);' value='<?php echo $teilnehmer->start;?>' id='start<?php echo $teilnehmer->id;?>'></td>
	<td><?php echo $teilnehmer->id;?></td>
	<td><?php echo $teilnehmer->vorname." ".$teilnehmer->nachname;?></td></tr>
    <?php
}?>
</table>