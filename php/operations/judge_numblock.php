<table class="panel">
<tr>
<th colspan="3" class="teilnehmer" id="teilnehmer"></th>
</tr>
<tr>
<th colspan="3" class="figur" id="recent_figur"></th></tr>
<tr style="text-align:center;">
	
	<th style="background-color:white; font-size:30pt; width:33%">
	<button id="pOk" onClick="saveNew();" style="width:70%; height:95%; font-size: 28pt; background-color:lightblue; border: 5px solid black;">OK</button></th>
	
	<th style="background-color:white; font-size:40pt; width:33%" id="recent_value"></th>
	
	<th style="background-color:white; font-size:30pt; width:33%">
	<button id="pDel" onclick="delLast();" style="width:70%; height:95%; font-size: 28pt; background-color:lightblue; border: 5px solid black;">DEL</button></th>
	
	<!--<th colspan="2" style="background-color:white; font-size:20pt; text-align:center;">
	<button id="pOk" onClick="saveNew();" style="width:30%; height:90%; font-size: 36pt; background-color:lightblue; border: 3px solid black;">OK</button>
	<button id="pDel" onclick="delLast();" style="width:30%; height:90%; font-size: 36pt; background-color:lightblue; border: 3px solid black;">DEL</button></th> -->
</tr>
<tr>
<td><button id="p1" onClick="calc(1);">1</button></td>
<td><button id="p2" onClick="calc(2);">2</button></td>
<td><button id="p3" onClick="calc(3);">3</button></td>
</tr>

<tr align="center">
<td><button id="p4" onClick="calc(4);">4</button></td>
<td><button id="p5" onClick="calc(5);">5</button></td>
<td><button id="p6" onClick="calc(6);">6</button></td>
</tr>
<tr align="center">
<td><button id="p7" onClick="calc(7);">7</button></td>
<td><button id="p8" onClick="calc(8);">8</button></td>
<td><button id="p9" onClick="calc(9);">9</button></td>
</tr>
<tr align="center">
<td><button id="p10" onClick="calc('.');">&nbsp;.</button></td>
<td><button id="p0" onClick="calc(0);">0</button></td>
<td><button id="pno" onClick="calc('NO');">NO</button></td>
</tr>

</table>