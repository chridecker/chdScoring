<?php
require_once("../host.inc");
if(isset($_GET['judge']) && isset($_GET['panel']) && isset($_GET['value'])){
	if($_GET['value'] == 0)	$query = "DELETE FROM judge_panel WHERE judge = ".$_GET['judge']." AND panel = ".$_GET['panel'];
	else if($_GET['value'] == 1)	$query = "INSERT INTO judge_panel (`judge`,`panel`) VALUES (".$_GET['judge'].",".$_GET['panel'].")";
	mysqli_query($link,$query);
}
else if($_GET['panel'] == 'new'){
	$new_panel = mysqli_fetch_object(mysqli_query($link,"SELECT max(panel) + 1 as new_panel FROM judge_panel"));
	$res_judges = mysqli_query($link,"SELECT id FROM judge");
	while($judge = mysqli_fetch_object($res_judges))mysqli_query($link,"INSERT INTO judge_panel (`judge`,`panel`) VALUES (".$judge->id.",".$new_panel->new_panel.")");
}
