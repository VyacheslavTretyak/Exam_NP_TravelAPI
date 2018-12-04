<?php
/**
 * Created by PhpStorm.
 * User: Teacher
 * Date: 07.11.2018
 * Time: 18:56
 */
session_start();
include_once "views/functions.php";
?>
<!doctype html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport"
          content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <link rel="stylesheet" href="css/style.css">
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <title>Travel Agency</title>
</head>
<body>
<div class="container">
    <div class="row">
        <header class="col-sm-12"></header>
    </div>
    <div class="row">
        <nav class="col-sm-12">
            <?php include_once "views/menu.php"?>
        </nav>
    </div>
    <div class="row">
        <section class="col-sm-12">
            <?php
                if(isset($_GET['page'])){
                    $page = $_GET['page'];
                    switch ($page){
                        case 1:
                            include_once "views/tours.php";
                            break;
                        case 2:
                            include_once "views/comments.php";
                            break;
                        case 3:
                            include_once "views/registration.php";
                            break;
                        case 4:
                            include_once "views/admin.php";
                            break;
                        default:
                            include_once "views/tours.php";
                    }
                }
            ?>
        </section>
    </div>
</div>

<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
<script src="js/bootstrap.min.js"></script>
</body>
</html>
