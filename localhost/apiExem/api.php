<?php
include_once 'Country.php';
include_once '../views/functions.php';
connect();
if(checkToken($_POST['token'])){
    $dsn = 'mysql:host=localhost;dbname=traveling_db';
    $login = 'root';
    $password = '';
   switch($_POST['param']) {
       case 'getCountries':
           $items = [];
           $db = new PDO ($dsn , $login, $password, array (
               PDO::ATTR_PERSISTENT => true
           ) );
           $sql = "select * from countries";
           $query = $db->prepare ( $sql, array (
               PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
           ) );
           if($query->execute () === false){
               $res = new stdClass();
               $res->result = 500;
               echo json_encode($res);
           }else {
               while ($row = $query->fetch()) {
                   $obj = new stdClass();
                   $obj->id = $row['id'];
                   $obj->countryName = $row['countryName'];
                   $items[] = $obj;
               }
               $obj = new stdClass();
               $obj->rows = $items;
               $obj->result = 200;
               echo json_encode($obj);
           }
           break;
       case 'getCities':
       $items = [];
           $db = new PDO ($dsn , $login, $password, array (
               PDO::ATTR_PERSISTENT => true
           ) );
       if(isset($_POST['countryId'])){
           $sql = "select * 
                    from cities
                    where countryId = :id";
           $query = $db->prepare ( $sql, array (
               PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
           ) );
           $res = $query->execute (array(':id'=>$_POST['countryId']));
       }else{
           $sql = "select c.*, co.countryName
                    from cities as c 
                    join countries as co on co.id = c.countryId";
           $query = $db->prepare ( $sql, array (
               PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
           ) );
           $res = $query->execute ();
       }
       if($res === false){
           $res = new stdClass();
           $res->result = 500;
           echo json_encode($res);
       }else {
           while ($row = $query->fetch()) {
               $obj = new stdClass();
               $obj->id = $row['id'];
               $obj->cityName = $row['cityName'];
               if(!isset($_POST['countryId'])){
                   $obj->countryName = $row['countryName'];
               }
               $items[] = $obj;
           }
           $obj = new stdClass();
           $obj->rows = $items;
           $obj->result = 200;
           echo json_encode($obj);
       }
       break;
       case 'getHotels':
           $items = [];
           $db = new PDO ($dsn , $login, $password, array (
               PDO::ATTR_PERSISTENT => true
           ) );
           if(isset($_POST['cityId'])) {
               $sql = "select h.*, ci.cityName, co.countryName
                    from hotels as h
                    join cities as ci on ci.id = h.cityId 
                    join countries as co on co.id = h.countryId
                    where h.cityId = :id";
               $query = $db->prepare($sql, array(
                   PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
               ));
               $res = $query->execute (array(':id'=>$_POST['cityId']));
           }else{
               $sql = "select h.*, ci.cityName, co.countryName
                    from hotels as h
                    join cities as ci on ci.id = h.cityId 
                    join countries as co on co.id = h.countryId";
               $query = $db->prepare($sql, array(
                   PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
               ));
               $res = $query->execute ();
           }
           if($res === false){
               $res = new stdClass();
               $res->result = 500;
               echo json_encode($res);
           }else {
               while ($row = $query->fetch()) {
                   $obj = new stdClass();
                   $obj->id = $row['id'];
                   $obj->hotelName = $row['hotelName'];
                   $obj->cityId = $row['cityId'];
                   $obj->stars = $row['stars'];
                   $obj->cost = $row['cost'];
                   $obj->info = $row['info'];
                   $obj->countryName = $row['countryName'];
                   $obj->cityName = $row['cityName'];
                   $items[] = $obj;
               }
               $obj = new stdClass();
               $obj->rows = $items;
               $obj->result = 200;
               echo json_encode($obj);
           }
           break;
       case 'login':
       $items = [];
       $db = new PDO ($dsn , $login, $password, array (
           PDO::ATTR_PERSISTENT => true
       ) );
       $sql = "select * 
                    from users
                    where login = :login and
                    pass = :password";
       $query = $db->prepare ( $sql, array (
           PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
       ) );
       if($query->execute (array(
                   ':login'=>$_POST['login'],
                   ':password'=>$_POST['password'])
           ) === false){
           $res = new stdClass();
           $res->result = 500;
           // echo $_POST['password'];
           echo json_encode($res);
       }else {
           while ($row = $query->fetch()) {
               $obj = new stdClass();
               $obj->login = $row['login'];
               $obj->roleId = $row['roleId'];
               $items[] = $obj;
           }
           $obj = new stdClass();
           $obj->rows = $items;
           $obj->result = 200;
           echo json_encode($obj);
       }
       break;
       case 'registration':
           $items = [];
           $db = new PDO ($dsn , $login, $password, array (
               PDO::ATTR_PERSISTENT => true
           ) );
           $sql = "insert into users(login, pass, email, roleId) values(
                    :login, 
                    :password,
                    :email,
                    2)";
           $query = $db->prepare ( $sql, array (
               PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
           ) );
           if($query->execute (array(
                       ':login'=>$_POST['login'],
                       ':password'=>$_POST['password'],
                       ':email'=>$_POST['email']
                   )) === false){
               $res = new stdClass();
               $res->result = 500;
               echo json_encode($res);
           }else {
               $obj = new stdClass();
               $obj->login = $_POST['login'];
               $obj->roleId = 2;
               $items[] = $obj;
               $obj = new stdClass();
               $obj->rows = $items;
               $obj->result = 200;
               echo json_encode($obj);
           }
           break;
       case 'insertCountry':
           $data = json_decode($_POST['data']);

           $db = new PDO ($dsn , $login, $password, array (
               PDO::ATTR_PERSISTENT => true
           ) );
           $sql = "insert into countries(countryName) values(
                    :countryName)";
           $query = $db->prepare ( $sql, array (
               PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
           ) );
           if($query->execute (array(
                   ':countryName'=>$data->CountryName
               )) === false){
               $res = new stdClass();
               $res->result = 500;
               echo json_encode($res);
           }else {
               $obj = new stdClass();
               $obj->rows = null;
               $obj->result = 200;
               echo json_encode($obj);
           }
           break;
       case 'insertCity':
           $data = json_decode($_POST['data']);

           $db = new PDO ($dsn , $login, $password, array (
               PDO::ATTR_PERSISTENT => true
           ) );
           $sql = "insert into cities(cityName, countryid) values(
                    :cityName, :countryId)";
           $query = $db->prepare ( $sql, array (
               PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
           ) );
           if($query->execute (array(
                   ':cityName'=>$data->CityName,
                   ':countryId'=>$data->CountryId
               )) === false){
               $res = new stdClass();
               $res->result = 500;
               echo json_encode($res);
           }else {
               $obj = new stdClass();
               $obj->rows = null;
               $obj->result = 200;
               echo json_encode($obj);
           }
           break;
       case 'insertHotel':
           $data = json_decode($_POST['data']);

           $db = new PDO ($dsn , $login, $password, array (
               PDO::ATTR_PERSISTENT => true
           ) );
           $sql = "insert into hotels(hotelName, cityId, countryId, stars, cost, info) values(
                    :hotelName, :cityId, :countryId, :stars, :cost, :info)";
           $query = $db->prepare ( $sql, array (
               PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
           ) );
           if($query->execute (array(
                   ':hotelName'=>$data->HotelName,
                   ':cityId'=>$data->CityId,
                   ':countryId'=>$data->CountryId,
                   ':stars'=>$data->stars,
                   ':cost'=>$data->Cost,
                   ':info'=>$data->Info
               )) === false){
               $res = new stdClass();
               $res->result = 500;
               echo json_encode($res);
           }else {
               $obj = new stdClass();
               $obj->rows = null;
               $obj->result = 200;
               echo json_encode($obj);
           }
           break;
       case 'remove':
           $data = json_decode($_POST['data']);
           $table = $_POST['table'];
           $db = new PDO ($dsn , $login, $password, array (
               PDO::ATTR_PERSISTENT => true
           ) );
           $sql = "delete from $table
                    where id = :id";
           $query = $db->prepare ( $sql, array (
               PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY
           ) );
           if($query->execute (array(
                   ':id'=>$data->Id
               )) === false){
               $res = new stdClass();
               $res->result = 500;
               echo json_encode($res);
           }else {
               $obj = new stdClass();
               $obj->rows = null;
               $obj->result = 200;
               echo json_encode($obj);
           }
           break;
       default:
           $res = new stdClass();
           $res->result = 501;
           echo json_encode($res);
   }
}else {
    $res = new stdClass();
    $res->result = 498;
    echo json_encode($res);
}