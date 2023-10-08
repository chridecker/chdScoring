<?php
require_once("../host.inc");
$teilnehmer = $_GET['teilnehmer'];
$durchgang = $_GET['durchgang'];
load($teilnehmer,$durchgang,$link);