using Kons.ShopCallpass.Parser;
using Kons.ShopCallpass.Utility;
using Kons.ShopCallpass.Object;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Kons.ShopCallpass.Model;
using System.Runtime.InteropServices;

namespace Kons.ShopCallpass.Controller
{
    public class SerialPortOrderInput : KnNotify
    {
        public const int READ_BUF_MAX_SIZE = 16384/*20480*/;

        static string prevPaper = "";
        static int count = 0;
        static int count2 = 0;
        static int count3 = 0;
        static int count4 = 0;

        string[] CONFIG_SECTION = { "BEMINENDCHAR", "YOGIYOENDCHAR", "EASYPOSENDCHAR", "DELGENENDCHAR", "OKPOSENDCHAR", "POSFEEDENDCHAR", "BEDALTONGENDCHAR", "ETCENDCHAR"};

        string[] keyValues = { "bemin_end_char", "yogiyo_end_char", "easypos_end_char", "delgen_end_char", "okpos_end_char", "posfeed_end_char", "bedaltong_end_char", "etc_end_char"};

        public class SerialRawDataBufInfo // Notify 시 상위에 전달할 버퍼 정보
        {
            public SerialPortOrderInput caller_serial = null;
            public byte[] buf = null;
            public int offset = 0;
            public int len = 0;
            
            public void setRawDataInfo(SerialPortOrderInput _caller_serial, byte[] _buf, int _offset, int _len)
            {
                caller_serial = _caller_serial;
                buf = _buf;
                offset = _offset;
                len = _len;
                Debug.WriteLine("caller_serial : " + caller_serial);
            }
        }
        public enum SERIAL_PORT_NOTIFY_WHAT
        {
            TEXT_NOTIFY_MESSAGE = 0,
            TEXT_ERROR_MESSAGE,
            RAW_DATA_STREAM,
            ORDER_OBJECT,
        }

        private object m_recv_lock = new object();
        private byte[] m_recv_buf = new byte[READ_BUF_MAX_SIZE];
        private byte[] m_save_buf = new byte[8000];
        private int m_save_len = 0;

        private SerialPort m_serial_port;
        private String m_port_num = "";
        private int m_port_baud_rate = 0;
        private String m_print_output_port_num = ""; // 설정한 연결포트 참조만 저장해 둔다

        // 수신데이터의 시작, 종료 값을 저장해서 이 구간을 다 받아서 리턴하게 한다.
        private byte[] PROTOCOL_SOD = null;
        private byte[] PROTOCOL_EOD = null;
        private byte[] PROTOCOL_TMP = null;
        private bool m_is_receive_sod = false;

        // 
        private ParserOrderInputBase m_parser = null;
        private SerialRawDataBufInfo m_raw_data_buf = new SerialRawDataBufInfo();

        // ----------------------------------------------------------
        //
        public SerialPortOrderInput(String _port_num, int _port_baud_rate, ParserOrderInputBase _parser, String _print_output_port_num)
        {
            Debug.WriteLine("SerialPortOrderInput호출");
            m_port_num = _port_num;
            m_port_baud_rate = _port_baud_rate;
            m_parser = _parser;
            m_print_output_port_num = _print_output_port_num;

            Debug.WriteLine("m_port_num : " + m_port_num + "   m_print_output_port_num : " + m_print_output_port_num);
            // 파서에 정이된 데이터의 시작, 종료 값 저장
            byte[] _sod = null;
            byte[] _eod = null;
            if (null != _parser)
            {
                _sod = _parser.getInputData_SOD();
                _eod = _parser.getInputData_EOD();
            }

            if (null != _sod && null != _eod) // 시작, 종료 둘 다 있는경우
            {
                PROTOCOL_SOD = (byte[])_sod.Clone();            // start of data
                //PROTOCOL_EOD = (byte[])_eod.Clone();            // end of data
                //PROTOCOL_TMP = new byte[PROTOCOL_EOD.Length];   // 비교를 위해 담아둘 공간
                Debug.Assert(_sod.Length == _eod.Length);       // 시작, 종료 길이가 같아야 한다.
            }
            else if (null != _sod && null == _eod) // 시작만 있고 종료는 없는경우
            {
                PROTOCOL_SOD = (byte[])_sod.Clone();            // start of data
                //PROTOCOL_EOD = _eod;
                PROTOCOL_TMP = new byte[PROTOCOL_SOD.Length];   // 비교를 위해 담아둘 공간
            }
            else
            {
                PROTOCOL_SOD = null;
                //PROTOCOL_EOD = null;
                PROTOCOL_TMP = null;
            }

            // set notify sener
            this.NotifySender = _port_num;
        }

