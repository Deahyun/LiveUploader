windows10 에서 C# 으로 제작된 프로그램입니다.
"C:\Tmp\1" 디렉토리에 파일이 생기면 해당 파일을 tomcat8 기반의 서버에 POST 방식으로 업로드하는 프로그램입니다.

![Image](https://github.com/user-attachments/assets/ce68ced4-2735-47db-9d08-ed07b2680707)

<a href="https://drive.google.com/file/d/1dQJKXW7qta3OPSG76dNijrJ_WjDz_3za/view?usp=drive_link" target="_blank">동영상 링크</a>

LiveUploaderDemo 은 Visual Studio 2017 로 만든 C# 프로그램이며, 
기능은 폴더(C:\Tmp\1") 감시 중 파일 생성시 tomcat8 서버로 해당 파일을 POST 로 업로드 합니다.
LiveUploader/LiveUploaderDemo.sln 파일은 LiveUploaderDemo 의 솔루션 파일입니다. 

t1_svc 은 tomcat8 서버에서 가동되는 java 프로그램이며, 기능은 LiveUploaderDemo 프로그램으로 부터 전송된 파일을 서버내의 특정 폴더로 받아주고
전송된 파일이 이미지 파일일 경우 보여주는 역할을 합니다. 
