using Kons.ShopCallpass.Model;
using Kons.TsLibrary;
using Kons.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.Object
{
    public class ObjConfigPrintOutput
    {
        private const string CONFIG_SECTION = "CONFIG_PRINT_OUTPUT";

        public bool IS_USE { get { return 1 == m_is_use; } }

        public int m_config_id; // key
        public int m_is_use;

        public String m_print_connect_type;
        public String m_print_port_num;
        public int m_print_port_baud;


        // ----------------------------------------------------------
        //
        public ObjConfigPrintOutput()
        {
            initObj();
        }

        public void initObj()
        {
            m_config_id = 0;
            m_is_use = 0;

            m_print_connect_type = "";
            m_print_port_num = "";
            m_print_port_baud = 0;
        }

        // --------------------------------------
        //
        public void setObj(ObjConfigPrintOutput _src)
        {
            m_config_id = _src.m_config_id;
            m_is_use = _src.m_is_use;

            m_print_connect_type = _src.m_print_connect_type;
            m_print_port_num = _src.m_print_port_num;
            m_print_port_baud = _src.m_print_port_baud;
        }

        // --------------------------------------
        //
        public bool loadFromDevice(int _obj_key)
        {
            // init
            initObj();

            // load
            bool is_success = false;
            try
            {
                // id 
                m_config_id = _obj_key;

                // id tag
                String id_tag = "_" + m_config_id.ToString();

                // load
                ModelAppDevice device = new ModelAppDevice();
                if (null != device)
                {
                    m_is_use = device.readConfigLong(CONFIG_SECTION, "is_use" + id_tag);

                    m_print_connect_type = device.readConfigString(CONFIG_SECTION, "print_connect_type" + id_tag);
                    m_print_port_num = device.readConfigString(CONFIG_SECTION, "input_port_num" + id_tag);
                    m_print_port_baud = device.readConfigLong(CONFIG_SECTION, "input_port_baud" + id_tag);
                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }

            return is_success;
        }

        public bool saveToDevice()
        {
            // save
            bool is_success = false;
            try
            {
                // id tag
                String id_tag = "_" + m_config_id.ToString();

                // load
                ModelAppDevice device = new ModelAppDevice();
                if (null != device)
                {
                    device.writeConfigLong(CONFIG_SECTION, "is_use" + id_tag, m_is_use);

                    device.writeConfigString(CONFIG_SECTION, "print_connect_type" + id_tag, m_print_connect_type);
                    device.writeConfigString(CONFIG_SECTION, "input_port_num" + id_tag, m_print_port_num);
                    device.writeConfigLong(CONFIG_SECTION, "input_port_baud" + id_tag, m_print_port_baud);
                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }

            return is_success;
        }
    }
}