        public SerialPort getSerialPort()
        {
            return m_serial_port;
        }

        public ParserOrderInputBase getOrderParser()
        {
            return m_parser;
        }

        public String getPrintOutputPortNum()
        {
            return m_print_output_port_num;
        }
        // ----------------------------------------------------------
        //
        public void connectSerialPort()
        {
            Utility.Utility.LogWrite("connectSerialPort호출");
            disconnectSerialPort();
            Utility.Utility.LogWrite("connectSerialPort");

            try
            {
                m_serial_port = new SerialPort(m_port_num, m_port_baud_rate, Parity.None, 8, StopBits.One);
                m_serial_port.Handshake = Handshake.None;
                m_serial_port.ReadBufferSize = m_recv_buf.Length;
                m_serial_port.ReadTimeout = 2000;
                m_serial_port.DataReceived += new SerialDataReceivedEventHandler(onDataReceivedSerialPort);
                m_serial_port.DtrEnable = true;

                Utility.Utility.LogWrite("m_serial_port.Open()직전");
                Utility.Utility.LogWrite("m_port_num : " + m_port_num + " m_port_baud_rate : " + m_port_baud_rate + " Parity.None : " + Parity.None + " StopBits.One : " + StopBits.One);
                
                m_serial_port.Open();

                Utility.Utility.LogWrite("m_serial_port.Open()직후");

            // message
            //sendMyNotify(SERIAL_PORT_NOTIFY_WHAT.TEXT_NOTIFY_MESSAGE, "port open");
        }
            catch (Exception err)
            {
                Utility.Utility.LogWrite("connectSerialPort에서 예외발생 : " + err.Message);
                sendMyNotify(SERIAL_PORT_NOTIFY_WHAT.TEXT_ERROR_MESSAGE, String.Format("{0}: {1}", m_port_num, err.Message));
            }
}

        public void disconnectSerialPort()
        {
            Utility.Utility.LogWrite("disconnectSerialPort호출");
            if (null != m_serial_port)
            {
                m_serial_port.Close();
                m_serial_port = null;
                Debug.WriteLine("m_serial_port.Close()");
                // message
                //sendMyNotify(SERIAL_PORT_NOTIFY_WHAT.TEXT_NOTIFY_MESSAGE, "> disconnected from " + m_port_num);
            }

            resetSaveBuf();
        }

        private void onDataReceivedSerialPort(object sender, SerialDataReceivedEventArgs e)
        {
            Utility.Utility.LogWrite("onDataReceivedSerialPort1");
            lock (m_recv_lock)
            {
                Utility.Utility.LogWrite("onDataReceivedSerialPort2");
                System.Array.Clear(m_recv_buf, 0, m_recv_buf.Length);

                int total_read_len = 0;
                int read_len = 0;

                Utility.Utility.LogWrite("첫 m_serial_port.BytesToRead : " + m_serial_port.BytesToRead);

                while (m_serial_port.BytesToRead > 0)
                {
                    Utility.Utility.LogWrite("BytesToRead : " + m_serial_port.BytesToRead);
                    Utility.Utility.LogWrite("onDataReceivedSerialPort3");

                    read_len = ((total_read_len + m_serial_port.BytesToRead) < m_recv_buf.Length ? m_serial_port.BytesToRead : (m_recv_buf.Length - total_read_len));
                    m_serial_port.Read(m_recv_buf, total_read_len, read_len);

                    ModelAppDevice device = new ModelAppDevice();
                    device.writeConfigString("TEMP_PORTNAME", "temp_portName", m_serial_port.PortName);

                    Utility.Utility.LogWrite("리드 직후 m_recv_buf : " + EncodingToStringFitness(CheckSender(), m_recv_buf));
                    Utility.Utility.LogWrite("리드 직후 m_recv_buf 출력 횟수 : " + (++count3) + " _read_buf.Length : " + m_recv_buf.Length + " total_read_len : " + total_read_len + " read_len : " + read_len);

                    ByteArrayToFile(Environment.GetLogicalDrives().ElementAt(0) + @"\CallPass_log\OriginalBinary.txt", m_recv_buf);

                    Utility.Utility.LogWrite("onDataReceivedSerialPort4");
                    total_read_len += read_len;

                    Utility.Utility.LogWrite("read_len : " + read_len + " total_read_len : " + total_read_len);
                }
                // 받은 데이터 중 처리할(시작,종료) 프로토톨로 찾아서 받은 버퍼에 저장 해 둔다.
                onReceivedData(m_recv_buf, 0, total_read_len);
            }
        }

