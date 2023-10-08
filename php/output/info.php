<?php
if($_SERVER['PHP_SELF'] == '/output/info.php') require_once("../host.inc");
?>
<body>
<table>
<tr>
<th class="headline" colspan="2">
<?php echo $turnier;?></th>
</tr>
<tr>
<th class="info_text" colspan="2">
<img id="info_img" src="../operations/load_image.php?id=<?php echo $_GET['info_img'];?>" style="height: 400px;" /><br/>
<h2>Pilotenbesprechung & Auslosung 14:00</h2>
<h2>Start 1. Final-Durchgang (F21): 14:30</h2>
</th></tr>
</table>
</body>

