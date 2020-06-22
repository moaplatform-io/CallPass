using Kons.ShopCallpass.Utility;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Kons.ShopCallpass.Controller
{
    public class SerialPortPrintOutput : KnNotify
    {
        public class SerialRawDataBufInfo // Notify 시 상위에 전달할 버퍼 정보
        {
            public String port_num = "";
            public byte[] buf = null;
            public int offset = 0;
            public int len = 0;
            public void setRawDataInfo(String _port_num, byte[] _buf, int _offset, int _len)
            {
                port_num = _port_num;
                buf = _buf;
                offset = _offset;
                len = _len;
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
        private byte[] m_recv_buf = new byte[2048];

        private SerialPort m_serial_port;
        private String m_port_num = "";
        private int m_port_baud_rate = 0;

        // 
        private SerialRawDataBufInfo m_raw_data_buf = new SerialRawDataBufInfo();

        // ----------------------------------------------------------
        //
        public SerialPortPrintOutput(String _port_num, int _port_baud_rate)
        {
            m_port_num = _port_num;
            m_port_baud_rate = _port_baud_rate;

            Debug.WriteLine("m_port_num", m_port_num);
            // set notify sener
            this.NotifySender = _port_num;
        }

        public SerialPort getSerialPort()
        {
            return m_serial_port;
        }

        // ----------------------------------------------------------
        //
        public void connectSerialPort()
        {

            // disconnect if open
            disconnectSerialPort();

            try
            {
                // Create the serial port with basic settings
                m_serial_port = new SerialPort(m_port_num, m_port_baud_rate, Parity.None, 8, StopBits.One);
                m_serial_port.Handshake = Handshake.None;
                m_serial_port.ReadBufferSize = m_recv_buf.Length;
                m_serial_port.ReadTimeout = 2000;

                // Attach a method to be called when there
                // is data waiting in the port's buffer
                m_serial_port.DataReceived += new SerialDataReceivedEventHandler(onDataReceivedSerialPort);
                m_serial_port.DtrEnable = true;

                // Begin communications
                //m_serial_port.Open();

                // message
                //sendMyNotify(SERIAL_PORT_NOTIFY_WHAT.TEXT_NOTIFY_MESSAGE, "port open");
            }
            catch (Exception err)
            {
                Utility.Utility.LogWrite("프린터 connectSerialPort에서 예외발생 : " + err.Message);
                sendMyNotify(SERIAL_PORT_NOTIFY_WHAT.TEXT_ERROR_MESSAGE, String.Format("{0}: {1}", m_port_num, err.Message));
            }
        }

        public void disconnectSerialPort()
        {

            if (null != m_serial_port)
            {
                m_serial_port.Close();
                m_serial_port = null;

                // message
                //sendMyNotify(SERIAL_PORT_NOTIFY_WHAT.TEXT_NOTIFY_MESSAGE, "> disconnected from " + m_port_num);
            }
        }

        private void onDataReceivedSerialPort(object sender, SerialDataReceivedEventArgs e)
        {
            Debug.WriteLine("onDataReceivedSerialPort");
            lock (m_recv_lock)
            {
                System.Array.Clear(m_recv_buf, 0, m_recv_buf.Length);

                int total_read_len = 0;
                int read_len = 0;
                while (m_serial_port.BytesToRead > 0)
                {
                    read_len = ((total_read_len + m_serial_port.BytesToRead) < m_recv_buf.Length ? m_serial_port.BytesToRead : (m_recv_buf.Length - total_read_len));
                    m_serial_port.Read(m_recv_buf, total_read_len, read_len);
                    Debug.WriteLine("m_serial_port.name : " + m_serial_port.PortName);
                    total_read_len += read_len;
                }

                // 상위에 받은 전체 RAW 데이터를 통지함 ( SerialRawDataBuf 객체에 정보를 담아서 보낸다 )
                if (null != m_raw_data_buf)
                {
                    m_raw_data_buf.setRawDataInfo(m_port_num, m_recv_buf, 0, total_read_len);
                    sendMyNotify(SERIAL_PORT_NOTIFY_WHAT.RAW_DATA_STREAM, m_raw_data_buf);
                }
            }
        }
    }
}
