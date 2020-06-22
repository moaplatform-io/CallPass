using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kons.Utility;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormDlgBase : DevExpress.XtraEditors.XtraForm
    {
        public enum DLG_TYPE
        {
            TYPE_NORMAL = 0, // 일반적인 경우 사용 - 대부분 콜백이 없는 경우
            TYPE_END
        }

        public delegate void MyCallbackFunc(DLG_TYPE _who, object _obj);
        public MyCallbackFunc mMyCallback = null;
        private DLG_TYPE mMyType;

        public FormDlgBase()
        {
            InitializeComponent();
        }

        protected void setBaseFormData(Form _owner, MyCallbackFunc _my_callback, DLG_TYPE _my_type = DLG_TYPE.TYPE_NORMAL)
        {
            KnDegine.setFormFixedDlgStyle(this, _owner);
            this.DoubleBuffered = true;
            mMyCallback = _my_callback;
            mMyType = _my_type;
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

        // ---------------------------------------------------------- dialog common functions
        // init dialog object
        virtual protected void initDlgObjects()
        {
            // 폼 클래스 변수 초기화
        }

        // init dialog view - grid...
        virtual protected void initDlgView()
        {
        }

        // init dialog controls
        virtual protected void initDlgControls()
        {
            // 폼 컨트롤 디자인, 초기값 등 초기화 - 콤보박스에 값 채우기 등
            // 맴버변수의 값을 컨트롤에 채우는것은 setDlgDataObjectToControl() 함수 이용하도록
        }

        // object -> control
        virtual protected void setDlgObjectDataToControls()
        {
            // 폼 변수의 값을 컨트롤에 넣을 때
        }

        // control -> object
        virtual protected void setDlgControlDataToObjects()
        {
            // 폼의 컨트롤 값을 변수에 담을때
        }

        // ---------------------------------------------------------- dialog callback functions
        // form callback
        protected void sendCallback(object _obj)
        {
            if (null != mMyCallback)
            {
                mMyCallback(mMyType, _obj);
            }
        }
    }
}