using Kons.ShopCallpass.Object;
using Kons.TsLibrary;
using Kons.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormStoreInfoSelecter : FormDlgBase
    {
        private object m_dlg_obj = null;
        private Dictionary<String, ObjConfigStoreApiInfo> m_dic_store_info = new Dictionary<string, ObjConfigStoreApiInfo>();

        public ObjConfigStoreApiInfo m_sel_obj = null;

        // ---------------------------------------------------------- basic method
        //
        public FormStoreInfoSelecter(Form _parnet = null, object _sel_obj = null)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();

            m_dlg_obj = _sel_obj;

            initDlgObjects();
            initDlgView();
            initDlgControls();
        }

        private void FormDlg_Load(object sender, EventArgs e)
        {
            loadReportControlData();
        }

        // ---------------------------------------------------------- BaseForm override - init, set data ...
        //
        override protected void initDlgObjects()
        {
        }

        override protected void initDlgView()
        {
            if (null == gridView)
            {
                Debug.Assert(false);
            }

            // style - common
            KnDevexpressFunc.gridviewSetCommonStyle(gridControl, this.Font);

            gridView.OptionsView.ColumnAutoWidth = true;
            gridView.OptionsView.ShowIndicator = false;

            // load saved setting
            if (null != gridView)
            {
                // clear columns
                KnDevexpressFunc.gridviewColumnClearAll(gridView);

                // set columns
                KnDevexpressFunc.gridviewColumnAdd(gridView, "순번", "display_index", 45, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "상점번호", "store_num", 70, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "상점명", "store_name", 120, true, DevExpress.Utils.HorzAlignment.Near);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "전화번호", "store_tel", 90, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "주소", "store_addr", 235, true, DevExpress.Utils.HorzAlignment.Near);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "사업자번호", "store_pno", 80, true, DevExpress.Utils.HorzAlignment.Center);

                // basic sorting
                gridView.ClearSorting();
                if (null != gridView.Columns["store_name"])
                {
                    gridView.Columns["store_name"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
            }
        }

        override protected void initDlgControls()
        {
        }

        // ----------------------------------------------------------
        //
        private DataTable getGridViewDataSource()
        {
            DataTable dt = (DataTable)gridControl.DataSource;
            if (null == dt)
            {
                dt = new DataTable();
                if (null != dt)
                {
                    ObjConfigStoreApiInfo.makeTableSchema(ref dt); // set table columns with ObjOrder class
                    gridControl.DataSource = dt;
                }
                else
                {
                    Debug.Assert(false);
                }
            }

            return dt;
        }

        public void loadReportControlData()
        {
            // set container
            ArrayList store_list = (ArrayList)m_dlg_obj;

            // check data list
            if (null == store_list)
            {
                return;
            }

            // check view datasource
            DataTable view_table = getGridViewDataSource();
            if (null == view_table)
            {
                return;
            }

            // reset
            m_dic_store_info.Clear();

            // set 
            for (int i=0; i<store_list.Count; i++)
            {
                ObjConfigStoreApiInfo sel_obj = (ObjConfigStoreApiInfo)store_list[i];
                if (null != sel_obj)
                {
                    DataRow sel_row = view_table.NewRow();
                    if (null != sel_row)
                    {
                        ObjConfigStoreApiInfo.setTableData(ref sel_row, ref sel_obj); // obj -> data row
                        view_table.Rows.Add(sel_row);

                        // 관리목록에 넣기
                        m_dic_store_info.Add(sel_obj.m_store_num, sel_obj);
                    }
                }
            }

            // refresh
            gridView.RefreshData();
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            // row handle & sender check
            if (gridView.FocusedRowHandle < 0 && sender.GetType() != typeof(GridView))
            {
                return;
            }

            try
            {
                //switch (e.Column.FieldName)
                //{
                //    case "call_datetime":
                //        {
                //            DateTime datetime = ((null == e.Value || 8 > e.Value.ToString().Length) ? DateTime.MinValue : (DateTime)e.Value);
                //            e.DisplayText = (DateTime.MinValue == datetime ? "" : datetime.ToString("MM-dd HH:mm"));
                //        }
                //        break;
                //}
            }
            catch (Exception)
            {
            }
        }

        private void gridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (gridView.FocusedRowHandle < 0 && sender.GetType() != typeof(GridView))
            {
                return;
            }

            try
            {
                switch (e.Column.FieldName)
                {
                    case "display_index":
                        {
                            e.DisplayText = (gridControl.FocusedView.RowCount - e.RowHandle).ToString();
                        }
                        break;
                    case "store_tel":
                        {
                            e.DisplayText = ((null == e.CellValue || 8 > e.CellValue.ToString().Length) ? e.CellValue.ToString() : KnUtil.formatTelNumber(e.CellValue.ToString()));
                        }
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void gridView_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (0 > e.RowHandle && sender.GetType() != typeof(GridView))
            {
                return;
            }

            DataRow dr = gridView.GetDataRow(e.RowHandle);
            if (null != dr && dr.Table.Columns.Contains("display_index"))
            {
                try
                {
                    // 색상 적용
                    int display_index = KnUtil.parseInt32(dr["display_index"].ToString());
                    if (0 == (display_index % 2))
                    {
                        e.Appearance.BackColor = Color.FromArgb(253, 253, 253);
                    }
                }
                catch (Exception ex)
                {
                    TsLog.writeLog(ex.Message);
                }

            }
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            // row handle & sender check
            if (gridView.FocusedRowHandle < 0 && sender.GetType() != typeof(GridView))
            {
                return;
            }

            setGridViewFocusedRowObject();
        }

        private ObjConfigStoreApiInfo getSelObject(String _store_num)
        {
            if (null == _store_num || 0 == _store_num.Length)
                {
                return null;
            }

            if (m_dic_store_info.ContainsKey(_store_num))
            {
                return m_dic_store_info[_store_num];
            }

            return null;
        }

        private void setGridViewFocusedRowObject()
        {
            object order_num = gridView.GetFocusedRowCellValue("store_num");
            if (null != order_num)
            {
                m_sel_obj = getSelObject(order_num.ToString());
                if (null != m_sel_obj)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void ctr_btn_save_Click(object sender, EventArgs e)
        {
            setGridViewFocusedRowObject();
        }

        private void ctr_btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
