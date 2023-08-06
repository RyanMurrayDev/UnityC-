<?php

    //echo "yes";
    $con = new mysqli('165.22.4.105','root','****','mathgame'); //on server so never seen by user

    //check that connection happened
    if($con->connect_error){
        echo "Connection failed! " . $con->connect_error;
        exit();
    }

    //whatever posted from form
    $username = $_POST["name"];
    $easywins = $_POST["easywins"];
    $mediumwins = $_POST["mediumwins"];
    $hardwins = $_POST["hardwins"];
    $multiplayerwins = $_POST["multiplayerwins"];
    $coins = $_POST["coins"];

    //make sure name exists in database
    $namecheckquery = "SELECT username FROM userdata WHERE username='" . $username . "';";

    $namecheck = mysqli_query($con,  $namecheckquery) or die("2: name checked query failed"); //error code 2 - name check query failed
    if(mysqli_num_rows($namecheck) != 1 ) // 0 or more 1 user in database  problem
    {
        echo "5: Either no user or more than 1";
        exit();
    }



    $updatequery = "UPDATE userdata SET wins_easy=" . $easywins .",wins_medium=" . $mediumwins.
      ",wins_hard=" . $hardwins.  ",wins_multiplayer=" . $multiplayerwins. ",coins=" . $coins." WHERE username='" . $username . "';";
    mysqli_query($con,$updatequery) or die("7: Save query failed");

    echo "0";



?>
