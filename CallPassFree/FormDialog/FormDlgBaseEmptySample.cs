using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormDlgBaseEmptySample : FormDlgBase
    {
        object m_dlg_obj = null;

        // ---------------------------------------------------------- basic method
        //
        public FormDlgBaseEmptySample(Form _parnet = null, object _sel_obj = null)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();

            m_dlg_obj = _sel_obj;

            initDlgObjects();
            initDlgControls();
        }

        private void FormDlg_Load(object sender, EventArgs e)
        {

        }

        // ---------------------------------------------------------- BaseForm override - init, set data ...
        //
        override protected void initDlgObjects()
        {
        }

        override protected void initDlgControls()
        {
        }
    }
}
