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
    public class ObjConfigOrderInput
    {
        private const string CONFIG_SECTION = "CONFIG_ORDER_INPUT";
        private const string TABLE_NAME = "tbConfigOrderInput";

        public bool IS_USE { get { return 1 == m_is_use; } }
        public bool IS_INSTALL { get { return 1 == m_is_use; } }

        public int m_config_id; // key
        public int m_is_use;
        public int m_is_install;
        public String m_input_type;
        public String m_input_port_num;
        public int m_input_port_baud;

        public String m_listen_port_num;
        public int m_listen_port_baud;

        public String m_conn_print_port_num;

        // ----------------------------------------------------------
        //
        public ObjConfigOrderInput()
        {
            initObj();
        }

        public void initObj()
        {
            m_config_id = 0;
            m_is_use = 0;
            m_is_install = 0;
            m_input_type = "";
            m_input_port_num = "";
            m_input_port_baud = 0;

            m_listen_port_num = "";
            m_listen_port_baud = 0;

            m_conn_print_port_num = "";
        }

        // -------------------------------------- for sqlite 
        //
        static public String sqlite_getExistsTableString()
        {
            String query = String.Format("SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{0}'", TABLE_NAME);
            return query;
        }

        static public String sqlite_getCreateTableString()
        {
            String query = String.Format("CREATE TABLE {0} (config_id INT PRIMARY KEY, is_use INT, input_type VARCHAR(20), input_port_num VARCHAR(20), input_port_baud INT, listen_port_num VARCHAR(20), listen_port_baud INT, conn_print_port_num VARCHAR(20));", TABLE_NAME);
            return query;
        }

        static public String sqlite_getSelectRowString(int _config_id)
        {
            String query = String.Format("select * from {0} where config_id = {1}"
                , TABLE_NAME
                , _config_id);
            return query;
        }

        static public String sqlite_getInsertRowString(ObjConfigOrderInput _src)
        {
            String query = String.Format("insert into {0} (config_id, is_use, input_type, input_port_num, input_port_baud, listen_port_num, listen_port_baud, conn_print_port_num) values ({1}, {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')"
                        , TABLE_NAME
                        , _src.m_config_id
                        , _src.m_is_use
                        , _src.m_is_install
                        , _src.m_input_type
                        , _src.m_input_port_num
                        , _src.m_input_port_baud
                        , _src.m_listen_port_num
                        , _src.m_listen_port_baud
                        , _src.m_conn_print_port_num);
            return query;
        }

        static public String sqlite_getUpdateRowString(ObjConfigOrderInput _src)
        {
            String query = String.Format("update {0} set is_use={2}, is_install={3}, input_type='{4}', input_port_num='{5}', input_port_baud='{6}', listen_port_num='{7}', listen_port_baud='{8}' where config_id = {1};"
                , TABLE_NAME
                , _src.m_config_id
                , _src.m_is_use
                , _src.m_is_install
                , _src.m_input_type
                , _src.m_input_port_num
                , _src.m_input_port_baud
                , _src.m_listen_port_num
                , _src.m_listen_port_baud
                , _src.m_conn_print_port_num);
            return query;
        }

        // --------------------------------------
        //
        public void setObj(ObjConfigOrderInput _src)
        {
            m_config_id = _src.m_config_id;
            m_is_use = _src.m_is_use;
            m_is_install = _src.m_is_install;
            m_input_type = _src.m_input_type;
            m_input_port_num = _src.m_input_port_num;
            m_input_port_baud = _src.m_input_port_baud;
            m_listen_port_num = _src.m_listen_port_num;
            m_listen_port_baud = _src.m_listen_port_baud;
            m_conn_print_port_num = _src.m_conn_print_port_num;
        }

        public bool setObj(DataRow _dr)
        {
            if (null == _dr)
            {
                return false;
            }

            try
            {
                m_config_id = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "config_id"));
                m_is_use = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "is_use"));
                m_is_install = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "is_install"));
                m_input_type = KnUtil.getDataRowString(_dr, "input_type");
                m_input_port_num = KnUtil.getDataRowString(_dr, "input_port_num");
                m_input_port_baud = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "input_port_baud"));
                m_listen_port_num = KnUtil.getDataRowString(_dr, "listen_port_num");
                m_listen_port_baud = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "listen_port_baud"));
                m_conn_print_port_num = KnUtil.getDataRowString(_dr, "conn_print_port_num");
            }
            catch (Exception ex)
            {
                TsLog.writeLog(ex.Message);
                Debug.Assert(false);
                return false;
            }
            return true;
        }

        // --------------------------------------
        //
        public ObjProcedureResult loadFromDB(int _obj_key)
        {
            // save
            ObjProcedureResult proc_result = new ObjProcedureResult();
            try
            {
                using (ModelFileDatabase.MyFileDbConnection my_db = ModelFileDatabase.getConnection())
                {
                    if (my_db.openDB())
                    {
                        //DataTable dt = my_db.loadConfigOrderInputObject(_obj_key);
                        //if (null != dt && 0 < dt.Rows.Count)
                        //{
                        //    this.setObj(dt.Rows[0]);
                        //}
                        //else
                        //{
                        //    this.m_config_id = _obj_key;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }

            return proc_result;
        }

        public ObjProcedureResult saveToDB()
        {
            // save
            ObjProcedureResult proc_result = new ObjProcedureResult();
            try
            {
                //using (ModelFileDatabase.MyFileDbConnection my_db = ModelFileDatabase.getConnection())
                //{
                //    if (my_db.openDB())
                //    {
                //        my_db.saveConfigOrderInputObject(this);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }

            return proc_result;
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
                    m_is_install = device.readConfigLong(CONFIG_SECTION, "is_install" + id_tag);
                    m_input_type = device.readConfigString(CONFIG_SECTION, "input_type" + id_tag);
                    m_input_port_num = device.readConfigString(CONFIG_SECTION, "input_port_num" + id_tag);
                    m_input_port_baud = device.readConfigLong(CONFIG_SECTION, "input_port_baud" + id_tag);
                    m_listen_port_num = device.readConfigString(CONFIG_SECTION, "listen_port_num" + id_tag);
                    m_listen_port_baud = device.readConfigLong(CONFIG_SECTION, "listen_port_baud" + id_tag);
                    m_conn_print_port_num = device.readConfigString(CONFIG_SECTION, "conn_print_port_num" + id_tag);


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
                    device.writeConfigLong(CONFIG_SECTION, "is_install" + id_tag, m_is_install);
                    device.writeConfigString(CONFIG_SECTION, "input_type" + id_tag, m_input_type);
                    device.writeConfigString(CONFIG_SECTION, "input_port_num" + id_tag, m_input_port_num);
                    device.writeConfigLong(CONFIG_SECTION, "input_port_baud" + id_tag, m_input_port_baud);
                    device.writeConfigString(CONFIG_SECTION, "listen_port_num" + id_tag, m_listen_port_num);
                    device.writeConfigLong(CONFIG_SECTION, "listen_port_baud" + id_tag, m_listen_port_baud);
                    device.writeConfigString(CONFIG_SECTION, "conn_print_port_num" + id_tag, m_conn_print_port_num);
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
