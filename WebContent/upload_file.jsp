<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>

<%@page import="java.util.*"%>
<%@page import="com.site.entity.*"%>
<%@ page import="utils.UserLog"%>

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
		// 4자리 이하 임의의 자리 숫자값 (예: '1234') 체크
		function validCode(value) {
			var isCode = /^[0-9]{1,4}$/;
			if ( !isCode.test(value) ) {
				return false;
			}
			return true;
		}
		
		// 4자리 숫자값 체크
		function validPwd(value) {
			var isCode = /^[0-9]{4}$/;
			if ( !isCode.test(value) ) {
				return false;
			}
			return true;
		}
		
		// init load
		$("#seq").val("<%=info.seq%>");
		$("#param1").val("<%=info.param1%>");
		$("#param2").val("<%=info.param2%>");
		//
		
		// 이부분은 $('#fileupload1').fileupload({ }); 블럭내로 들어가서 동작한다. ???
		$("#update_button").click( function() {
			//
			var seq = $("#seq").val();
			var param1 = $("#param1").val();
			var param2 = $("#param2").val();
			//			
			if (param1.length == 0
					|| param2.length == 0) {
				$("#message").html("<font color=red>빈 항목없이 입력후 파일을 선택해 주세요.</font>");
				setTimeout(function() {
					$("#message").html("");
				}, 5000);
				//$('#fileupload1')[0].reset();
				return;
			}

			// photo 파일 이름이 정상적인지 체크한다.
			var bUpdatePhoto = true;
			var up_file = $("#file_param_1").val();

			if (up_file == null || up_file.length == 0) {
				bUpdatePhoto = false;

			} else {
				var up_file_ext = up_file.split(".").pop();
				up_file_ext = up_file_ext.toLowerCase();
				if (up_file_ext == "jpg" || up_file_ext == "png") {
					// correct
					$("#message").html("");

				} else {
					$("#message").html("<font color=red>확장자가 jpg, png 인 파일만 업로드 할 수 있습니다.</font>");
					setTimeout(function() {
						$("#message").html("");
					}, 3000);
					$("#file_name_1").val("");
					return;
				}
			}

			if (bUpdatePhoto) {
				//alert("data with photo -> submit");
				$("#fileupload1").submit();
			} else {
				//alert("data -> submit");
				$("#fileupload1").submit();
			}
			//

		}); // photo_upload_button click(function()

		$("#fileupload1").submit(
			function(e) {
				var formObj = $(this);
				var formUrl = formObj.attr('action');
				var formData = new FormData(this);
					$.ajax({
						type : "POST",
						url : formUrl,
						data : formData,
						mimeType : "multipart/form-data",
						contentType : false,
						cache : false,
						processData : false,
						success : function(
								data,
								textStatus,
								jqXHR) {
							//alert(data);
							var jo = JSON.parse(data);
							var msg = jo.result;
							//alert(msg);
							if (msg == "yes") {
								//
								$("#message").html("<font color=blue>업로드 성공</font>");
								$("#fileupload1").slideUp('normal', function() {
													//location.reload();
													window.location.href = "#";
												});
								//
							} else {
								$("#message").html("<font color=red>업로드 실패</font>");
								setTimeout(function() {
											$("#message").html("");
											//$('#fileupload1')[0].reset();
										}, 5000);
							}
							//
						},
						error : function(jqXHR,
								textStatus,
								errorThrown) {
						}
					});
					e.preventDefault();
					e.unbind();
				});

				// file upload button implement				
				$('#file_btn_photo').click(function() {
					$('#file_param_1').click();
				});

			}); // ready(function()
</script>
</head>
<body>
	<div class="wrapper">
		<!-- start header  -->
		<header>
		<h1>dummy</h1>
		</header>
		<!-- end header  -->

		<!-- start aside  -->
		<aside> <!-- start section  --> 
		<section class="popular-recipes">
			<a href="#">메뉴 1</a>
			<a href="list_file.jsp">메뉴 2</a>
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
				<font color="green">업로드 테스트 페이지</font>
			</h2>
			<p>
		</ul>
		<ul>
			<p id="message"></p>
		</ul>
		<form id="fileupload1" name="fileupload1" action="upload_file_req.jsp" method="POST" enctype="multipart/form-data">
			<table>
				<tr>
					<td>일련번호(코드)</td>
					<td>
						<p class="cb_general">
							<%=info.seq%>
						</p>
					</td>
				</tr>
				<input type="hidden" id="seq" name="seq"  value="<%=info.seq%>"/>
				<tr>
					<td>Param1</td>
					<td><input type="text" id="param1" name="param1" maxlength="8" />(8자리 이하)</td>
				</tr>
				<tr>
					<td>Param2<br>(400 자리 이하)
					</td>
					<td>
						<textarea id="param2" name="param2" cols="56" rows="8" maxlength="400"><%=info.param2%></textarea>
					</td>
				</tr>
				<tr>
					<td>사진</td>
					<td>
						<input type="file" id="file_param_1" name="file_param_1" class="input_text" /> <img src="<%=info.image_path%>" border=0 height="100">
					</td>
				</tr>
			</table>
		</section>
		<!-- end section  -->

		<!-- start section  -->
		<section class="save_buttons">
		<div class="btn_action">
			<p class="va_upload">
				<input type="button" id="update_button" class="exec_btn" value="파일 업로드" /> 
			</p>
		</div>
		</section>
		<!-- end section  -->

		</form>

		<footer> &copy; 2025 EnterTera Inc.. All rights reserved. </footer>
	</div>
</body>
</html>