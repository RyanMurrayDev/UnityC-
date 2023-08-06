<?php

    //echo "yes";
    $con = new mysqli('165.22.4.105','root','LionS7810M','mathgame'); //on server so never seen by user

    //check that connection happened
    if($con->connect_error){
        echo "Connection failed! " . $con->connect_error;
        exit();
    }

    //$username = $_POST["name"];
    $username = mysqli_real_escape_string($con,$_POST["name"]); //looks for anything that can run sql querys and strips it out
    //low checks for low asci code  high above 128 any special international characters
    //can filter for email to keep @
    $usernameclean = filter_var($username,FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);


    if($username != $usernameclean){
        //put in funky characters return some type of  error
        //change query to insert usernameclean
        echo "Please remove special characters from username";
        exit();
    }

    $password = $_POST["password"];
    //echo "username = " . $username;
   // echo "\n";
    //echo "password = " . $password;

    //check if name already exists
    $namecheckquery = "SELECT username,email,salt,hash FROM users WHERE username='" . $username . "';";

    $namecheck = mysqli_query($con,  $namecheckquery) or die("2: name checked query failed"); //error code 2 - name check query failed


    if(mysqli_num_rows($namecheck) != 1 ) // 0 or more 1 user in database  problem
    {
        //only for testing  change to username or password incorrect
        echo "No user with username " . $usernameclean . " found. Please register or try a different username";
        exit();
    }

    //get login info from query
    $existinginfo = mysqli_fetch_assoc($namecheck);
    $salt = $existinginfo["salt"];
    $hash = $existinginfo["hash"];

    $loginhash = crypt($password, $salt);
    if($hash != $loginhash)
    {
        echo "incorrect password"; // 6 password does not hash to match table
        exit();
    }
    else{
        echo "0";
    }








?>