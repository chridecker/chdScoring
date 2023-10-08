var xmlHttp = createXmlHttpRequestObject();
var xmlHttp2 = createXmlHttpRequestObject();
var xmlHttp3 = createXmlHttpRequestObject();
var xmlHttp4 = createXmlHttpRequestObject();

var controlGamepad = JSON.parse(getCookie('gamepad'));
var controlKeyboard = JSON.parse(getCookie('keyboard'));


function goFullScreen(){
	var x = document.documentElement;
	x.webkitRequestFullScreen();
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    var expires = "expires="+d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for(var i=0; i<ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0)==' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length,c.length);
    }
    return false;
}

function changeControl(type){
	if(type == 'keyboard'){
		controlKeyboard = !controlKeyboard;
		setCookie(type,controlKeyboard,7);
	}
	if(type == 'gamepad'){
		controlGamepad = !controlGamepad;
		setCookie(type,controlGamepad,7);
	}
	control();
}
function control(){
		if(controlKeyboard)document.getElementById('keyboard').style.backgroundColor="lightblue";
		else document.getElementById('keyboard').style.backgroundColor="white";
		if(controlGamepad)document.getElementById('gamepad').style.backgroundColor="lightblue";
		else document.getElementById('gamepad').style.backgroundColor="white";
}
function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
}

