<?php

    //echo "yes";
	$con = new mysqli('165.22.4.105','root','LionS7810M','mathgame'); //on server so never seen by user

	//check that connection happened
	if($con->connect_error){
		echo "Connection failed! " . $con->connect_error;
		exit();
	}
    //$username = $_POST["name"];
    //low checks for low asci code  high above 128 any special international characters
    //can filter for email to keep @
    $username = mysqli_real_escape_string($con,$_POST["name"]); //looks for anything that can run sql querys and strips it out
    $usernameclean = filter_var($username,FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);


    if($username != $usernameclean){
        //put in funky characters return some type of  error
        //change query to insert usernameclean
        echo "Please remove special characters from username";
        exit();
    }

    $email = mysqli_real_escape_string($con,$_POST["email"]);
    $emailclean = filter_var($email,FILTER_SANITIZE_EMAIL);

    if($email != $emailclean){
        //put in funky characters return some type of  error
        //change query to insert usernameclean
        echo "Please enter a valid email";
        exit();
    }

    //code to check for only letters, dashes, apostrophes and whitespace
    //$name = test_input($_POST["name"]);  https://www.w3schools.com/php/php_form_url_email.asp
    //if (!preg_match("/^[a-zA-Z-' ]*$/",$name)) {
        //$nameErr = "Only letters and white space allowed";
    //}

    //checking if email format https://www.w3schools.com/php/php_form_url_email.asp
    if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
        echo "Invalid email address";
        exit();
    }

    //password salted and hashed before inserted so dont have to clean
	$password = $_POST["password"];
    //echo "username = " . $username;
    //echo "\n";
    //echo "password = " . $password;

	//check if name already exists
	$namecheckquery = "SELECT username FROM users WHERE username='" . $username . "';";

	$namecheck = mysqli_query($con,  $namecheckquery) or die("failed when checking if user exists in database  please try again"); //error code 2 - name check query failed

    //check if email already exists
    $emailcheckquery = "SELECT email FROM users WHERE email='" . $email . "';";

    $emailcheck = mysqli_query($con,  $emailcheckquery) or die("failed when checking if email exists in database  please try again"); //error code 2 - name check query failed


	if(mysqli_num_rows($emailcheck) > 0)
	{
		echo "email already exists. please sign in";
		exit();
	}


	//add user to the table
	//slash means actual dollar sign not variable    sha256 encryption  5000 rounds of shifting characters around
	//string at end makes random  must be atleast 16 characters

	$salt = "\$5\$rounds=5000\$" . "steamedhams" . $username . "\$";
	$hash = crypt($password,$salt);
    //echo  $usernameclean . "', '" . $emailclean . "', '" .$hash . "', '" . $salt;
	$insertuserquery = "INSERT INTO users (username,email, hash,salt) VALUES ('" . $usernameclean . "', '" . $emailclean . "', '" .$hash . "', '" . $salt . "');";
	mysqli_query($con,$insertuserquery) or die("insert user failed " . mysqli_error($con)); //error code 4 - insert query failed
    $insertuserdataquery = "INSERT INTO userdata (username) VALUES ('" . $usernameclean . "');";
    mysqli_query($con,$insertuserdataquery) or die("insert data failed " . mysqli_error($con)); //error code 4 - insert query failed
	echo ("0"); //success  just passing back 0



?>