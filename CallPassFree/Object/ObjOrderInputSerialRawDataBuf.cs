using Kons.ShopCallpass.AppMain;
using Kons.ShopCallpass.Controller;
using Kons.ShopCallpass.Parser;
using Kons.TsLibrary;
using Kons.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Kons.ShopCallpass.Object
{
    public class ObjOrderInputSerialRawDataBuf : IKnRecycleObj<ObjOrderInputSerialRawDataBuf>
    {
        public SerialPortOrderInput m_caller_serial_port = null;
        public byte[] m_data_buf = new byte[SerialPortOrderInput.READ_BUF_MAX_SIZE]; // 최대 4096 byte 저장
        public int m_data_len = 0;

        public int saveRawDataBufInfo(SerialPortOrderInput _serial, byte[] _data_buf, int _data_offset, int _data_len)
        {
            // set port info
            m_caller_serial_port = _serial;

            // determine copy length
            int copy_len = (_data_len < m_data_buf.Length ? _data_len : m_data_buf.Length);

            // buf copy
            System.Array.Copy(_data_buf, _data_offset, m_data_buf, 0, copy_len);
            m_data_len = copy_len;

            // return
            return copy_len;
        }

        public void initObj()
        {
            m_caller_serial_port = null;

            // m_data_buf 궂이 버퍼는 초기화 않아도 된다.
            m_data_len = 0;
        }

        public void copyObj(ObjOrderInputSerialRawDataBuf _src)
        {
            m_caller_serial_port = _src.m_caller_serial_port;

            System.Array.Copy(_src.m_data_buf, 0, this.m_data_buf, 0, _src.m_data_len);
            m_data_len = _src.m_data_len;
        }

        public bool saveRawDataToDevice()
        {
            return true;
        }

        public bool requestSendRowData()
        {
            // head
            String uri = Kons.ShopCallpass.AppMain.AppCore.Instance.getWebRestUrl() + "/regOrderInputRawData.aspx";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            req.Headers.Add("__ak", Kons.ShopCallpass.AppMain.AppCore.Instance.getAppKey());
            req.Headers.Add("__av", Kons.ShopCallpass.AppMain.AppCore.Instance.getAppVersion().ToString());
            req.Headers.Add("__lk", Kons.ShopCallpass.AppMain.AppCore.Instance.getLoginUserLoginKey());
            req.Timeout = 9000;
            //req.KeepAlive = false;

            // prepare data
            ParserOrderInputBase parser = (null == m_caller_serial_port ? null : m_caller_serial_port.getOrderParser());
            String input_port_name = (null == parser ? "none" : ParserOrderInputBase.obtainParserName(parser.getParserType()));
            byte[] order_input_type_buf = Encoding.UTF8.GetBytes(input_port_name);
            Int32 order_input_type_len = order_input_type_buf.Length;

            byte[] order_input_raw_data_buf = this.m_data_buf;
            Int32 order_input_raw_data_len = this.m_data_len;

            Utility.Utility.LogWrite("서버 작업 부분 : " + Encoding.Default.GetString(this.m_data_buf));
            Utility.Utility.LogWrite("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            String print_port_name = (null == m_caller_serial_port.getPrintOutputPortNum() ? "null" : m_caller_serial_port.getPrintOutputPortNum());
            byte[] print_output_type_buf = Encoding.UTF8.GetBytes(print_port_name);
            Int32 print_output_type_len = print_output_type_buf.Length;

            // body
            byte[] send_body_data_buf = new byte[4 + order_input_type_len + 4 + order_input_raw_data_len + 4 + print_output_type_len];

            int wLen = 0;
            try
            {
                using (MemoryStream ms = new MemoryStream(send_body_data_buf, 0, send_body_data_buf.Length))
                {
                    using (BinaryWriter bw = new BinaryWriter(ms, Encoding.Default))
                    {
                        bw.Write(order_input_type_len); wLen += 4;
                        bw.Write(order_input_type_buf, 0, order_input_type_len); wLen += order_input_type_len;
                        bw.Write(order_input_raw_data_len); wLen += 4;
                        bw.Write(order_input_raw_data_buf, 0, order_input_raw_data_len); wLen += order_input_raw_data_len;
                        bw.Write(print_output_type_len); wLen += 4;
                        bw.Write(print_output_type_buf, 0, print_output_type_len); wLen += print_output_type_len;
                    }
                }
            }
            catch (Exception err)
            {
                TsLog.writeLog(err.Message);
            }

            // write body data on request stream
            //String recv_result = "";
            //int recv_len = 0;
            try
            {
                req.ContentLength = (null == send_body_data_buf ? 0 : send_body_data_buf.Length);
                if (null != send_body_data_buf)
                {
                    using (Stream stream_data = req.GetRequestStream())
                    {
                        stream_data.Write(send_body_data_buf, 0, send_body_data_buf.Length);
                    }
                }

                using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                {
                    //Stream stream = res.GetResponseStream();

                    //Array.Clear(m_recv_buf, 0, m_recv_buf.Length);
                    //Byte[] buf = m_temp_buf;
                    //int read_len = 0;
                    //do
                    //{
                    //    Array.Clear(buf, 0, buf.Length);
                    //    read_len = stream.Read(buf, 0, buf.Length);
                    //    if (read_len != 0)
                    //    {
                    //        Array.Copy(buf, 0, m_recv_buf, recv_len, read_len);
                    //        recv_len += read_len;
                    //    }
                    //} while (read_len > 0);

                    //// parsing to string
                    //recv_result = Encoding.UTF8.GetString(m_recv_buf, 0, recv_len);
                }
            }
            catch (Exception err)
            {
                TsLog.writeLog(err.Message);
                return false;
            }

            return true;
        }
    }
}
