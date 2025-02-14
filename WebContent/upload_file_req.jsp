<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>

<%@ page trimDirectiveWhitespaces="true" %>
<%@ page import="java.io.*"%>
<%@ page import="java.util.*"%>     
<%@ page import="java.io.PrintWriter"%>

<%@ page import="java.util.Date" %>
<%@ page import="java.text.*" %>
<%@ page import="java.sql.*" %>
<%@ page import="javax.sql.*" %>
<%@ page import="javax.naming.*" %>
<%@ page import="org.json.JSONArray" %>
<%@ page import="org.json.JSONObject" %>

<%@ page import="com.oreilly.servlet.*"%>
<%@ page import="com.oreilly.servlet.MultipartRequest"%>
<%@ page import="com.oreilly.servlet.multipart.DefaultFileRenamePolicy"%>

<%@ page import="utils.UserLog" %>

<%@page import="com.site.entity.*"%>

<%@ page import="utils.*"%>

<%!
String errorResult() {
	JSONObject jo = null;
	try {
		jo = new JSONObject();
		jo.put("result", "no");
	} catch ( Exception e ) {
		
	}
	return jo.toString();
}
%>

<%
	response.setContentType("text/html;charset=UTF-8; application/octet-stream");
	//response.setContentType("application/octet-stream");
	request.setCharacterEncoding("UTF-8");
	
	String tag = "upload_file_req";

	String urlPath = "upload"; // url path
	String encType = "UTF-8"; //엔코딩 타입
	int maxSize = 20 * 1024 * 1024; //최대 업로드될 파일크기 20MB
	// 현재 jsp 페이지의 웹 어플리케이션 상의 절대 경로를 구한다
	ServletContext context = getServletContext();
	String uploadRealFolder = context.getRealPath(urlPath);
	//
	UserLog.Log(tag, "the realpath is : [" + uploadRealFolder + "]<br>");
	
	boolean bWindowsOS;
	String strData = System.getProperty("os.name");
   	String strOS = strData.toLowerCase();
   	if ( strOS.contains("windows") ) {
   		// windows os
   		bWindowsOS = true;
   	} else {
   		// other os. like linux
   		bWindowsOS = false;
   	}
   	//
   	String photoUrlPath = "upload/images";
   	String photoRealFolder = context.getRealPath(photoUrlPath);
   	UserLog.Log(tag, "the realpath is : [" + photoRealFolder + "]<br>");
   	
	// 폴더가 존재하지 않으면 폴더를 만든다.
	File upload_dirs = new File(uploadRealFolder);
	if ( !upload_dirs.exists() ) upload_dirs.mkdirs();
	//
	File photo_dirs = new File(photoRealFolder);
	if ( !photo_dirs.exists() ) photo_dirs.mkdirs();
	//
	//
	boolean bUpdatePhoto = true;
	//
	String strNewFile1 = null;
	String strNewFile2 = null;
	////
	try {
		MultipartRequest multi = null;
		multi = new MultipartRequest(request, uploadRealFolder, maxSize, encType);

		//////
		// 전송되는 post parameter 이름
		String strSeq = multi.getParameter("seq");
		int nSeq = Integer.parseInt(strSeq);
		//String strServerId = multi.getParameter("ars_server_id");
		String strParam1 = multi.getParameter("param1");
		String strParam2 = multi.getParameter("param2");
		
		String strPost = String.format("%s, %s, %s", strSeq, strParam1, strParam2);
		UserLog.Log(tag, strPost);

		// file operation
		String file1 = multi.getFilesystemName("file_param_1");
		UserLog.Log(tag, file1);
		//String original_file1 = multi.getOriginalFileName("file_param_1");
		if ( file1 == null || file1.length() == 0 ) {
			bUpdatePhoto = false;
			
		} else {
			//
			UserLog.Log(tag, "download1 -> " + file1);
			//UserLog.Log(tag, "download2 -> " + original_file1);
			//
			String[] arrParam = file1.split("\\.");
			if ( arrParam.length != 2 ) {
				UserLog.Log(tag, "잘못된 파일 형식입니다.");
				out.print(errorResult());
				return;
			}
			
			//strNewFile1 = String.format("%s.%s", strParam1, arrParam[1]);
			strNewFile1 = file1;
			
			// rename file: not automic
			if ( file1 != null ) {
				UserLog.Log(tag, "file_param_1 -> " + file1 + ".");
				File oldFile1 = new File(uploadRealFolder + File.separator +  file1);
				File newFile1 = new File(photoRealFolder + File.separator + strNewFile1);
				//
				if ( newFile1.exists() ) {
					newFile1.delete();
					UserLog.Log(tag, " delete file1 -> " + newFile1);
				}
				//
				byte[] buf = new byte[1024];
				FileInputStream fin = null;
				FileOutputStream fout = null;
				//
				if ( !oldFile1.renameTo(newFile1) ) {
					UserLog.Log(tag, "rename failure 1");
					//
					buf = new byte[1024];
				    fin = new FileInputStream(oldFile1);
				    fout = new FileOutputStream(newFile1);
				 
				    int read = 0;
				    while ( (read = fin.read(buf,0,buf.length)) != -1 ) {
				        fout.write(buf, 0, read);
				    }
				     
				    fin.close();
				    fout.close();
				    oldFile1.delete();
				}
				UserLog.Log(tag, "새로운 파일 -> " + strNewFile1 + ".");
				// thumbnail
				//UserFunc.makeThumbnail(photoRealFolder, strNewFile1);
			}
		} // end if
		
		//
		UploadInfo info = new UploadInfo();
		//
		try {
			//
			info.seq = nSeq;
			info.param1 = strParam1;
			info.param2 = strParam2;
			//
			if ( bUpdatePhoto ) {
				info.image_path = String.format("%s/%s", photoUrlPath, strNewFile1);
			} else {
				info.image_path = null;
			}
		} catch ( Exception e ) {
			UserLog.Log(tag, e.getStackTrace() + "\n" + e.getMessage());
			out.print(errorResult());
			return;
		}

	} catch (IOException ioe) {
		System.out.println(ioe);
		out.print(errorResult());
		return;
		
	} catch (Exception ex) {
		System.out.println(ex);
		out.print(errorResult());
		return;
	}
	
	JSONObject jo = new JSONObject();
	jo.put("result", "yes");
	out.print(jo.toString());
	//UserLog.Log(tag, jo.toString());
%>