function process(airfield){
	if(xmlHttp){
		try{
			xmlHttp.open("GET","wettkampf.php?airfield=" + airfield,true);
			xmlHttp.onreadystatechange = handleRequestStateChange;
			xmlHttp.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function handleRequestStateChange(){
	myDiv = document.getElementById('main');
	if(xmlHttp.readyState == 4){
		if(xmlHttp.status == 200){
			response = xmlHttp.responseText;
			myDiv.innerHTML = response;
		}
	}
}

function start(durchgang){
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/start.php?durchgang="+durchgang,true);
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
	setTimeout(location.reload(),1000);
}
function login(action){
	var username = '';
	var password = '';
	var remember = '';
	if(action == 1){
		username = document.getElementById('username').value;
		password = document.getElementById('password').value;
		remember = document.getElementById('remember').checked;
	}
	var file = "operations/login.php?username=" + username + "&password=" + password + "&remember=" + remember + "&action=" + action;
	alert(file);
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET",file,true);
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
	setTimeout(location.reload(),1000);
}

function save(teilnehmer,durchgang,wert){
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/save_durchgang.php?teilnehmer=" + teilnehmer + "&durchgang=" + durchgang + "&wert=" + wert,true);
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
	timer('stop');
}
function ladeTeilnehmer(teilnehmer,durchgang,airfield){
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/load_teilnehmer.php?teilnehmer=" + teilnehmer + "&durchgang=" + durchgang,true);
			xmlHttp2.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function loadAirfield(af){
	airfield = af;
	process(af);
}
function save_durchgang(durchgang){
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/werte_durchgang.php?durchgang=" + durchgang,true);
			//xmlHttp2.onreadystatechange = function (){loaction.reload; timer('stop');}
			xmlHttp2.send(null)
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function vorflieger(airfield){
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/vorflieger.php?airfield=" + airfield,true);
			xmlHttp2.send(null)
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
	setTimeout(location.reload(),500);
}
function oeffnen(datei){
	var win = window.open(datei,"_blank");
	win.focus();
}
function printer(datei){
	var win = window.open("print/" + datei,"_blank");
	win.focus();
	win.print();
}
function timer(action,airfield){
	if(xmlHttp3){
		try{
			xmlHttp3.open("GET","operations/timer.php?action=" + action + "&airfield=" + airfield,true);
			xmlHttp3.send(null)
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}

function pause(airfield){
	if(xmlHttp2){
		try{
			xmlHttp2.open("GET","operations/pause.php?airfield=" + airfield,true);
			xmlHttp2.send(null)
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
	timer('stop',airfield);
}
function suche(){
	var stype = prompt("1 -> Pilot | 2 -> Judge | 3 -> Official","1");
	var svalue = prompt("Enter ID or scan License Code","");
	if(svalue != "" && svalue != null && stype != "" && stype > 0 && stype < 4){
		if(stype == 1)oeffnen("teilnehmer_detail.php?id=" + svalue);
		if(stype == 2)oeffnen("judges_detail.php?id=" + svalue);
		if(stype == 3)oeffnen("officials_detail.php?id=" + svalue);
	}
}

function clearData(){
	var check = confirm("Do you really want to CLEAR ALL DATA?");
	var pass = prompt("Please type 'DELETE' to confirm");
	if(xmlHttp2 && check && pass == 'DELETE'){
		try{
			xmlHttp2.open("GET","operations/clear.php?confirm=yes",true);
			xmlHttp2.send(null)
			timer('stop');
			window.location.reload();
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
window.onkeydown = function(e) {
   var key = e.keyCode ? e.keyCode : e.which;
   if(controlGamepad){
	   switch(key){
	   case 166:
		   e.preventDefault();
		   window.location = "";
			break;
	   case 176:
			var check = confirm("You want to move on to the next pilot. Please confirm with pressing [ OK ]");
		   if(check)document.getElementById("b1").click();
			break;
	   case 13:
		   e.preventDefault();
		   var check = confirm("You want to save the flight of the current pilot. Please confirm with pressing [ OK ]");
		   if(check)document.getElementById("bsave").click();
			break;
	   case 177:
		   document.getElementById("bresettimer").click();
			break;
	   case 174:
		   document.getElementById("bstoptimer").click();
		   break;
	   case 175:
		   document.getElementById("bstarttimer").click();
		   break;
	   case 27:
			e.preventDefault();
		   document.getElementById("bstartround").click();
		   break;
	   default:
			//alert(key);
			break;
	   }
	}
   if(controlKeyboard){
	   switch(key){
		  case 8:
			e.preventDefault();
			break;
	   case 166:
		   e.preventDefault();
		   window.location = "";
			break;
		case 51:
			if(e.ctrlKey){
				e.preventDefault();
				oeffnen("teilnehmer_detail.php?id=0");
			}
			break;
	   case 80:
			if(e.ctrlKey){
				e.preventDefault();
				document.getElementById('bprint').click();
			}
		   else pause();
			break;
	   case 39:
			var check = true; //confirm("You want to move on to the next pilot. Please confirm with pressing [ OK ]");
		   if(check)document.getElementById("b1").click();
			break;
	   case 13:
		   e.preventDefault();
		   var check = true; //confirm("You want to save the flight of the current pilot. Please confirm with pressing [ OK ]");
		   if(check)document.getElementById("bsave").click();
			break;
	   case 27:
		   e.preventDefault();
			box('hidden','print/');
			break;
	   case 82:
		   document.getElementById("bresettimer").click();
			break;
	   case 83:
		   document.getElementById("bstoptimer").click();
		   break;
	   case 84:
		   document.getElementById("bstarttimer").click();
		   break;
	   case 119:
		   document.getElementById("bprintroundsheet").click();
		   break;
	   case 114:
		   document.getElementById('bsearch').click();
		   break;
	   case 112:
		   document.getElementById("bpreferences").click();
		   break;
	   case 113:
		   document.getElementById("bjudges").click();
		   break;
	   case 115:
		   document.getElementById("bpilots").click();
		   break;
	   case 117:
		   document.getElementById("bprint").click();
		   break;
	   case 118:
		   document.getElementById("bscheduleedit").click();
		   break;
	   case 120:
		   document.getElementById("vorflieger").click();
		   break;
	   case 121:
		   document.getElementById("bpublic").click();
		   break;
	   default:
			//alert(key);
			break;
	   }
   }
}
function box(vis,type){
	var box = document.getElementById('box');
	box.style.visibility = vis;
	if(xmlHttp4){
		try{
			xmlHttp4.open("GET",type,true);
			xmlHttp4.onreadystatechange = handleRequestStateChangeBox;
			xmlHttp4.send(null);
		}
		catch(e){
			alert("Can't connect to Server: " + e.toString());
		}
	}
}
function handleRequestStateChangeBox(){
	var myDiv = document.getElementById('inbox');
	if(xmlHttp4.readyState == 4){
		if(xmlHttp4.status == 200){
			response = xmlHttp4.responseText;
			myDiv.innerHTML = response;
		}
	}
}
