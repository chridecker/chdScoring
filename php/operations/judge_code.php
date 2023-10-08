<?php
require_once("../host.inc");
$judge = $_GET['judge'];
mysqli_query($link,"UPDATE judge SET pin = '".genCode(1,0,0,0,4)."' WHERE id = ".$judge);