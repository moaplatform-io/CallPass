using Kons.ShopCallpass.FormView;
using Kons.ShopCallpass.Object;
using Kons.Utility;
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
    public partial class FormDlgOrderDetail : FormDlgBase
    {
        private FormViewOrderList m_parent_form = null;
        ObjOrder m_dlg_obj = null;

        // ---------------------------------------------------------- basic method
        //
        public FormDlgOrderDetail(Form _parnet = null, ObjOrder _sel_obj = null)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();

            m_parent_form = (FormViewOrderList)_parnet;
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
            if (null == m_dlg_obj)
            {
                return;
            }

            // border
            //ctr_tbx_order_num.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            //ctr_tbx_order_type.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            //ctr_tbx_state_cd.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            //ctr_tbx_call_num.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            //ctr_tbx_call_datetime.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            //ctr_tbx_customer_cost.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            //ctr_tbx_customer_request_memo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            //ctr_lbx_arv_locate_address.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            //ctr_lbx_arv_locate_alternative_address.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            //ctr_lbx_arv_locate_memo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            // set enable
            ctr_tbx_order_num.Properties.ReadOnly = true;
            ctr_tbx_order_type.Properties.ReadOnly = true;
            ctr_tbx_state_cd.Properties.ReadOnly = true;
            ctr_tbx_call_num.Properties.ReadOnly = true;
            ctr_tbx_cust_pay_type_name.Properties.ReadOnly = true;
            ctr_tbx_call_datetime.Properties.ReadOnly = true;
            ctr_tbx_customer_cost.Properties.ReadOnly = true;
            ctr_tbx_customer_request_memo.Properties.ReadOnly = true;
            ctr_lbx_arv_locate_address.Properties.ReadOnly = true;
            ctr_lbx_arv_locate_alternative_address.Properties.ReadOnly = true;
            ctr_lbx_arv_locate_memo.Properties.ReadOnly = true;

            // set content
            ctr_tbx_order_num.Text = m_dlg_obj.m_order_num;
            ctr_tbx_order_type.Text = ObjOrder.getOrderTypeString(m_dlg_obj.m_order_type);
            ctr_tbx_state_cd.Text = ObjOrder.getStateString(m_dlg_obj.m_state_cd);
            ctr_tbx_call_num.Text = m_dlg_obj.m_call_num;
            ctr_tbx_cust_pay_type_name.Text = ObjOrder.getCustPayTypeString(m_dlg_obj.m_customer_pay_type_cd);
            ctr_tbx_call_datetime.Text = m_dlg_obj.m_call_datetime.ToString("yyyy-MM-dd HH:mm:ss");
            ctr_tbx_customer_cost.Text = Kons.Utility.KnUtil.formatMoney(m_dlg_obj.m_customer_cost);
            ctr_tbx_customer_request_memo.Text = m_dlg_obj.m_customer_request_memo;
            ctr_lbx_arv_locate_address.Text = m_dlg_obj.m_arv_locate_address;
            ctr_lbx_arv_locate_alternative_address.Text = m_dlg_obj.m_arv_locate_alternative_address; // 대체 가능한 주소 - 신주소
            ctr_lbx_arv_locate_memo.Text = m_dlg_obj.m_arv_locate_memo;

            // 
            if ((int)ObjOrder.STATE_TYPE.ORDER_STATE_0 == m_dlg_obj.m_state_cd)
            {
                ctr_btn_req_delivery.Enabled = true;
            }
            else
            {
                ctr_btn_req_delivery.Enabled = false;
            }
        }

        private void ctr_btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctr_btn_req_print_Click(object sender, EventArgs e)
        {
            using (new KnFormWaitCursors(this))
            {
                if (null != m_parent_form)
                {
                    m_parent_form.requestOrderPrint(m_dlg_obj);
                    KnDevexpressFunc.showMessage("영수증 프린트로 출력을 요청 하였습니다.", MessageBoxIcon.Information);
                }
                label1.Focus();
            }
        }

        private void ctr_btn_req_delivery_Click(object sender, EventArgs e)
        {
            using (new KnFormWaitCursors(this))
            {
                if (null != m_parent_form)
                {
                    m_parent_form.requestDelivery(m_dlg_obj);
                }
                label1.Focus();
            }
        }
    }
}
