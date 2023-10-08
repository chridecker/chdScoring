
<?php
$canvas = imagecreatetruecolor(800,600);
$pink = imagecolorallocate($canvas, 255, 105, 180);

imagerectangle($canvas, 50, 50, 150, 150, $pink);
// Output and free from memory
header('Content-Type: image/jpeg');

imagejpeg($canvas);
imagedestroy($canvas);
?>