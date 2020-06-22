using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kons.Utility;
using Kons.ShopCallpass.FormDialog;
using Kons.ShopCallpass.Object;

namespace Kons.ShopCallpass.AppMain
{
    public class AppCore
    {
        // --------------------------------------
        // define - 앱 설정에 관한 값 지정, 공용(public) define은 AppDefine 이용할 것
        private const int APP_VERSION = 1012;
        private const string APP_TITLE = "Callpass";

        private const string APP_KEY = "";

        private const string WEB_REST_URL = "";

        // 원격지원 URL
        static public string REMOTE_HELPER_URL = "";

        // --------------------------------------
        // singleton values
        static private AppCore m_this = null;
        static public AppCore Instance { get { if (null == m_this) { m_this = new Kons.ShopCallpass.AppMain.AppCore(); } return m_this; } }

        // context
        private ApplicationContext m_context = null;

        // app document
        private AppDoc m_app_doc = new AppDoc();

        // form main
        private FormMain m_main_form = null;
        public FormMain Mainform { get { return m_main_form; } }

        // mode values
        public enum APP_MODE
        {
            MODE_NONE = 0,
            MODE_UPDATE,
            MODE_LOGIN,
            MODE_MAIN,
            MODE_EXIT
        }
        private APP_MODE m_cur_app_mode = APP_MODE.MODE_NONE;

        // update info
        public string UPDATE_URL { set; get; }
        public string UPDATE_KEY { set; get; }
        public int UPDATE_VERSION { set; get; }
        public bool UPDATE_SKIP_POSSIBLE { set; get; }

        // --------------------------------------
        // data values
        public ObjLoginUser m_login_user = null;

        // 
        public const int ORDER_INPUT_CONFIG_COUNT = 4;
        public const int PRINT_OUTPUT_CONFIG_COUNT = 4;

        // ------------------------------------------------------------------------------
        // method
        public AppCore()
        {
            init();
        }

        public void init()
        {
            m_main_form = null;
            m_login_user = new ObjLoginUser();
        }

        public void deInit()
        {
        }

        // load 
        public void loadBaseData()
        {

        }

        // set main form
        public void setMainForm(FormMain _main_form)
        {
            m_main_form = _main_form;
        }

        // ------------------------------------------------------------------------------
        // application start point and mode control
        public ApplicationContext initInstance()
        {
            return setAppMode(APP_MODE.MODE_LOGIN);       // start mode setting
        }

        // application mode control
        public ApplicationContext setAppMode(APP_MODE _app_mode)
        {
            // check context
            if (null == m_context)
            {
                m_context = new ApplicationContext();
            }

            // old - new
            Form oldForm = null;

            // set old form
            if ((null != m_context) && (null != m_context.MainForm))
            {
                oldForm = m_context.MainForm;
            }

            // set next form ...
            switch (_app_mode)
            {
                case APP_MODE.MODE_LOGIN:
                    {
                        m_cur_app_mode = APP_MODE.MODE_LOGIN;
                        m_context.MainForm = new FormDlgLogin(null);
                    }
                    break;
                case APP_MODE.MODE_UPDATE:
                    {
                        m_cur_app_mode = APP_MODE.MODE_UPDATE;
                        m_context.MainForm = new FormDlgUpdate(null, UPDATE_URL, UPDATE_KEY, UPDATE_VERSION, UPDATE_SKIP_POSSIBLE);
                    }
                    break;
                case APP_MODE.MODE_MAIN:
                    {
                        m_cur_app_mode = APP_MODE.MODE_MAIN;
                        m_context.MainForm = new FormMain();
                    }
                    break;
                case APP_MODE.MODE_NONE:
                    {
                        m_cur_app_mode = APP_MODE.MODE_NONE;
                        m_context.MainForm = null;
                    }
                    break;
                case APP_MODE.MODE_EXIT:
                    {
                        m_cur_app_mode = APP_MODE.MODE_EXIT;
                    }
                    return m_context;
            }

            // hide, show, close ...
            if (null != oldForm)
            {
                oldForm.Hide();
                oldForm.Close();
            }
            if ((null != m_context) && (null != m_context.MainForm))
            {
                m_context.MainForm.Show();
            }

            // return
            return m_context;
        }

        public void goNextMode()
        {
            // go next mode
            APP_MODE curr_app_mode = m_cur_app_mode;
            APP_MODE next_app_mode = getNextMode(curr_app_mode);
            setAppMode(next_app_mode);
        }

        public APP_MODE getNextMode(APP_MODE _curr_app_mode)
        {
            APP_MODE next_app_mode = APP_MODE.MODE_NONE;

            switch (_curr_app_mode)
            {
                case APP_MODE.MODE_NONE:
                    {
                        next_app_mode = APP_MODE.MODE_LOGIN;
                    }
                    break;
                case APP_MODE.MODE_LOGIN:
                    {
                        next_app_mode = APP_MODE.MODE_MAIN;
                    }
                    break;
                case APP_MODE.MODE_UPDATE:
                    {
                        next_app_mode = APP_MODE.MODE_MAIN;
                    }
                    break;
                default:
                    {
                        next_app_mode = APP_MODE.MODE_NONE;
                    }
                    break;
            }

            return next_app_mode;
        }

        public AppDoc getAppDoc()
        {
            return m_app_doc;
        }

        public APP_MODE getCurrentAppMode()
        {
            return m_cur_app_mode;
        }

        public void exitApp()
        {
            try
            {
                deInit();
                m_this = null;

                Application.Exit();
            }
            catch { }
        }

        // ------------------------------------------------------------------------------
        //
        public int getAppVersion()
        {
            return APP_VERSION;
        }

        public string getAppVersionString()
        {
            int verMajor = (APP_VERSION / 1000);
            int verMinor = (APP_VERSION % 1000 / 100);
            int verSub = (APP_VERSION % 100);
            Console.WriteLine("verMajor : " + verMajor);
            Console.WriteLine("verMinor : " + verMinor);
            Console.WriteLine("verSub : " + verSub);
            Console.WriteLine(string.Format("{0:0}.{1:0}.{2:00}", verMajor, verMinor, verSub));
            return string.Format("{0:0}.{1:0}.{2:00}", verMajor, verMinor, verSub);
        }

        public string getAppTitle()
        {
            return APP_TITLE;
        }

        // ------------------------------------------------------------------------------
        //
        public string getAppKey()
        {
            return APP_KEY;
        }

        public string getWebRestUrl()
        {
            return WEB_REST_URL;
        }

        // ------------------------------------------------------------------------------
        //
        public ObjLoginUser getLoginUserObj()
        {
            return m_login_user;
        }

        public string getLoginUserLoginKey() // 빈번하게 사용하는 것만 따로 빼놓음
        {
            return (null == Kons.ShopCallpass.AppMain.AppCore.Instance.getLoginUserObj() ? "" : Kons.ShopCallpass.AppMain.AppCore.Instance.getLoginUserObj().m_login_unique_key);
        }

        public string getLoginUserCompanyName() // 빈번하게 사용하는 것만 따로 빼놓음
        {
            return (null == Kons.ShopCallpass.AppMain.AppCore.Instance.getLoginUserObj() ? "" : Kons.ShopCallpass.AppMain.AppCore.Instance.getLoginUserObj().m_company_name);
        }

        // ------------------------------------------------------------------------------
        //
        static public void detectException(String _message, Boolean _isShow = false, Boolean _isSaveLog = false)
        {
            if (_isShow)
            {
                KnDevexpressFunc.detectException(_message, true);
            }

            if (_isSaveLog)
            {

            }
        }
    }
}
