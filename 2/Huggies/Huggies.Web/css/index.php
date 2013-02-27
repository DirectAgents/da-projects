<?php

session_start();

include ('config.php');

//Warnings will not be shown.
ini_set('display_errors', 1);
ini_set('error_reporting', E_ERROR);

if ($_POST) {
// if the form has been submitted



function getRealIpAddr()
{
    if (!empty($_SERVER['HTTP_CLIENT_IP']))   //check ip from share internet
    {
      $ip=$_SERVER['HTTP_CLIENT_IP'];
    }
    elseif (!empty($_SERVER['HTTP_X_FORWARDED_FOR']))   //to check ip is pass from proxy
    {
      $ip=$_SERVER['HTTP_X_FORWARDED_FOR'];
    }
    else
    {
      $ip=$_SERVER['REMOTE_ADDR'];
    }
    return $ip;
}




$ip_address = getRealIpAddr();




$the_date = date("Y-m-d");
date_default_timezone_set('America/New_York');
$the_time = date('h:i:s A');




$insert_sql = "INSERT INTO data (Firstname, Lastname, Email, Zip, Ethnicity, Language, Firstchild, Duedate, Gender, CD, Date, Time) values 
('".$_POST['fm_firstname']."', '".$_POST['fm_lastname']."' , '".$_POST['fm_email']."','".$_POST['fm_zip']."',
'".$_POST['fm_ethnicity']."','".$_POST['fm_language']."',
'".$_POST['fm_firstchild']."','".$_POST['fm_duedate']."',
'".$_POST['fm_gender']."','".$_POST['fm_pid']."', '".$the_date."', '".$the_time."')";

				
$rs=mysql_query($insert_sql);	


//$_SESSION['email'] = $_POST['fm_email'];
$_SESSION["refdir2"] = $_POST['fm_pid'];

$_SESSION["email"] = $_POST['fm_email'];




//echo $_SESSION['unique_id'];
			
header('Location: thank_you.php');

 

  }
	

?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>Huggies</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>

<!--CSS-->

<link href="css/style.css" rel="stylesheet" type="text/css" />

<!--[if IE 7]>
		<link href="css/style_IE7.css" rel="stylesheet" type="text/css" />	
	<![endif]-->

<!--JAVASCRIPT-->
<script type="text/javascript" src="js/validate.js"></script>
<script src="js/customfade.js" type="text/javascript"></script>
<script type="text/javascript" src="js/jquery.min.js"></script>
<script language="javascript" type="application/javascript" src="js/cal2.js"></script>
<script language="javascript" type="application/javascript" src="js/cal_conf2.js"></script>
<script type="application/javascript">

$(document).ready(function() {
	
	$('#due_date').hide();
	$('#child_gender').hide();
	$("#fm_duedate").attr('class', '');
	
	
var currentradio= $("input[name='fm_firstchild']:checked")[0];
$("input[name='fm_firstchild']").change(function(event) {
	
	var value = $(this).val();
	
	if(value == 'Yes'){
		$('#due_date').show();
		$('#child_gender').show();
		$("#fm_duedate").attr('class', 'duedate req');
	}
	
	if(value == 'No'){
		$('#due_date').hide();
		$('#child_gender').hide();
		$("#fm_duedate").attr('class', '');
	}
   
       
 
});

});



</script>
</head>

