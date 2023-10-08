<table class="panel">
<tr>
<th colspan="3" class="teilnehmer" id="teilnehmer"></th>
<tr>
<th colspan="3" class="figur" id="recent_figur"></th></tr>
<tr>
<td><button id="mnf" onclick='calc(-0.5);'>- 0,5</button></td>
<td id="recent_value" rowspan="2" style="font-size:70pt; text-align:center; font-weight:bold;">10</td>
<td><button id="pnf" onclick='calc(+0.5);'>+ 0,5</button></td>
</tr>

<tr align="center">
<td><button id="me" onclick='calc(-1);'>- 1,0</button></td>
<td><button id="pe" onclick='calc(1);'>+ 1,0</button></td>
</tr>
<tr align="center">
<td><button id="n" onclick='calc(-10);'>00</button></td>
<td><button id="save" onclick='save(99);'>Save</button></td>
<td><button id="no" onclick='save(-1);'>no</button></td>
</tr>
</table>