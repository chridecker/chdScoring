<script type="text/javascript">
var xmlHttp2 = createXmlHttpRequestObject();

function createXmlHttpRequestObject(){
	var xmlHttp;
	xmlHttp = new XMLHttpRequest();
	if(!xmlHttp)alert("ERROR creating XMLHttpRequetObject");
	else return xmlHttp;
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
</script>
<?php
//include("host.inc");
if(!isset($_SESSION['cs_user'])){
	echo "User <input type='text' id='username' name='username'>";
	echo "Password <input type='password' id='password' name='password'>";
	echo "<input type='checkbox' id='remember' name='remember'>";
	echo "<input type='button' onClick='login(1);' value='Login' style='background-color:lightgrey;'>";
}
else {
	echo "You're logged in as ".$_SESSION['cs_user'];
	echo "<input type='button' onClick='login(0);' value='Logout' style='background-color:lightgrey;'>";
}
