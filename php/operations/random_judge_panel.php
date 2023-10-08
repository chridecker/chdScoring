<?php
require_once("../host.inc");

if(isset($_GET['spanel']))$spanel = $_GET['spanel'];
if(isset($_GET['epanel']))$epanel = $_GET['epanel'];
$lmr = $_GET['lmr'] === 'true'? true: false;

if(isset($spanel) && isset($epanel) && !$lmr){
	$judges = mysqli_fetch_object(mysqli_query($link,"SELECT count(id) as ids FROM judge"))->ids;
	$anzahl = $judges / ($epanel-$spanel+1);
	$panel = array();
	for($p = $spanel;$p <= $epanel; $p++){
		$panel[$p] = array();
	}
	$j = 1;
	while($j <= $judges){
		$rand = mt_rand($spanel,$epanel);
		if(count($panel[$rand])<$anzahl){
			array_push($panel[$rand],$j);
			$j++;
		}
	}
	mysqli_query($link,"DELETE FROM judge_panel WHERE panel >= ".$spanel." AND panel <= ".$epanel);
	for($p = $spanel;$p <= $epanel; $p++){
		for($i=0;$i<count($panel[$p]);$i++)mysqli_query($link,"INSERT INTO judge_panel (`panel`,`judge`) VALUES (".$p.",".$panel[$p][$i].")");
	}
}

elseif(isset($spanel) && isset($epanel) && $lmr){
	$judges = mysqli_fetch_object(mysqli_query($link,"SELECT count(id) as ids FROM judge"))->ids;
	$anzahl_middle = $judges / 2;
	$anzahl_corner = $anzahl_middle / 2;
	$panel = array();
	for($p = $spanel;$p <= $epanel; $p++){
		$panel[$p] = array();
	}
	$j = 1;
	while($j <= $judges){
		$rand = mt_rand($spanel,$epanel);
		if($rand < $epanel && count($panel[$rand])<$anzahl_corner){
			array_push($panel[$rand],$j);
			$j++;
		}
		elseif($rand == $epanel && count($panel[$rand])<$anzahl_middle){
			array_push($panel[$rand],$j);
			$j++;
		}
	}
	mysqli_query($link,"DELETE FROM judge_panel WHERE panel >= ".$spanel." AND panel <= ".$epanel);
	for($p = $spanel;$p <= $epanel; $p++){
		for($i=0;$i<count($panel[$p]);$i++)mysqli_query($link,"INSERT INTO judge_panel (`panel`,`judge`) VALUES (".$p.",".$panel[$p][$i].")");
	}
}