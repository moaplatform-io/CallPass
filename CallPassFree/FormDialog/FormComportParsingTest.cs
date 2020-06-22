using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kons.ShopCallpass.Object;
using Kons.TsLibrary;
using Kons.Utility;
using System.Diagnostics;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormComportParsingTest : Form
    {
        public const String COMPORT_LISTEN = "COM12";
        public const String COMPORT_PRINT = "COM1";

        private SerialPort m_listen_port;
        private SerialPort m_print_port;

        private byte[] m_listen_port_read_buf = new byte[4096];
        private byte[] m_print_port_read_buf = new byte[4096];

        private int m_listen_port_read_len = 0;
        private int m_print_port_read_len = 0;

        // receive raw-data pool
        private object m_recv_raw_data_list_lock = new object();
        private ArrayList m_recv_raw_data_list = new ArrayList();

        // timer tick define
        private const int TIMER_TICK_INTERVAL = 2000;
        private const int TIMER_TICK_RUNNING = (int)((1000.0 / TIMER_TICK_INTERVAL) * 5.0);                  // 1시간에 한번 - 1시간에 한번씩 리셋되므로 일반적인 곳에 이용됨.
        private const int TIMER_TICK_ALIVE = (int)((1000.0 / TIMER_TICK_INTERVAL) * 30.0);                   // 30초에 한번
        private const int TIMER_TICK_MAIN_WND_INFO = (int)((1000.0 / TIMER_TICK_INTERVAL) * 5.0 * 60.0);     // 5분에 한번

        // timer tick values
        private int m_tick_running = TIMER_TICK_RUNNING;
        private int m_tick_sync_server_alive = TIMER_TICK_ALIVE;
        private int m_tick_tel_server_alive = TIMER_TICK_ALIVE;
        private int m_tick_main_window_info = 0; // 시작시 바로 로딩시키기 위해

        public FormComportParsingTest()
        {
            InitializeComponent();

            // prepare
            TsLog.prepareInstance("zlog");

            // 프린터는 데이터 받인 후에만 활성화 시킴
            setFormControlEnable(false);
        }

        private void FormComportParsingTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            disconnectListenPort();
            disconnectPrintPort();
        }

        private void btn_listen_port_connect_Click(object sender, EventArgs e)
        {
            connectListenPort();
            btn_listen_port_connect.Enabled = false;
        }

        private void btn_listen_port_disconnect_Click(object sender, EventArgs e)
        {
            disconnectListenPort();
            btn_listen_port_connect.Enabled = true;
        }

        private void btn_print_port_connect_Click(object sender, EventArgs e)
        {
            connectPrintPort();
            btn_print_port_connect.Enabled = false;
        }

        private void btn_print_port_disconnect_Click(object sender, EventArgs e)
        {
            disconnectPrintPort();
            btn_print_port_connect.Enabled = true;
        }

        private void btn_send_to_listen_port_Click(object sender, EventArgs e)
        {
            String input_message = tbx_send_message_0.Text.Trim();
            if (0 < input_message.Length)
            {
                sendMessageToListenPort(input_message);
            }
        }

        private void btn_send_to_print_port_Click(object sender, EventArgs e)
        {
            String input_message = tbx_send_message_1.Text.Trim();
            if (0 < input_message.Length)
            {
                sendMessageToPrintPort(input_message);
            }
        }

        // timer
        //=======================================================================================================================================================
        private void startWatchDogTimer()
        {
            timerWatchDog.Enabled = true;
            timerWatchDog.Interval = TIMER_TICK_INTERVAL;
            timerWatchDog.Tick += onWatchDogTimerTick;
            timerWatchDog.Start();
        }

        private void stopWatchDogTimer()
        {
            timerWatchDog.Stop();
            timerWatchDog.Enabled = false;
        }

        private void onWatchDogTimerTick(object sender, EventArgs e)
        {
           
        }

        // ------------------------------------------------------------------------------
        //
        private void connectListenPort()
        {
            // disconnect if open
            disconnectListenPort();

            try
            {
                // Create the serial port with basic settings
                m_listen_port = new SerialPort(COMPORT_LISTEN, 9600, Parity.None, 8, StopBits.One);
                m_listen_port.Handshake = Handshake.None;
                m_listen_port.ReadBufferSize = 4096;
                m_listen_port.ReadTimeout = 2000;

                // Attach a method to be called when there
                // is data waiting in the port's buffer
                m_listen_port.DataReceived += new SerialDataReceivedEventHandler(onDataReceivedListenPort);
                m_listen_port.DtrEnable = true;

                Debug.WriteLine("m_listen_port.PortName : " + m_listen_port.PortName);
                // Begin communications
                m_listen_port.Open();

                // message
                printListenPortReceivedData("> connected " + COMPORT_LISTEN);
            }
            catch (Exception err)
            {
                KnDevexpressFunc.showMessage(err.Message);
                disconnectListenPort();
            }
        }

        private void disconnectListenPort()
        {
            setFormControlEnable(false);

            if (null != m_listen_port)
            {
                m_listen_port.Close();
                m_listen_port = null;

                // message
                printListenPortReceivedData("> disconnected from " + COMPORT_LISTEN);
            }
        }

        private void onDataReceivedListenPort(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate { printListenPortReceivedData("-------------------- recv start"); }));

            System.Array.Clear(m_listen_port_read_buf, 0, m_listen_port_read_buf.Length);
            m_listen_port_read_len = 0;

            int onetime_read_len = 0;
            while (m_listen_port.BytesToRead > 0)
            {
                onetime_read_len = ((m_listen_port_read_len + m_listen_port.BytesToRead) < m_listen_port_read_buf.Length ? m_listen_port.BytesToRead : (m_listen_port_read_buf.Length - m_listen_port_read_len));
                m_listen_port.Read(m_listen_port_read_buf, m_listen_port_read_len, onetime_read_len);
                m_listen_port_read_len += onetime_read_len;
            }

            //String read_msg_0 = System.Text.Encoding.ASCII.GetString(m_listen_port_read_buf);
            //String read_msg_1 = System.Text.Encoding.UTF8.GetString(m_listen_port_read_buf);
            //String read_msg_2 = System.Text.Encoding.Unicode.GetString(m_listen_port_read_buf);
            //String read_msg_3 = System.Text.Encoding.UTF32.GetString(m_listen_port_read_buf);
            String read_msg = System.Text.Encoding.Default.GetString(m_listen_port_read_buf); // 운영체제에 맞는 코드 페이지 
            TsLog.writeLog(read_msg);

            // log
            this.Invoke(new MethodInvoker(delegate { printListenPortReceivedData("  >> read: " + m_listen_port_read_len.ToString()); }));
            this.Invoke(new MethodInvoker(delegate { printListenPortReceivedData("-------------------- recv end--"); }));

            //String read_data = m_listen_port.ReadExisting();
            //this.Invoke(new MethodInvoker(delegate { printListenPortReceivedData(read_data); }));

            if (0 < m_listen_port_read_len)
            {
                // request save
                ObjPrinterSerialRawDataBuf input_raw_data = new ObjPrinterSerialRawDataBuf();
                input_raw_data.setObj("test_input_type", m_listen_port_read_buf, 0, m_listen_port_read_len, "test_output_type");
                input_raw_data.requestSendRowData();

                // push to list
                lock(m_recv_raw_data_list_lock)
                {
                    m_recv_raw_data_list.Add(input_raw_data);
                }

                // button enable
                this.Invoke(new MethodInvoker(delegate { setFormControlEnable(true); }));
            }
        }

        private void printListenPortReceivedData(String _message)
        {
            String print_msg = COMPORT_LISTEN + ": " + _message;
            KnUtil.insertListBoxAtFirst(lbx_listen_port, 1000, print_msg);
        }

        // ------------------------------------------------------------------------------
        //
        private void connectPrintPort()
        {
            // disconnect if open
            disconnectPrintPort();
            
            try
            {
                // Create the serial port with basic settings
                m_print_port = new SerialPort(COMPORT_PRINT, 9600, Parity.None, 8, StopBits.One);
                m_print_port.Handshake = Handshake.None;
                m_print_port.ReadTimeout = 2000;

                // Attach a method to be called when there
                // is data waiting in the port's buffer
                m_print_port.DataReceived += new SerialDataReceivedEventHandler(onDataReceivedPrintPort);

                // Begin communications
                m_print_port.Open();

                // message
                printPrintPortReceivedData("> connected " + COMPORT_PRINT);
            }
            catch(Exception err)
            {
                KnDevexpressFunc.showMessage(err.Message);
                disconnectPrintPort();
            }
        }

        private void disconnectPrintPort()
        {
            if (null != m_print_port)
            {
                m_print_port.Close();
                m_print_port = null;

                // message
                printPrintPortReceivedData("> disconnected from " + COMPORT_PRINT);
            }
        }

        private void onDataReceivedPrintPort(object sender, SerialDataReceivedEventArgs e)
        {
            String read_data = m_print_port.ReadExisting();
            this.Invoke(new MethodInvoker(delegate { printPrintPortReceivedData(read_data); }));
        }

        private void printPrintPortReceivedData(String _message)
        {
            String print_msg = COMPORT_PRINT + ": " + _message;
            KnUtil.insertListBoxAtFirst(lbx_print_port, 1000, print_msg);
        }

        // ------------------------------------------------------------------------------
        //
        private void sendMessageToListenPort(String _message)
        {
            if (null != m_listen_port)
            {
                m_listen_port.Write(_message);
            }
        }

        private void sendMessageToPrintPort(String _message)
        {
            if (null != m_print_port)
            {
                m_print_port.Write(_message);
            }
        }

        private void setFormControlEnable(bool _is_enable)
        {
            btn_recv_data_send_to_print_port.Enabled = _is_enable;
            btn_recv_data_send_to_print_port.BackColor = _is_enable ? Color.LightBlue : Color.WhiteSmoke;
        }

        private void btn_recv_data_send_to_print_port_Click(object sender, EventArgs e)
        {
            if (null == m_print_port || !m_print_port.IsOpen)
            {
                MessageBox.Show("먼저 프린트 포트를 연결 해 주세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lock (m_recv_raw_data_list_lock)
            {
                foreach(ObjPrinterSerialRawDataBuf obj in m_recv_raw_data_list)
                {
                    sendToPrintPort(obj);
                }
                m_recv_raw_data_list.Clear();
            }

            // 출력 후 비활성화
            setFormControlEnable(false);
        }

        private void sendToPrintPort(ObjPrinterSerialRawDataBuf _obj)
        {
            // 출력
            if (null != _obj && 0 < _obj.m_order_input_raw_data_len)
            {
                m_print_port.Write(_obj.m_order_input_raw_data_buf, 0, _obj.m_order_input_raw_data_len); // send data
                printListenPortReceivedData(String.Format(">> recv data send to print port: {0} byte", _obj.m_order_input_raw_data_len)); // log
            }
        }
    }
}
