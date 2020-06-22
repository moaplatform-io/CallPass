using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kons.ShopCallpass.FormView
{
    public partial class FormViewBase : DevExpress.XtraEditors.XtraForm
    {
        public enum VIEW_TYPE
        {
            VIEW_HOME = 0,      // 홈
            VIEW_ORDER_LIST,    // 접수화면
            TYPE_END
        }

        protected VIEW_TYPE m_form_view_type;
        public VIEW_TYPE FORM_VIEW_TYPE { get { return m_form_view_type; } }

        protected bool m_is_only = true;
        public bool IS_ONLY { get { return m_is_only; } }

        protected string m_form_view_name = string.Empty;
        public string FORM_VIEW_NAME { get { return m_form_view_name; } }

        public FormViewBase()
        {
            InitializeComponent();
        }

        protected void setBaseFormData(Form _owner, VIEW_TYPE _form_view_type)
        {
            setOwner(_owner);
            setViewType(_form_view_type);
        }

        private void setOwner(Form _owner)
        {
            if (null != _owner)
            {
                this.Owner = _owner;
            }
        }

        private void setViewType(VIEW_TYPE _form_view_type)
        {
            m_form_view_type = _form_view_type;
            m_is_only = isOnly(m_form_view_type);
        }

        public VIEW_TYPE getViewType()
        {
            return m_form_view_type;
        }

        static public bool isOnly(VIEW_TYPE _form_view_type)
        {
            switch (_form_view_type)
            {
                // 유일성 필요
                case VIEW_TYPE.VIEW_HOME:
                case VIEW_TYPE.VIEW_ORDER_LIST:
                    return true;
                default:
                    return false;
            }
        }
    }
}