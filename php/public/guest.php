<?php
require_once("../host.inc");
?>
<table>
<tr>
<th class="headline" colspan="3">Public Views</th></tr>
<tr>
<td><button title="Public Timer" onClick="oeffnen('?file=timer_calc');"><img src="../bilder/buttons/start_timer.png"/></button></td>
<td><button title="Public Durchgang" onClick="oeffnen('?file=durchgangswertung');"><img src="../bilder/buttons/round.png" /></button></td>
<td><button title="Public Live" onClick="oeffnen('?file=live<?php if($score_mode == 1) echo "_tbl";?>');"><img src="../bilder/buttons/live.png" /></button></td>
</tr>
<tr>
<td><button title="Public Figur" onClick="oeffnen('?file=figuren_wertung');"><img src="../bilder/buttons/figuren.png" /></button></td>
<td><button title="Public Info" onClick="oeffnen('?file=info');"><img src="../bilder/buttons/info.png" /></button></td>
</tr>
</table>
