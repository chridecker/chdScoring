<?php
require_once("../host.inc");

$judge = mysqli_fetch_object(mysqli_query($link,"SELECT pin FROM judge WHERE id = ".$_GET['judge']));
if($judge->pin == $_GET['pin']) mysqli_query($link,"INSERT INTO judge_log (`judge`,`teilnehmer`,`durchgang`,`zeit`,`confirm`) VALUES (".$_GET['judge'].",".$_GET['teilnehmer'].",".$_GET['durchgang'].",'".date("Y-m-d H:i:s")."',1)");