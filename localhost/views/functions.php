<?php
/**
 * Created by PhpStorm.
 * User: Teacher
 * Date: 07.11.2018
 * Time: 19:01
 */
function checkToken($token){
    if($token == "ps_rpo_2" || $token == "ps_rpo_1")
        return true;
    else
        return false;
}
function connect(
    $host='localhost',
    $user='root',
    $pass = '',
    $dbname='traveling_db'
){
    $link = mysql_connect($host, $user, $pass) or die('connection error');
    mysql_select_db($dbname ) or die('db open error');
    mysql_query("set names 'utf8'");
}

function register($name, $pass, $email){
    $name = trim(htmlspecialchars($name));
    $pass = trim(htmlspecialchars($pass));
    $email = trim(htmlspecialchars($email));
    if(empty($name) || empty($pass) || empty($email)){
        echo "<h3 class='alert alert-danger'>Fill All Required Fields</h3>";
        return false;
    }
    $pass = md5($pass);
    $insert = "insert into users (login, pass, email, roleId)
                values('$name', '$pass', '$email', 2)";
    connect();
    mysql_query($insert);
    $error = mysql_errno();
    if($error){
        if($error == 1062)
            echo "<h3 class='alert alert-danger'>This Login Is Alredy Taken!</h3>";
        else
            echo "<h3 class='alert alert-danger'>Error code: $error</h3>";
        return false;
    }
    return true;
}