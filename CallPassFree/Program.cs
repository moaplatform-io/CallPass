using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.Drawing;
using Kons.ShopCallpass.AppMain;
using System.Diagnostics;
using System.IO;
using System.Net;
//using ICSharpCode.SharpZipLib.Zip;
using Kons.ShopCallpass.Model;
using Kons.ShopCallpass.Utility;
using System.IO.Compression;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Text;

using System.Collections.Generic;

namespace CallpassPcAppMain
{
    //TCP
    #region
    //public class ClientHandler1
    //{
    //    public StreamReader readerStream;

    //    //쓰레드에서 처리할 부분
    //    //계속 값을 읽어 서버에서 보내는 값이 있으면 화면에 출력
    //    //계속 대기해야 하므로 쓰레드로 처리
    //    public void runClient()
    //    {
    //        try
    //        {
    //            bool flag = true;
    //            bool isMyData = false;
    //            string confFilePath = "";

    //            while (true)
    //            {
    //                string returnDate = "";
    //                returnDate = readerStream.ReadLine();
    //                Console.WriteLine("returnData : " + returnDate);

    //                if (flag)
    //                {
    //                    ModelAppDevice device = new ModelAppDevice();
    //                    string crn = device.readConfigString("CONFIG_STORE_API_INFO", "store_pno_" + device.readConfigString("CONFIG_LAST_DELIVERY_REQUEST_INFO", "delivery_company_type"));
    //                    if (returnDate.Contains(crn))
    //                    {
    //                        isMyData = true;
    //                        returnDate = returnDate.Replace(crn, "");
    //                    }
    //                }

    //                if (isMyData)
    //                {
    //                    if (returnDate.Contains("show me a config file"))
    //                    {
    //                        if (File.Exists(/*@"C:\Program Files (x86)\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini"*/"Kons.ShopCallpass_cfg.ini"))
    //                        {
    //                            string str = File.ReadAllText(/*@"C:\Program Files (x86)\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini"*/"Kons.ShopCallpass_cfg.ini");
    //                            Console.WriteLine(str);
    //                            byte[] buf = new byte[20000];
    //                            str += "endString";
    //                            str += "\r\n";

    //                            buf = Encoding.Default.GetBytes(str);
    //                            //Console.WriteLine("buf한번 보자 : " + Encoding.Default.GetString(buf));
    //                            readerStream.BaseStream.Write(buf, 0, buf.Length);
    //                            Console.WriteLine("conf전송asdf");

    //                        }
    //                        else if (File.Exists(@"C:\Program Files\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini"))
    //                        {
    //                            string str = File.ReadAllText(@"C:\Program Files\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini");
    //                            Console.WriteLine(str);
    //                            str += "endString";
    //                            str += "\r\n";

    //                            byte[] buf = new byte[20000];
    //                            buf = Encoding.Default.GetBytes(str);
    //                            readerStream.BaseStream.Write(buf, 0, buf.Length);
    //                            Console.WriteLine("conf전송");
    //                        }
    //                        else
    //                        {
    //                            Console.WriteLine("구성파일 없음");
    //                            string response = "can not found config file";
    //                            response += "\r\n";
    //                            byte[] buf = new byte[20000];
    //                            buf = Encoding.Default.GetBytes(response);
    //                            readerStream.BaseStream.Write(buf, 0, buf.Length);
    //                        }
    //                        isMyData = false;
    //                    }
    //                    else if (returnDate.Contains("show me a log file"))
    //                    {
    //                        if (File.Exists(@"C:\CallPass_log\Log_20191221.log"))
    //                        {
    //                            string str = File.ReadAllText(@"C:\CallPass_log\Log_20191221.log");
    //                            Console.WriteLine(str);
    //                            str += "endString";
    //                            str += "\r\n";

    //                            byte[] buf = new byte[20000];
    //                            buf = Encoding.Default.GetBytes(str);
    //                            readerStream.BaseStream.Write(buf, 0, buf.Length);
    //                            Console.WriteLine("log전송");

