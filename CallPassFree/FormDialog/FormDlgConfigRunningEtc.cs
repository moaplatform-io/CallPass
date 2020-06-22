using Kons.ShopCallpass.AppMain;
using Kons.ShopCallpass.Controller;
using Kons.ShopCallpass.FormPopup;
using Kons.ShopCallpass.Object;
using Kons.ShopCallpass.OpenApi;
using Kons.ShopCallpass.Model;
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
using Microsoft.Win32;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormDlgConfigRunningEtc : FormDlgBase
    {
        private ObjConfigRunningEtc m_running_etc_config = null;
        private ObjConfigLastDeliveryRequestInfo m_last_request_config = null;

        private ObjConfigStoreApiInfo m_store_api_info = null;
        private ObjConfigStoreApiRegInfo m_store_reg_info = null;

        // ---------------------------------------------------------- basic method
        // 
        public FormDlgConfigRunningEtc(Form _parnet = null)
        {
            setBaseFormData(_parnet, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();

            ModelAppDevice device = new ModelAppDevice();
            if(device.readConfigString("AUTORUNNING", "auto_running") == "0")
            {
                checkBox1.Checked = false;
            }
            else if (device.readConfigString("AUTORUNNING", "auto_running") == "1")
            {
                checkBox1.Checked = true;
            }
        }

        private void FormDlg_Load(object sender, EventArgs e)
        {
            initDlgObjects();
            initDlgControls();

            // object -> control
            setDlgObjectDataToControls();

            // enable or disable
            setDlgControlEnableState();
        }

        // ---------------------------------------------------------- BaseForm override - init, set data ...
        //
        override protected void initDlgObjects()
        {
            m_running_etc_config = new ObjConfigRunningEtc();
            m_running_etc_config.loadFromDevice();

            m_last_request_config = new ObjConfigLastDeliveryRequestInfo();
            m_last_request_config.loadFromDevice();
        }

        override protected void initDlgControls()
        {
            // read only
            ctr_tbx_store_num.Properties.ReadOnly = true;
            ctr_tbx_store_name.Properties.ReadOnly = true;
            ctr_ebx_store_id.Properties.ReadOnly = true;

            // delivery company
            KnDevexpressFunc.setComboboxCommonStyle(ctr_cbx_open_api_type);
            setDlgComboboxOpenApiTypeItems(ctr_cbx_open_api_type, m_last_request_config.m_delivery_company_type);
        }

        // object -> control
        override protected void setDlgObjectDataToControls()
        {
            ctr_chk_use_save_order_input.Checked = (1 == m_running_etc_config.m_is_use_save_order_input);
            ctr_chk_use_send_order_input.Checked = (1 == m_running_etc_config.m_is_use_send_order_input);

            if (null != m_store_api_info)
            {
                ctr_tbx_store_pno.Text = m_store_api_info.m_store_pno;
                ctr_tbx_store_num.Text = m_store_api_info.m_store_num;
                ctr_tbx_store_name.Text = m_store_api_info.m_store_name;
            }

            if (null != m_store_reg_info)
            {
                ctr_ebx_store_id.Text = m_store_reg_info.m_store_id;
            }

            // 컨트롤 비활성/활성
            if (null != m_store_reg_info && 0 < m_store_reg_info.m_store_id.Length)
            {
                ctr_tbx_store_pno.ReadOnly = true;
                ctr_btn_req_store_del.Enabled = true;
            }
            else
            {
                ctr_tbx_store_pno.ReadOnly = false;
                ctr_btn_req_store_del.Enabled = false;
            }
        }

        // control -> object
        override protected void setDlgControlDataToObjects()
        {
            m_running_etc_config.m_is_use_save_order_input = (ctr_chk_use_save_order_input.Checked ? 1 : 0);
            m_running_etc_config.m_is_use_send_order_input = (ctr_chk_use_send_order_input.Checked ? 1 : 0);
        }

        // controls enable or disable
        private void setDlgControlEnableState()
        {

        }

        // ----------------------------------------------------------
        //
        private void setDlgComboboxOpenApiTypeItems(DevExpress.XtraEditors.ComboBoxEdit _control, string _default_key = null)
        {
            for (int i = 0; i < OpenApiBase.OPEN_API_TYPE_LIST.Length; i++)
            {
                OpenApiBase.OPEN_API_TYPE sel_type = OpenApiBase.OPEN_API_TYPE_LIST[i];

                KnDevexpressFunc.ComboBoxAddItem(_control, ((int)sel_type).ToString(), OpenApiBase.obtainDeliveryCompanyTypeName(sel_type));
            }

            if (null != _default_key)
            {
                KnDevexpressFunc.ComboBoxSetSelectByKey(_control, _default_key);
            }
        }

        private OpenApiBase getSelOpenApiManager()
        {
            int sel_delivery_open_api_type = KnUtil.formatInt32(KnDevexpressFunc.ComboBoxGetSelectedItemKey(ctr_cbx_open_api_type));
            if (-1 != sel_delivery_open_api_type)
            {
                return OpenApiBase.getOpenApiManager((OpenApiBase.OPEN_API_TYPE)sel_delivery_open_api_type);
            }
            return null;
        }

        private ArrayList requestStoreApiInfo(String _store_pno)
        {
            OpenApiBase api_mgr = getSelOpenApiManager();
            if (null != api_mgr)
            {
                return api_mgr.requestStoreFind(_store_pno);
            }
            return null;
        }

        private ObjConfigStoreApiRegInfo requestShopRegInfo(String _store_num, String _store_pno, String _store_id)
        {
            OpenApiBase api_mgr = getSelOpenApiManager();
            if (null != api_mgr)
            {
                return api_mgr.requestStoreReg(_store_num, _store_pno, _store_id);
            }
            return null;
        }

        private ObjApiReqStoreDelResult requestShopDel(String _store_pno, String _store_id)
        {
            OpenApiBase api_mgr = getSelOpenApiManager();
            if (null != api_mgr)
            {
                return api_mgr.requestStoreDel(_store_pno, _store_id);
            }
            return null;
        }

        // ---------------------------------------------------------- parent event handler
        //
        public void saveDlgObjectData()
        {
            // controls -> object
            setDlgControlDataToObjects();

            // save
            m_running_etc_config.saveToDevice();

            ModelAppDevice device = new ModelAppDevice();

            string keyName = "Callpass";
            string assemblyLocation = /*System.Reflection.Assembly.GetExecutingAssembly().Location;*/Application.ExecutablePath.ToString();  // Or the EXE path.

            if (checkBox1.Checked)
            {
                // Set Auto-start.
                if (!IsAutoStartEnabled(keyName, assemblyLocation))
                {
                    SetAutoStart(keyName, assemblyLocation);
                    Utility.Utility.LogWrite("자동실행 ON");
                }
                device.writeConfigString("AUTORUNNING", "auto_running", "1");
            }
            else
            {
                // Unset Auto-start.
                if (IsAutoStartEnabled(keyName, assemblyLocation))
                {
                    UnSetAutoStart(keyName);
                    Utility.Utility.LogWrite("자동실행 OFF");
                }
                device.writeConfigString("AUTORUNNING", "auto_running", "0");
            }
        }

        /// <summary>
        /// Sets the autostart value for the assembly.
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
        public void SetAutoStart(string keyName, string assemblyLocation)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            key.SetValue(keyName, assemblyLocation);
        }

        /// <summary>
        /// Returns whether auto start is enabled.
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
        public bool IsAutoStartEnabled(string keyName, string assemblyLocation)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            if (key == null)
                return false;

            string value = (string)key.GetValue(keyName);
            if (value == null)
                return false;

            return (value == assemblyLocation);
        }

        /// <summary>
        /// Unsets the autostart value for the assembly.
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        public void UnSetAutoStart(string keyName)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            key.DeleteValue(keyName);
        }
        // ----------------------------------------------------------
        //
        private void ctr_cbx_open_api_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            String sel_delivery_open_api_type = KnDevexpressFunc.ComboBoxGetSelectedItemKey(ctr_cbx_open_api_type);

            m_store_api_info = new ObjConfigStoreApiInfo();
            m_store_api_info.loadFromDevice(sel_delivery_open_api_type);

            m_store_reg_info = new ObjConfigStoreApiRegInfo();
            m_store_reg_info.loadFromDevice(sel_delivery_open_api_type);

            setDlgObjectDataToControls();
        }

        private void ctr_btn_req_store_find_Click(object sender, EventArgs e)
        {
            String req_store_pno = ctr_tbx_store_pno.Text.Trim().Replace("-", "");
            if (0 >= req_store_pno.Length)
            {
                FormPopupNotify.Show(this.Owner, "먼저 사업자번호를 입력 해 주십시오.", "알림");
                return;
            }

            // 이미매핑되어 있는지 확인
            if (0 < m_store_reg_info.m_store_id.Length)
            {
                if (DialogResult.Yes != MessageBox.Show(this.Owner, "이미 상점이 매핑되어 있습니다. \r다시 매핑하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
            }

            // 가맹정보 확인
            ObjConfigStoreApiInfo sel_store_info = null;
            ArrayList res_store_list = requestStoreApiInfo(req_store_pno);
            if (null != res_store_list)
            {
                if (1 == res_store_list.Count)
                {
                    sel_store_info = (ObjConfigStoreApiInfo)res_store_list[0];
                }
                else if (1 < res_store_list.Count)
                {
                    FormStoreInfoSelecter sel_dlg = new FormStoreInfoSelecter(this, res_store_list);
                    if (DialogResult.OK == sel_dlg.ShowDialog())
                    {
                        sel_store_info = sel_dlg.m_sel_obj;
                    }
                }
            }

            // 받은 값 확인
            if (null == sel_store_info || 0 == sel_store_info.m_store_num.Length)
            {
                FormPopupNotify.Show(this.Owner, "선택된 상점 코드가 없습니다. 확인 해 주십시오.", "알림");
                return;
            }

            if (0 < sel_store_info.m_store_num.Length)
            {
                m_store_api_info.setObj(sel_store_info); // 정상적으로 받았으면 교체
                m_store_api_info.saveToDevice();
            }

            // 가맹코드( 매핑 ) 할 값 확인 - 로그인키를 사용한다.
            String req_store_id = Kons.ShopCallpass.AppMain.AppCore.Instance.getLoginUserLoginKey(); // 가맹코드
            if (0 > req_store_id.Length)
            {
                FormPopupNotify.Show(this.Owner, "상점 매핑을 위한 접속 정보를 가져올 수 없습니다.", "알림");
                return;
            }

            // 매핑요청
            ObjConfigStoreApiRegInfo res_store_reg = requestShopRegInfo(sel_store_info.m_store_num, sel_store_info.m_store_pno, req_store_id);
            if (null == res_store_reg)
            {
                FormPopupNotify.Show(this.Owner, "상점 매핑코드를 가지고 올수 없습니다.", "알림");
                return;
            }

            if (0 == res_store_reg.m_ret_cd && 0 < res_store_reg.m_store_id.Length)
            {
                // 정상적으로 받았으면 교체
                m_store_reg_info.setObj(res_store_reg);
                m_store_reg_info.saveToDevice();

                // 마지막 매핑값으로 저장
                if (null != m_last_request_config)
                {
                    m_last_request_config.m_delivery_company_type = KnDevexpressFunc.ComboBoxGetSelectedItemKey(ctr_cbx_open_api_type);
                    m_last_request_config.saveToDevice();
                }

                // 알림
                FormPopupNotify.Show(this.Owner, "상점 매핑에 성공하였습니다. 배달요청을 \n이용하실 수 있습니다.", "알림");
            }
            else if (0 < res_store_reg.m_ret_msg.Length)
            {
                FormPopupNotify.Show(this.Owner, res_store_reg.m_ret_msg, "알림");
                return;
            }
            else
            {
                switch (res_store_reg.m_ret_cd)
                {
                    case 1:
                        FormPopupNotify.Show(this.Owner, "인증키가 잘못되었습니다. ( 인증키 오류 )", "알림");
                        return;
                    case 2:
                        FormPopupNotify.Show(this.Owner, "상점코드가 중복등록 되었습니다. ( 상점 코드 중복 )", "알림");
                        return;
                }
            }

            // set control
            setDlgObjectDataToControls();
        }

        private void ctr_btn_req_store_del_Click(object sender, EventArgs e)
        {
            String req_store_pno = ctr_tbx_store_pno.Text.Trim().Replace("-", "");
            if (0 >= req_store_pno.Length)
            {
                FormPopupNotify.Show(this.Owner, "먼저 사업자번호를 입력 해 주십시오.", "알림");
                return;
            }

            // 이미매핑되어 있는지 확인
            if (0 == m_store_reg_info.m_store_id.Length)
            {
                FormPopupNotify.Show(this.Owner, "해제할 매핑정보가 없습니다.", "알림");
                return;
            }
            else
            {
                if (DialogResult.Yes != MessageBox.Show(this.Owner, "이미 설정하신 매핑정보를 해제 하시겠습니까? \r\n해제 시 해당 주문연동이 중지됩니다.", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
            }

            // 매핑해제 요청
            ObjApiReqStoreDelResult res_store_del = requestShopDel(m_store_api_info.m_store_pno, m_store_reg_info.m_store_id);
            if (null == res_store_del)
            {
                FormPopupNotify.Show(this.Owner, "상점 매핑해제 요청 중 알수 없는 오류가 발생 하였습니다.", "알림");
                return;
            }

            if (0 == res_store_del.m_ret_cd && 0 < res_store_del.m_store_id.Length)
            {
                m_store_api_info.deleteOnDevice();
                m_store_api_info.initObj();

                m_store_reg_info.deleteOnDevice();
                m_store_reg_info.initObj();

                ModelAppDevice device = new ModelAppDevice();
                device.writeConfigString("STOREMAPPINGCHECK", "store_mapping_check", "0");
                FormPopupNotify.Show(this.Owner, "매핑해제 성공하였습니다. 확인 해 주십시오.", "알림");
            }
            else if (0 < res_store_del.m_ret_msg.Length)
            {
                FormPopupNotify.Show(this.Owner, res_store_del.m_ret_msg, "알림");
                return;
            }
            else
            {
                switch (res_store_del.m_ret_cd)
                {
                    case 1:
                        FormPopupNotify.Show(this.Owner, "인증키가 잘못되었습니다. ( 인증키 오류 )", "알림");
                        return;
                    case 2:
                        FormPopupNotify.Show(this.Owner, "상점코드가 중복등록 되었습니다. ( 상점 코드 중복 )", "알림");
                        return;
                }
            }

            // set control
            setDlgObjectDataToControls();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
