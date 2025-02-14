using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Threading;

using System.Security.Permissions;
using System.IO;
//
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LiveUploaderDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Run();
        }

        void Test()
        {
            if (true)
            {
                byte[] buffer = new byte[20 * 1000 * 1000];
                int bytesRead;
                // upload from file
                string strPath = @"C:\Tmp\1\QR1.JPG";
                using (FileStream fileStream = File.OpenRead(strPath))
                {
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        //requestStream.Write(buffer, 0, bytesRead);
                        //fileStream.Close();
                    }
                    Debug.WriteLine("Read file done.");
                }
            }
        }

        async void SendPostData_v3Async(string strFilePath)
        {
            string url = "http://localhost:8080/t1_svc/upload_file_req.jsp";
            string filePath = strFilePath;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (MultipartFormDataContent form = new MultipartFormDataContent())
                    {
                        // Read the file and create the content
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            var fileContent = new StreamContent(fileStream);
                            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                            // Add the file to the form data with the key expected by the JSP server
                            form.Add(fileContent, "file_param_1", Path.GetFileName(filePath));

                            // If the JSP server expects additional form data (e.g., parameters):
                            form.Add(new StringContent("3456"), "seq");
                            form.Add(new StringContent("value1"), "param1");
                            form.Add(new StringContent("value2"), "param2");

                            // Send the POST request
                            HttpResponseMessage response = await client.PostAsync(url, form);

                            // Check the response
                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine("File uploaded successfully.");
                            }
                            else
                            {
                                Console.WriteLine($"Failed to upload file. Status code: {response.StatusCode}");
                                string responseContent = await response.Content.ReadAsStringAsync();
                                Console.WriteLine($"Response: {responseContent}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        async void SendPostData_v2Async(string strFilePath)
        {
            string url = "http://localhost:8080/t1_svc/upload_file_req.jsp";
            string filePath = strFilePath;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (MultipartFormDataContent content = new MultipartFormDataContent())
                    {
                        // Read the file content
                        byte[] fileData = File.ReadAllBytes(filePath);

                        // Create a ByteArrayContent for the file data
                        ByteArrayContent fileContent = new ByteArrayContent(fileData);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                        //fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/form-data");

                        // Add file content to the multipart form data content
                        //content.Add(fileContent, "file_param_1", Path.GetFileName(filePath));
                        //content.Add(fileContent, "file_param_1", strFilePath);
                        //content.Add(fileContent, "file", strFilePath);
                        content.Add(fileContent, strFilePath, "file_param_1");

                        content.Add(new StringContent("2345"), "seq");
                        content.Add(new StringContent("gogogo1"), "param1");
                        content.Add(new StringContent("gogogo2"), "param2");

                        // Send the POST request
                        HttpResponseMessage response = await client.PostAsync(url, content);
                        // Check the response
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("File uploaded successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to upload file. Status code: {response.StatusCode}");
                            string responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Response: {responseContent}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        void SendPostData_v1(string strFilePath)
        {
            string uri = "http://localhost:8080/t1_svc/upload_file_req.jsp";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);   //요청 준비
            HttpWebResponse response = null;

            try
            {
                request.Method = "POST"; //기본값은 GET
                                         //그 밖에도 timeout, credential 등 다양한 옵션을 줄 수 있습니다.

                //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Disposition
                //2개 이상 multipart로 보내는 방법
                //보낼 값) field1 = value, 파일 = byte file

                //경계를 통해 데이터 타입 명시 및 다음과 같은 형태로 만들어서 보내면 됩니다.
                /*
                    Content-Type: multipart/form-data; boundary=isBoundary
                
                    --isBoundary
                    Content-Disposition: form-data; name="field1"

                    value
                    --isBoundary
                    Content-Disposition: form-data; name="파일"; filename="myImage.tiff" (filename은 부가적인 부분으로 없어도 된다고 봤네요.)
                    Content-Type: image/tiff (타입에 대한 명시 필요)

                    byte file
                    --isBoundary-- (경계의 끝은 -- 붙여주기)
                 */

                string endLine = "\r\n";            //웹에서 개행을 나타내는 문자열입니다. 
                string boundary = "isBoundary";     //구분자입니다. --isBoundary ~ --isBoundary-- 로 끝나게 됩니다.

                request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                byte[] boundaryBytes = Encoding.ASCII.GetBytes(endLine + "--" + boundary + endLine);

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                //
                //--isBoundary
                //

                string field1 = "param1";
                string value = "gogogo1";
                string data = string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{2}{3}", field1, endLine, endLine, value);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                requestStream.Write(dataBytes, 0, dataBytes.Length);
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);

                ///
                string field2 = "param2";
                string value2 = "gogogo2";
                string data2 = string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{2}{3}", field2, endLine, endLine, value2);
                byte[] dataBytes2 = Encoding.UTF8.GetBytes(data2);

                requestStream.Write(dataBytes2, 0, dataBytes2.Length);
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                /// 
                string field3 = "seq";
                string value3 = "4567";
                string data3 = string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{2}{3}", field3, endLine, endLine, value3);
                byte[] dataBytes3 = Encoding.UTF8.GetBytes(data3);

                requestStream.Write(dataBytes3, 0, dataBytes3.Length);
                //requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                /// 
                ///



                //[진행 현황]
                //
                //--isBoundary
                //Content-Disposition; form-data; name="field1"
                //
                //value

                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                //[진행 현황]
                //
                //--isBoundary
                //Content-Disposition; form-data; name="field1"
                //
                //value
                //--isBoundary
                //

                string name = "file_param_1";
                //string filename = strFilePath;
                string filename = Path.GetFileName(strFilePath);

                string data4 = string.Format("Content-Disposition: form-data; name=\"{0}\" filename=\"{1}\"{2}Content-Type: image/tiff{3}{4}",
                                                name, filename, endLine, endLine, endLine);
                byte[] data2Bytes = Encoding.UTF8.GetBytes(data4);

                requestStream.Write(data2Bytes, 0, data2Bytes.Length);
                //[진행 현황]
                //
                //--isBoundary
                //Content-Disposition; form-data; name="field1"
                //
                //value
                //--isBoundary
                //Content-Disposition: form-data; name="파일" filename="myImage.tiff"
                //Content-Type: image/tiff
                //
                //

                //예시 경로 파일 바이트로 읽어서 보내기
                string imagePath = strFilePath;
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] imageBuffer = new byte[4096];
                    int bytesRead = 0;
                    while (true)
                    {
                        bytesRead = fileStream.Read(imageBuffer, 0, imageBuffer.Length);
                        if (bytesRead <= 0) //이미지 바이트 다 읽은 경우
                        {
                            break;
                        }
                        requestStream.Write(imageBuffer, 0, bytesRead);
                    }
                    //fileStream.Close();
                }

                //FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                //byte[] imageBuffer = new byte[4096];
                //int bytesRead = 0;
                //while (true)
                //{
                //    bytesRead = fileStream.Read(imageBuffer, 0, imageBuffer.Length);
                //    if (bytesRead <= 0) //이미지 바이트 다 읽은 경우
                //    {
                //        break;
                //    }
                //    requestStream.Write(imageBuffer, 0, bytesRead);
                //}
                //fileStream.Close();

                //fileStream.Dispose();
                //fileStream = null;

                byte[] endBoundaryByte = Encoding.ASCII.GetBytes(endLine + "--" + boundary + "--" + endLine);
                requestStream.Write(endBoundaryByte, 0, endBoundaryByte.Length);
                //[진행 현황]
                //
                //--isBoundary
                //Content-Disposition; form-data; name="field1"
                //
                //value
                //--isBoundary
                //Content-Disposition: form-data; name="파일" filename="myImage.tiff"
                //Content-Type: image/tiff
                //
                //byte file
                //--isBoundary--

                response = (HttpWebResponse)request.GetResponse(); //요청 후 결과 반환
                Stream responseStream = response.GetResponseStream();   //요청에 대한 stream 가져오기
                StreamReader responseStreamReader = new StreamReader(responseStream); //해당 stream 읽기 위한 streamReader

                //응답값 받아서 리턴하거나 사용하면 됩니다.
                string result = responseStreamReader.ReadToEnd();
                Debug.WriteLine(result);
                responseStreamReader.Close();
                responseStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                //로그 남기면 좋을 듯
                MessageBox.Show("Request Error" + Environment.NewLine + ex.ToString());
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }


        public class FileForm
        {
            public string Name { get; set; }
            public string ContentType { get; set; }
            public string FilePath { get; set; }
            public Stream Stream { get; set; }
        }

        void SendPostData_v0(string strFilePath)
        {

            string boundary = string.Format("-------------------{0:N}", Guid.NewGuid());
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            // 요청 URL
            string url = "http://localhost:8080/t1_svc/upload_file_req.jsp";

            // Request 생성
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // HTTP Method 설정
            request.Method = "POST"; // 필수
            // header
            request.ContentType = "multipart/form-data; boundary=" + boundary; // 필수
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            // 전송 할 데이터
            Dictionary<string, Object> dic = new Dictionary<string, object>();
            FileForm fileData = new FileForm();
            fileData.Name = "a1.jpg";
            fileData.ContentType = "application/octet-stream";
            fileData.FilePath = strFilePath;
            //fileData.FilePath = "C:\\Tmp\\111\\QR2.JPG";
            //string key = "uploadFile";
            string key = "file_param_1";
            dic.Add(key, fileData);
            //
            dic.Add("seq", 2345);
            dic.Add("param1", "sososo1");
            dic.Add("param2", "gogogo1");

            using (Stream requestStream = request.GetRequestStream())
            {
                // 전송 과정
                foreach (KeyValuePair<string, object> pair in dic)
                {
                    requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    if (pair.Value is FileForm)
                    {
                        FileForm file = pair.Value as FileForm;
                        string header = "Content-Disposition: form-data; name=\"" + "paramName" + "\"; filename=\"" + file.Name + "\"\r\nContent-Type: " + file.ContentType + "\r\n\r\n";
                        byte[] bytes = Encoding.UTF8.GetBytes(header);
                        requestStream.Write(bytes, 0, bytes.Length);

                        //byte[] buffer = new byte[32768];
                        byte[] buffer = new byte[20 * 1000 * 1000];
                        int bytesRead;
                        // upload from file
                        using (FileStream fileStream = File.OpenRead(file.FilePath))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                requestStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }

                    byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    requestStream.Write(trailer, 0, trailer.Length);
                    //requestStream.Close();
                }
            }

            //
            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string result = reader.ReadToEnd();
                }
            }
        }

        bool IsFileLocked(string filePath)
        {
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    return false; // File is not locked
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"error: {ex.Message}");
                return true; // File is locked
            }
        }


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        void Run()
        {
            //string[] args = Environment.GetCommandLineArgs();
            //// If a directory is not specified, exit program.
            //if (args.Length != 2)
            //{
            //    // Display the proper way to call the program.
            //    Console.WriteLine("Usage: Watcher.exe (directory)");
            //    return;
            //}

            //Console.WriteLine("Watcher directory:{0}", args[0]);


            string[] args = { "LIveUplader", "C:\\Tmp\\1" };
            Debug.WriteLine(string.Format("Watcher directory:{0}", args[1]));
            // Create a new FileSystemWatcher and set its properties.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = args[1];

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                //watcher.NotifyFilter = NotifyFilters.LastAccess
                //                     | NotifyFilters.LastWrite
                //                     | NotifyFilters.FileName
                //                     | NotifyFilters.DirectoryName;

                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "*.*";

                // Add event handlers.
                watcher.Created += OnCreated;
                //watcher.Changed += OnChanged;
                //watcher.Deleted += OnDeleted;
                //watcher.Renamed += OnRenamed;
                ///
                //watcher.Created += (s, e) =>
                //{
                //    Task.Delay(200).ContinueWith(_ =>
                //    {
                //        // Access the file here
                //        Debug.WriteLine($"File: {e.FullPath} {e.ChangeType} create");
                //        string strPath = e.FullPath;
                //        SendPostData(strPath);
                //    });
                //};
                ///

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                //Console.WriteLine("Press 'q' to quit the sample.");
                //while (Console.Read() != 'q') ;

                while (true)
                {
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(100);
                }

            }
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            //Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            Debug.WriteLine($"File: {e.FullPath} {e.ChangeType} change");
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            //Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            Debug.WriteLine($"File: {e.FullPath} {e.ChangeType} create");

            string strPath = e.FullPath;
            //
            int nRetry = 5;
            for (int i = 0; i < nRetry; i++)
            {
                if (IsFileLocked(strPath))
                {
                    //Debug.WriteLine(string.Format("{0} file is locked[{1}]", strPath, i));
                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }

            SendPostData_v1(strPath);
            //SendPostData_v3Async(strPath);
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            //Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            Debug.WriteLine($"File: {e.FullPath} {e.ChangeType} delete");
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            //Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
            Debug.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }
    }
}
