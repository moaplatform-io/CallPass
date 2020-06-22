using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using Kons.TsLibrary;

namespace Kons.ShopCallpass.Model
{
    /// <summary>
    /// 외부에서 TsConfig, TsLog 직접 사용하지 않고 이 모델을 통해 사용한다
    /// 
    /// ModelDevice model = new ModelDevice();
    /// model.writeLog("test");
    /// model.writeConfigString("section", "key", "value");
    /// 
    /// </summary>
    public class ModelAppDevice
    {
        // define
        private const String LOG_FILE_NM = "zlog";

        // ---------------------------------------------------------- ModelDevice
        //
        public ModelAppDevice() // private will be protect creator by new
        {
            if (null == m_this_singleton_instance)
            {
                m_this_singleton_instance = new __ModelDevice();
                m_this_singleton_instance.init();
            }
        }
        // --------------------------------------
        // log
        public void writeLog(string _log)
        {
            if (null != TsLog.getInstance())
            {
                TsLog.writeLog(_log);
            }
        }

        // --------------------------------------
        // config method
        public void writeConfigString(string _section, string _key, string _value)
        {
            TsConfig config = TsConfig.getInstance();
            if (null != config)
            {
                config.writeString(_section, _key, _value);
            }
        }

        public void writeConfigLong(string _section, string _key, long _value)
        {
            TsConfig config = TsConfig.getInstance();
            if (null != config)
            {
                config.writeLong(_section, _key, _value);
            }
        }

        public void deleteConfigSection(string _section)
        {
            TsConfig config = TsConfig.getInstance();
            if (null != config)
            {
                config.deleteSection(_section);
            }
        }

        public string readConfigString(string _section, string _key, string _default_value = "")
        {
            TsConfig config = TsConfig.getInstance();
            if (null != config)
            {
                return config.readString(_section, _key, _default_value);
            }
            return "";
        }

        public Int32 readConfigLong(string _section, string _key, long _default_value = 0)
        {
            TsConfig config = TsConfig.getInstance();
            if (null != config)
            {
                return config.readLong(_section, _key, _default_value);
            }
            return 0;
        }

        // --------------------------------------
        // device info
        public string getDeviceUniqueID()
        {
            if (null == m_this_singleton_instance)
            {
                return "";
            }
            return m_this_singleton_instance.m_device_unique_id;
        }

        public string getDeviceUniqueName()
        {
            if (null == m_this_singleton_instance)
            {
                return "";
            }
            return m_this_singleton_instance.m_device_unique_name;
        }

        // time
        public void updateLocalDateTime(DateTime _datetime)
        {
            //try
            //{
            //    SYSTEMTIME updatedTime = new SYSTEMTIME();
            //    updatedTime.wYear = (ushort)_datetime.Year;
            //    updatedTime.wMonth = (ushort)_datetime.Month;
            //    updatedTime.wDayOfWeek = (ushort)_datetime.DayOfWeek;
            //    updatedTime.wDay = (ushort)_datetime.Day;
            //    updatedTime.wHour = (ushort)_datetime.Hour;
            //    updatedTime.wMinute = (ushort)_datetime.Minute;
            //    updatedTime.wSecond = (ushort)_datetime.Second;
            //    updatedTime.wMilliseconds = (ushort)_datetime.Millisecond;

            //    SetLocalTime(ref updatedTime);
            //}
            //catch(Exception ex)
            //{
            //    String error_msg = err.Message;
            //}
        }

        // --------------------------------------
        // application config values
        public int getEncryptVersion()
        {
            return 1000; // 아래에 사용한 암/복호화 로직이 변경하면 버전도 변경해서 알수 있게 한다.
        }

        public string encryptKey(string _key)
        {
            char[] arr = _key.ToCharArray();
            for (int ii = 0; ii < arr.Length; ii++)
            {
                arr[ii] ^= (char)14;
            }
            byte[] b = new byte[arr.Length];
            for (int ii = 0; ii < arr.Length; ii++)
            {
                b[ii] = (byte)arr[ii];
            }

            return Convert.ToBase64String(b);
        }

        public string decryptKey(string _key)
        {
            byte[] b = Convert.FromBase64String(_key);
            char[] arr = new char[b.Length];

            for (int ii = 0; ii < b.Length; ii++)
            {
                arr[ii] = (char)(b[ii] ^ 14);
            }

            return (new string(arr));
        }

        // --------------------------------------
        // application config values
        public string getConfigLastOpenPath()
        {
            String last_open_path = readConfigString("RUNNING_ENVIROMENT", "last_open_path");
            return (("" == last_open_path) ? @"C:\" : last_open_path);
        }

        public void setConfigLastOpenPath(string _last_open_path)
        {
            writeConfigString("RUNNING_ENVIROMENT", "last_open_path", _last_open_path);
        }

        // ##############################################################################
        // ---------------------------------------------------------- Singleton Instance
        //
        static private __ModelDevice m_this_singleton_instance = null;
        private class __ModelDevice
        {
            public string m_device_unique_id;
            public string m_device_unique_name;

            public void init()
            {
                TsConfig.prepareInstance();
                TsLog.prepareInstance(LOG_FILE_NM, null);

                m_device_unique_id = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
                m_device_unique_name = Environment.MachineName;
            }

            public void deInit()
            {
                if (null != TsConfig.getInstance())
                {
                    TsConfig.releaseInstance();
                }

                if (null != TsLog.getInstance())
                {
                    TsLog.releaseInstance();
                }
            }
        }
    }
}
