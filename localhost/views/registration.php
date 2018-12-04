<h3>Registration Form</h3>
<?php if(!isset($_POST['regbtn'])):?>
    <form action="index.php?page=3" method="post">
        <div class="form-group">
            <label for="login">Login:</label>
            <input type="text" class="form-control" name="login">
        </div>
        <div class="form-group">
            <label for="password">Password:</label>
            <input type="password" class="form-control" name="password">
        </div>
        <div class="form-group">
            <label for="email">Email:</label>
            <input type="email" class="form-control" name="email">
        </div>
        <button type="submit" class="btn btn-primary" name="regbtn">Register</button>
    </form>
<?php else:?>
    <?php
        if(register($_POST['login'], $_POST['password'],$_POST['email'])){
            echo "<h3 class='alert alert-success'>New User Added!</h3>";
        }
    ?>
<?php endif;?>
