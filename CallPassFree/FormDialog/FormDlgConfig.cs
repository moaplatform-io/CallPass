using Kons.ShopCallpass.FormPopup;
using Kons.ShopCallpass.Model;
using Kons.ShopCallpass.Object;
using Kons.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormDlgConfig : FormDlgBase
    {
        public enum TAB_TYPE
        {
            CONFIG_ORDER_INPUT = 0, // 일반적인 경우 사용 - 대부분 콜백이 없는 경우
            CONFIG_PRINT_OUTPUT,
            CONFIG_RUNNING_ETC,
            CONFIG_PRINT_SETTING,
            CONFIG_MANUAL
        }

        public bool m_is_change_config = false;

        // ---------------------------------------------------------- basic method
        //
        public FormDlgConfig(Form _parnet = null)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();
        }

        private void FormDlg_Load(object sender, EventArgs e)
        {
            initDlgObjects();
            initDlgControls();
        }

        // ---------------------------------------------------------- BaseForm override - init, set data ...
        //
        override protected void initDlgObjects()
        {
        }

        override protected void initDlgControls()
        {
            for(int i=0; i<tabPane.Pages.Count; i++)
            {
                FormDlgBase add_form = null;
                switch(i)
                {
                    case (int)TAB_TYPE.CONFIG_ORDER_INPUT:
                        add_form = new FormDlgConfigOrderInput(this);
                        break;
                    case (int)TAB_TYPE.CONFIG_PRINT_OUTPUT:
                        add_form = new FormDlgConfigPrintOutput(this);
                        break;
                    case (int)TAB_TYPE.CONFIG_RUNNING_ETC:
                        add_form = new FormDlgConfigRunningEtc(this);
                        break;
                    case (int)TAB_TYPE.CONFIG_PRINT_SETTING:
                        add_form = new FormDlgConfigParseSetting(this);
                        break;
                    case (int)TAB_TYPE.CONFIG_MANUAL:
                        add_form = new FormDlgConfigManual(this);
                        break;
                }

                if (null != add_form)
                {
                    add_form.TopLevel = false;
                    add_form.FormBorderStyle = FormBorderStyle.None;
                    add_form.Dock = DockStyle.Fill;
                    add_form.Show();

                    tabPane.Pages[i].Controls.Add(add_form);
                }
            }

            if (0 < tabPane.Pages.Count)
            {
                tabPane.SelectedPageIndex = 0;
            }
        }

        // ---------------------------------------------------------- event handler
        //
        private void tabPane_SelectedPageIndexChanged(object sender, EventArgs e)
        {

        }

        private void ctr_btn_save_Click(object sender, EventArgs e)
        {
            if (0 == tabPane.SelectedPage.Controls.Count)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            for (int i = 0; i < tabPane.Pages.Count; i++)
            {
                Control sel_control = tabPane.Pages[i].Controls[0];
                switch (i)
                {
                    case (int)TAB_TYPE.CONFIG_ORDER_INPUT:
                        {
                            if (sel_control.GetType() == typeof(FormDlgConfigOrderInput))
                            {
                                FormDlgConfigOrderInput sel_dlg = (FormDlgConfigOrderInput)sel_control;
                                sel_dlg.saveDlgObjectData();
                            }
                        }
                        break;
                    case (int)TAB_TYPE.CONFIG_PRINT_OUTPUT:
                        {
                            if (sel_control.GetType() == typeof(FormDlgConfigPrintOutput))
                            {
                                FormDlgConfigPrintOutput sel_dlg = (FormDlgConfigPrintOutput)sel_control;
                                sel_dlg.saveDlgObjectData();
                            }
                        }
                        break;
                    case (int)TAB_TYPE.CONFIG_RUNNING_ETC:
                        {
                            if (sel_control.GetType() == typeof(FormDlgConfigRunningEtc))
                            {
                                FormDlgConfigRunningEtc sel_dlg = (FormDlgConfigRunningEtc)sel_control;
                                sel_dlg.saveDlgObjectData();
                            }
                        }
                        break;
                    case (int)TAB_TYPE.CONFIG_PRINT_SETTING:
                        {
                            if (sel_control.GetType() == typeof(FormDlgConfigParseSetting))
                            {
                                FormDlgConfigParseSetting sel_dlg = (FormDlgConfigParseSetting)sel_control;
                                sel_dlg.saveDlgObjectData();
                            }
                        }
                        break;
                }
            }

            m_is_change_config = true; // 저장 버튼 눌렀는지만 알기위함
            this.Cursor = Cursors.Default;

            // 저장 버튼의 포커싱 없애기 위해
            tabPane.Focus();

            FormMain formMain = new FormMain();
            formMain.startOrderSync();
        }

        private void ctr_btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
