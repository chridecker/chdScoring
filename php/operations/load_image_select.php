<?php
require_once("../host.inc");
$query = "SELECT DISTINCT(i.img_id) as id FROM images i LEFT JOIN teilnehmer t ON (i.img_id = t.bild) WHERE (t.bild IS NULL OR t.bild = 1";
if(isset($teilnehmer->bild)) $query .= " OR t.bild = ".$teilnehmer->bild;
$query .= ") AND i.img_profil = 1";

$res = mysqli_query($link,$query);
?>
<style type="text/css">
<?php
while($obj = mysqli_fetch_object($res)){?>
.c<?php echo $obj->id;?>:before {
	display: inline-block;
    width: 100px;
    height: 100px;
    content: "";
    background: url("operations/load_image.php?id=<?php echo $obj->id;?>") no-repeat 0 0;
    background-size: 100%;
}
<?php
}
$res = mysqli_query($link,$query);
?>
</style>
<select size="2" id="bild" onChange="speichern();">
<?php
while($obj = mysqli_fetch_object($res)){?>
	<option <?php if(isset($teilnehmer))if($teilnehmer->bild == $obj->id) echo "selected='selected'";?> value="<?php echo $obj->id;?>" class="c<?php echo $obj->id;?>"></option>
    <?php
}?>
</select>