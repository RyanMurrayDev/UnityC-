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


    //make sure name exists in database
    $namecheckquery = "SELECT username,wins_easy,wins_medium,wins_hard,wins_multiplayer,coins FROM userdata WHERE username='" . $username . "';";

    $namecheck = mysqli_query($con,  $namecheckquery) or die("2: name checked query failed"); //error code 2 - name check query failed
    if(mysqli_num_rows($namecheck) != 1 ) // 0 or more 1 user in database  problem
    {
        echo "5: Either no user or more than 1";
        exit();
    }


    //$dataquery = "SELECT wins_easy,wins_medium,wins_hard,wins_multiplayer,coins FROM userdata WHERE username='" . $username . "';";
    $data =mysqli_query($con,$namecheckquery) or die("7: User data query failed");

        //get login info from query
        $existinginfo = mysqli_fetch_assoc($namecheck);
        //echo $existinginfo[0] . $existinginfo[1];
        //echo $existinginfo;
        $s = "0\t" . $existinginfo["wins_easy"] . "\t" . $existinginfo["wins_medium"] . "\t" . $existinginfo["wins_hard"] .
            "\t" . $existinginfo["wins_multiplayer"] . "\t". $existinginfo["coins"] . "\t";
        echo $s;

?>
