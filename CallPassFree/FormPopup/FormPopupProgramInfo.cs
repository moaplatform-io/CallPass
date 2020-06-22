using Kons.ShopCallpass.AppMain;
using Kons.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kons.ShopCallpass.FormPopup
{
    public partial class FormPopupProgramInfo : Form
    {
        public FormPopupProgramInfo()
        {
            InitializeComponent();

            ctr_lbl_version_info.Text = "Version " + Kons.ShopCallpass.AppMain.AppCore.Instance.getAppVersionString();
        }

        // ---------------------------------------------------------- for esc key press
        // override Escape key to close this form
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ---------------------------------------------------------- 
        //
        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}