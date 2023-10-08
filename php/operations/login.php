<?php
require_once("../host.inc");
if(isset($_GET['username']) && isset($_GET['password']) && $_GET['action'] == 1){
	$username = $_GET['username'];
	$password = md5($_GET['password']);
	$remember = $_GET['remember'];
	$res_login = mysqli_query($link,"SELECT level FROM user WHERE username = '".$username."' AND password = '".$password."'");
	if($obj_level = mysqli_fetch_object($res_login)){
		unset($_SESSION['cs_user']);
		$_SESSION['cs_user'] = $username;
		if($remember)setcookie('cs_user',$username,time() + 3600 * 24);
	}
}
else if($_GET['action'] == 0) {
	unset($_SESSION['cs_user']);
	if(isset($_COOKIE['cs_user']))setcookie('cs_user',$username,time() - 3600 * 24);
}