    //                        }
    //                        else
    //                        {
    //                            Console.WriteLine("로그파일 없음");
    //                            string response = "can not found log file";
    //                            response += "\r\n";
    //                            byte[] buf = new byte[20000];
    //                            buf = Encoding.Default.GetBytes(response);
    //                            readerStream.BaseStream.Write(buf, 0, buf.Length);
    //                        }
    //                        isMyData = false;
    //                    }
    //                    else
    //                    {
    //                        if (returnDate != "")
    //                        {
    //                            if (flag == true)
    //                            {
    //                                if (File.Exists(/*@"C:\Program Files (x86)\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini"*/"Kons.ShopCallpass_cfg.ini"))
    //                                {
    //                                    FileInfo fileDel = new FileInfo(/*@"C:\Program Files (x86)\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini"*/"Kons.ShopCallpass_cfg.ini");
    //                                    fileDel.Delete();
    //                                    flag = false;
    //                                    confFilePath = /*@"C:\Program Files (x86)\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini"*/"Kons.ShopCallpass_cfg.ini";
    //                                }
    //                                else if (File.Exists(@"C:\Program Files\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini"))
    //                                {
    //                                    FileInfo fileDel = new FileInfo(@"C:\Program Files\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini");
    //                                    fileDel.Delete();
    //                                    flag = false;
    //                                    confFilePath = @"C:\Program Files\Default Company Name\Callpass\Kons.ShopCallpass_cfg.ini";
    //                                }
    //                            }

    //                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(confFilePath, true, Encoding.Default))
    //                            {
    //                                file.WriteLine(returnDate);
    //                            }
    //                            Console.WriteLine("qwerqwer");
    //                            if (returnDate == "endString")
    //                            {
    //                                Console.WriteLine("herehere");
    //                                flag = true;
    //                                string response = "endString";
    //                                response += "\r\n";
    //                                byte[] buf = new byte[20000];
    //                                buf = Encoding.Default.GetBytes(response);
    //                                Console.WriteLine("endString check");
    //                                readerStream.BaseStream.Write(buf, 0, buf.Length);
    //                                isMyData = false;
    //                                Console.WriteLine("conf업데이트");
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (returnDate != "")
    //                    {
    //                        string response = "endString";
    //                        response += "\r\n";
    //                        byte[] buf = new byte[20000];
    //                        buf = Encoding.Default.GetBytes(response);
    //                        Console.WriteLine("endString check");
    //                        readerStream.BaseStream.Write(buf, 0, buf.Length);
    //                        Console.WriteLine("나아니야");
    //                    }
    //                }

    //                //readerStream.BaseStream.Write()
    //                Console.WriteLine("Server : " + returnDate);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.ToString());
    //        }
    //    }
    //}
    #endregion // TCP

    public class FormFile
    {
        public string Name;
        public string ContentType;
        public string FilePath;
        public Stream Stream;
    }

    public static class Program
    {
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        public static FileInfo fileInfo = null;
        public static string fileLocate = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //TCP
            #region
            //TcpClient client = null;
            //try
            //{
            //    //한글처리를 위한 인코딩
            //    Encoding encode = Encoding.GetEncoding("KS_C_5601-1987");

            //    //TcpClient를 이용하여 서버의 5001번 포트로 접속
            //    client = new TcpClient();
            //    client.Connect("localhost", 5001);

            //    NetworkStream stream = client.GetStream();
            //    StreamReader readerStream = new StreamReader(stream, encode);

            //    string sendstr = "";
            //    byte[] senddata = new byte[200];
            //    ClientHandler1 cHandler = new ClientHandler1();

            //    //서버로 부터 날아오는 메세지 처리를 위한 쓰레드 생성
            //    //계속 대기해야 하므로 쓰레드로 처리
            //    Thread clientThread = new Thread(new ThreadStart(cHandler.runClient));
            //    clientThread.Start();
            //    cHandler.readerStream = readerStream;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}
            //finally
            //{
            //    //client.Close();
            //}
            #endregion

