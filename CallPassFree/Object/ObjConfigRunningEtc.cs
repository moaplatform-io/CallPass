using Kons.ShopCallpass.AppMain;
using Kons.ShopCallpass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.Object
{
    public class ObjConfigRunningEtc
    {
        private const string CONFIG_SECTION = "CONFIG_RUNNING_ETC";

        public int m_is_use_save_order_input;
        public int m_is_use_send_order_input;

        // ----------------------------------------------------------
        //
        public ObjConfigRunningEtc()
        {
            initObj();
        }

        public void initObj()
        {
            m_is_use_save_order_input = 0;
            m_is_use_send_order_input = 0;
        }


        // --------------------------------------
        //
        public void setObj(ObjConfigRunningEtc _src)
        {
            m_is_use_save_order_input = _src.m_is_use_save_order_input;
            m_is_use_send_order_input = _src.m_is_use_send_order_input;
        }

        // --------------------------------------
        //
        public bool loadFromDevice()
        {
            // init
            initObj();

            // load
            try
            {
                // load
                ModelAppDevice device = new ModelAppDevice();
                if (null != device)
                {
                    // 프로그램 실행중에만 의미있게 함
                    m_is_use_save_order_input = Kons.ShopCallpass.AppMain.AppCore.Instance.getAppDoc().m_is_use_save_order_input;
                    m_is_use_send_order_input = Kons.ShopCallpass.AppMain.AppCore.Instance.getAppDoc().m_is_use_send_order_input;

                    // 장치에 저장하지 않고 일회성으로만 사용하게 바꾸면서 주석처리
                    //m_is_use_save_order_input = device.readConfigLong(CONFIG_SECTION, "is_use_save_order_input"); // 저장하지 않고 일회성으로만 사용한다.
                    //m_is_use_send_order_input = device.readConfigLong(CONFIG_SECTION, "is_use_send_order_input"); // 저장하지 않고 일회성으로만 사용한다.
                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }

            return true;
        }

        public bool saveToDevice()
        {
            // save
            try
            {
                // load
                ModelAppDevice device = new ModelAppDevice();
                if (null != device)
                {
                    // 프로그램 실행중에만 의미있게 함
                    Kons.ShopCallpass.AppMain.AppCore.Instance.getAppDoc().m_is_use_save_order_input = m_is_use_save_order_input;
                    Kons.ShopCallpass.AppMain.AppCore.Instance.getAppDoc().m_is_use_send_order_input = m_is_use_send_order_input;

                    // 장치에 저장하지 않고 일회성으로만 사용하게 바꾸면서 주석처리
                    //device.writeConfigLong(CONFIG_SECTION, "is_use_save_order_input", m_is_use_save_order_input); // 저장하지 않고 일회성으로만 사용한다.
                    //device.writeConfigLong(CONFIG_SECTION, "is_use_send_order_input", m_is_use_send_order_input); // 저장하지 않고 일회성으로만 사용한다.
                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }

            return true;
        }
    }
}
