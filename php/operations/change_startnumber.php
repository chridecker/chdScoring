<?php
require_once("../host.inc");
if(isset($_GET['id'],$_GET['newid']))mysqli_query($link,"UPDATE teilnehmer SET id = ".$_GET['newid']." WHERE id = ".$_GET['id']);