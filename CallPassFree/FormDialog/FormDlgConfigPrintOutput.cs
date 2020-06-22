using Kons.ShopCallpass.AppMain;
using Kons.ShopCallpass.FormPopup;
using Kons.ShopCallpass.Object;
using Kons.ShopCallpass.Parser;
using Kons.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using Kons.ShopCallpass.Model;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormDlgConfigPrintOutput : FormDlgBase
    {
        public class Controls_ConfigUnit
        {
            public DevExpress.XtraEditors.CheckEdit chk_is_use;

            public DevExpress.XtraEditors.ComboBoxEdit cbx_print_connect_type;
            public DevExpress.XtraEditors.ComboBoxEdit cbx_print_port_num;
            public DevExpress.XtraEditors.ComboBoxEdit cbx_print_port_baud;

            public void setEnable(bool _enalbe)
            {
                cbx_print_connect_type.Enabled = _enalbe;
                cbx_print_port_num.Enabled = _enalbe;
                cbx_print_port_baud.Enabled = _enalbe;
            }
        }

        public PoolConfigPrintOutput m_config_list = null;
        public Controls_ConfigUnit[] m_controls_config_unit_list = null;

        // ---------------------------------------------------------- basic method
        //
        public FormDlgConfigPrintOutput(Form _parnet = null)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
                foreach (var printer in printerQuery.Get())
                {
                    var name = printer.GetPropertyValue("Name");
                    comboBox1.Items.Add(name.ToString());
                }

                ManagementObjectSearcher usbPrinterSearcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub");
                foreach (ManagementObject Port in usbPrinterSearcher.Get())
                {
                    if (Port.GetPropertyValue("DeviceID").ToString().Contains("USB\\VID"))
                    {
                        comboBox1.Items.Add(Port.GetPropertyValue("Name").ToString());
                    }
                }

                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (device != null)
                    {
                        if (comboBox1.Items[i].ToString() == device.readConfigString("CONFIG_PRINT_OUTPUT", "usb_printer_name"))
                        {
                            comboBox1.SelectedItem = comboBox1.Items[i];
                            break;
                        }
                    }
                }
                checkBox1.Checked = Int32.Parse(device.readConfigString("CONFIG_PRINT_OUTPUT", "is_use_usb_printer")) == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
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
                m_config_list = new PoolConfigPrintOutput(Kons.ShopCallpass.AppMain.AppCore.PRINT_OUTPUT_CONFIG_COUNT);
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
                        m_controls_config_unit_list[i].chk_is_use = ctr_chk_use_print_ouput_0;
                        m_controls_config_unit_list[i].cbx_print_connect_type = ctr_cbx_print_connect_type_0;
                        m_controls_config_unit_list[i].cbx_print_port_num = ctr_cbx_print_port_num_0;
                        m_controls_config_unit_list[i].cbx_print_port_baud = ctr_cbx_print_port_baud_0;
                        break;
                    case 1:
                        m_controls_config_unit_list[i].chk_is_use = ctr_chk_use_print_ouput_1;
                        m_controls_config_unit_list[i].cbx_print_connect_type = ctr_cbx_print_connect_type_1;
                        m_controls_config_unit_list[i].cbx_print_port_num = ctr_cbx_print_port_num_1;
                        m_controls_config_unit_list[i].cbx_print_port_baud = ctr_cbx_print_port_baud_1;
                        break;
                    case 2:
                        m_controls_config_unit_list[i].chk_is_use = ctr_chk_use_print_ouput_2;
                        m_controls_config_unit_list[i].cbx_print_connect_type = ctr_cbx_print_connect_type_2;
                        m_controls_config_unit_list[i].cbx_print_port_num = ctr_cbx_print_port_num_2;
                        m_controls_config_unit_list[i].cbx_print_port_baud = ctr_cbx_print_port_baud_2;
                        break;
                    case 3:
                        m_controls_config_unit_list[i].chk_is_use = ctr_chk_use_print_ouput_3;
                        m_controls_config_unit_list[i].cbx_print_connect_type = ctr_cbx_print_connect_type_3;
                        m_controls_config_unit_list[i].cbx_print_port_num = ctr_cbx_print_port_num_3;
                        m_controls_config_unit_list[i].cbx_print_port_baud = ctr_cbx_print_port_baud_3;
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
                    KnDevexpressFunc.setComboboxCommonStyle(des_controls.cbx_print_connect_type);
                    KnDevexpressFunc.setComboboxCommonStyle(des_controls.cbx_print_port_num);
                    KnDevexpressFunc.setComboboxCommonStyle(des_controls.cbx_print_port_baud);

                    ObjConfigPrintOutput sel_object = m_config_list.getObject(i);
                    if (null != sel_object)
                    {
                        des_controls.chk_is_use.Checked = (0 == sel_object.m_is_use ? false : true);
                        setSerialPortComboboxConnectType(des_controls.cbx_print_connect_type, sel_object.m_print_connect_type);
                        setSerialPortComboboxPortItems(des_controls.cbx_print_port_num, sel_object.m_print_port_num);
                        setSerialPortComboboxBaudItems(des_controls.cbx_print_port_baud, sel_object.m_print_port_baud.ToString());
                    }
                }
            }
        }

        private void setSerialPortComboboxConnectType(DevExpress.XtraEditors.ComboBoxEdit _control, string _default_key = null)
        {
            KnDevexpressFunc.ComboBoxAddItem(_control, "0", "시리얼");

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
            KnDevexpressFunc.ComboBoxAddItem(_control, "COM30", "COM30");

            ManagementObjectSearcher serialPortSearcher = new ManagementObjectSearcher("Select * from WIN32_SerialPort");
            foreach (ManagementObject Port in serialPortSearcher.Get())
            {
                KnDevexpressFunc.ComboBoxAddItem(_control, (string)Port.GetPropertyValue("DeviceId"), (string)Port.GetPropertyValue("Name"));
            }

            ManagementObjectSearcher win32PrinterSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            foreach (ManagementObject Port in win32PrinterSearcher.Get())
            {
                KnDevexpressFunc.ComboBoxAddItem(_control, (string)Port.GetPropertyValue("PortName"), (string)Port.GetPropertyValue("Name"));
            }

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
                    ObjConfigPrintOutput sel_obj = m_config_list.getObject(i);
                    if (null != sel_ctr)
                    {
                        sel_obj.m_is_use = (sel_ctr.chk_is_use.Checked ? 1 : 0);
                        sel_obj.m_print_connect_type = KnDevexpressFunc.ComboBoxGetSelectedItemKey(sel_ctr.cbx_print_connect_type);
                        sel_obj.m_print_port_num = KnDevexpressFunc.ComboBoxGetSelectedItemKey(sel_ctr.cbx_print_port_num);
                        sel_obj.m_print_port_baud = KnUtil.parseInt32(KnDevexpressFunc.ComboBoxGetSelectedItemKey(sel_ctr.cbx_print_port_baud));
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
                    }
                    else
                    {
                        des_controls.setEnable(false);
                    }
                }
            }
        }

        // ---------------------------------------------------------- parent event handler
        //
        public void saveDlgObjectData()
        {
            if (ctr_chk_use_print_ouput_0.Checked || ctr_chk_use_print_ouput_1.Checked || ctr_chk_use_print_ouput_2.Checked || 
                ctr_chk_use_print_ouput_3.Checked)
            {
                if (checkBox1.Checked)
                {
                    FormPopupNotify.Show(this.Owner, "시리얼 포트와 USB포트를 동시에 사용할 수 없습니다. 확인 해 주십시오.", "알림");
                    return;
                }
            }
            ModelAppDevice device = new ModelAppDevice();
            if (null != device)
            {
                if (comboBox1.SelectedItem != null)
                {
                    device.writeConfigLong("CONFIG_PRINT_OUTPUT", "is_use_usb_printer", checkBox1.Checked ? 1 : 0);
                    device.writeConfigString("CONFIG_PRINT_OUTPUT", "usb_printer_name", comboBox1.SelectedItem.ToString());
                }
                
            }

            // controls -> object
            setDlgControlDataToObjects();

            // save
            if (m_config_list.isVailedData())
            {
                m_config_list.saveObjectAll();
            }
            else
            {
                FormPopupNotify.Show(this.Owner, "프린트 포트가 중복으로 지정 되었습니다. 확인 해 주십시오.", "알림");
            }
        }

        // ---------------------------------------------------------- control event handler
        //
        private void ctr_chk_use_print_ouput_0_CheckedChanged(object sender, EventArgs e)
        {
            setDlgControlEnableState();
        }

        private void ctr_chk_use_print_ouput_1_CheckedChanged_1(object sender, EventArgs e)
        {
            setDlgControlEnableState();
        }

        private void ctr_chk_use_print_ouput_2_CheckedChanged_1(object sender, EventArgs e)
        {
            setDlgControlEnableState();
        }

        private void ctr_chk_use_print_ouput_3_CheckedChanged_1(object sender, EventArgs e)
        {
            setDlgControlEnableState();
        }
    }
}