            DownloadCheck();

            OperatingSystem os = System.Environment.OSVersion;
            Version v = os.Version;

            Utility.LogWrite("major : " + v.Major + " minor : " + v.Minor);
            
            if (fileLocate == null)
            {
                try
                {
                    if (Environment.Is64BitOperatingSystem)
                    {
                        Utility.LogWrite("com0com download version windows 64bit");
                        WebClient myWebClient = new WebClient();
                        myWebClient.DownloadFile("http://175.198.102.230:8016/bServer/res/b2b/Windows7-64bit.exe.exe",
                            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Windows7-64bit.exe");

                        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Windows7-64bit.exe");
                    }
                    else
                    {
                        Utility.LogWrite("com0com download version windows 32bit");
                        WebClient myWebClient = new WebClient();
                        myWebClient.DownloadFile("http://175.198.102.230:8016/bServer/res/b2b/Windows7-32bit.exe.exe",
                            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Windows7-32bit.exe");

                        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Windows7-32bit.exe");
                    }
                }
                catch (Exception e)
                {
                    Utility.LogWrite("com0com zone exception : " + e.Message);
                }
            }
            else
            {
                Utility.LogWrite("com0com 설치되어 있음.");
            }

            Utility.LogWrite("complete com0com");

            ModelAppDevice device = new ModelAppDevice();

            string pa = Application.StartupPath + "Kons.ShopCallpass_cfg.ini";
            Utility.LogWrite(pa);
            // ini파일 있는지 체크 후 없으면 초기화
            if (!File.Exists(Application.StartupPath + @"\Kons.ShopCallpass_cfg.ini"))
            {
                Utility.LogWrite("구성파일 초기화");

                device.writeConfigString("v1.0", "", "");
                device.writeConfigString("APP_VERSION", "app_version", Kons.ShopCallpass.AppMain.AppCore.Instance.getAppVersion().ToString());

                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_bemin", 1);
                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_yogiyo", 1);
                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_pos", 1);

                device.writeConfigLong("FORM_MAIN", "optimization_mode", 0);

                for (int i = 0; i < 4; i++)
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString(), 0);
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_install_" + i.ToString(), 0);
                    device.writeConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString(), "99");
                    device.writeConfigString("CONFIG_ORDER_INPUT", "input_port_num_" + i.ToString(), "-");
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "input_port_baud_" + i.ToString(), 9600);
                    device.writeConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString(), "-");
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "listen_port_baud_" + i.ToString(), 9600);
                    device.writeConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + i.ToString(), "COM1");
                }

                for (int i = 0; i < 4; i++)
                {
                    device.writeConfigLong("CONFIG_PRINT_OUTPUT", "is_use_" + i.ToString(), 0);
                    device.writeConfigString("CONFIG_PRINT_OUTPUT", "print_connect_type_" + i.ToString(), "0");
                    device.writeConfigString("CONFIG_PRINT_OUTPUT", "input_port_num_" + i.ToString(), "-");
                    device.writeConfigLong("CONFIG_PRINT_OUTPUT", "input_port_baud_" + i.ToString(), 9600);
                }
                device.writeConfigLong("CONFIG_PRINT_OUTPUT", "is_use_usb_printer", 0);
                device.writeConfigString("CONFIG_PRINT_OUTPUT", "usb_printer_name", "-");

                device.writeConfigString("AUTORUNNING", "auto_running", "1");

                device.writeConfigString("UNWANTEDKEYWORD", "unwanted_keyword", "매장용+주방+마 감+내점");
                device.writeConfigString("ORDER_NUM", "order_num", "nnnnnnnnnnnnnn+B?????????");
                device.writeConfigString("COST", "cost", "n,nnn+nn,nnn+nnn,nnn+n,nnn,nnn");
                device.writeConfigString("CALL_NUM", "call_num", "nn-nnn-nnnn+nn-nnnn-nnnn+nnn-nnn-nnnn+nnn-nnnn-nnnn+nnnn-nnn-nnnn+nnnn-nnnn-nnnn+nn)nnn-nnnn+nn)-nnnn-nnnn+nnn)nnn-nnnn+nnn)nnnn-nnnn+nnnn)nnn-nnnn+nnnn)nnnn-nnnn+nnnnnnnnnnnn+010nnnnnnnn+nnn-nnnnn-nnnn");
                device.writeConfigString("ADDRESS", "address", "가산");
                device.writeConfigString("PAYMENT_OPTION", "payment_option", "사전결제 여부: 0+사전결제 여부: O+요기요결제완료");
                device.writeConfigString("ORDER_DATE", "order_date", "y/m/d T:M+y-m-d(w)  T:M+y-m-d(w) T:M+y년m월d일(w) 오후T:M+y년m월d일(w) 오전T:M");
                device.writeConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword", "서울시 중구 세종대로39 7층+02)2054-8341+02-2054-8341+! a! a+!+2aEMB+---+서울 중구 세종대로 39+서울상공회의");

                device.writeConfigString("TEMP_PORTNAME", "temp_portName", "");

                device.writeConfigString("BEMINENDCHAR", "bemin_end_char", "0D1B69");
                device.writeConfigString("EASYPOSENDCHAR", "easypos_end_char", "1B6D");
                device.writeConfigString("YOGIYOENDCHAR", "yogiyo_end_char", "5601");
                device.writeConfigString("OKPOSENDCHAR", "okpos_end_char", "1B69");
                device.writeConfigString("DELGENENDCHAR", "delgen_end_char", "1B69");
                device.writeConfigString("POSFEEDENDCHAR", "posfeed_end_char", "1B40");
                device.writeConfigString("BEDALTONGENDCHAR", "bedaltong_end_char", "1B6D");
                device.writeConfigString("ETCENDCHAR", "etc_end_char", "1B69");

                device.writeConfigString("INTERVALREMOVEBINARYKEY", "interval_remove_binary_key", "2400001D7630302C~2D2D2D");

                device.writeConfigString("BEMINREQUESTLINE", "bemin_request_line", "배달 요청사항:,밑,1");
                device.writeConfigString("YOGIYOREQUESTLINE", "yogiyo_request_line", "연락처,밑,2");
                device.writeConfigString("EASYPOSREQUESTLINE", "easypos_request_line", "#");
                device.writeConfigString("DELGENREQUESTLINE", "delgen_request_line", "#");
                device.writeConfigString("OKPOSREQUESTLINE", "okpos_request_line", "#");
                device.writeConfigString("POSFEEDREQUESTLINE", "posfeed_request_line", "#");
                device.writeConfigString("BEDALTONGREQUESTLINE", "bedaltong_request_line", "#");
                device.writeConfigString("ETCREQUESTLINE", "etc_request_line", "#");

                device.writeConfigString("BEMINENCODING", "bemin_encoding", "EUC-KR");
                device.writeConfigString("YOGIYOENCODING", "yogiyo_encoding", "EUC-KR");
                device.writeConfigString("EASYPOSENCODING", "easypos_encoding", "EUC-KR");
                device.writeConfigString("DELGENENCODING", "delgen_encoding", "EUC-KR");
                device.writeConfigString("OKPOSENCODING", "okpos_encoding", "EUC-KR");
                device.writeConfigString("POSFEEDENCODING", "posfeed_encoding", "EUC-KR");
                device.writeConfigString("BEDALTONGENCODING", "bedaltong_encoding", "EUC-KR");
                device.writeConfigString("ETCENCODING", "etc_encoding", "EUC-KR");

                device.writeConfigString("STOREMAPPINGCHECK", "store_mapping_check", "0");

                const string ADDRESSENDCHAR = "ADDRESSENDCHAR";
                device.writeConfigString(ADDRESSENDCHAR, "bemin_addr_end_char", "(도로명)");
                device.writeConfigString(ADDRESSENDCHAR, "yogiyo_addr_end_char", "연락처:");
                device.writeConfigString(ADDRESSENDCHAR, "easypos_addr_end_char", "a0");
                device.writeConfigString(ADDRESSENDCHAR, "delgen_addr_end_char", "#");
                device.writeConfigString(ADDRESSENDCHAR, "okpos_addr_end_char", "#");
                device.writeConfigString(ADDRESSENDCHAR, "posfeed_addr_end_char", "#");
                device.writeConfigString(ADDRESSENDCHAR, "bedaltong_addr_end_char", "#");
                device.writeConfigString(ADDRESSENDCHAR, "etc_addr_end_char", "#");

                const string REQUESTEXCLUSIVEKEY = "REQUESTEXCLUSIVEKEY";
                device.writeConfigString(REQUESTEXCLUSIVEKEY, "bemin_request_exclusive_key", "#");
                device.writeConfigString(REQUESTEXCLUSIVEKEY, "yogiyo_request_exclusive_key", "메뉴명");
                device.writeConfigString(REQUESTEXCLUSIVEKEY, "easypos_request_exclusive_key", "#");
                device.writeConfigString(REQUESTEXCLUSIVEKEY, "delgen_request_exclusive_key", "#");
                device.writeConfigString(REQUESTEXCLUSIVEKEY, "okpos_request_exclusive_key", "#");
                device.writeConfigString(REQUESTEXCLUSIVEKEY, "posfeed_request_exclusive_key", "#");
                device.writeConfigString(REQUESTEXCLUSIVEKEY, "bedaltong_request_exclusive_key", "#");
                device.writeConfigString(REQUESTEXCLUSIVEKEY, "etc_request_exclusive_key", "#");
            }
            else
            {

                string text = "";
                int euckrCodepage = 51949;
                System.Text.Encoding euckr = System.Text.Encoding.GetEncoding(euckrCodepage);

                using (StreamReader file = new StreamReader(Application.StartupPath + @"\Kons.ShopCallpass_cfg.ini", Encoding.Default))
                {
                    string firstLine = file.ReadLine();

                    if (!firstLine.Contains("[v"))
                    {
                        string temp = file.ReadToEnd();
                        Console.WriteLine(temp);
                        text = "[v1.0]\n";
                        text += temp;
                    }
                }
                if (text != "")
                {
                    File.WriteAllText(Application.StartupPath + @"\Kons.ShopCallpass_cfg.ini", text);

                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_bemin", 1);
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_yogiyo", 1);
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_pos", 1);

                    device.writeConfigLong("FORM_MAIN", "optimization_mode", 0);

                    device.writeConfigLong("CONFIG_PRINT_OUTPUT", "is_use_usb_printer", 0);
                    device.writeConfigString("CONFIG_PRINT_OUTPUT", "usb_printer_name", "-");

                    device.writeConfigString("AUTORUNNING", "auto_running", "1");

                    device.writeConfigString("UNWANTEDKEYWORD", "unwanted_keyword", "매장용+주방+마 감+내점");
                    device.writeConfigString("ORDER_NUM", "order_num", "nnnnnnnnnnnnnn+B?????????");
                    device.writeConfigString("COST", "cost", "n,nnn+nn,nnn+nnn,nnn+n,nnn,nnn");
                    device.writeConfigString("CALL_NUM", "call_num", "nn-nnn-nnnn+nn-nnnn-nnnn+nnn-nnn-nnnn+nnn-nnnn-nnnn+nnnn-nnn-nnnn+nnnn-nnnn-nnnn+nn)nnn-nnnn+nn)-nnnn-nnnn+nnn)nnn-nnnn+nnn)nnnn-nnnn+nnnn)nnn-nnnn+nnnn)nnnn-nnnn+nnnnnnnnnnnn+010nnnnnnnn+nnn-nnnnn-nnnn");
                    device.writeConfigString("ADDRESS", "address", "가산");
                    device.writeConfigString("PAYMENT_OPTION", "payment_option", "사전결제 여부: 0+사전결제 여부: O+요기요결제완료");
                    device.writeConfigString("ORDER_DATE", "order_date", "y/m/d T:M+y-m-d(w)  T:M+y-m-d(w) T:M+y년m월d일(w) 오후T:M+y년m월d일(w) 오전T:M");
                    device.writeConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword", "서울시 중구 세종대로39 7층+02)2054-8341+02-2054-8341+! a! a+!+2aEMB+---+서울 중구 세종대로 39+서울상공회의");

                    device.writeConfigString("TEMP_PORTNAME", "temp_portName", "");

                    device.writeConfigString("BEMINENDCHAR", "bemin_end_char", "0D1B69");
                    device.writeConfigString("EASYPOSENDCHAR", "easypos_end_char", "1B6D");
                    device.writeConfigString("YOGIYOENDCHAR", "yogiyo_end_char", "5601");
                    device.writeConfigString("OKPOSENDCHAR", "okpos_end_char", "1B69");
                    device.writeConfigString("DELGENENDCHAR", "delgen_end_char", "1B69");
                    device.writeConfigString("POSFEEDENDCHAR", "posfeed_end_char", "1B40");
                    device.writeConfigString("BEDALTONGENDCHAR", "bedaltong_end_char", "1B6D");
                    device.writeConfigString("ETCENDCHAR", "etc_end_char", "1B69");

                    device.writeConfigString("INTERVALREMOVEBINARYKEY", "interval_remove_binary_key", "2400001D7630302C~2D2D2D");

                    device.writeConfigString("BEMINREQUESTLINE", "bemin_request_line", "배달 요청사항:,밑,1");
                    device.writeConfigString("YOGIYOREQUESTLINE", "yogiyo_request_line", "연락처,밑,2");
                    device.writeConfigString("EASYPOSREQUESTLINE", "easypos_request_line", "#");
                    device.writeConfigString("DELGENREQUESTLINE", "delgen_request_line", "#");
                    device.writeConfigString("OKPOSREQUESTLINE", "okpos_request_line", "#");
                    device.writeConfigString("POSFEEDREQUESTLINE", "posfeed_request_line", "#");
                    device.writeConfigString("BEDALTONGREQUESTLINE", "bedaltong_request_line", "#");
                    device.writeConfigString("ETCREQUESTLINE", "etc_request_line", "#");

                    device.writeConfigString("BEMINENCODING", "bemin_encoding", "EUC-KR");
                    device.writeConfigString("YOGIYOENCODING", "yogiyo_encoding", "EUC-KR");
                    device.writeConfigString("EASYPOSENCODING", "easypos_encoding", "EUC-KR");
                    device.writeConfigString("DELGENENCODING", "delgen_encoding", "EUC-KR");
                    device.writeConfigString("OKPOSENCODING", "okpos_encoding", "EUC-KR");
                    device.writeConfigString("POSFEEDENCODING", "posfeed_encoding", "EUC-KR");
                    device.writeConfigString("BEDALTONGENCODING", "bedaltong_encoding", "EUC-KR");
                    device.writeConfigString("ETCENCODING", "etc_encoding", "EUC-KR");

                    device.writeConfigString("STOREMAPPINGCHECK", "store_mapping_check", "0");

                    const string ADDRESSENDCHAR = "ADDRESSENDCHAR";
                    device.writeConfigString(ADDRESSENDCHAR, "bemin_addr_end_char", "(도로명)");
                    device.writeConfigString(ADDRESSENDCHAR, "yogiyo_addr_end_char", "연락처:");
                    device.writeConfigString(ADDRESSENDCHAR, "easypos_addr_end_char", "a0");
                    device.writeConfigString(ADDRESSENDCHAR, "delgen_addr_end_char", "#");
                    device.writeConfigString(ADDRESSENDCHAR, "okpos_addr_end_char", "#");
                    device.writeConfigString(ADDRESSENDCHAR, "posfeed_addr_end_char", "#");
                    device.writeConfigString(ADDRESSENDCHAR, "bedaltong_addr_end_char", "#");
                    device.writeConfigString(ADDRESSENDCHAR, "etc_addr_end_char", "#");

                    const string REQUESTEXCLUSIVEKEY = "REQUESTEXCLUSIVEKEY";
                    device.writeConfigString(REQUESTEXCLUSIVEKEY, "bemin_request_exclusive_key", "#");
                    device.writeConfigString(REQUESTEXCLUSIVEKEY, "yogiyo_request_exclusive_key", "메뉴명");
                    device.writeConfigString(REQUESTEXCLUSIVEKEY, "easypos_request_exclusive_key", "#");
                    device.writeConfigString(REQUESTEXCLUSIVEKEY, "delgen_request_exclusive_key", "#");
                    device.writeConfigString(REQUESTEXCLUSIVEKEY, "okpos_request_exclusive_key", "#");
                    device.writeConfigString(REQUESTEXCLUSIVEKEY, "posfeed_request_exclusive_key", "#");
                    device.writeConfigString(REQUESTEXCLUSIVEKEY, "bedaltong_request_exclusive_key", "#");
                    device.writeConfigString(REQUESTEXCLUSIVEKEY, "etc_request_exclusive_key", "#");
                }

                Utility.LogWrite("구성파일 이미 설치되어 있음");
            }

            if (device.readConfigLong("STOREMAPPINGCHECK", "store_mapping_check") == 1)
            {
                string company_type = device.readConfigString("CONFIG_LAST_DELIVERY_REQUEST_INFO", "delivery_company_type");
                string store_pno = device.readConfigString("CONFIG_STORE_API_INFO", "store_pno_" + company_type);
                string store_name = device.readConfigString("CONFIG_STORE_API_INFO", "store_name_" + company_type);

                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("store_name", store_name);
                dic.Add("store_pno", store_pno);

                FormFile f1 = new FormFile();
                //f1.Name = store_name + store_pno;
                f1.Name = "iniFile";
                f1.FilePath = "Kons.ShopCallpass_cfg.ini";
                f1.ContentType = "text/ini";
                f1.Stream = null;
                dic.Add("iniFile", f1);

                FormFile f2 = new FormFile();
                f2.Name = "binaryFile";
                f2.FilePath = @"C:\CallPass_log\OriginalBinary.txt";
                f2.ContentType = "text/txt";
                f2.Stream = null;
                dic.Add("binaryFile", f2);

                FormFile f3 = new FormFile();
                f3.Name = "CallpassLog";
                f3.FilePath = @"C:\CallPass_log\CallpassLog.log";
                f3.ContentType = "text/log";
                f3.Stream = null;
                dic.Add("CallpassLog", f3);

                try
                {
                    string returnStr = PostMultipart("http://175.198.102.230:8016/bServer/moaBtwoB/versionCehck.do", dic);
                    //string returnStr = PostMultipart("http://localhost:8080/bServer/moaBtwoB/versionCehck.do", dic);
                    byte[] bytearray = Encoding.Default.GetBytes(returnStr);
                    returnStr = Encoding.Default.GetString(bytearray);

                    Utility.LogWrite("returnStr : " + returnStr + " returnStr.length : " + returnStr.Length);
                    if (returnStr.Length > 0)
                    {
                        string savePath = "Kons.ShopCallpass_cfg.ini";
                        Debug.WriteLine("starcraft");
                        File.WriteAllText(savePath, returnStr, Encoding.Default);
                    }
                }
                catch (Exception e)
                {
                    Utility.LogWrite("ini zone exception : " + e.Message);
                }
            }

            if (File.Exists(Environment.GetLogicalDrives().ElementAt(0) + @"\CallPass_log\CallpassLog.log"))
            {
                File.Delete(Environment.GetLogicalDrives().ElementAt(0) + @"\CallPass_log\CallpassLog.log");
            }

            if (File.Exists(Environment.GetLogicalDrives().ElementAt(0) + @"\CallPass_log\OriginalBinary.txt"))
            {
                File.Delete(Environment.GetLogicalDrives().ElementAt(0) + @"\CallPass_log\OriginalBinary.txt");
            }

            // ------------------------ DevExpress enviroment
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            SkinManager.EnableMdiFormSkins();
            //UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");      // DevExpress Style, Office 2013, VS2010, Office 2010 Blue, Office 2013

            // AppConfig 파일 참조
            UserLookAndFeel.Default.SetSkinStyle("Visual Studio 2013 Blue");

            // 전체 폰트 지정
            DevExpress.XtraEditors.WindowsFormsSettings.DefaultFont = new Font("Gulim", 9F);

            // 스타일
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(ExApplicationExit);

            // 시작
            Application.Run(Kons.ShopCallpass.AppMain.AppCore.Instance.initInstance());
        }

        static void ExApplicationExit(object sender, EventArgs e)
        {
            try
            {
                //AppMgr.releaseInstance();
            }
            catch { }
        }

        

        public static int DownloadCheck()
        {
            if (File.Exists("C:\\Program Files\\com0com\\setupc.exe"))
            {
                fileLocate = "C:\\Program Files\\com0com\\setupc.exe";
                return 1;
            }
            else if (File.Exists("C:\\Program Files (x86)\\com0com\\setupc.exe"))
            {
                fileLocate = "C:\\Program Files (x86)\\com0com\\setupc.exe";
                return 1;
            }
            else if (File.Exists("C:\\Program Files\\com0com\\setup.bat"))
            {
                fileLocate = "C:\\Program Files\\com0com\\setup.bat";
                return 2;
            }
            else if (File.Exists("C:\\Program Files (x86)\\com0com\\setup.bat"))
            {
                fileLocate = "C:\\Program Files (x86)\\com0com\\setup.bat";
                return 2;
            }
            return 0;
        }

        public static string PostMultipart(string url, System.Collections.Generic.Dictionary<string, object> parameters)
        {
            string boundary = "---------------------------" + System.DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;

            if (parameters != null && parameters.Count > 0)
            {

                using (Stream requestStream = request.GetRequestStream())
                {

                    foreach (KeyValuePair<string, object> pair in parameters)
                    {

                        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        if (pair.Value is FormFile)
                        {
                            FormFile file = pair.Value as FormFile;
                            string header = "Content-Disposition: form-data; name=\"" + pair.Key + "\"; filename=\"" + file.Name + "\"\r\nContent-Type: " + file.ContentType + "\r\n\r\n";
                            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(header);
                            requestStream.Write(bytes, 0, bytes.Length);
                            byte[] buffer = new byte[32768];
                            int bytesRead;
                            if (File.Exists(file.FilePath))
                            {
                                if (file.Stream == null)
                                {
                                    // upload from file
                                    using (FileStream fileStream = File.OpenRead(file.FilePath))
                                    {
                                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                            requestStream.Write(buffer, 0, bytesRead);
                                        fileStream.Close();
                                    }
                                }
                                else
                                {
                                    // upload from given stream
                                    while ((bytesRead = file.Stream.Read(buffer, 0, buffer.Length)) != 0)
                                        requestStream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                        else
                        {
                            string data = "Content-Disposition: form-data; name=\"" + pair.Key + "\"\r\n\r\n" + pair.Value;
                            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
                            requestStream.Write(bytes, 0, bytes.Length);
                        }
                    }

                    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    requestStream.Write(trailer, 0, trailer.Length);
                    requestStream.Close();
                }
            }

            using (WebResponse response = request.GetResponse())
            {
                Console.WriteLine("enter");
                using (Stream responseStream = response.GetResponseStream())
                {

                    using (StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.Default))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}