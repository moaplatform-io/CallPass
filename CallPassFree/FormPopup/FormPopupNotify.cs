using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kons.Utility;

namespace Kons.ShopCallpass.FormPopup
{
    public partial class FormPopupNotify : DevExpress.XtraEditors.XtraForm
    {
        private System.Windows.Forms.Timer m_timer_notify = null;

        public FormPopupNotify(Form _parent)
        {
            InitializeComponent();

            this.Owner = _parent;
            KnUtil.setDlgLocationParentCenter(_parent, this);
        }

        static public void Show(Form _owner, string _message, string _caption, int _auto_close_after_ms = 0)
        {
            FormPopupNotify form = new FormPopupNotify(_owner);
            form.setData(_message, " " + _caption, _auto_close_after_ms);
            form.ShowDialog();
        }

        public void setData(string _message, string _caption, int _auto_close_after)
        {
            this.Text = _caption;
            this.label_notify.Text = _message;

            // auto hide
            if (0 < _auto_close_after)
            {
                if (1000 > _auto_close_after)
                {
                    _auto_close_after = 1000;       // 최소값 지정
                }

                if (null != m_timer_notify)
                {
                    m_timer_notify.Stop();
                    m_timer_notify = null;
                }

                m_timer_notify = new Timer();
                m_timer_notify.Enabled = true;
                m_timer_notify.Interval = _auto_close_after;
                m_timer_notify.Tick += new EventHandler(this.onTimer);
                m_timer_notify.Start();
            }
        }

        private void onTimer(object sender, EventArgs e)
        {
            if (null != m_timer_notify)
            {
                m_timer_notify.Stop();
            }
            this.Close();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        void NotifyClick(object sender, EventArgs e)
        {
            Console.WriteLine("화면 최상단으로 이동");
        }
    }
}