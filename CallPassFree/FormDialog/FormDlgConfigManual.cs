using Kons.ShopCallpass.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kons.ShopCallpass.FormDialog
{
    
    public partial class FormDlgConfigManual : FormDlgBase
    {
        string[] CONFIG_SECTION = { "UNWANTEDKEYWORD", "ORDER_NUM", "COST", "CALL_NUM", "ADDRESS",
                                            "PAYMENT_OPTION", "ORDER_DATE", "EXCLUSIVEKEYWORD" };

        string[] keyValues = { "unwanted_keyword", "order_num", "cost", "call_num", "address",
                                "payment_option", "order_date", "exclusive_keyword" };

        public FormDlgConfigManual()
        {

        }

        public FormDlgConfigManual(Form _parnet = null)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
