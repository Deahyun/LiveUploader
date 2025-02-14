<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>

<%@page import="java.util.*"%>
<%@page import="com.site.entity.*"%>
<%@ page import="utils.UserLog"%>

<%@ page import="java.io.File" %>
<%@ page import="java.util.ArrayList" %>
<%@ page import="java.util.List" %>

<%@ page import="java.io.File" %>
<%@ page import="java.io.FilenameFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<title>Admin Page</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge, chrome=1" />
<meta http-equiv="Content-Style-Type" content="text/css" />
<link href="css/dash-board.css" rel="stylesheet">
<link href="images/favicon.ico" rel="shortcut icon" />
<script type="text/javascript" src="js/jquery-1.11.1.min.js"></script>
<!--[if lt IE 9]>alert(form_data);
	<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->

<%
	UploadInfo info = new UploadInfo();
	info.seq = 1234;
	info.image_path ="";
	info.param1 = "gogogo1";
	info.param2 = "gogogo2";
	//
	String tag = "upload_file";
	//
%>

<script type="text/javascript">
	$(document).ready( function() {
		// function
		
			}); // ready(function()
</script>
</head>
<body>
	<div class="wrapper">
		<!-- start header  -->
		<header>
		<style>
		     body {
		         font-family: Arial, sans-serif;
		     }
		     .image-list {
		         list-style-type: none;
		         padding: 0;
		     }
		     .image-list li {
		         margin-bottom: 20px;
		     }
		     .image-list img {
		         max-width: 200px;
		         height: auto;
		     }
		</style>
		<h1>dummy</h1>
		</header>
		<!-- end header  -->

		<!-- start aside  -->
		<aside> <!-- start section  --> 
		<section class="popular-recipes">
			<a href="upload_file.jsp">메뉴 1</a>
			<a href="#">메뉴 2</a>
			<a href="#">메뉴 3</a>
			<a href="#">메뉴 4</a>
			<a href="#">메뉴 5</a>

		</section>
		<!-- end section  -->
		
		<!-- start section  -->
		<section class="contact-details">
		<h2>Contact</h2>
		<!-- <div id="contact"></div> -->
		<p>
			If you have any question <br /> email to <font color="#de6581">teraget@gmail.com</font>
		</p>
		</section> <!-- start section  --> </aside>
		<!-- end aside  -->

		<!-- start section  -->
		<section class="courses">
		<ul>
			<h2>
				<font color="green">업로드된 파일 리스트</font>
			</h2>
			<p>
		</ul>
		<ul>
			<p id="message"></p>
		</ul>

		<ul class="image-list">
        <%
            // Define the directory containing images
            String imagePath = application.getRealPath("upload/images");
            File imageDir = new File(imagePath);

            // Check if the directory exists and is accessible
            if (imageDir.exists() && imageDir.isDirectory()) {
                // Get all image files in the directory
                File[] imageFiles = imageDir.listFiles(new FilenameFilter() {
                    @Override
                    public boolean accept(File dir, String name) {
                        return name.toLowerCase().endsWith(".jpg") || 
                               name.toLowerCase().endsWith(".png") || 
                               name.toLowerCase().endsWith(".gif");
                    }
                });

                if (imageFiles != null) {
                    for (File imageFile : imageFiles) {
                        String fileName = imageFile.getName();
                        %>
                        <li>
                            <img src="<%= request.getContextPath() + "/upload/images/" + fileName %>" alt="<%= fileName %>">
                            <p><%= fileName %></p>
                        </li>
                        <%
                    }
                } else {
                    %>
                    <p>No images found in the directory.</p>
                    <%
                }
            } else {
                %>
                <p>Image directory not found or inaccessible.</p>
                <%
            }
        %>
		</ul>

		<footer> &copy; 2025 EnterTera Inc.. All rights reserved. </footer>
	</div>
</body>
</html>