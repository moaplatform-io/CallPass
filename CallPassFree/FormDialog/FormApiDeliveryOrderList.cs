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
using System.Windows.Forms;

namespace Kons.ShopCallpass.FormDialog
{
    public partial class FormApiDeliveryOrderList : FormDlgBase
    {
        private object m_dlg_obj = null;

        // ---------------------------------------------------------- basic method
        //
        public FormApiDeliveryOrderList(Form _parnet = null, String _title = null, object _sel_obj = null)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();

            if (null != _title && 0 < _title.Length)
            {
                this.Text = _title;
            }
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
                KnDevexpressFunc.gridviewColumnAdd(gridView, "고객전화", "cust_tel", 90, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "배달주소", "dest_info", 300, true, DevExpress.Utils.HorzAlignment.Near);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "배차회사", "com_info", 120, true, DevExpress.Utils.HorzAlignment.Near);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "배달상태", "order_state", 70, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "배차기사", "rider_info", 120, true, DevExpress.Utils.HorzAlignment.Near);

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
                    ObjApiDeliveryOrder.makeTableSchema(ref dt); // set table columns with ObjOrder class
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

            // set 
            for (int i = 0; i < store_list.Count; i++)
            {
                ObjApiDeliveryOrder sel_obj = (ObjApiDeliveryOrder)store_list[i];
                if (null != sel_obj)
                {
                    DataRow sel_row = view_table.NewRow();
                    if (null != sel_row)
                    {
                        ObjApiDeliveryOrder.setTableData(ref sel_row, ref sel_obj); // obj -> data row
                        view_table.Rows.Add(sel_row);
                    }
                }
            }

            // refresh
            gridView.RefreshData();
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            // row handle & sender check
            if (gridView.FocusedRowHandle < 0 && sender.GetType() != typeof(DevExpress.XtraGrid.Views.Grid.GridView))
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
            if (gridView.FocusedRowHandle < 0 && sender.GetType() != typeof(DevExpress.XtraGrid.Views.Grid.GridView))
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
                    case "com_tel":
                    case "cust_tel":
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
            if (0 > e.RowHandle && sender.GetType() != typeof(DevExpress.XtraGrid.Views.Grid.GridView))
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
            if (gridView.FocusedRowHandle < 0 && sender.GetType() != typeof(DevExpress.XtraGrid.Views.Grid.GridView))
            {
                return;
            }
        }
    }
}
