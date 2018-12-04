<?php

include_once "../views/functions.php";
include_once 'Country.php';
class user{
    public $id;
    public $login;
    public function __construct($id, $login)
    {
        $this->id = $id;
        $this->login = $login;
    }
}

class countries{
    public $id;
    public $countryName;
    public function __construct($id, $login)
    {
        $this->id = $id;
        $this->countryName = $login;
    }
}
connect();
$param = 'Неизвестный';
$token = 'anonymus';
$respnse = [];
if(isset($_POST['param']) && checkToken($_POST['token'])){
    switch ($_POST['param']){
//        case 'getUsers':
//            $usersResourse = mysql_query("select * from users");
//
//            while($row = mysql_fetch_array($usersResourse, MYSQL_ASSOC)) {
//                    $item = new user($row['id'], $row['login']);
//                    $respnse[] = $item;
//            }
//            echo json_encode($respnse);
        case 'getCountries':
            $respnse = [];
            $countryResourse = mysql_query("select * from countries");

            while($row = mysql_fetch_array($countryResourse, MYSQL_ASSOC)) {
                $item = new Country($row['id'], $row['countryName']);
                $respnse[] = $item;
            }
            file_put_contents('testForAlexandr.txt', json_encode($respnse));
            echo json_encode($respnse);
    }
}else{
    echo "<h1>Test</h1>";
    $data = json_decode($_POST['param']);
    $d = $data['countryName'];
    mysql_query("insert into countries(countryName)values('$d')");
}

