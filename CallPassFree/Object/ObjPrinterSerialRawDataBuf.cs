using Kons.ShopCallpass.AppMain;
using Kons.TsLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Kons.ShopCallpass.Object
{
    public class ObjPrinterSerialRawDataBuf
    {
        public String m_order_input_type;
        public int m_order_input_raw_data_len;
        public byte[] m_order_input_raw_data_buf;
        public String m_print_output_type;

        private byte[] m_recv_buf = new byte[2048];
        private byte[] m_temp_buf = new byte[2048];

        // ----------------------------------------------------------
        //
        public ObjPrinterSerialRawDataBuf()
        {
            initObj();
        }

        public void initObj()
        {
            m_order_input_type = "";
            m_order_input_raw_data_len = 0;
            m_order_input_raw_data_buf = null;
            m_print_output_type = "";
        }

        public bool setObj(String _input_type, byte[] _recv_data_buf, int _recv_data_offset, int _recv_data_len, String _output_type)
        {
            try
            {
                m_order_input_type = _input_type;
                m_order_input_raw_data_len = _recv_data_len;
                if (0 < _recv_data_len)
                {
                    m_order_input_raw_data_buf = new byte[_recv_data_len];
                    System.Array.Copy(_recv_data_buf, _recv_data_offset, m_order_input_raw_data_buf, 0, m_order_input_raw_data_buf.Length);
                }
                else
                {
                    m_order_input_raw_data_buf = null;
                }
                m_print_output_type = _output_type;
            }
            catch(Exception err)
            {
                TsLog.writeLog(err.Message);
                return false;
            }

            return true;
        }

        public bool saveRawDataToDevice()
        {
            return true;
        }

        public bool requestSendRowData()
        {//
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
            byte[] order_input_type_buf = Encoding.UTF8.GetBytes(this.m_order_input_type);
            Int32  order_input_type_len = order_input_type_buf.Length;

            byte[] order_input_raw_data_buf = m_order_input_raw_data_buf;
            Int32  order_input_raw_data_len = m_order_input_raw_data_len;

            byte[] print_output_type_buf = Encoding.UTF8.GetBytes(this.m_print_output_type);
            Int32  print_output_type_len = print_output_type_buf.Length;

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
            String recv_result = "";
            int recv_len = 0;
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
                    Stream stream = res.GetResponseStream();

                    Array.Clear(m_recv_buf, 0, m_recv_buf.Length);
                    Byte[] buf = m_temp_buf;
                    int read_len = 0;
                    do
                    {
                        Array.Clear(buf, 0, buf.Length);
                        read_len = stream.Read(buf, 0, buf.Length);
                        if (read_len != 0)
                        {
                            Array.Copy(buf, 0, m_recv_buf, recv_len, read_len);
                            recv_len += read_len;
                        }
                    } while (read_len > 0);

                    // parsing to string
                    recv_result = Encoding.UTF8.GetString(m_recv_buf, 0, recv_len);
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
