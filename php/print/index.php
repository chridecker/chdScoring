<?php 
require_once("../host.inc");
if(isset($_GET['airfield']))$airfield = $_GET['airfield'];
else $airfield = 1;
?>
<script type="text/javascript">
function printer(datei){
	var win = window.open(datei,"_blank");
	win.focus();
	win.print();
}
</script>
<link rel="stylesheet" type="text/css" href="<?php echo "../css/print_".$_GET['file'];?>.css">
<?php
if(isset($_GET['file']))include("../output/".$_GET['file'].".php");
else include("guest.php");
?>
