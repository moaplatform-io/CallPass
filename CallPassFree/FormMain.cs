using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kons.Utility;
using Kons.ShopCallpass.AppMain;
using Kons.ShopCallpass.FormDialog;
using Kons.ShopCallpass.FormPopup;
using Kons.ShopCallpass.FormView;
using Kons.ShopCallpass.Object;
using Kons.ShopCallpass.Controller;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Alerter;
using Kons.ShopCallpass.Model;
using Kons.TsLibrary;
using Kons.ShopCallpass.OpenApi;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Kons.ShopCallpass
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public enum FORM_MODE
        {
            MODE_NONE = 0,
            MODE_SHW_CHILD_CONTROLS,
            MODE_RUNNING,
            MODE_EXIT,
        }

        // max order window count
        private FORM_MODE m_curr_mode;

        // timer tick define
        private const int TIMER_TICK_INTERVAL = 2000;
        private const int TIMER_TICK_RUNNING = (int)((1000.0 / TIMER_TICK_INTERVAL) * 5.0);
        private const int TIMER_TICK_UPDATE_TIME = (int)((1000.0 / TIMER_TICK_INTERVAL) * 60.0);             // 1분에 한번
        private const int TIMER_TICK_MAIN_WND_INFO = (int)((1000.0 / TIMER_TICK_INTERVAL) * 5.0 * 60.0);     // 5분에 한번
        private const int TIMER_TICK_BAR_MESSAGE = (int)((1000.0 / TIMER_TICK_INTERVAL) * 10.0);             // 1분에 한번

        // timer tick values
        private int m_tick_running = TIMER_TICK_RUNNING;
        private int m_tick_update_time = 0; // 시작시 바로 로딩시키기 위해
        private int m_tick_main_window_info = 0; // 시작시 바로 로딩시키기 위해
        private int m_tick_bar_message_hide = 0;

        // statusbar item
        BarStaticItem m_statusbar_datetime = null;
        BarStaticItem m_statusbar_order_input = null;
        BarStaticItem m_statusbar_print = null;
        BarStaticItem m_statusbar_parsing = null;
        BarStaticItem m_statusbar_item_notify = null;

        // 뷰 접근성 및 중복생성 방지
        private Dictionary<FormViewBase.VIEW_TYPE, FormViewBase> m_dic_formview = new Dictionary<FormViewBase.VIEW_TYPE, FormViewBase>();

        // 접수화면
        private FormViewOrderList m_form_order_list = null;

        // 컨트롤러
        ControllerOrderInput m_mgr_order_input = null;
        
        // 프린트 포트
        private Dictionary<String, SerialPortPrintOutput> m_dic_print_serial_port = new Dictionary<string, SerialPortPrintOutput>();
        private PoolConfigPrintOutput m_pool_print_config = null;
        private int m_connected_print_port_count = 0;

        // extra config
        private ObjConfigRunningEtc m_running_etc_config = null;
        private bool m_is_need_send_order_input_data = false; // 본사(관리자)가 필요에 의해 데이터를 받아볼 필요가 있는경우 경우 강제로 전송하게 만듬

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        public FormMain()
        {
            //this.WindowState = FormWindowState.Maximized;

            InitializeComponent();

            // init objects
            initDlgObjects();

            // init controls
            initDlgControls();

            // set main form
            Kons.ShopCallpass.AppMain.AppCore.Instance.setMainForm(this);

            // 타이틀
            string app_title = "Callpass";
            this.Text = string.Format("{0} - Ver {1}", app_title, Kons.ShopCallpass.AppMain.AppCore.Instance.getAppVersionString());

            // 초기값
            m_curr_mode = FORM_MODE.MODE_NONE;

            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_enroll_bemin") == 1)
                {
                    barCheckItem4.Checked = true;
                }
                else
                {
                    barCheckItem4.Checked = false;
                }

                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_enroll_yogiyo") == 1)
                {
                    barCheckItem5.Checked = true;
                }
                else
                {
                    barCheckItem5.Checked = false;
                }

                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_enroll_pos") == 1)
                {
                    barCheckItem6.Checked = true;
                }
                else
                {
                    barCheckItem6.Checked = false;
                }

                if (device.readConfigLong("FORM_MAIN", "optimization_mode") == 1)
                {
                    barCheckItem7.Checked = true;
                }
                else
                {
                    barCheckItem7.Checked = false;
                }
            }
            else
            {
                MessageBox.Show("디바이스 생성 오류");
                Application.Exit();
            }

        }

        private void FormMain_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
            }
            else
            {
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // 모래시계 커서
            this.Cursor = Cursors.WaitCursor;

            // start timer
            startWatchdogTimer(100); // 최초 시작시 바로 INIT 실행될 수 있도록
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool is_exit = isExit();

            if (CloseReason.UserClosing != e.CloseReason || is_exit)
            {
                releaseForm();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        // ---------------------------------------------------------- init
        //
        private void initDlgObjects()
        {
        }

        private void initDlgControls()
        {
            // init
            initControlStatusBar();

            // properties
            setDlgControlProperties();
        }

        private void initControlStatusBar()
        {
            // ------------------------------------------------------ left
            //
            m_statusbar_datetime = new BarStaticItem();
            if (null != m_statusbar_datetime)
            {
                m_statusbar_datetime.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                m_statusbar_datetime.AutoSize = BarStaticItemSize.Content;
                m_statusbar_datetime.Name = "statusbar_datetime";
                m_statusbar_datetime.Caption = Kons.ShopCallpass.AppMain.AppCore.Instance.getLoginUserObj().m_login_id;
                m_statusbar_datetime.TextAlignment = StringAlignment.Near;

                ctr_bar_main_status.ItemLinks.Add(m_statusbar_datetime, false);
            }

            BarStaticItem temp_empty = new BarStaticItem(); // 간격을 맞추기 위해
            if (null != temp_empty)
            {
                temp_empty.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                temp_empty.AutoSize = BarStaticItemSize.None;
                temp_empty.Width = 14;
                temp_empty.Name = "statusbar_empty";
                temp_empty.Caption = "";

                ctr_bar_main_status.ItemLinks.Add(temp_empty, true);
            }

            m_statusbar_order_input = new BarStaticItem();
            if (null != m_statusbar_order_input)
            {
                m_statusbar_order_input.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                m_statusbar_order_input.AutoSize = BarStaticItemSize.None;
                m_statusbar_order_input.Width = 90;
                m_statusbar_order_input.Name = "statusbar_order_input";
                m_statusbar_order_input.Caption = "연동: 확인중...";

                ctr_bar_main_status.ItemLinks.Add(m_statusbar_order_input, false);
            }

            m_statusbar_print = new BarStaticItem();
            if (null != m_statusbar_print)
            {
                m_statusbar_print.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                m_statusbar_print.AutoSize = BarStaticItemSize.None;
                m_statusbar_print.Width = 90;
                m_statusbar_print.Name = "statusbar_print";
                m_statusbar_print.Caption = " 출력: 확인중...";

                ctr_bar_main_status.ItemLinks.Add(m_statusbar_print, false);
            }

            m_statusbar_parsing = new BarStaticItem();
            if (null != m_statusbar_parsing)
            {
                m_statusbar_parsing.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                m_statusbar_parsing.AutoSize = BarStaticItemSize.None;
                m_statusbar_parsing.Width = 90;
                m_statusbar_parsing.Name = "statusbar_parsing_info";
                m_statusbar_parsing.Caption = " 작업: 대기중...";

                ctr_bar_main_status.ItemLinks.Add(m_statusbar_parsing, false);
            }

            // ------------------------------------------------------ right
            //
            m_statusbar_item_notify = new BarStaticItem();
            if (null != m_statusbar_item_notify)
            {
                m_statusbar_item_notify.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                m_statusbar_item_notify.AutoSize = BarStaticItemSize.Content;
                m_statusbar_item_notify.Name = "statusbar_notify_message";
                m_statusbar_item_notify.Alignment = BarItemLinkAlignment.Right;
                m_statusbar_item_notify.Caption = "wait...";

                ctr_bar_main_status.ItemLinks.Add(m_statusbar_item_notify);
            }

        }

        public void setDlgControlProperties()
        {
            ribbonControl.DrawGroupCaptions = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl.ShowToolbarCustomizeItem = false;
            //ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;

            // 현재 지원하지 않는 기능은 숨김
            order_bar_btn_search.Visibility = BarItemVisibility.Never;

            // 관리자가 아닌경우
            if (!Kons.ShopCallpass.AppMain.AppCore.Instance.getLoginUserObj().IS_AUTHORITY_MAX)
            {
                ribbonPageGroup_etc.Visible = false;
            }
            ribbonPageGroup_admin.Visible = false;
        }

        public bool isExit()
        {
            Kons.ShopCallpass.AppMain.AppCore.APP_MODE cur_mode = Kons.ShopCallpass.AppMain.AppCore.Instance.getCurrentAppMode();
            if (cur_mode == Kons.ShopCallpass.AppMain.AppCore.APP_MODE.MODE_LOGIN || cur_mode == Kons.ShopCallpass.AppMain.AppCore.APP_MODE.MODE_NONE)
            {
                return true;
            }

            //  "프로그램 종료"
            if (KnDevexpressFunc.showMessage("프로그램을 종료 하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                return true;
            }

            return false;
        }

        private void releaseForm()
        {
            // stop timer
            stopWatchdogTimer();

            // exit
            Kons.ShopCallpass.AppMain.AppCore.Instance.setAppMode(Kons.ShopCallpass.AppMain.AppCore.APP_MODE.MODE_EXIT);
        }

        // ----------------------------------------------------------
        //
        private void loadBasicData()
        {

        }

        // ----------------------------------------------------------
        //
        private void setWatDogTimerInterval(int _interval)
        {
            timer_watch_dog.Interval = _interval;
        }

        private void startWatchdogTimer(int _interval)
        {
            timer_watch_dog.Enabled = true;
            timer_watch_dog.Interval = _interval;
            timer_watch_dog.Tick += onTickWatchDogTimer;
            timer_watch_dog.Start();
        }

        private void stopWatchdogTimer()
        {
            timer_watch_dog.Stop();
            timer_watch_dog.Tick -= onTickWatchDogTimer;
            timer_watch_dog.Enabled = false;
        }

        private void onTickWatchDogTimer(object sender, EventArgs e)
        {
            // check init
            switch (m_curr_mode)
            {
                case FORM_MODE.MODE_NONE:
                    {
                        m_curr_mode = FORM_MODE.MODE_SHW_CHILD_CONTROLS; // set next mode
                        this.Cursor = Cursors.WaitCursor;

                        // load basic data
                        loadBasicData();

                        // load first view
                        showMainFirstView();

                        // INIT 타이머 후 감시 타이머 간격으로 교체한다.
                        setWatDogTimerInterval(TIMER_TICK_INTERVAL);
                    }
                    return;
                case FORM_MODE.MODE_SHW_CHILD_CONTROLS:
                    {
                        m_curr_mode = FORM_MODE.MODE_RUNNING; // set next mode
                        startOrderSync();

                        m_statusbar_item_notify.Caption = "";

                        this.Cursor = Cursors.Default;
                    }
                    return;
            }

            // ------------------------------------------------------
            //
            checkOrderSync();
            procOrderSync();

            // bar message
            if (0 < m_tick_bar_message_hide)
            {
                m_tick_bar_message_hide--;
                if (0 >= m_tick_bar_message_hide)
                {
                    m_statusbar_item_notify.Caption = "";
                }
            }

            // 시간갱신
            if (0 >= m_tick_update_time--)
            {
                m_tick_update_time = TIMER_TICK_UPDATE_TIME;
                //if (null != m_statusbar_datetime)
                //{
                //    m_statusbar_datetime.Caption = DateTime.Now.ToString("yy-MM-dd");
                //}
            }

            // home info
            if (0 >= m_tick_main_window_info--)
            {
                m_tick_main_window_info = TIMER_TICK_MAIN_WND_INFO;
                loadMainWndInfo();
            }

            // running tick
            if (0 >= m_tick_running--)
            {
                m_tick_running = TIMER_TICK_RUNNING;
            }
        }

        private void loadMainWndInfo()
        {

        }

        public void stopOrderSync()
        {
            Utility.Utility.LogWrite("stopOrderSync호출");
            // order input
            if (null != m_mgr_order_input)
            {   
                m_mgr_order_input.stopListen();
            }

            Utility.Utility.LogWrite("stopOrderSync");
            // print output
            foreach (KeyValuePair<String, SerialPortPrintOutput> sel_item in m_dic_print_serial_port)
            {
                SerialPortPrintOutput sel_port = sel_item.Value;
                sel_port.disconnectSerialPort();
            }
            m_dic_print_serial_port.Clear();
            m_connected_print_port_count = 0;
        }

        public void startOrderSync()
        {
            Utility.Utility.LogWrite("startOrderSync호출");
            // ---------------------------------- stop
            // stop controllsers
            stopOrderSync();

            Utility.Utility.LogWrite("startOrderSync");
            // ---------------------------------- load config
            // print config
            if (null == m_pool_print_config)
            {
                m_pool_print_config = new PoolConfigPrintOutput(Kons.ShopCallpass.AppMain.AppCore.PRINT_OUTPUT_CONFIG_COUNT);
            }
            m_pool_print_config.loadObjectAll();

            // etc config
            if (null == m_running_etc_config)
            {
                m_running_etc_config = new ObjConfigRunningEtc();
            }
            m_running_etc_config.loadFromDevice();

            // etc flag
            m_is_need_send_order_input_data = Kons.ShopCallpass.AppMain.AppCore.Instance.getLoginUserObj().IS_NEED_SEND_TO_SERVER;

            // ---------------------------------- print port
            // connect to print port
            for (int i = 0; i < m_pool_print_config.COUNT; i++)
            {
                try
                {
                    ObjConfigPrintOutput print_config = m_pool_print_config.getObject(i);
                    if (null != print_config && print_config.IS_USE)
                    {
                        SerialPortPrintOutput new_port = new SerialPortPrintOutput(print_config.m_print_port_num, print_config.m_print_port_baud);
                        //new_port.Notify += onNotifyPrintOutput;
                        new_port.connectSerialPort();

                        m_dic_print_serial_port.Add(print_config.m_print_port_num, new_port);

                        m_connected_print_port_count++;
                    }
                }
                catch (Exception err)
                {
                    Utility.Utility.LogWrite("startOrderSync에서 예외발생 : " + err.Message);
                    ModelAppDevice device = new ModelAppDevice();
                    device.writeLog(err.Message);
                }
            }

            // ---------------------------------- order input
            // order input
            if (null == m_mgr_order_input)
            {
                m_mgr_order_input = new ControllerOrderInput();
                m_mgr_order_input.Notify += onNotifyOrderInput;
            }
            m_mgr_order_input.startListen();
        }

        private void checkOrderSync()
        {
            // check
            if (0 < m_mgr_order_input.CONNECTED_INPUT_PORT_COUNT)
            {
                m_statusbar_order_input.Caption = String.Format("연동: 연결( {0} )", m_mgr_order_input.CONNECTED_INPUT_PORT_COUNT);
            }
            else
            {
                m_statusbar_order_input.Caption = "연동: 사용안함";
            }

            // check
            if (0 < m_connected_print_port_count)
            {
                m_statusbar_print.Caption = String.Format("출력: 연결( {0} )", m_connected_print_port_count);
            }
            else
            {
                m_statusbar_print.Caption = " 출력: 사용안함";
            }

            // parsing
            if (0 < m_mgr_order_input.CONNECTED_INPUT_PORT_COUNT)
            {
                m_statusbar_parsing.Caption = String.Format("작업: {0} / {1}", m_mgr_order_input.getPoolHaveCount(), m_mgr_order_input.getPoolTotalCount());
            }
            else
            {
                m_statusbar_parsing.Caption = "작업: 대기중...";
            }
        }

        private void procOrderSync()
        {
            // proc
            if (null != m_mgr_order_input)
            {
                m_mgr_order_input.procParsing();
            }
        }

        public void onNotifyOrderChange(ObjOrder _order)
        {
            ObjOrder recv_order = (ObjOrder)_order;
            if (null != recv_order.m_order_num && 0 < recv_order.m_order_num.Length)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    m_form_order_list.onChangeObj(_order);
                }));
            }
        }

        private void onNotifyOrderInput(object _sender, object _what, object _obj)
        {
            Utility.Utility.LogWrite("onNotifyOrderInput");

            switch ((SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT)_what)
            {
                case SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT.TEXT_NOTIFY_MESSAGE:
                    {
                        Utility.Utility.LogWrite("onNotifyOrderInput case 0");
                        if (typeof(String) == _obj.GetType())
                        {
                            showBarMessage((String)_obj);
                        }
                    }
                    break;

                case SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT.TEXT_ERROR_MESSAGE:
                    {
                        Utility.Utility.LogWrite("onNotifyOrderInput case 1");
                        if (typeof(String) == _obj.GetType())
                        {
                            showWindowErrorMessage((String)_obj);
                        }
                    }
                    break;

                case SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT.RAW_DATA_STREAM:
                    {
                        Utility.Utility.LogWrite("onNotifyOrderInput case 2");
                        onOrderInputSerialPortReceiveRawData((ObjOrderInputSerialRawDataBuf)_obj);
                    }
                    break;

                case SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT.ORDER_OBJECT:
                    {
                        Utility.Utility.LogWrite("onNotifyOrderInput case 3");
                        ObjOrder recv_order = (ObjOrder)_obj;
                        if (null != recv_order.m_order_num && 0 < recv_order.m_order_num.Length)
                        {
                            // 목록
                            this.Invoke(new MethodInvoker(delegate
                            {
                                showWindowAlertOrderInput();
                                m_form_order_list.onChangeObj(recv_order);
                            }));
                        }
                    }
                    break;
            }
        }

        private void onOrderInputSerialPortReceiveRawData(ObjOrderInputSerialRawDataBuf _recv_buf_info)
        {
            Utility.Utility.LogWrite("onOrderInputSerialPortReceiveRawData");
            // ------------------------------------------------------
            //
            SerialPortOrderInput input_port = _recv_buf_info.m_caller_serial_port;
            if (null != input_port)
            {
                byte[] input_port_recv_buf = _recv_buf_info.m_data_buf;
                int input_port_recv_len = _recv_buf_info.m_data_len;

                // ------------------------------------------------------
                // 프린트로 데이터 출력
                printRawData(input_port.getPrintOutputPortNum(), input_port_recv_buf, 0, input_port_recv_len);
                Debug.WriteLine("drink");
            }
            Debug.WriteLine("tiger");
            // 데이터 저장 및 전송
            if (null != m_running_etc_config)
            {
                Debug.WriteLine("lion");
                if (1 == m_running_etc_config.m_is_use_save_order_input || 1 == m_running_etc_config.m_is_use_send_order_input || m_is_need_send_order_input_data)
                {
                    Debug.WriteLine("monkey");
                    if (1 == m_running_etc_config.m_is_use_save_order_input)
                    {
                        Debug.WriteLine("mouse");
                        //_recv_buf_info.saveRawDataToDevice();
                    }

                    if (1 == m_running_etc_config.m_is_use_send_order_input || m_is_need_send_order_input_data)
                    {
                        Debug.WriteLine("cat");
                        //_recv_buf_info.requestSendRowData();
                    }
                }
            }
        }

        private void onNotifyPrintOutput(object _sender, object _what, object _obj)
        {
            Utility.Utility.LogWrite("onNotifyPrintOutput");
            if (!this.IsHandleCreated)
            {
                return;
            }

            switch ((SerialPortPrintOutput.SERIAL_PORT_NOTIFY_WHAT)_what)
            {
                case SerialPortPrintOutput.SERIAL_PORT_NOTIFY_WHAT.TEXT_NOTIFY_MESSAGE:
                    {
                        if (typeof(String) == _obj.GetType())
                        {
                            showBarMessage((String)_obj);
                        }
                    }
                    break;

                case SerialPortPrintOutput.SERIAL_PORT_NOTIFY_WHAT.TEXT_ERROR_MESSAGE:
                    {
                        if (typeof(String) == _obj.GetType())
                        {
                            showWindowErrorMessage((String)_obj);
                        }
                    }
                    break;

                case SerialPortPrintOutput.SERIAL_PORT_NOTIFY_WHAT.RAW_DATA_STREAM:
                    {
                        try
                        {
                            onPrintSerialPortReceiveRawData((SerialPortPrintOutput.SerialRawDataBufInfo)_obj);
                        }
                        catch (Exception err)
                        {
                            Utility.Utility.LogWrite("onNotifyPrintOutput에서 예외발생 : " + err.Message);
                            TsLog.writeLog(err.Message);
                        }

                    }
                    break;
            }
        }

        private void onPrintSerialPortReceiveRawData(SerialPortPrintOutput.SerialRawDataBufInfo _recv_buf_info)
        {
            // ------------------------------------------------------
            //
            String print_port_name = _recv_buf_info.port_num;
            byte[] print_port_recv_buf = _recv_buf_info.buf;
            int print_port_recv_offset = _recv_buf_info.offset;
            int print_port_recv_len = _recv_buf_info.len;

            // ------------------------------------------------------
            // 프린트로 데이터 출력
            //printRawData(print_output_port_num, input_port_recv_buf, input_port_recv_offset, input_port_recv_len);
            Debug.WriteLine("aboutserver0");
            // 데이터 저장 및 전송
            if (null != m_running_etc_config)
            {
                if (1 == m_running_etc_config.m_is_use_save_order_input || 1 == m_running_etc_config.m_is_use_send_order_input || m_is_need_send_order_input_data)
                {
                    ObjPrinterSerialRawDataBuf raw_data = new ObjPrinterSerialRawDataBuf();
                    raw_data.setObj(print_port_name, print_port_recv_buf, print_port_recv_offset, print_port_recv_len, "from print");

                    if (1 == m_running_etc_config.m_is_use_save_order_input)
                    {
                        Debug.WriteLine("aboutserver1");
                        //raw_data.saveRawDataToDevice();
                    }

                    if (1 == m_running_etc_config.m_is_use_send_order_input || m_is_need_send_order_input_data)
                    {
                        Debug.WriteLine("aboutserver2");
                        //raw_data.requestSendRowData();
                    }
                }
            }
        }

        private void ribbonControl_SelectedPageChanged(object sender, EventArgs e)
        {
            DevExpress.XtraBars.Ribbon.RibbonPage sel_ribbon_page = ribbonControl.SelectedPage;
            switchRibbonPage(sel_ribbon_page.PageIndex);
        }

        private void switchRibbonPage(int _next_page_index)
        {
            switch (_next_page_index)
            {
                case 0:
                    break;
                case 1:
                    break;
            }
        }

        private void showMainFirstView()
        {
            showFormChildView(FormViewBase.VIEW_TYPE.VIEW_ORDER_LIST);
        }

        private void showFormChildView(FormViewBase.VIEW_TYPE _view_type)
        {
            // CompanyManager 는 MODE_RUNNING 이후에 초기화 되므로 초기화가 완료 후 MODE_RUNNING로 셋팅 되기 까지 띄우지 않는다.
            if (FORM_MODE.MODE_NONE == m_curr_mode)
            {
                FormPopupNotify.Show(this, "시스템 초기화 중입니다.\n\n잠시 후, 다시 시도해 주세요.", "알림", 3000);
                return;
            }

            // 유일성을 필요로 하는 폼은 추가 생성 없이 활성화
            FormViewBase new_form = (m_dic_formview.ContainsKey(_view_type) ? m_dic_formview[_view_type] : null);
            if (null != new_form)
            {
                foreach (Form openForm in panelMain.Controls)
                {
                    if (openForm.IsHandleCreated && openForm.GetType() == new_form.GetType()) // 다른폼에 추가된 같은 view 가 있을수 있으므로 mdi 확인한다. 
                    {
                        openForm.Activate();
                        return;
                    }
                }
            }

            // 모래시계 커서
            this.Cursor = Cursors.WaitCursor;

            // 닫힌 윈도우는 객체는 살아 있으나 내부 Form이 삭제된 상태이기 때문에 새로 생성한다.
            switch (_view_type)
            {
                case FormViewBase.VIEW_TYPE.VIEW_ORDER_LIST:
                    {
                        m_form_order_list = new FormViewOrderList();
                        new_form = m_form_order_list;
                    }
                    break;
            }

            if (null != new_form)
            {
                // 유일성을 필요로 하는 폼은 관리
                if (new_form.IS_ONLY)
                {
                    if (m_dic_formview.ContainsKey(_view_type))     // 이미 닫힌 윈도우 객체는 삭제한다
                    {
                        m_dic_formview.Remove(_view_type);
                    }
                    m_dic_formview.Add(_view_type, new_form);
                }

                new_form.TopLevel = false;
                new_form.FormBorderStyle = FormBorderStyle.None;
                new_form.Dock = DockStyle.Fill;
                new_form.Show();

                panelMain.Controls.Add(new_form);
            }

            // 일반커서
            this.Cursor = Cursors.Default;
        }

        private void order_bar_btn_search_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (null != m_form_order_list)
            {
                //int search_df = KnUtil.parseInt32(((DateTime)order_bar_edt_search_date_f.EditValue).ToString("yyyyMMdd"));
                //int search_dt = KnUtil.parseInt32(((DateTime)order_bar_edt_search_date_t.EditValue).ToString("yyyyMMdd"));
                //String filter_text = (null == bar_tbx_filter_text.EditValue ? "" : bar_tbx_filter_text.EditValue.ToString());

                //m_form_order_list.searchOrder(search_df, search_dt, filter_text);
            }
        }

        private void repositoryItemFilterText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (null != m_form_order_list)
                {
                    String filter_text = "";
                    if (sender.GetType() == typeof(DevExpress.XtraEditors.TextEdit))
                    {
                        DevExpress.XtraEditors.TextEdit edit_box = (DevExpress.XtraEditors.TextEdit)sender;
                        filter_text = edit_box.EditValue.ToString();
                    }
                    m_form_order_list.searchFilterChange(filter_text);
                }
                e.Handled = true;
            }
        }

        private void btn_order_request_delivery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Utility.Utility.LogWrite("btn_order_request_delivery_ItemClick");

            if (null != m_form_order_list)
            {
                m_form_order_list.requestDelivery();
            }
        }

        private void btn_order_print_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (new KnFormWaitCursors(this))
            {
                if (null != m_form_order_list)
                {
                    m_form_order_list.requestOrderPrint();
                }
            }
        }

        private void btn_program_setup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Utility.Utility.LogWrite("btn_program_setup_ItemClick");
            using (new KnFormWaitCursors(this))
            {
                FormDlgConfig dlg = new FormDlgConfig();
                if (null != dlg)
                {
                    dlg.ShowDialog();

                    // 변경 했는지 확인
                    if (dlg.m_is_change_config)
                    {
                        startOrderSync();
                    }
                }
            }
        }

        private void btn_application_info_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormPopupProgramInfo dlg = new FormPopupProgramInfo();
            dlg.ShowDialog();
        }

        private void btn_request_remote_help_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string url = Kons.ShopCallpass.AppMain.AppCore.REMOTE_HELPER_URL;
            if (null != url)
            {
                System.Diagnostics.Process.Start(url);
            }
        }

        private void etc_bar_btn_order_input_test_ItemClick(object sender, ItemClickEventArgs e)
        {
            // ------------------------------------------------------
            //
            parsingOrderInputTestData();
            //
            // ------------------------------------------------------

        }


        private SerialPortPrintOutput getConnectedPrintOutputSerialPort(String _port_num)
        {
            if (null != m_dic_print_serial_port)
            {
                if (m_dic_print_serial_port.ContainsKey(_port_num))
                {
                    return m_dic_print_serial_port[_port_num];
                }
            }
            return null;
        }

        private void mgr_bar_btn_user_list_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void mgr_bar_btn_user_add_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 상태변경은 코드로 바꿀 수 있으므로 클릭 이벤트로 연결한다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bar_chk_order_state_0_ItemClick(object sender, ItemClickEventArgs e)
        {
            int sel_state_cd = (int)ObjOrder.STATE_TYPE.ORDER_STATE_0;
            bool is_checked = bar_chk_order_state_0.Checked;
            if (null != m_form_order_list)
            {
                m_form_order_list.onCheckStateClick(sel_state_cd, is_checked);
            }

            // 전체선택 체크박스
            if (isCheckedStateAll())
            {
                bar_chk_order_state_all.Checked = true;
            }
            else
            {
                bar_chk_order_state_all.Checked = false;
            }
        }

        private void bar_chk_order_state_1_ItemClick(object sender, ItemClickEventArgs e)
        {
            int sel_state_cd = (int)ObjOrder.STATE_TYPE.ORDER_STATE_1;
            bool is_checked = bar_chk_order_state_1.Checked;
            if (null != m_form_order_list)
            {
                m_form_order_list.onCheckStateClick(sel_state_cd, is_checked);
            }

            // 전체선택 체크박스
            if (isCheckedStateAll())
            {
                bar_chk_order_state_all.Checked = true;
            }
            else
            {
                bar_chk_order_state_all.Checked = false;
            }
        }

        private void bar_chk_order_state_all_ItemClick(object sender, ItemClickEventArgs e)
        {
            int sel_state_cd = -1;
            bool is_checked = bar_chk_order_state_all.Checked;
            if (null != m_form_order_list)
            {
                m_form_order_list.onCheckStateClick(sel_state_cd, is_checked);
            }

            changeStateCheckedAll(is_checked);
        }

        private void changeStateCheckedAll(bool _checked)
        {
            bar_chk_order_state_0.Checked = _checked;
            bar_chk_order_state_1.Checked = _checked;
            bar_chk_order_state_all.Checked = _checked;
        }

        private bool isCheckedStateAll()
        {
            if (!bar_chk_order_state_0.Checked)
            {
                return false;
            }

            if (!bar_chk_order_state_1.Checked)
            {
                return false;
            }

            return true;
        }

        public void printOrder(ObjOrder _order)
        {
            if (null == _order)
            {
                Utility.Utility.LogWrite("111");
                return;
            }

            if (null == _order.m_print_port_num || 0 == _order.m_print_port_num.Length)
            {
                Utility.Utility.LogWrite("222");
                return;
            }
            Utility.Utility.LogWrite("333");
            printRawData(_order.m_print_port_num, _order.m_print_raw_data_buf, 0, _order.m_print_raw_data_buf.Length);
        }

        private bool printRawData(String _port_num, byte[] _data_buf, int _data_offset, int _data_len)
        {
            Utility.Utility.LogWrite("444");
            SerialPortPrintOutput sel_print_port = getConnectedPrintOutputSerialPort(_port_num);
            Utility.Utility.LogWrite("555");
            ModelAppDevice device = new ModelAppDevice();
            if (null != sel_print_port && null != sel_print_port.getSerialPort())
            {
                Utility.Utility.LogWrite("666");
                Utility.Utility.LogWrite("sel_print_port.getSerialPort().PortName : " + sel_print_port.getSerialPort().PortName);
                sel_print_port.getSerialPort().Open();
                if (sel_print_port.getSerialPort().IsOpen)
                {
                    Utility.Utility.LogWrite("777");
                    if (null != _data_buf && 0 < _data_len)
                    {
                        Utility.Utility.LogWrite("888");
                        sel_print_port.getSerialPort().Write(_data_buf, _data_offset, _data_len);
                        //showBarMessage(String.Format("{0}.Print", sel_print_port.getSerialPort().PortName));
                        sel_print_port.getSerialPort().Close();
                        Utility.Utility.LogWrite("sel_print_port.getSerialPort().Write : " + sel_print_port.getSerialPort().PortName);
                        return true;
                    }
                    else
                    {
                        //showBarMessage("PrintData.0");
                    }
                }
                else
                {
                    //showBarMessage(String.Format("{0}.Closed", sel_print_port.getSerialPort().PortName));
                }
            }
            else if (device.readConfigString("CONFIG_PRINT_OUTPUT", "is_use_usb_printer") == "1")
            {
                if (null != _data_buf && 0 < _data_len)
                {
                    IntPtr unmanagedPointer = Marshal.AllocHGlobal(_data_len);
                    Marshal.Copy(_data_buf, 0, unmanagedPointer, _data_len);
                    Utility.Utility.LogWrite("데이터버퍼 렝쓰 : " + _data_len);
                    SendBytesToPrinter(device.readConfigString("CONFIG_PRINT_OUTPUT", "usb_printer_name"), unmanagedPointer, _data_len);
                    //SendBytesToPrinter("USB 인쇄 지원", unmanagedPointer, _data_len);

                    return true;
                }
                else
                {
                    //showBarMessage("PrintData.0");
                }
            }

            else
            {
                //showBarMessage(String.Format("{0}.None", _port_num));
            }
            
            return false;
        }

        public bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.
            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes = IntPtr.Zero;
            Int32 dwCount;

            // How many characters are in the string?
            // Fix from Nicholas Piasecki:
            // dwCount = szString.Length;
            dwCount = (szString.Length + 1) * Marshal.SystemMaxDBCSCharSize;

            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

        // ----------------------------------------------------------
        //
        private void showWindowAlertOrderInput()
        {
            Utility.Utility.LogWrite("showWindowAlertOrderInput");
            this.Invoke(new MethodInvoker(delegate
            {
                AlertInfo info = new AlertInfo("콜패스: 주문연계", "\n연계 주문이 등록되었습니다");
                info.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.if_Sync_Center_99944_b_32;
                alertControl.Show(null, info);
            }));
        }

        private void showWindowErrorMessage(String _msg)
        {
            Utility.Utility.LogWrite("showWindowErrorMessage");
            this.Invoke(new MethodInvoker(delegate
            {
                AlertInfo info = new AlertInfo("콜패스: 오류알림", "\n" + _msg);
                info.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.info_32x32;
                alertControl.Show(null, info);
            }));
        }

        public void showBarMessage(String _msg)
        {
            Utility.Utility.LogWrite("showBarMessage");
            if (!this.IsHandleCreated)
            {
                Utility.Utility.LogWrite("showBarMessagereturn");
                return;
            }

            this.Invoke(new MethodInvoker(delegate
            {
                Utility.Utility.LogWrite("showBarMessageInvoke: ");
                m_statusbar_item_notify.Caption = _msg;
                m_tick_bar_message_hide = TIMER_TICK_BAR_MESSAGE;
            }));
        }

        // ----------------------------------------------------------
        //
        private void parsingOrderInputTestData()
        {
            if (null == m_mgr_order_input)
            {
                showBarMessage("not ready!");
                return;
            }

            // ------------------------------------------------------
            //
            /*
            // 배민 오브젝트 단위 파싱 테스트
            byte[] recv_raw_data = TsUtil.HexStringToByteArray("1B401B321B33281B61021B45001D21002020202020202020202020202020202020202020202020202020202020203138313131332D30303030320A1B33281B61001B45001D21000A1B33281B61011B45011D21015BB9E8B4DEC0C7B9CEC1B75D20C1D6B9AEC0CC20C1A2BCF6B5C7BEFABDC0B4CFB4D92E0A1B33281B61001B45001D21000A1B33281B61001B45001D21002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A1B33281B61001B45011D2100C1D6B9AEBDC3B0A32020203A20323031382D31312D31332C2031363A32370A1B33281B61001B45011D2101B0EDB0B4BFACB6F4C3B3203A203035303731323031353038390A1B33281B61001B45011D2100BCADBFEF20C3B5BFD5B5BF20372D3120BFACC1F6C5B8BFEE32B4DCC1F620323039B5BF20343033C8A30A1B33281B61001B45011D2100BCADBFEF20BFC0B8AEB7CE20313130322D313020BFACC1F6C5B8BFEE32B4DCC1F620323039B5BF20343033C8A30A1B33281B61011B45001D21003D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D0A1B33281B61001B45001D210020B8DEB4BAB8ED20202020202020202020202020202020202020BCF6B7AE20202020202020B1DDBED70A1B33281B61001B45001D21002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A1B33281B61001B45011D2100382EC5A9B8AEBDBAC7C7B9DD2FBEE7B3E4B9DD202B2020202020202031202020202031362C303030BFF80D0A20C4DDB6F3203530306D6C202BC4A1C5B20D0AB9AB2BBCF8BBEC0A1B33281B61001B45011D2100B9E8B4DEC6C12020202020202020202020202020202020202020202031202020202020312C303030BFF80A1B33281B61011B45001D21003D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D0A1B33281B61021B45011D2101B0E1C1A6BCF6B4DC20202020202020202020BFCFBAD2200A1B33281B61001B45001D21002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A1B33281B61021B45011D2100C3D1C1D6B9AEB1DDBED720202020202031372C303030BFF8200A1B33281B61021B45011D2100B0F8B1DEB0A120202020202031352C343535BFF8200A1B33281B61021B45011D2100BACEB0A1BCBC20202020202020312C353435BFF8200A1B33281B61001B45001D21002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A1B33281B61021B45001D2111C3D1B0E1C1A6B1DDBED720202031372C303030BFF80A1B33281B61001B45001D21002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A1B33281B61001B45001D21005BB1A4B8EDC0FCB9E8B7C35DBEFBC5ACBDBAC4A1C5B220B1A4B8ED31C8A3C1A10A1B33281B61001B45001D2100C0FCC8ADB9F8C8A3203A2030322D323631362D353030340A1B33281B61001B45001D2100C1D6BCD220202020203A20B0E6B1E220B1A4B8EDB5BF203333372D353820B9AEBFB5BDC3C6BCBAF4B6F32031C3FE0A1B33281B61001B45001D21000A1B33281B61001B45001D21000A1B33281B61001B45001D21000A1B33281B61001B45001D21000A1B33281B61001B45001D21000A1D56001B321B401B401B321B33281B61021B45001D21005BC1D6B9E6BFEB5D202020202020202020202020202020202020202020203138313131332D30303030320A1B33281B61001B45001D21000A1B33281B61011B45011D21015BB9E8B4DEC0C7B9CEC1B75D20C1D6B9AEC0CC20C1A2BCF6B5C7BEFABDC0B4CFB4D92E0A1B33281B61001B45001D21000A1B33281B61001B45001D2100C1D6B9AEBDC3B0A32020203A20323031382D31312D31332C2031363A32370A1B33281B61011B45001D21003D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D0A1B33281B61001B45001D210020B8DEB4BAB8ED2020202020202020202020202020202020202020202020202020202020BCF6B7AE0A1B33281B61011B45001D21002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A1B33281B61001B45011D2101382EC5A9B8AEBDBAC7C7B9DD2FBEE7B3E4B9DD202B20C4DDB6F32035303020202020202020310D0A6D6C202BC4A1C5B2B9AB2BBCF8BBEC0A1B33281B61001B45011D2101B9E8B4DEC6C120202020202020202020202020202020202020202020202020202020202020310A1B33281B61011B45001D21003D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D3D0A1B33281B61001B45001D21000A1B33281B61001B45001D21000A1B33281B61001B45001D21000A1B33281B61001B45001D21000A1B33281B61001B45001D21000A1D56001B321B40"); 
            ParserOrderInputBaemin parser = new ParserOrderInputBaemin();
            ObjOrder recv_order = parser.parsingInputOrderRawData(recv_raw_data, recv_raw_data.Length);
            */

            // 파싱 테스트
            //byte[] recv_raw_data = KnUtil.HexStringToByteArray("1B40BBE7C0FCB0E1C1A620BFA9BACE3A205820B8B8B3AABCADB0E1C1A620C4ABB5E520205BB0EDB0B4BFEB5D0A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB9E8B4DEC1D6BCD2203A0A0DB0E6B1E220C0C7C1A4BACEBDC320C0C7C1A4BACEB5BF203231332D313120B0C5BACFC0CCBBE7B9ABBDC70A0D28B5B5B7CEB8ED2920B0E6B1E220C0C7C1A4BACEBDC320C6F2C8ADB7CE353531B9F8B1E6203320B0C50A0DBACFC0CCBBE7B9ABBDC70A0DBFACB6F4C3B3203A203031302D383230352D303630340A0D1D2400001D7630302C0003000A0DB9E8B4DEC1D6BCD2203A20B0E6B1E220C0C7C1A4BACEBDC320C0C7C1A4BACEB5BF203231332D3131200A0DB0C5BACFC0CC20C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A203031302D383230352D303630340A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D001D2400001D7630302C0003000A0DB9E8B4DEC1D6BCD2203A20B0E6B1E220C0C7C1A4BACEBDC320C0C7C1A4BACEB5BF203231332D3131200A0DB0C5BACFC0CC20C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A203031302D383230352D303630340A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB8DEB4BA202020202020202020202020202020202020202020202020BCF6B7AE202020202020B1DDBED70A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB8C6B9DDBCAE20B1B8BFEEC1B7B9DF28E1B329202020202020202020202031202020202032362C3030300A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DC7D5B0E8203A20202020202020202020202020202020202020202020202020202032362C30303020BFF80A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DC1D6B9AEBEF7BCD2203A20BEC6B0D5C1B7B9DF0A0DC1D6B9AEC0CFBDC3203A20323031382D31322D313128C8AD292031333A35340A0DC1D6B9AEB9F8C8A3203A204230394B3030353346560A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB9E8B4DEC1D6B9AEC0B8B7CE20C0CEC7D120BAD2C6EDC0CCB3AA20B0B3C0CEC1A4BAB820B0FCB7C30A0DC7C7C7D820B5EEC0BB20B0DEC0B8BCCCC0BB20B0E6BFEC0A0DB0EDB0B4BEC8BDC9BCBEC5CD28313630302D3938383029B7CE20BFACB6F4C1D6BCBCBFE42E0A0D200A0D200A0D200A0D200A0D200A0D1B69BBE7C0FCB0E1C1A620BFA9BACE3A205820B8B8B3AABCADB0E1C1A620C4ABB5E520205BB8C5C0E5BFEB5D0A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB9E8B4DEC1D6BCD2203A0A0DB0E6B1E220C0C7C1A4BACEBDC320C0C7C1A4BACEB5BF203231332D313120B0C5BACFC0CCBBE7B9ABBDC70A0D28B5B5B7CEB8ED2920B0E6B1E220C0C7C1A4BACEBDC320C6F2C8ADB7CE353531B9F8B1E6203320B0C50A0DBACFC0CCBBE7B9ABBDC70A0DBFACB6F4C3B3203A203031302D383230352D303630340A0D1D2400001D7630302C0003000A0DB9E8B4DEC1D6BCD2203A20B0E6B1E220C0C7C1A4BACEBDC320C0C7C1A4BACEB5BF203231332D3131200A0DB0C5BACFC0CC20C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A203031302D383230352D303630340A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D001D2400001D7630302C0003000A0DB9E8B4DEC1D6BCD2203A20B0E6B1E220C0C7C1A4BACEBDC320C0C7C1A4BACEB5BF203231332D3131200A0DB0C5BACFC0CC20C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A203031302D383230352D303630340A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB8DEB4BA202020202020202020202020202020202020202020202020BCF6B7AE202020202020B1DDBED70A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB8C6B9DDBCAE20B1B8BFEEC1B7B9DF28E1B329202020202020202020202031202020202032362C3030300A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DC7D5B0E8203A20202020202020202020202020202020202020202020202020202032362C30303020BFF80A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB0EDB0B4C1A4BAB8B8A620B9E8B4DEB8F1C0FB20BFDC20BBE7BFEBC7CFB0C5B3AA20BAB8B0FC2C20B0F80A0DB0B3C7D220B0E6BFEC20B9FDC0FBC3B3B9FAC0BB20B9DEC0BB20BCF620C0D6BDC0B4CFB4D92E0A0D200A0D200A0D200A0D200A0D200A0D1B69");
            //m_mgr_order_input.sendTestData("COM12", recv_raw_data, 0, recv_raw_data.Length);

            byte[] recv_raw_data = KnUtil.HexStringToByteArray("1B40BBE7C0FCB0E1C1A620BFA9BACE3A204F20202020202020202020202020202020205BB0EDB0B4BFEB5D0A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1D2400001D7630302C0004000A0D1B61B9E8B4DEC1D6BCD2203A20B0E6B3B220B1E8C7D8BDC320B3BBB5BF20313132352D3420C7F6B4EB330A0DC2F7333033B5BF343020C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A2028BEC8BDC9B9F8C8A3293035302D373833372D373634320A0D2ABEC8BDC9B9F8C8A3B4C220C1D6B9AEC1A2BCF620C8C420C3D6B4EB2033BDC3B0A320C0AFC8BF2A0A2D2D2D2D2D2D2D2D2D2D2D2D2D2DD2B9E8B4DEC1D6BCD2203A0A0DB0E6B3B220B1E8C7D8BDC320B3BBB5BF20313132352D3420C7F6B4EB33C2F7333033B5BF343035C8A3200A0D28B5B5B7CEB8ED2920B0E6B3B220B1E8C7D8BDC320B0E6BFF8B7CE20323120C7F6B4EB33C2F73330330A0DB5BF343035C8A30A0DBFACB6F4C3B3203A2028BEC8BDC9B9F8C8A3293035302D373833372D373634320A0D2ABEC8BDC9B9F8C8A3B4C220C1D6B9AEC1A2BCF620C8C420C3D6B4EB2033BDC3B0A320C0AFC8BF2A0A0D1D2400001D7630302C0004000A0DB9E8B4DEC1D6BCD2203A20B0E6B3B220B1E8C7D8BDC320B3BBB5BF20313132352D3420C7F6B4EB33C2F70A0D333033B5BF343020C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A2028BEC8BDC9B9F8C8A3293035302D373833372D373634320A0D2ABEC8BDC9B9F8C8A3B4C220C1D6B9AEC1A2BCF620C8C420C3D6B4EB2033BDC3B0A320C0AFC8BF2A0A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2DD21D2400001D7630302C0004000A0DB9E8B4DEC1D6BCD2203A20B0E6B3B220B1E8C7D8BDC320B3BBB5BF20313132352D3420C7F6B4EB33C2F70A0D333033B5BF343020C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A2028BEC8BDC9B9F8C8A3293035302D373833372D373634320A0D2ABEC8BDC9B9F8C8A3B4C220C1D6B9AEC1A2BCF620C8C420C3D6B4EB2033BDC3B0A320C0AFC8BF2A0A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2DD22D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB8DEB4BA202020202020202020202020202020202020202020202020BCF6B7AE202020202020B1DDBED70A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB0A3C1F6C4A1C5B220B9DD2028BBC029202020202020202020202020202031202020202031392C3930300A0D202BBEE7B3E420B9DD200A0DB6B1BABAC0CC20202020202020202020202020202020202020202020202031202020202020352C3030300A0DB9E8B4DEC6C120202020202020202020202020202020202020202020202020202020202020312C3030300A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DC7D5B0E8203A20202020202020202020202020202020202020202020202020202032352C39303020BFF80A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DC1D6B9AEBEF7BCD2203A203630B0E8C4A1C5B220BACFBACEB3BBBFDCC1A10A0DC1D6B9AEC0CFBDC3203A20323031392D30342D313228B1DD292031373A32390A0DC1D6B9AEB9F8C8A3203A2042304359303039394C310A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB9E8B4DEC1D6B9AEC0B8B7CE20C0CEC7D120BAD2C6EDC0CCB3AA20B0B3C0CEC1A4BAB820B0FCB7C30A0DC7C7C7D820B5EEC0BB20B0DEC0B8BCCCC0BB20B0E6BFEC0A0DB0EDB0B4BEC8BDC9BCBEC5CD28313630302D3938383029B7CE20BFACB6F4C1D6BCBCBFE42E0A0D200A0D200A0D200A0D1B69BBE7C0FCB0E1C1A620BFA9BACE3A204F20202020202020202020202020202020205BB8C5C0E5BFEB5D0A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1D2400001D7630302C0004000A0D1B61B9E8B4DEC1D6BCD2203A20B0E6B3B220B1E8C7D8BDC320B3BBB5BF20313132352D3420C7F6B4EB330A0DC2F7333033B5BF343020C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A2028BEC8BDC9B9F8C8A3293035302D373833372D373634320A0D2ABEC8BDC9B9F8C8A3B4C220C1D6B9AEC1A2BCF620C8C420C3D6B4EB2033BDC3B0A320C0AFC8BF2A0A2D2D2D2D2D2D2D2D2D2D2D2D2D2DD2B9E8B4DEC1D6BCD2203A0A0DB0E6B3B220B1E8C7D8BDC320B3BBB5BF20313132352D3420C7F6B4EB33C2F7333033B5BF343035C8A3200A0D28B5B5B7CEB8ED2920B0E6B3B220B1E8C7D8BDC320B0E6BFF8B7CE20323120C7F6B4EB33C2F73330330A0DB5BF343035C8A30A0DBFACB6F4C3B3203A2028BEC8BDC9B9F8C8A3293035302D373833372D373634320A0D2ABEC8BDC9B9F8C8A3B4C220C1D6B9AEC1A2BCF620C8C420C3D6B4EB2033BDC3B0A320C0AFC8BF2A0A0D1D2400001D7630302C0004000A0DB9E8B4DEC1D6BCD2203A20B0E6B3B220B1E8C7D8BDC320B3BBB5BF20313132352D3420C7F6B4EB33C2F70A0D333033B5BF343020C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A2028BEC8BDC9B9F8C8A3293035302D373833372D373634320A0D2ABEC8BDC9B9F8C8A3B4C220C1D6B9AEC1A2BCF620C8C420C3D6B4EB2033BDC3B0A320C0AFC8BF2A0A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2DD21D2400001D7630302C0004000A0DB9E8B4DEC1D6BCD2203A20B0E6B3B220B1E8C7D8BDC320B3BBB5BF20313132352D3420C7F6B4EB33C2F70A0D333033B5BF343020C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A2028BEC8BDC9B9F8C8A3293035302D373833372D373634320A0D2ABEC8BDC9B9F8C8A3B4C220C1D6B9AEC1A2BCF620C8C420C3D6B4EB2033BDC3B0A320C0AFC8BF2A0A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2DD22D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB8DEB4BA202020202020202020202020202020202020202020202020BCF6B7AE202020202020B1DDBED70A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB0A3C1F6C4A1C5B220B9DD2028BBC029202020202020202020202020202031202020202031392C3930300A0D202BBEE7B3E420B9DD200A0DB6B1BABAC0CC20202020202020202020202020202020202020202020202031202020202020352C3030300A0DB9E8B4DEC6C120202020202020202020202020202020202020202020202020202020202020312C3030300A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DC7D5B0E8203A20202020202020202020202020202020202020202020202020202032352C39303020BFF80A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB0EDB0B4C1A4BAB8B8A620B9E8B4DEB8F1C0FB20BFDC20BBE7BFEBC7CFB0C5B3AA20BAB8B0FC2C20B0F80A0DB0B3C7D220B0E6BFEC20B9FDC0FBC3B3B9FAC0BB20B9DEC0BB20BCF620C0D6BDC0B4CFB4D92E0A0D200A0D200A0D200A0D1B69");
            m_mgr_order_input.sendTestData("COM12", recv_raw_data, 0, recv_raw_data.Length);

            /* 배민출력테스트
            byte[] recv_raw_data = TsUtil.HexStringToByteArray("1B401D427920BBE7C0FCB0E1C1A620BFA9BACE3A204F201D42202020202020202020202020202020205BB0EDB0B4BFEB5D0A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1D2400001D7630302C0003000A0DB9E8B4DEC1D6BCD2203A20BACEBBEA20C7D8BFEEB4EBB1B820BFECB5BF203338372D3120C8ADB8B2BAF40A0DB6F3333032C8A320C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A203031302D343533302D363530330A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D001B4501B9E8B4DEC1D6BCD2203A0A0D1B4500BACEBBEA20C7D8BFEEB4EBB1B820BFECB5BF203338372D3120C8ADB8B2BAF4B6F3333032C8A30A0D28B5B5B7CEB8ED2920BACEBBEA20C7D8BFEEB4EBB1B820BFECB5BF31B7CE3731B9F8B1E620323020C8AD0A0DB8B2BAF4B6F3333032C8A30A0DBFACB6F4C3B3203A203031302D343533302D363530330A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1B4501BFE4C3BBBBE7C7D70A0D1B4500B8AEBAE4C0CCBAA5C6AE203A20BFC0BAECBDBAC6C4B0D4C6BC0A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB8DEB4BA202020202020202020202020202020202020202020202020BCF6B7AE202020202020B1DDBED70A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1B4501B0EDB1B8B8B6C6FEB5E0284D29202020202020202020202020202020202031202020202031362C3930300A0D1B4500202BC8E6B9CC200A0D1B4501B9E8B4DEC6C120202020202020202020202020202020202020202020202020202020202020312C3030300A0D1B45002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1B4501C7D5B0E8203A20202020202020202020202020202020202020202020202020202031372C39303020BFF80A0D1B45002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DC1D6B9AEBEF7BCD2203A20C7C7C0DABFACC7D520C7D8BFEEB4EBC1A10A0DC1D6B9AEC0CFBDC3203A20323031382D31312D323228B8F1292031363A32330A0DC1D6B9AEB9F8C8A3203A20423039313030355A4E530A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB9E8B4DEC1D6B9AEC0B8B7CE20C0CEC7D120BAD2C6EDC0CCB3AA20B0B3C0CEC1A4BAB820B0FCB7C30A0DC7C7C7D820B5EEC0BB20B0DEC0B8BCCCC0BB20B0E6BFEC0A0DB0EDB0B4BEC8BDC9BCBEC5CD28313630302D3938383029B7CE20BFACB6F4C1D6BCBCBFE42E0A0D200A0D200A0D200A0D200A0D1B691D427920BBE7C0FCB0E1C1A620BFA9BACE3A204F201D42202020202020202020202020202020205BB8C5C0E5BFEB5D0A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1D2400001D7630302C0003000A0DB9E8B4DEC1D6BCD2203A20BACEBBEA20C7D8BFEEB4EBB1B820BFECB5BF203338372D3120C8ADB8B2BAF40A0DB6F3333032C8A320C1D6B9AE20C1A4BAB8B4C220B0EDB0B4C0C720B0B3C0CE20C1A4BAB80A0DBFACB6F4C3B3203A203031302D343533302D363530330A2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D001B4501B9E8B4DEC1D6BCD2203A0A0D1B4500BACEBBEA20C7D8BFEEB4EBB1B820BFECB5BF203338372D3120C8ADB8B2BAF4B6F3333032C8A30A0D28B5B5B7CEB8ED2920BACEBBEA20C7D8BFEEB4EBB1B820BFECB5BF31B7CE3731B9F8B1E620323020C8AD0A0DB8B2BAF4B6F
            032C8A30A0DBFACB6F4C3B3203A203031302D343533302D363530330A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1B4501BFE4C3BBBBE7C7D70A0D1B4500B8AEBAE4C0CCBAA5C6AE203A20BFC0BAECBDBAC6C4B0D4C6BC0A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB8DEB4BA202020202020202020202020202020202020202020202020BCF6B7AE202020202020B1DDBED70A0D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1B4501B0EDB1B8B8B6C6FEB5E0284D29202020202020202020202020202020202031202020202031362C3930300A0D1B4500202BC8E6B9CC200A0D1B4501B9E8B4DEC6C120202020202020202020202020202020202020202020202020202020202020312C3030300A0D1B45002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0D1B4501C7D5B0E8203A20202020202020202020202020202020202020202020202020202031372C39303020BFF80A0D1B45002D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D0A0DB0EDB0B4C1A4BAB8B8A620B9E8B4DEB8F1C0FB20BFDC20BBE7BFEBC7CFB0C5B3AA20BAB8B0FC2C20B0F80A0DB0B3C7D220B0E6BFEC20B9FDC0FBC3B3B9FAC0BB20B9DEC0BB20BCF620C0D6BDC0B4CFB4D92E0A0D200A0D200A0D200A0D200A0D1B69");
            PrintOutputSerialPort port = new PrintOutputSerialPort("COM1", 9600);
            port.connectSerialPort();
            port.getSerialPort().Write(recv_raw_data, 0, recv_raw_data.Length);
            */

            // 배달천재 포트 파싱 테스트
            /*
            byte[] recv_raw_data = TsUtil.HexStringToByteArray("1B401B61311B2D321D2110B9E820B4DE20C0FC20C7A51B64011B21001B64011B61304E6F3A313831313038413030303120202020202020202020202031312D303820BFC0C8C420323A33381B21001B64011D2110A2CF203031302D313233342D3536373820202020201B21001B64011D211020202020202020202020202020B0EDB0B44E6F3A341B21001B64011D2101B0A1BBEAB5BF203134332D32302C20B0A1BBEAC7C7BEEEB4CFBAF42041505420BFACB8B320BFACB8B35B1B21001B64011D2101B0A1BBEAB5F0C1F6C5D031B7CE203139362C2028B0A1BBEAB5BF2C20BFA1C0CCBDBAC5D7C5A9B3EBC5B81B21001B64011D2101BFF63130C2F7295D1B21001B64011D2110C0DF20C7D8C1D6BCBCBFE41B21001B64012D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D1B21001B6401202020BBF3202020C7B0202020B8ED202020202020202020BCF6B7AE2020202020C6C7B8C5B1DDBED71B21001B64012D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D1B21001B64011D2101BAD2B0EDB1E2C7C7C0DA4C2020202020202020202020202020203520202020202020203130342C3530301B21001B64011D2101BAD2B0EDB1E2C7C7C0DA4C2020202020202020202020202020202034202020202020202038332C3630301B21001B64012D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D1B21001B64011D211020C7D520B0E83A202020203138382C31303020BFF81B21001B64012D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D1B21001B6401C1D8BAF1C7D220C0DCB5B7203A20312C39303020BFF81B21001B64012D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D2D1B21001B6401A1D9A1DA20BBF520B0EDB0B4B4D420A1DAA1D91B21001B64011B64041B69");
            m_mgr_order_input("COM12", recv_raw_data, 0, recv_raw_data.Length);
            */
        }

        private void btn_api_reg_list_today_ItemClick(object sender, ItemClickEventArgs e)
        {
            ObjConfigLastDeliveryRequestInfo last_request_config = new ObjConfigLastDeliveryRequestInfo();
            last_request_config.loadFromDevice();

            ObjConfigStoreApiRegInfo store_reg_info = new ObjConfigStoreApiRegInfo();
            store_reg_info.loadFromDevice(last_request_config.m_delivery_company_type);

            // 매핑되어 있는지 확인
            if (0 == store_reg_info.m_store_id.Length)
            {
                FormPopupNotify.Show(this.Owner, "먼저 가맹확인(상점매핑)을 진행 해 주십시오.", "알림");
                return;
            }

            // 배달대행 요청
            this.Cursor = Cursors.WaitCursor;
            int sel_delivery_open_api_type = KnUtil.formatInt32(last_request_config.m_delivery_company_type);
            OpenApiBase api_mgr = OpenApiBase.getOpenApiManager((OpenApiBase.OPEN_API_TYPE)sel_delivery_open_api_type);
            if (null != api_mgr)
            {
                ArrayList res_order_list = api_mgr.requestListToday(store_reg_info.m_store_id);
                this.Cursor = Cursors.Default;
                if (null != res_order_list)
                {
                    String sel_delivery_open_api_name = OpenApiBase.obtainDeliveryCompanyTypeName((OpenApiBase.OPEN_API_TYPE)sel_delivery_open_api_type);
                    String dlg_title = String.Format("당일 배달대행 요청 내역 ( {0} )", sel_delivery_open_api_name);

                    FormApiDeliveryOrderList sel_dlg = new FormApiDeliveryOrderList(this, dlg_title, res_order_list);
                    sel_dlg.ShowDialog(this);
                }
                else
                {
                    FormPopupNotify.Show(this.Owner, "해당 배달대행사에 등록된 내역이 없습니다.", "알림");
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                if (barCheckItem4.Checked)
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_bemin", 1);
                }
                else
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_bemin", 0);
                }
            }
            else
            {
                MessageBox.Show("디바이스 생성 오류");
                Application.Exit();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                if (barCheckItem5.Checked)
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_yogiyo", 1);
                }
                else
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_yogiyo", 0);
                }
            }
            else
            {
                MessageBox.Show("디바이스 생성 오류");
                Application.Exit();
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                if (barCheckItem6.Checked)
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_pos", 1);
                }
                else
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_pos", 0);
                }
            }
            else
            {
                MessageBox.Show("디바이스 생성 오류");
                Application.Exit();
            }
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                if (barCheckItem4.Checked)
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_bemin", 1);
                }
                else
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_bemin", 0);
                }
            }
            else
            {
                MessageBox.Show("디바이스 생성 오류");
                Application.Exit();
            }
        }

        private void barCheckItem5_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                if (barCheckItem5.Checked)
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_yogiyo", 1);
                }
                else
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_yogiyo", 0);
                }
            }
            else
            {
                MessageBox.Show("디바이스 생성 오류");
                Application.Exit();
            }
        }

        private void barCheckItem6_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                if (barCheckItem6.Checked)
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_pos", 1);
                }
                else
                {
                    device.writeConfigLong("CONFIG_ORDER_INPUT", "is_enroll_pos", 0);
                }
            }
            else
            {
                MessageBox.Show("디바이스 생성 오류");
                Application.Exit();
            }
        }

        private void barCheckItem7_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                if (barCheckItem7.Checked)
                {
                    device.writeConfigLong("FORM_MAIN", "optimization_mode", 1);
                }
                else
                {
                    device.writeConfigLong("FORM_MAIN", "optimization_mode", 0);
                }
            }
            else
            {
                MessageBox.Show("디바이스 생성 오류");
                Application.Exit();
            }
        }
    }
}
