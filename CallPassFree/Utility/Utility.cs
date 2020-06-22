using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kons.ShopCallpass.Utility
{
    class Utility
    {
        public static void LogWrite(string str)
        {
            Debug.WriteLine(str);

            string FilePath = Environment.GetLogicalDrives().ElementAt(0) + @"\CallPass_log\CallpassLog.log";
            string DirPath = Environment.GetLogicalDrives().ElementAt(0) + @"\CallPass_log";
            string temp;

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (!di.Exists) Directory.CreateDirectory(DirPath);
                if (!fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                LogWrite("로그함수에서 발생 : " + e.Message);
            }
        }
        public static string StringToInputType(string _inputString)
        {
            string value = "";

            switch (_inputString)
            {
                case "배달의민족":
                    value = "10";
                    break;
                case "OKPOS(오케이포스)":
                    value = "21";
                    break;
                case "배달천재":
                    value = "26";
                    break;
                case "포스피드":
                    value = "27";
                    break;
                case "이지포스":
                    value = "31";
                    break;
                case "요기요":
                    value = "32";
                    break;
                case "배달통":
                    value = "33";
                    break;
                case "기타":
                    value = "98";
                    break;
                case "[출력만사용]":
                    value = "99";
                    break;
            }
            return value;
        }

        public static void SendHTTP(string _sEntity, string _url)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(_sEntity);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                LogWrite("디비연동 예외발생 : " + ex.Message);
            }
        }

    }
}