        /// <summary>
        /// 테스트 등을 위해 분리
        /// </summary>
        public void onReceivedData(byte[] _recv_buf, int _recv_offset, int _recv_length)
        {
            Utility.Utility.LogWrite("onReceivedData");
            // 상위에 받은 전체 RAW 데이터를 통지함 ( SerialRawDataBuf 객체에 정보를 담아서 보낸다 )
            m_raw_data_buf.setRawDataInfo(this, _recv_buf, _recv_offset, _recv_length);

            sendMyNotify(SERIAL_PORT_NOTIFY_WHAT.RAW_DATA_STREAM, m_raw_data_buf);
        }

        // ----------------------------------------------------------
        // 이 함수는 외부에서 호출한다.
        public void saveRawDataToParsing(byte[] _recv_buf, int _recv_offset, int _recv_length)
        {
            Utility.Utility.LogWrite("saveRawDataToParsing : " + m_parser);

            // 한번에 읽은 데이터를 프로토콜로 잘라 처리한다.
            if (null != m_parser) // 파서가 없으면 프로토콜로 잘라 저장하는 부분 의미가 없음
            {
                int remain_len = _recv_length;
                int save_offset = 0;
                do
                {
                    //Utility.Utility.LogWrite("_recv_buf : " + EncodingToStringFitness(CheckSender(), _recv_buf) + " save_offset : " + save_offset + " remain_len : " + remain_len);
                    int save_len = saveReadData(_recv_buf, save_offset, remain_len);
                    remain_len -= save_len;
                    save_offset += save_len;
                }
                while (0 < remain_len);
            }
        }

        // 시작문자를 찾아서 이후부터 저장버퍼에 저장한다.
        // 시작을 했다면 종료문자를 찾아서 메시지 보내고 이후 앞 부분을 저장 해 둔다
        private int saveReadData(byte[] _read_buf, int _read_offset, int _read_length)
        {
            Utility.Utility.LogWrite("saveReadData");

            int copy_offset = 0;

            Utility.Utility.LogWrite("saveReadData _read_buf : \n" + EncodingToStringFitness(CheckSender(), _read_buf) + "\n");
            Utility.Utility.LogWrite("saveReadData _read_buf 출력 횟수 : " + (++count) + " _read_buf.Length : " + _read_buf.Length + " _read_length : " + _read_length);

            ModelAppDevice device = new ModelAppDevice();
            string val = device.readConfigString("TEMP_PORTNAME", "temp_portName");
            int inputType = 0;
            for (int i = 0; i < 4; i++)
            {
                if (val.Equals(device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString())))
                {
                    inputType = Int32.Parse(device.readConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString()));
                }
            }
            Utility.Utility.LogWrite("inputType : " + inputType);
            int index = 0;
            switch (inputType)
            {
                case 10:
                    index = 0;
                    break;
                case 21:
                    index = 4;
                    break;
                case 26:
                    index = 3;
                    break;
                case 27:
                    index = 5;
                    break;
                case 31:
                    index = 2;
                    break;
                case 32:
                    index = 1;
                    break;
                case 33:
                    index = 6;
                    break;
                case 98:
                    index = 7;
                    break;
            }
            string str = device.readConfigString(CONFIG_SECTION[index], keyValues[index]);
            Utility.Utility.LogWrite("str : " + str);
            PROTOCOL_EOD = ConvertByteArray(str);
            Utility.Utility.LogWrite("PROTOCOL_EOD: " + PROTOCOL_EOD);
            PROTOCOL_TMP = new byte[PROTOCOL_EOD.Length];
            bool is_receive_eod = false;
            int copy_length = 0;
            Utility.Utility.LogWrite("isWaitDataEnd() : " + isWaitDataEnd());
            if (isWaitDataEnd())
            {
                copy_length += PROTOCOL_SOD.Length; // 시작문자 부분은 건너뛰고 검색을 시작하므로 아래 +1 이 안되는 부분이기 때문에 먼저 대해준다.

                int isOptimizationMode = device.readConfigLong("FORM_MAIN", "optimization_mode");

                for (int i = (copy_offset + PROTOCOL_SOD.Length); i < (_read_length - PROTOCOL_EOD.Length + 1); i++)
                {
                    Array.Copy(_read_buf, _read_offset + i, PROTOCOL_TMP, 0, PROTOCOL_TMP.Length);

                    if (isOptimizationMode == 0)
                    {
                        //log지우면안됨!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        Utility.Utility.LogWrite("PROTOCOL_TMP:" + ConvertByteToHexString(PROTOCOL_TMP) + "   eod:" + ConvertByteToHexString(PROTOCOL_EOD));
                    }

                    if (PROTOCOL_TMP.SequenceEqual(PROTOCOL_EOD))
                    {
                        is_receive_eod = true;
                        copy_length += PROTOCOL_TMP.Length;

                        Utility.Utility.LogWrite("끝문자 i위치 : " + i);
                        Utility.Utility.LogWrite("기타 파서 전용 이프문");
                        break;
                    }
                    copy_length += 1;
                }

                Utility.Utility.LogWrite("끝문자체크is_receive_eod : " + is_receive_eod);

                if (!is_receive_eod) // 못찾았으면 검색한 길이만큼 더해 준다
                {
                    copy_length += (PROTOCOL_EOD.Length - 1);
                }
            }
            else
            {
                is_receive_eod = true; // 종료를 찾는 것이 아니면 받은만큼이 종료이기 때문에...
                copy_length = _read_length;
            }

