<?php
require_once("../host.inc");

//XML Config File
$file = "../config.xml";
$xml = simplexml_load_file($file);
$system_name = $xml->information->name;
$version = $xml->information->version;
$year = $xml->information->year;

$round = false;
$programm = 1;
if(isset($_GET['programm']))$programm = $_GET['programm'];
if(isset($_GET['round'])){
	$round = true;
	if($res = mysqli_query($link,"SELECT dp.programm FROM durchgang_programm as dp JOIN wettkampf_leitung wl ON (wl.durchgang = dp.durchgang) WHERE wl.durchgang = ".$_GET['round']))$programm = mysqli_fetch_object($res)->programm;
	else $programm = 1;
}
if(isset($_GET['judge'])){
	if($judge_name = mysqli_fetch_object(mysqli_query($link,"SELECT * FROM judge WHERE id = ".$_GET['judge']))){
		$text_judge = $judge_name->name." ".$judge_name->vorname." - [&#8470; ".$judge_name->id."]";
	}
}
else $text_judge = "JUDGE [&nbsp;&nbsp;&nbsp;]";
/*$query = "SELECT f.id as id, f.name as figur, f.wert as k, p.id as pid, p.title as programm, p.description as beschreibung FROM figur as f JOIN figur_programm as fp ON (f.id = fp.figur) JOIN programm as p ON (fp.programm = p.id) WHERE p.id = ".$programm;
$res = mysqli_query($link,$query);
$obj = mysqli_fetch_object($res);	
$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm WHERE p.id = ".$obj->pid;
$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));*/
?>
<script type="text/javascript">
window.onkeydown = function (e){
	var key = e.keyCode;
	if(key == 27)window.close();
}
</script>
<link rel="stylesheet" href="../css/print_programm.css" />

<?php
if($round){
	$res_teilnehmer = mysqli_query($link,"SELECT t.* FROM teilnehmer t JOIN wettkampf_leitung wl ON (t.id = wl.teilnehmer) WHERE wl.durchgang = ".$_GET['round']." ORDER BY wl.start ASC");
	$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm WHERE p.id = ".$programm;
	$res_figur = mysqli_query($link,$query_count_figur);
	$obj_count_figur = mysqli_fetch_object($res_figur);
	while($teilnehmer = mysqli_fetch_object($res_teilnehmer)){
		$query = "SELECT f.id as id, f.name as figur, f.wert as k, p.id as pid, p.title as programm, p.description as beschreibung FROM figur as f JOIN figur_programm as fp ON (f.id = fp.figur) JOIN programm as p ON (fp.programm = p.id) WHERE p.id = ".$programm;
		$res = mysqli_query($link,$query);
		$obj = mysqli_fetch_object($res);
		$res = mysqli_query($link,$query);	
		?>
        <table>
        <tr class="header">
        <th colspan="4" style="font-size:18pt; border-bottom:1px solid black;">Scorecard <?php echo $text_judge;?></th></tr>
        <tr class="header">
        <th colspan="4"><?php echo "&#8470; ".$teilnehmer->id." ".$teilnehmer->vorname." ".$teilnehmer->nachname." - Round ".$_GET['round']." (".$obj->programm.")";?></th></tr>
        <tr class="headline">
        <th> # </th><th>Manoeuvre</th><th>K</th><th>Score</th></tr>
        <?php 
        $count = 1;
        while($figur = mysqli_fetch_object($res)){
            $id = $figur->id;
            $name = $figur->figur;
            $k = $figur->k;?>
            <tr<?php if($count % 2 == 0)echo " class='gerade'";?>>
            <td><?php echo str_pad(($id - $obj_count_figur->anfang + 1),2,0,STR_PAD_LEFT);?></td>
            <td><?php echo $name;?></td>
            <td><?php echo $k;?></td>
            <td style="padding-left:3cm;"></td>
            </tr>
            <?php
            $count++;
        }
        $query = "SELECT SUM(f.wert) as sumk FROM figur as f JOIN figur_programm as fp ON (f.id = fp.figur) JOIN programm as p ON (fp.programm = p.id) WHERE p.id = ".$programm;
        $res = mysqli_query($link,$query);
        $obj = mysqli_fetch_object($res);?>
        <tr><td colspan="4"><br /></td></tr>
        <tr><td colspan="2" style="text-align:right;">Confirmed</td><td colspan="2" style="width:auto; height:1cm; border:1px solid black;"></td></tr>
        <tr><td colspan="4" style="text-align:center; font-size:10pt; border-top:1px solid black;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td></tr>
        </table>
        <?php
	}
}
else {
	$query_programName = "SELECT * FROM programm WHERE id = ".$programm;
	$obj_name = mysqli_fetch_object(mysqli_query($link,$query_programName));		
	$query_count_figur = "SELECT count(f.id) as anzahl, min(f.id) as anfang, max(f.id) as ende FROM figur f JOIN figur_programm fp ON fp.figur = f.id JOIN programm p ON p.id = fp.programm WHERE p.id = ".$programm;
	$obj_count_figur = mysqli_fetch_object(mysqli_query($link,$query_count_figur));?>
    <table>
    <tr class="header">
    <th colspan="4" style="font-size:22pt; border-bottom:1px solid black;"><?php echo $obj_name->title;?></th></tr>
    <tr class="header">
    <th colspan="4"><?php echo $obj_name->beschreibung;?></th></tr>
    <tr class="headline">
    <th> # </th><th>Manoeuvre</th><th>K</th><th>Score</th></tr>
    <?php
    $count = 1;
	$query = "SELECT f.id as id, f.name as figur, f.wert as k, p.id as pid, p.title as programm, p.description as beschreibung FROM figur as f JOIN figur_programm as fp ON (f.id = fp.figur) JOIN programm as p ON (fp.programm = p.id) WHERE p.id = ".$programm;
	$res = mysqli_query($link,$query);
    while($figur = mysqli_fetch_object($res)){
        $id = $figur->id;
        $name = $figur->figur;
        $k = $figur->k;?>
        <tr<?php if($count % 2 == 0)echo " class='gerade'";?>>
        <td><?php echo str_pad(($id - $obj_count_figur->anfang + 1),2,0,STR_PAD_LEFT);?></td>
        <td><?php echo $name;?></td>
        <td><?php echo $k;?></td>
        <td style="padding-left:3cm;"></td>
        </tr>
        <?php
        $count++;
    }
    $query = "SELECT SUM(f.wert) as sumk FROM figur as f JOIN figur_programm as fp ON (f.id = fp.figur) JOIN programm as p ON (fp.programm = p.id) WHERE p.id = ".$programm;
    $res = mysqli_query($link,$query);
    $obj = mysqli_fetch_object($res);?>
    <tr><td colspan="4" style="text-align:center; font-size:10pt; border-top:1px solid black;"><?php echo $system_name." ".$version." &copy; ".$year; ?></td></tr>
    
    </table>
    <?php
}