<body>
<div id="top">
  <div id="container">
    <div id="header">
      <div id="logo"><img src="images/logo.jpg" alt="Huggies"/> </div>
    </div>
    <div id="main">
      <div id="special_offers"><img src="images/special_offers.png" alt="Special Offers"/></div>
      <div id="baby"><img src="images/baby.png" alt="" /></div>
      <div id="box2">
        <div id="details"><img src="images/details.gif" alt="" /></div>
        <!-- ****************FORM START **************** -->
        
        <form method="post" action="" enctype="multipart/form-data" id="cForm" name="cForm" class="validate">
          
          <!--****************PID ****************-->
          
          <input type="hidden" class="pid" value="" id="fm_pid" name="fm_pid"/>
          
          <!--****************PID ****************-->
          
          <table id="form_evaluation">
            <tr>
              <td><div id="innerform">
                  <div class="row"> <span class="label">
                    <label for="fm_firstname">First Name</label>
                    </span> <span class="form">
                    <input type="text" class="firstname req" name="fm_firstname" id="fm_firstname" size="25" maxlength="20" tabindex="1" />
                    </span> </div>
                  <div class="row"> <span class="label">
                    <label for="fm_lastname">Last Name</label>
                    </span> <span class="form">
                    <input type="text" class="lastname req" name="fm_lastname" id="fm_lastname" size="25" maxlength="20" tabindex="2" />
                    </span> </div>
                  <div class="row"> <span class="label">
                    <label for="fm_email">Email Address</label>
                    </span> <span class="form">
                    <input  type="text" class="email req" name="fm_email" id="fm_email" size="25" maxlength="100" tabindex="3" />
                    </span> </div>
                  <div class="row"> <span class="label">
                    <label for="fm_zip">Zip/Postal Code</label>
                    </span> <span class="form">
                    <input  type="text" class="zip req" name="fm_zip" id="fm_zip" size="25" maxlength="10" tabindex="4" />
                    </span> </div>
                  <div class="row"> <span class="label">
                    <label for="fm_ethnicity">Ethnicity</label>
                    </span> <span class="form">
                    <select name="fm_ethnicity" class="ethnicity req" id="fm_ethnicity" tabindex="5">
                      <option value="fm_ethnicity">Select ethnicity...</option>
                      <option value="American Indian or Alaskan Native">American Indian or Alaskan Native</option>
                      <option value="Asian">Asian</option>
                      <option value="African American">African American</option>
                      <option value="Hispanic/Latino">Hispanic/Latino</option>
                      <option value="Pacific Islander">Pacific Islander</option>
                      <option value="White/Caucasian">White/Caucasian</option>
                      <option value="Other">Other</option>
                    </select>
                    </span> </div>
                  <div class="row2"> <span class="label2">
                    <label for="fm_language">In what language do you prefer to receive HUGGIES&reg; Brand communications?</label>
                    </span> <span class="form">
                    <select name="fm_language" class="language req" id="fm_language" tabindex="6">
                      <option value="fm_language">Select language...</option>
                      <option value="English">English</option>
                      <option value="Spanish">Spanish</option>
                      <option value="French">French</option>
                    </select
      >
                    </span> </div>
                  <div class="row"> <span class="label">
                    <label for="fm_firstchild">Is this your first child?</label>
                    </span> <span class="form">
                    <input type="radio" name="fm_firstchild" id="fm_firstchild" value="Yes"/>
                    Yes&nbsp;&nbsp;
                    <input type="radio" name="fm_firstchild" value="No" checked="checked"/>
                    No </span> </div>
                  <div id="due_date">
                    <div class="row3"> <span class="label3">
                      <label for="fm_duedate">Due date or birthdate of your child?</label>
                      </span> <span class="form">
                      <input type="text" name="fm_duedate" id="fm_duedate" class="duedate req" style="width:216px !important;"/>
                      <small><a href="javascript:showCal('Calendar1')"><img src="images/cal.jpg" alt=""/></a></small> </span> </div>
                  </div>
                  <div id="child_gender">
                    <div class="row"> <span class="label">
                      <label for="fm_gender">Child Gender:</label>
                      </span> <span class="form">
                      <input type="radio" size="1" name="fm_gender" id="fm_gender" value="Boy" style="float:left; display:inline;"/>
                      <span class="boy">Boy</span>
                      <input type="radio" name="fm_gender" value="Girl" style="float:left; display:inline;"/>
                      <span class="girl">Girl</span>
                      <input type="radio" name="fm_gender" value="I dont know yet" style="float:left; display:inline;" checked="checked"/>
                      <span class="i-dont-know">I don't know yet</span> </span> </div>
                  </div>
                  <button id="Submit" type="submit" class="submitbutton" name="form_submitted"  tabindex="10"></button>
                </div></td>
            </tr>
          </table>
        </form>
        
        <!-- ****************FORM END **************** --> 
        
      </div>
    </div>
  </div>
  <div class="footer">
    <div id="footer_container">
      <div class="text"> All names, logos and trademarks are the property of Kimberly-Clark Worldwide, Inc. or its affiliates. &copy; 2013 . All rights reserved.
        Your visit to this site and use of the information hereon is subject to the terms of our <a href="#">Privacy Policy</a>. Please review our <a href="#">Legal Statement</a>. </div>
    </div>
  </div>
</div>
</body>
</html>
