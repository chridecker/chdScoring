<?php
require_once("../host.inc");
$durchgang = $_GET['durchgang'];
include("tbl.php");
umrechnung($durchgang,$link);
mysqli_query($link,"UPDATE wettkampf_leitung SET status = 3 WHERE durchgang = ".$durchgang);