            Utility.Utility.LogWrite("copy_length > (m_save_buf.Length - m_save_len) : " + (copy_length > (m_save_buf.Length - m_save_len)));

            // 복사할 버퍼의 남은크기 확인
            if (copy_length > (m_save_buf.Length - m_save_len))
            {
                resetSaveBuf(); // 버퍼가 가득찬 경우 받은것을 파기한다.
                return (copy_offset + copy_length);
            }

            // 복사할 길이를 찾아서 저장 버퍼에 복사한다.
            System.Array.Copy(_read_buf, _read_offset + copy_offset, m_save_buf, m_save_len, copy_length);
            m_save_len += copy_length;

            Utility.Utility.LogWrite("m_save_buf : " + EncodingToStringFitness(CheckSender(), m_save_buf));
            Utility.Utility.LogWrite("m_save_len : " + m_save_len);

            Utility.Utility.LogWrite("m_save_buf 출력 횟수 : " + (++count2) + " m_save_buf.Length : " + m_save_buf.Length + " m_save_len : " + m_save_len);
            Utility.Utility.LogWrite("is_receive_eod : " + is_receive_eod);

            // 상위로 메시지 보내고 버퍼 초기화
            if (is_receive_eod)
            {
                Utility.Utility.LogWrite("m_parser : " + m_parser);
                
                if (null != m_parser)
                {
                    string tmp = device.readConfigString("UNWANTEDKEYWORD", "unwanted_keyword");
                    Utility.Utility.LogWrite("tmp : " + tmp);
                    string[] exclusivePatterns = tmp.Split('+');
                    Utility.Utility.LogWrite("exclusivePatterns.Length : " + exclusivePatterns.Length);
                    for (int i = 0; i < exclusivePatterns.Length; i++)
                    {
                        Utility.Utility.LogWrite("exclusivePatterns[i] : " + exclusivePatterns[i]);
                    }

                    bool flag = false;
                    for (int i = 0; i < exclusivePatterns.Length; i++)
                    {
                        flag = EncodingToStringFitness(CheckSender(), m_save_buf).Contains(exclusivePatterns[i]);
                        if (flag)
                        {
                            break;
                        }
                    }

                    ObjOrder recv_order = null;
                    if (prevPaper != EncodingToStringFitness(CheckSender(), m_save_buf))
                    {
                        Utility.Utility.LogWrite("apple");
                        if (!flag)
                        {
                            Utility.Utility.LogWrite("grape");
                            recv_order = m_parser.parsingInputOrderRawData(m_save_buf, m_save_len);
                        }
                    }
                    //Utility.Utility.LogWrite("recv_order : " + recv_order);

                    if (null != recv_order)
                    {
                        // 주문에 print 정보 저장
                        recv_order.setRawDataForPrint(m_print_output_port_num, m_save_buf, 0, m_save_len);

                        bool isThisDataToRegister = false;

                        Utility.Utility.LogWrite("inputType : " + inputType);
                        switch (inputType)
                        {
                            case 10:
                                
                                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_enroll_bemin") == 1)
                                {
                                    isThisDataToRegister = true;
                                    Utility.Utility.LogWrite("배민꺼 : " + inputType);
                                }
                                break;

                            case 32:

                                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_enroll_yogiyo") == 1)
                                {
                                    isThisDataToRegister = true;
                                    Utility.Utility.LogWrite("요기요꺼 : " + inputType);
                                }
                                break;

                            default:

                                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_enroll_pos") == 1)
                                {
                                    isThisDataToRegister = true;
                                    Utility.Utility.LogWrite("포스기 : " + inputType);
                                }
                                break;
                        }

                        if (isThisDataToRegister && prevPaper != EncodingToStringFitness(CheckSender(), m_save_buf))
                        {
                            if (!flag)
                            {
                                Utility.Utility.LogWrite("가나다라");
                                prevPaper = EncodingToStringFitness(CheckSender(), m_save_buf);
                                sendMyNotify(SERIAL_PORT_NOTIFY_WHAT.ORDER_OBJECT, recv_order);
                            }
                        }
                    }
                }
                if (device.readConfigString("CONFIG_PRINT_OUTPUT", "is_use_usb_printer") == "1")
                {
                    ByteArrayToFile(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\saveBuf" + (count4++).ToString() + ".txt", m_save_buf);
                }
                // 받은길이 초기화
                resetSaveBuf();
            }
            // 처리한 메시지 길이 반환
            return (copy_offset + copy_length);
        }
        private void resetSaveBuf()
        {
            m_is_receive_sod = false;
            m_save_len = 0;
        }
        private bool isWaitDataEnd()
        {
            if (null == PROTOCOL_EOD)
            {
                return false;
            }

            return true;
        }
        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Utility.Utility.LogWrite("ByteArrayToFile 예외 : " + ex.Message);
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        public static string ConvertByteToHexString(byte[] convertArr)
        {
            string convertArrString = string.Empty;
            convertArrString = string.Concat(Array.ConvertAll(convertArr, byt => byt.ToString("X2")));
            return convertArrString;
        }

