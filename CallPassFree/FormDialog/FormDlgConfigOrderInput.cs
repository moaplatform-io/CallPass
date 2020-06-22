using Kons.ShopCallpass.AppMain;
using Kons.ShopCallpass.FormPopup;
using Kons.ShopCallpass.Model;
using Kons.ShopCallpass.Object;
using Kons.ShopCallpass.Parser;
using Kons.ShopCallpass.Controller;
using Kons.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Management;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormDlgConfigOrderInput : FormDlgBase
    {
        public class Controls_ConfigUnit
        {
            public DevExpress.XtraEditors.CheckEdit chk_is_use;
            public DevExpress.XtraEditors.CheckEdit chk_is_install;

            public DevExpress.XtraEditors.ComboBoxEdit cbx_input_type;
            public DevExpress.XtraEditors.ComboBoxEdit cbx_input_port_num;
            public DevExpress.XtraEditors.ComboBoxEdit cbx_input_port_baud;
            public DevExpress.XtraEditors.ComboBoxEdit cbx_listen_port_num;
            public DevExpress.XtraEditors.ComboBoxEdit cbx_listen_port_baud;
            public DevExpress.XtraEditors.ComboBoxEdit cbx_conn_print_port_num;

            public void setEnable(bool _enalbe)
            {
                cbx_input_type.Enabled = _enalbe;
                cbx_input_port_num.Enabled = _enalbe;
                cbx_input_port_baud.Enabled = _enalbe;
                cbx_listen_port_num.Enabled = _enalbe;
                cbx_listen_port_baud.Enabled = _enalbe;
                cbx_conn_print_port_num.Enabled = _enalbe;
                
            }
        }

        public PoolConfigOrderInput m_config_list = null;
        public PoolConfigOrderInput m_prev_connect_port_list = null;
        public Controls_ConfigUnit[] m_controls_config_unit_list = null;

        // ---------------------------------------------------------- basic method
        //
        public FormDlgConfigOrderInput(Form _parnet = null)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();
        }

        private void FormDlg_Load(object sender, EventArgs e)
        {
            initDlgObjects();
            initDlgControls();

            // object -> control
            setDlgObjectDataToControls();

            // enable or disable
            setDlgControlEnableState();
        }

        // ---------------------------------------------------------- BaseForm override - init, set data ...
        //
        override protected void initDlgObjects()
        {
            if (null == m_config_list)
            {
                m_config_list = new PoolConfigOrderInput(Kons.ShopCallpass.AppMain.AppCore.ORDER_INPUT_CONFIG_COUNT);
                m_config_list.loadObjectAll();
            }

            // control containser
            m_controls_config_unit_list = new Controls_ConfigUnit[m_config_list.COUNT];

            // connectin config container
            for (int i = 0; i < m_config_list.COUNT; i++)
            {
                // 편의성을 위해 컨트롤 참조를 담아 둔다
                m_controls_config_unit_list[i] = new Controls_ConfigUnit();
                switch (i)
                {
                    case 0:
                        m_controls_config_unit_list[i].chk_is_use = ctr_chk_use_order_input_0;
                        m_controls_config_unit_list[i].chk_is_install = ctr_chk_install_order_input_0;
                        m_controls_config_unit_list[i].cbx_input_type = ctr_cbx_order_input_type_0;
                        m_controls_config_unit_list[i].cbx_input_port_num = ctr_cbx_order_input_port_num_0;
                        m_controls_config_unit_list[i].cbx_input_port_baud = ctr_cbx_order_input_port_baud_0;
                        m_controls_config_unit_list[i].cbx_listen_port_num = ctr_cbx_order_listen_port_num_0;
                        m_controls_config_unit_list[i].cbx_listen_port_baud = ctr_cbx_order_listen_port_baud_0;
                        m_controls_config_unit_list[i].cbx_conn_print_port_num = ctr_cbx_conn_print_port_num_0;
                        break;
                    case 1:
                        m_controls_config_unit_list[i].chk_is_use = ctr_chk_use_order_input_1;
                        m_controls_config_unit_list[i].chk_is_install = ctr_chk_install_order_input_1;
                        m_controls_config_unit_list[i].cbx_input_type = ctr_cbx_order_input_type_1;
                        m_controls_config_unit_list[i].cbx_input_port_num = ctr_cbx_order_input_port_num_1;
                        m_controls_config_unit_list[i].cbx_input_port_baud = ctr_cbx_order_input_port_baud_1;
                        m_controls_config_unit_list[i].cbx_listen_port_num = ctr_cbx_order_listen_port_num_1;
                        m_controls_config_unit_list[i].cbx_listen_port_baud = ctr_cbx_order_listen_port_baud_1;
                        m_controls_config_unit_list[i].cbx_conn_print_port_num = ctr_cbx_conn_print_port_num_1;
                        break;
                    case 2:
                        m_controls_config_unit_list[i].chk_is_use = ctr_chk_use_order_input_2;
                        m_controls_config_unit_list[i].chk_is_install = ctr_chk_install_order_input_2;
                        m_controls_config_unit_list[i].cbx_input_type = ctr_cbx_order_input_type_2;
                        m_controls_config_unit_list[i].cbx_input_port_num = ctr_cbx_order_input_port_num_2;
                        m_controls_config_unit_list[i].cbx_input_port_baud = ctr_cbx_order_input_port_baud_2;
                        m_controls_config_unit_list[i].cbx_listen_port_num = ctr_cbx_order_listen_port_num_2;
                        m_controls_config_unit_list[i].cbx_listen_port_baud = ctr_cbx_order_listen_port_baud_2;
                        m_controls_config_unit_list[i].cbx_conn_print_port_num = ctr_cbx_conn_print_port_num_2;
                        break;
                    case 3:
                        m_controls_config_unit_list[i].chk_is_use = ctr_chk_use_order_input_3;
                        m_controls_config_unit_list[i].chk_is_install = ctr_chk_install_order_input_3;
                        m_controls_config_unit_list[i].cbx_input_type = ctr_cbx_order_input_type_3;
                        m_controls_config_unit_list[i].cbx_input_port_num = ctr_cbx_order_input_port_num_3;
                        m_controls_config_unit_list[i].cbx_input_port_baud = ctr_cbx_order_input_port_baud_3;
                        m_controls_config_unit_list[i].cbx_listen_port_num = ctr_cbx_order_listen_port_num_3;
                        m_controls_config_unit_list[i].cbx_listen_port_baud = ctr_cbx_order_listen_port_baud_3;
                        m_controls_config_unit_list[i].cbx_conn_print_port_num = ctr_cbx_conn_print_port_num_3;
                        break;
                }
            }
        }

        override protected void initDlgControls()
        {
            for (int i = 0; i < m_controls_config_unit_list.Length; i++)
            {
                Controls_ConfigUnit des_controls = m_controls_config_unit_list[i];
                if (null != des_controls)
                {
                    KnDevexpressFunc.setComboboxCommonStyle(des_controls.cbx_input_type);
                    KnDevexpressFunc.setComboboxCommonStyle(des_controls.cbx_input_port_num);
                    KnDevexpressFunc.setComboboxCommonStyle(des_controls.cbx_input_port_baud);
                    KnDevexpressFunc.setComboboxCommonStyle(des_controls.cbx_listen_port_num);
                    KnDevexpressFunc.setComboboxCommonStyle(des_controls.cbx_listen_port_baud);
                    KnDevexpressFunc.setComboboxCommonStyle(des_controls.cbx_conn_print_port_num);

                    ObjConfigOrderInput sel_object = m_config_list.getObject(i);
                    if (null != sel_object)
                    {
                        // 지정된값 없으면 기본값 지정
                        if (0 == sel_object.m_input_port_num.Length || 0 == sel_object.m_listen_port_num.Length)
                        {
                            switch (i)
                            {
                                case 0:
                                    sel_object.m_input_type = ((int)ParserOrderInputBase.PARSER_TYPE.BAEMIN).ToString();
                                    sel_object.m_input_port_num = "COM11";
                                    sel_object.m_listen_port_num = "COM12";
                                    break;
                                case 1:
                                    sel_object.m_input_type = ((int)ParserOrderInputBase.PARSER_TYPE.OKPOS).ToString();
                                    sel_object.m_input_port_num = "COM13";
                                    sel_object.m_listen_port_num = "COM14";
                                    break;
                                case 2:
                                    sel_object.m_input_type = ((int)ParserOrderInputBase.PARSER_TYPE.DELGEN).ToString();
                                    sel_object.m_input_port_num = "COM15";
                                    sel_object.m_listen_port_num = "COM16";
                                    break;
                                case 3:
                                    sel_object.m_input_type = ((int)ParserOrderInputBase.PARSER_TYPE.POSFEED).ToString();
                                    sel_object.m_input_port_num = "COM17";
                                    sel_object.m_listen_port_num = "COM18";
                                    break;
                            }
                        }

                        // 불러온값 지정
                        des_controls.chk_is_use.Checked = (0 == sel_object.m_is_use ? false : true);
                        setSerialPortComboboxBrandItems(des_controls.cbx_input_type, sel_object.m_input_type);
                        setSerialPortComboboxPortItems(des_controls.cbx_input_port_num, sel_object.m_input_port_num);
                        setSerialPortComboboxBaudItems(des_controls.cbx_input_port_baud, sel_object.m_input_port_baud.ToString());
                        setSerialPortComboboxPortItems(des_controls.cbx_listen_port_num, sel_object.m_listen_port_num);
                        setSerialPortComboboxBaudItems(des_controls.cbx_listen_port_baud, sel_object.m_listen_port_baud.ToString());
                        //setSerialPortComboboxPortItems(des_controls.cbx_conn_print_port_num, sel_object.m_conn_print_port_num);
                        setPrinterSerialPortComboboxPortItems(des_controls.cbx_conn_print_port_num, sel_object.m_conn_print_port_num);
                    }
                }
            }
        }

        private void setSerialPortComboboxBrandItems(DevExpress.XtraEditors.ComboBoxEdit _control, string _default_key = null)
        {
            for (int i = 0; i < ParserOrderInputBase.PARSER_TYPE_LIST.Length; i++)
            {
                ParserOrderInputBase.PARSER_TYPE sel_type = ParserOrderInputBase.PARSER_TYPE_LIST[i];

                KnDevexpressFunc.ComboBoxAddItem(_control, ((int)sel_type).ToString(), ParserOrderInputBase.obtainParserName(sel_type));
            }

            if (null != _default_key)
            {
                KnDevexpressFunc.ComboBoxSetSelectByKey(_control, _default_key);
            }
        }

        private void setSerialPortComboboxPortItems(DevExpress.XtraEditors.ComboBoxEdit _control, string _default_key = null)
        {
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM1", "COM1");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM2", "COM2");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM3", "COM3");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM4", "COM4");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM5", "COM5");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM6", "COM6");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM7", "COM7");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM8", "COM8");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM9", "COM9");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM10", "COM10");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM11", "COM11");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM12", "COM12");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM13", "COM13");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM14", "COM14");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM15", "COM15");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM16", "COM16");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM17", "COM17");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM18", "COM18");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM19", "COM19");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM20", "COM20");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM21", "COM21");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM22", "COM22");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM23", "COM23");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM24", "COM24");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM25", "COM25");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM26", "COM26");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM27", "COM27");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM28", "COM28");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM29", "COM29");

            if (null != _default_key)
            {
                KnDevexpressFunc.ComboBoxSetSelectByKey(_control, _default_key);
            }
        }

        private void setPrinterSerialPortComboboxPortItems(DevExpress.XtraEditors.ComboBoxEdit _control, string _default_key = null)
        {
            //ManagementObjectSearcher serialPortSearcher = new ManagementObjectSearcher("Select * from WIN32_SerialPort");
            //ManagementObjectSearcher usbPrinterSearcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub");
            //ManagementObjectSearcher win32PrinterSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            //foreach (ManagementObject Port in serialPortSearcher.Get())
            //{
            //    KnDevexpressFunc.ComboBoxAddItem(_control, (string)Port.GetPropertyValue("DeviceId"), (string)Port.GetPropertyValue("Name"));
            //}

            KnDevexpressFunc.ComboBoxAddItem(_control, "COM1", "COM1");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM2", "COM2");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM3", "COM3");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM4", "COM4");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM5", "COM5");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM6", "COM6");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM7", "COM7");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM8", "COM8");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM9", "COM9");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM10", "COM10");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM11", "COM11");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM12", "COM12");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM30", "COM30");
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM31", "COM31");

            //foreach (ManagementObject Port in usbPrinterSearcher.Get())
            //{
            //    if (Port.GetPropertyValue("DeviceID").ToString().Contains("USB\\VID"))
            //    {
            //        KnDevexpressFunc.ComboBoxAddItem(_control, (string)Port.GetPropertyValue("DeviceId"), (string)Port.GetPropertyValue("Name"));
            //    }
            //}

            //foreach (ManagementObject Port in win32PrinterSearcher.Get())
            //{
            //    KnDevexpressFunc.ComboBoxAddItem(_control, (string)Port.GetPropertyValue("PortName"), (string)Port.GetPropertyValue("Name"));
            //}

            if (null != _default_key)
            {
                KnDevexpressFunc.ComboBoxSetSelectByKey(_control, _default_key);
            }
        }

        private void setSerialPortComboboxBaudItems(DevExpress.XtraEditors.ComboBoxEdit _control, string _default_key = null)
        {
            KnDevexpressFunc.ComboBoxAddItem(_control, "4800", "4,800");
            KnDevexpressFunc.ComboBoxAddItem(_control, "9600", "9,600");
            KnDevexpressFunc.ComboBoxAddItem(_control, "19200", "19,200");
            KnDevexpressFunc.ComboBoxAddItem(_control, "38400", "38,400");
            KnDevexpressFunc.ComboBoxAddItem(_control, "57600", "57,600");
            KnDevexpressFunc.ComboBoxAddItem(_control, "115200", "115,200");
            KnDevexpressFunc.ComboBoxAddItem(_control, "128000", "128,000");

            if (null != _default_key)
            {
                KnDevexpressFunc.ComboBoxSetSelectByKey(_control, _default_key);
            }
        }

        // object -> control
        override protected void setDlgObjectDataToControls()
        {

        }

        // control -> object
        override protected void setDlgControlDataToObjects()
        {
            for (int i = 0; i < m_controls_config_unit_list.Length; i++)
            {
                Controls_ConfigUnit sel_ctr = m_controls_config_unit_list[i];
                if (null != sel_ctr)
                {
                    ObjConfigOrderInput sel_obj = m_config_list.getObject(i);
                    if (null != sel_ctr)
                    {
                        sel_obj.m_is_use = (sel_ctr.chk_is_use.Checked ? 1 : 0);
                        sel_obj.m_input_type = KnDevexpressFunc.ComboBoxGetSelectedItemKey(sel_ctr.cbx_input_type);
                        sel_obj.m_input_port_num = KnDevexpressFunc.ComboBoxGetSelectedItemKey(sel_ctr.cbx_input_port_num);
                        sel_obj.m_input_port_baud = KnUtil.parseInt32(KnDevexpressFunc.ComboBoxGetSelectedItemKey(sel_ctr.cbx_input_port_baud));
                        sel_obj.m_listen_port_num = KnDevexpressFunc.ComboBoxGetSelectedItemKey(sel_ctr.cbx_listen_port_num);
                        sel_obj.m_listen_port_baud = KnUtil.parseInt32(KnDevexpressFunc.ComboBoxGetSelectedItemKey(sel_ctr.cbx_listen_port_baud));
                        sel_obj.m_conn_print_port_num = KnDevexpressFunc.ComboBoxGetSelectedItemKey(sel_ctr.cbx_conn_print_port_num);
                    }
                    else
                    {
                        sel_obj.m_is_use = 0;
                    }
                }
            }
        }

        // controls enable or disable
        private void setDlgControlEnableState()
        {
            Controls_ConfigUnit des_controls = null;
            for (int i = 0; i < m_controls_config_unit_list.Length; i++)
            {
                des_controls = m_controls_config_unit_list[i];
                if (null != des_controls)
                {
                    if (des_controls.chk_is_use.Checked)
                    {
                        des_controls.setEnable(true);
                        switch (i)
                        {
                            case 0:
                                button1.Enabled = true;
                                break;
                            case 1:
                                button2.Enabled = true;
                                break;
                            case 2:
                                button3.Enabled = true;
                                break;
                            case 3:
                                button4.Enabled = true;
                                break;

                        }
                    }
                    else
                    {
                        des_controls.setEnable(false);
                        switch (i)
                        {
                            case 0:
                                button1.Enabled = false;
                                break;
                            case 1:
                                button2.Enabled = false;
                                break;
                            case 2:
                                button3.Enabled = false;
                                break;
                            case 3:
                                button4.Enabled = false;
                                break;

                        }
                    }
                }
            }
        }

        // ---------------------------------------------------------- parent event handler
        //
        public void saveDlgObjectData()
        {
            #region
            //    m_prev_connect_port_list = new PoolConfigOrderInput(AppCore.ORDER_INPUT_CONFIG_COUNT);
            //    m_prev_connect_port_list.loadObjectAll();

            //    // controls -> object
            //    setDlgControlDataToObjects();

            //    bool isChange = false;

            //    for (int i = 0; i < m_config_list.m_list.Length; i++)
            //    {
            //        if (!(m_config_list.m_list[i].m_input_type == m_prev_connect_port_list.m_list[i].m_input_type &&
            //            m_config_list.m_list[i].m_is_use == m_prev_connect_port_list.m_list[i].m_is_use &&
            //            m_config_list.m_list[i].m_input_port_num == m_prev_connect_port_list.m_list[i].m_input_port_num &&
            //            m_config_list.m_list[i].m_input_port_baud == m_prev_connect_port_list.m_list[i].m_input_port_baud &&
            //            m_config_list.m_list[i].m_listen_port_num == m_prev_connect_port_list.m_list[i].m_listen_port_num &&
            //            m_config_list.m_list[i].m_listen_port_baud == m_prev_connect_port_list.m_list[i].m_listen_port_baud))
            //        {
            //            isChange = true;
            //        }
            //    }

            //    if (isChange)
            //    {
            //        Process process = new Process();
            //        process.StartInfo.FileName = "cmd.exe";
            //        process.StartInfo.RedirectStandardInput = true;
            //        process.StartInfo.RedirectStandardOutput = true;
            //        process.StartInfo.UseShellExecute = false;
            //        process.StartInfo.CreateNoWindow = true;
            //        process.Start();

            //        int recognize = -1;

            //        recognize = PcAppMain.Program.DownloadCheck();
            //        if (PcAppMain.Program.fileLocate != null)
            //        {
            //            switch (recognize)
            //            {
            //                case 1:
            //                    Utility.Utility.LogWrite("PcAppMain.Program.fileInfo.DirectoryName: " + PcAppMain.Program.fileLocate.Remove(PcAppMain.Program.fileLocate.Length - 11));
            //                    Utility.Utility.LogWrite("PcAppMain.Program.fileInfo: " + PcAppMain.Program.fileLocate);
            //                    process.StandardInput.WriteLine("cd " + PcAppMain.Program.fileLocate.Remove(PcAppMain.Program.fileLocate.Length - 11));
            //                    break;
            //                case 2:
            //                    Utility.Utility.LogWrite("PcAppMain.Program.fileInfo.DirectoryName: " + PcAppMain.Program.fileLocate.Remove(PcAppMain.Program.fileLocate.Length - 10));
            //                    Utility.Utility.LogWrite("PcAppMain.Program.fileInfo: " + PcAppMain.Program.fileLocate);
            //                    process.StandardInput.WriteLine("cd " + PcAppMain.Program.fileLocate.Remove(PcAppMain.Program.fileLocate.Length - 10));
            //                    break;
            //            }
            //        }

            //        PcAppMain.Program.DownloadCheck();

            //        // save
            //        if (PcAppMain.Program.fileLocate != null)
            //        {
            //            if (m_config_list.isVailedData())
            //            {
            //                if (m_config_list.isRepeatValue())
            //                {
            //                    if (m_config_list.isOtherDeviceUse(m_prev_connect_port_list))
            //                    {
            //                        Delay(500);

            //                        ControllerOrderInput controllerOrderInput = new ControllerOrderInput();
            //                        controllerOrderInput.disconnectSerialPorts();

            //                        Delay(500);

            //                        for (int i = 0; i < m_config_list.m_list.Length; i++)
            //                        {
            //                            Delay(1000);
            //                            process.StandardInput.WriteLine('\u0022' + PcAppMain.Program.fileLocate + '\u0022' + " remove " + i.ToString());
            //                        }

            //                        Delay(10000);

            //                        for (int i = 0; i < m_config_list.m_list.Length; i++)
            //                        {
            //                            if (m_config_list.m_list[i].IS_USE)
            //                            {
            //                                Delay(2000);
            //                                process.StandardInput.WriteLine('\u0022' + PcAppMain.Program.fileLocate + '\u0022' + " install PortName=" + m_config_list.m_list[i].m_input_port_num + " PortName=" + m_config_list.m_list[i].m_listen_port_num);
            //                            }
            //                        }

            //                        //Delay(80000);

            //                        bool isAllSetting = false;
            //                        int count = 0;
            //                        while (!isAllSetting && count < 80)
            //                        {
            //                            Delay(5000);
            //                            string[] otherDevicePortList = SerialPort.GetPortNames();

            //                            bool toggle = true;

            //                            for (int i = 0; i < m_config_list.m_list.Length; i++)
            //                            {
            //                                bool isInputPortSetting = false;
            //                                bool isListenPortSetting = false;
            //                                for (int j = 0; j < otherDevicePortList.Length; j++)
            //                                {
            //                                    if (m_config_list.m_list[i].m_input_port_num == otherDevicePortList[j])
            //                                    {
            //                                        isInputPortSetting = true;
            //                                    }
            //                                    if (m_config_list.m_list[i].m_listen_port_num == otherDevicePortList[j])
            //                                    {
            //                                        isListenPortSetting = true;
            //                                    }
            //                                }
            //                                if (!isInputPortSetting || !isListenPortSetting)
            //                                {
            //                                    toggle = false;
            //                                    break;
            //                                }
            //                            }
            //                            if (toggle)
            //                            {
            //                                isAllSetting = true;
            //                                Utility.Utility.LogWrite("포트 세팅 완료");
            //                            }
            //                            count++;
            //                        }

            //                        m_config_list.saveObjectAll();
            //                        controllerOrderInput.connectSerialPorts();
            //                        Delay(500);

            //                        FormPopupNotify.Show(this.Owner, "설정한 값으로 실행환경을 저장 하였습니다.", "알림");
            //                    }
            //                    else
            //                    {
            //                        string[] otherDevicePortList = SerialPort.GetPortNames();

            //                        Utility.Utility.LogWrite("다른 장치에서 사용중인 포트 목록 : ");
            //                        for (int i = 0; i < otherDevicePortList.Length; i++)
            //                        {
            //                            Utility.Utility.LogWrite(otherDevicePortList[i] + " ");
            //                        }
            //                        Utility.Utility.LogWrite("\r\n");

            //                        string str = null;
            //                        Utility.Utility.LogWrite("otherDevicePortList.Length : " + otherDevicePortList.Length);
            //                        for (int i = 0; i < otherDevicePortList.Length; i++)
            //                        {
            //                            str += otherDevicePortList[i];
            //                            str += " ";
            //                        }
            //                        FormPopupNotify.Show(this.Owner, str + " 는 이미 다른 장치에서 사용중인 포트입니다. 확인 해 주십시오.", "알림");
            //                    }
            //                }
            //                else
            //                {
            //                    FormPopupNotify.Show(this.Owner, "중복 선택된 포트가 있습니다. 확인 해 주십시오.", "알림");
            //                }
            //            }
            //            else
            //            {
            //                FormPopupNotify.Show(this.Owner, "COM포트가 중복으로 지정 되었습니다. 확인 해 주십시오.", "알림");
            //            }
            //        }
            //        else
            //        {
            //            FormPopupNotify.Show(this.Owner, "포트 관리자 (com0com) 설치가 완료되었는지 확인해 주십시오.\n 바탕화면에 자동으로 다운로드 된 포트 관리자 (com0com)를 압축 해제 후 설치 해 주십시오.", "알림");
            //        }
            //    }
            //    else
            //    {
            //        try
            //        {
            //            ModelAppDevice device = new ModelAppDevice();
            //            if (null != device)
            //            {
            //                for (int j = 0; j < 4; j++)
            //                {
            //                    device.writeConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + j.ToString(), m_config_list.m_list[j].m_conn_print_port_num);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Utility.Utility.LogWrite("saveDlgObjectData에서 예외발생 : " + ex.Message);
            //            AppMain.AppCore.detectException(ex.Message);
            //        }
            //    }
            #endregion
            ModelAppDevice device = new ModelAppDevice();
            if (device != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (ctr_chk_use_order_input_0.Checked)
                            {
                                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString(), 1);
                            }
                            else
                            {
                                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString(), 0);
                            }
                            device.writeConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString(), Utility.Utility.StringToInputType(ctr_cbx_order_input_type_0.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "input_port_num_" + i.ToString(), ctr_cbx_order_input_port_num_0.Text);
                            device.writeConfigLong("CONFIG_ORDER_INPUT", "input_port_baud_" + i.ToString(), KnUtil.formatInt32FromMoneyFormat(ctr_cbx_order_input_port_baud_0.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString(), ctr_cbx_order_listen_port_num_0.Text);
                            device.writeConfigLong("CONFIG_ORDER_INPUT", "listen_port_baud_" + i.ToString(), KnUtil.formatInt32FromMoneyFormat(ctr_cbx_order_listen_port_baud_0.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + i.ToString(), ctr_cbx_conn_print_port_num_0.Text);
                            break;
                        case 1:
                            if (ctr_chk_use_order_input_1.Checked)
                            {
                                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString(), 1);
                            }
                            else
                            {
                                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString(), 0);
                            }
                            device.writeConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString(), Utility.Utility.StringToInputType(ctr_cbx_order_input_type_1.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "input_port_num_" + i.ToString(), ctr_cbx_order_input_port_num_1.Text);
                            device.writeConfigLong("CONFIG_ORDER_INPUT", "input_port_baud_" + i.ToString(), KnUtil.formatInt32FromMoneyFormat(ctr_cbx_order_input_port_baud_1.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString(), ctr_cbx_order_listen_port_num_1.Text);
                            device.writeConfigLong("CONFIG_ORDER_INPUT", "listen_port_baud_" + i.ToString(), KnUtil.formatInt32FromMoneyFormat(ctr_cbx_order_listen_port_baud_1.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + i.ToString(), ctr_cbx_conn_print_port_num_1.Text);
                            break;
                        case 2:
                            if (ctr_chk_use_order_input_2.Checked)
                            {
                                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString(), 1);
                            }
                            else
                            {
                                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString(), 0);
                            }
                            device.writeConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString(), Utility.Utility.StringToInputType(ctr_cbx_order_input_type_2.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "input_port_num_" + i.ToString(), ctr_cbx_order_input_port_num_2.Text);
                            device.writeConfigLong("CONFIG_ORDER_INPUT", "input_port_baud_" + i.ToString(), KnUtil.formatInt32FromMoneyFormat(ctr_cbx_order_input_port_baud_2.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString(), ctr_cbx_order_listen_port_num_2.Text);
                            device.writeConfigLong("CONFIG_ORDER_INPUT", "listen_port_baud_" + i.ToString(), KnUtil.formatInt32FromMoneyFormat(ctr_cbx_order_listen_port_baud_2.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + i.ToString(), ctr_cbx_conn_print_port_num_2.Text);
                            break;
                        case 3:
                            if (ctr_chk_use_order_input_3.Checked)
                            {
                                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString(), 1);
                            }
                            else
                            {
                                device.writeConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString(), 0);
                            }
                            device.writeConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString(), Utility.Utility.StringToInputType(ctr_cbx_order_input_type_3.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "input_port_num_" + i.ToString(), ctr_cbx_order_input_port_num_3.Text);
                            device.writeConfigLong("CONFIG_ORDER_INPUT", "input_port_baud_" + i.ToString(), KnUtil.formatInt32FromMoneyFormat(ctr_cbx_order_input_port_baud_3.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString(), ctr_cbx_order_listen_port_num_3.Text);
                            device.writeConfigLong("CONFIG_ORDER_INPUT", "listen_port_baud_" + i.ToString(), KnUtil.formatInt32FromMoneyFormat(ctr_cbx_order_listen_port_baud_3.Text));
                            device.writeConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + i.ToString(), ctr_cbx_conn_print_port_num_3.Text);
                            break;
                    }
                }
            }
            else
            {
                FormPopupNotify.Show(this.Owner, "디바이스 생성 오류", "알림");
            }
            ControllerOrderInput controllerOrderInput = new ControllerOrderInput();
            controllerOrderInput.connectSerialPorts();
        }

        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }
        // ---------------------------------------------------------- control event handler
        //
        private void ctr_chk_use_order_input_0_CheckStateChanged(object sender, EventArgs e)
        {
            setDlgControlEnableState();
        }

        private void ctr_chk_use_order_input_1_CheckedChanged(object sender, EventArgs e)
        {
            setDlgControlEnableState();
        }

        private void ctr_chk_use_order_input_2_CheckedChanged(object sender, EventArgs e)
        {
            setDlgControlEnableState();
        }

        private void ctr_chk_use_order_input_3_CheckedChanged(object sender, EventArgs e)
        {
            setDlgControlEnableState();
        }

        private void ctr_cbx_order_input_type_3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void CreateVirtualSerialPort(int num)
        {
            setDlgControlDataToObjects();

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            
            int recognize = -1;

            recognize = CallpassPcAppMain.Program.DownloadCheck();
            if (CallpassPcAppMain.Program.fileLocate != null)
            {
                switch (recognize)
                {
                    case 1:
                        Utility.Utility.LogWrite("PcAppMain.Program.fileInfo.DirectoryName: " + CallpassPcAppMain.Program.fileLocate.Remove(CallpassPcAppMain.Program.fileLocate.Length - 11));
                        Utility.Utility.LogWrite("PcAppMain.Program.fileInfo: " + CallpassPcAppMain.Program.fileLocate);
                        process.StandardInput.WriteLine("cd " + CallpassPcAppMain.Program.fileLocate.Remove(CallpassPcAppMain.Program.fileLocate.Length - 11));
                        break;
                    case 2:
                        Utility.Utility.LogWrite("PcAppMain.Program.fileInfo.DirectoryName: " + CallpassPcAppMain.Program.fileLocate.Remove(CallpassPcAppMain.Program.fileLocate.Length - 10));
                        Utility.Utility.LogWrite("PcAppMain.Program.fileInfo: " + CallpassPcAppMain.Program.fileLocate);
                        process.StandardInput.WriteLine("cd " + CallpassPcAppMain.Program.fileLocate.Remove(CallpassPcAppMain.Program.fileLocate.Length - 10));
                        break;
                }
                
                switch  (num)
                {
                    case 0:
                        process.StandardInput.WriteLine('\u0022' + CallpassPcAppMain.Program.fileLocate + '\u0022' + " install PortName=" + ctr_cbx_order_input_port_num_0.Text + " PortName=" + ctr_cbx_order_listen_port_num_0.Text);
                        Utility.Utility.LogWrite("ctr_cbx_order_input_port_num_0.Text : " + ctr_cbx_order_input_port_num_0.Text + " ctr_cbx_order_listen_port_num_0.Text : " + ctr_cbx_order_listen_port_num_0.Text);
                        break;
                    case 1:
                        process.StandardInput.WriteLine('\u0022' + CallpassPcAppMain.Program.fileLocate + '\u0022' + " install PortName=" + ctr_cbx_order_input_port_num_1.Text + " PortName=" + ctr_cbx_order_listen_port_num_1.Text);
                        Utility.Utility.LogWrite("ctr_cbx_order_input_port_num_1.Text : " + ctr_cbx_order_input_port_num_1.Text + " ctr_cbx_order_listen_port_num_1.Text : " + ctr_cbx_order_listen_port_num_1.Text);
                        break;
                    case 2:
                        process.StandardInput.WriteLine('\u0022' + CallpassPcAppMain.Program.fileLocate + '\u0022' + " install PortName=" + ctr_cbx_order_input_port_num_2.Text + " PortName=" + ctr_cbx_order_listen_port_num_2.Text);
                        Utility.Utility.LogWrite("ctr_cbx_order_input_port_num_2.Text : " + ctr_cbx_order_input_port_num_2.Text + " ctr_cbx_order_listen_port_num_2.Text : " + ctr_cbx_order_listen_port_num_2.Text);
                        break;
                    case 3:
                        process.StandardInput.WriteLine('\u0022' + CallpassPcAppMain.Program.fileLocate + '\u0022' + " install PortName=" + ctr_cbx_order_input_port_num_3.Text + " PortName=" + ctr_cbx_order_listen_port_num_3.Text);
                        Utility.Utility.LogWrite("ctr_cbx_order_input_port_num_3.Text : " + ctr_cbx_order_input_port_num_3.Text + " ctr_cbx_order_listen_port_num_3.Text : " + ctr_cbx_order_listen_port_num_3.Text);
                        break;
                }               
            }
            else
            {
                FormPopupNotify.Show(this.Owner, "포트 관리자 (com0com) 설치가 완료되었는지 확인해 주십시오.\n 바탕화면에 자동으로 다운로드 된 포트 관리자 (com0com)를 압축 해제 후 설치 해 주십시오.", "알림");
            }
            process.Close();

            FormPopupNotify.Show(this.Owner, "다음 작업을 진행 전에 장치 관리자에서 com0com의 포트가 생성되었는지 확인하십시오", "알림");
        }

        private void ctr_chk_use_order_input_0_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateVirtualSerialPort(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateVirtualSerialPort(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CreateVirtualSerialPort(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CreateVirtualSerialPort(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            setDlgControlDataToObjects();

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            ControllerOrderInput controllerOrderInput = new ControllerOrderInput();
            controllerOrderInput.disconnectSerialPorts();

            Delay(500);

            int recognize = -1;

            recognize = CallpassPcAppMain.Program.DownloadCheck();
            if (CallpassPcAppMain.Program.fileLocate != null)
            {
                switch (recognize)
                {
                    case 1:
                        Utility.Utility.LogWrite("PcAppMain.Program.fileInfo.DirectoryName: " + CallpassPcAppMain.Program.fileLocate.Remove(CallpassPcAppMain.Program.fileLocate.Length - 11));
                        Utility.Utility.LogWrite("PcAppMain.Program.fileInfo: " + CallpassPcAppMain.Program.fileLocate);
                        process.StandardInput.WriteLine("cd " + CallpassPcAppMain.Program.fileLocate.Remove(CallpassPcAppMain.Program.fileLocate.Length - 11));
                        break;
                    case 2:
                        Utility.Utility.LogWrite("PcAppMain.Program.fileInfo.DirectoryName: " + CallpassPcAppMain.Program.fileLocate.Remove(CallpassPcAppMain.Program.fileLocate.Length - 10));
                        Utility.Utility.LogWrite("PcAppMain.Program.fileInfo: " + CallpassPcAppMain.Program.fileLocate);
                        process.StandardInput.WriteLine("cd " + CallpassPcAppMain.Program.fileLocate.Remove(CallpassPcAppMain.Program.fileLocate.Length - 10));
                        break;
                }

                for (int i = 0; i < m_config_list.m_list.Length; i++)
                {
                    Delay(1000);
                    process.StandardInput.WriteLine('\u0022' + CallpassPcAppMain.Program.fileLocate + '\u0022' + " remove " + i.ToString());
                }
                process.Close();

                ModelAppDevice device = new ModelAppDevice();
                if (device != null)
                {
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

                        switch (i)
                        {
                            case 0:
                                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString()) == 0)
                                {
                                    ctr_chk_use_order_input_0.EditValue = false;
                                }
                                else
                                {
                                    ctr_chk_use_order_input_0.EditValue = true;
                                }
                                ctr_cbx_order_input_type_0.Text = "[출력만사용]";
                                ctr_cbx_order_input_port_num_0.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "input_port_num_" + i.ToString());
                                ctr_cbx_order_input_port_baud_0.EditValue = device.readConfigLong("CONFIG_ORDER_INPUT", "input_port_baud_" + i.ToString());
                                ctr_cbx_order_listen_port_num_0.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString());
                                ctr_cbx_order_listen_port_baud_0.EditValue = device.readConfigLong("CONFIG_ORDER_INPUT", "listen_port_baud_" + i.ToString());
                                ctr_cbx_conn_print_port_num_0.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + i.ToString());

                                ctr_cbx_order_input_type_0.Enabled = false;
                                ctr_cbx_order_input_port_num_0.Enabled = false;
                                ctr_cbx_order_input_port_baud_0.Enabled = false;
                                ctr_cbx_order_listen_port_num_0.Enabled = false;
                                ctr_cbx_order_listen_port_baud_0.Enabled = false;
                                ctr_cbx_conn_print_port_num_0.Enabled = false;
                                button1.Enabled = false;
                                break;

                            case 1:
                                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString()) == 0)
                                {
                                    ctr_chk_use_order_input_1.EditValue = false;
                                }
                                else
                                {
                                    ctr_chk_use_order_input_1.EditValue = true;
                                }
                                ctr_cbx_order_input_type_1.Text = "[출력만사용]";
                                ctr_cbx_order_input_port_num_1.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "input_port_num_" + i.ToString());
                                ctr_cbx_order_input_port_baud_1.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "input_port_baud_" + i.ToString());
                                ctr_cbx_order_listen_port_num_1.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString());
                                ctr_cbx_order_listen_port_baud_1.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_baud_" + i.ToString());
                                ctr_cbx_conn_print_port_num_1.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + i.ToString());

                                ctr_cbx_order_input_type_1.Enabled = false;
                                ctr_cbx_order_input_port_num_1.Enabled = false;
                                ctr_cbx_order_input_port_baud_1.Enabled = false;
                                ctr_cbx_order_listen_port_num_1.Enabled = false;
                                ctr_cbx_order_listen_port_baud_1.Enabled = false;
                                ctr_cbx_conn_print_port_num_1.Enabled = false;
                                button2.Enabled = false;
                                break;

                            case 2:
                                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString()) == 0)
                                {
                                    ctr_chk_use_order_input_2.EditValue = false;
                                }
                                else
                                {
                                    ctr_chk_use_order_input_2.EditValue = true;
                                }
                                ctr_cbx_order_input_type_2.Text = "[출력만사용]";
                                ctr_cbx_order_input_port_num_2.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "input_port_num_" + i.ToString());
                                ctr_cbx_order_input_port_baud_2.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "input_port_baud_" + i.ToString());
                                ctr_cbx_order_listen_port_num_2.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString());
                                ctr_cbx_order_listen_port_baud_2.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_baud_" + i.ToString());
                                ctr_cbx_conn_print_port_num_2.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + i.ToString());

                                ctr_cbx_order_input_type_2.Enabled = false;
                                ctr_cbx_order_input_port_num_2.Enabled = false;
                                ctr_cbx_order_input_port_baud_2.Enabled = false;
                                ctr_cbx_order_listen_port_num_2.Enabled = false;
                                ctr_cbx_order_listen_port_baud_2.Enabled = false;
                                ctr_cbx_conn_print_port_num_2.Enabled = false;
                                button3.Enabled = false;
                                break;

                            case 3:
                                if (device.readConfigLong("CONFIG_ORDER_INPUT", "is_use_" + i.ToString()) == 0)
                                {
                                    ctr_chk_use_order_input_3.EditValue = false;
                                }
                                else
                                {
                                    ctr_chk_use_order_input_3.EditValue = true;
                                }
                                ctr_cbx_order_input_type_3.Text = "[출력만사용]";
                                ctr_cbx_order_input_port_num_3.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "input_port_num_" + i.ToString());
                                ctr_cbx_order_input_port_baud_3.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "input_port_baud_" + i.ToString());
                                ctr_cbx_order_listen_port_num_3.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString());
                                ctr_cbx_order_listen_port_baud_3.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_baud_" + i.ToString());
                                ctr_cbx_conn_print_port_num_3.EditValue = device.readConfigString("CONFIG_ORDER_INPUT", "conn_print_port_num_" + i.ToString());

                                ctr_cbx_order_input_type_3.Enabled = false;
                                ctr_cbx_order_input_port_num_3.Enabled = false;
                                ctr_cbx_order_input_port_baud_3.Enabled = false;
                                ctr_cbx_order_listen_port_num_3.Enabled = false;
                                ctr_cbx_order_listen_port_baud_3.Enabled = false;
                                ctr_cbx_conn_print_port_num_3.Enabled = false;
                                button4.Enabled = false;
                                break;
                        }
                    }
                }
                else
                {
                    FormPopupNotify.Show(this.Owner, "디바이스 생성 오류", "알림");
                }
            }
            else
            {
                FormPopupNotify.Show(this.Owner, "포트 관리자 (com0com) 설치가 완료되었는지 확인해 주십시오.\n 바탕화면에 자동으로 다운로드 된 포트 관리자 (com0com)를 압축 해제 후 설치 해 주십시오.", "알림");
            }

            FormPopupNotify.Show(this.Owner, "다음 작업을 진행 전에 장치 관리자에서 com0com의 모든 포트가 제거되었는지 확인하십시오", "알림");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ctr_cbx_conn_print_port_num_0_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
