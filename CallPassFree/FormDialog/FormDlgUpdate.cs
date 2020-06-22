using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kons.Utility;
using Kons.ShopCallpass.AppMain;
using Kons.ShopCallpass.FormPopup;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormDlgUpdate : FormDlgBase
    {
        private string m_url;
        private string m_key;
        private int m_version;
        private bool m_skip_possible;

        public FormDlgUpdate(Form _parent, string _url, string _key, int _version, bool _skip_possible)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();

            m_url = _url;
            m_key = _key;
            m_version = _version;
            m_skip_possible = _skip_possible;

            KnUtil.setDlgLocationParentCenter(_parent, this);
        }

        private void FormDlgUpdate_Load(object sender, EventArgs e)
        {
            if (null == m_url || 10 > m_url.Length)
            {
                FormPopupNotify.Show(this, "업데이트 정보가 없습니다.", "에러");
                this.Close();
                return;
            }

            if (!m_skip_possible)
            {
                btn_cancel.Text = "종료";
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                string runPath = Application.StartupPath;
                string exeName = Application.ExecutablePath.Substring(runPath.Length + 1, Application.ExecutablePath.Length - runPath.Length - 1);
                string runParm = m_url + " " + m_key + " " + exeName + " " + m_version.ToString();

                Process.Start(runPath + "\\KonsUpdater.exe", runParm);
            }
            catch (Exception ex)
            {
                FormPopupNotify.Show(this, ex.Message.ToString(), "에러");
            }
            finally
            {
                Kons.ShopCallpass.AppMain.AppCore.Instance.exitApp();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            if (m_skip_possible)
            {
                Kons.ShopCallpass.AppMain.AppCore.Instance.goNextMode();
                this.Close();
            }
            else
            {
                Kons.ShopCallpass.AppMain.AppCore.Instance.exitApp();
            }
        }
    }
}