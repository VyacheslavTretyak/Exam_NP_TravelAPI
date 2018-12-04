<ul class="nav nav-pills nav-justified">
    <li <?= ($_GET['page']== 1)? "class='active'":""?>><a href="index.php?page=1">Tours</a></li>
    <li <?= ($_GET['page']== 2)? "class='active'":""?>><a href="index.php?page=2">Comments</a></li>
    <li <?= ($_GET['page']== 3)? "class='active'":""?>><a href="index.php?page=3">Registration</a></li>
    <li <?= ($_GET['page']== 4)? "class='active'":""?>><a href="index.php?page=4">Admin Form</a></li>
</ul>