        public byte[] ConvertByteArray(string strHex)
        {
            int iLength = strHex.Length;
            byte[] bytes = new byte[iLength / 2];
            for (int i = 0; i < iLength; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(strHex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static int IndexOf(byte[] arrayToSearchThrough, byte[] patternToFind)
        {
            if (patternToFind.Length > arrayToSearchThrough.Length)
                return -1;
            for (int i = 0; i < arrayToSearchThrough.Length - patternToFind.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    if (arrayToSearchThrough[i + j] != patternToFind[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

        public static int CheckSender()
        {
            ModelAppDevice device = new ModelAppDevice();

            string val = device.readConfigString("TEMP_PORTNAME", "temp_portName");
            int num = 0;

            for (int i = 0; i < 4; i++)
            {
                if (val.Equals(device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString())))
                {
                    num = Int32.Parse(device.readConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString()));
                }
            }

            return num;
        }

        public static string EncodingToStringFitness(int _sender, byte[] _target)
        {
            ModelAppDevice device = new ModelAppDevice();
            string characterSet = "";
            string value = "";

            switch (_sender)
            {
                case 10:
                    characterSet = device.readConfigString("BEMINENCODING", "bemin_encoding");
                    break;
                case 21:
                    characterSet = device.readConfigString("OKPOSENCODING", "okpos_encoding");
                    break;
                case 26:
                    characterSet = device.readConfigString("DELGENENCODING", "delgen_encoding");
                    break;
                case 27:
                    characterSet = device.readConfigString("POSFEEDENCODING", "posfeed_encoding");
                    break;
                case 31:
                    characterSet = device.readConfigString("EASYPOSENCODING", "easypos_encoding");
                    break;
                case 32:
                    characterSet = device.readConfigString("YOGIYOENCODING", "yogiyo_encoding");
                    break;
                case 33:
                    characterSet = device.readConfigString("BEDALTONGENCODING", "bedaltong_encoding");
                    break;
                case 98:
                    characterSet = device.readConfigString("ETCENCODING", "etc_encoding");
                    break;
            }

            switch (characterSet)
            {
                case "EUC-KR":
                    value = Encoding.Default.GetString(_target);
                    break;
                case "UTF-8":
                    value = Encoding.UTF8.GetString(_target);
                    break;
            }
            return value;
        }
    }
}
