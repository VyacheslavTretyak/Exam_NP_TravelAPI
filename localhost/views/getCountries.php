<?php

include_once "functions.php";
connect();
if($_POST['Name'] == 'getCountries') {
    $sql = 'select countryName from countries';
    $sqlResourse = mysql_query($sql);
    $response = "";
    while ($row = mysql_fetch_array($sqlResourse, MYSQL_NUM)) {
        $response .= "$row[0]|";
    }
    echo $response;
}else{

}

/**
 * Created by PhpStorm.
 * User: Teacher
 * Date: 09.11.2018
 * Time: 19:19
 */