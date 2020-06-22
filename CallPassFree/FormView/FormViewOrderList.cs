using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Views.Grid;
using Kons.Utility;
using Kons.TsLibrary;
using Kons.ShopCallpass.Model;
using Kons.ShopCallpass.FormDialog;
using Kons.ShopCallpass.AppMain;
using Kons.ShopCallpass.Object;
using Kons.ShopCallpass.FormPopup;

namespace Kons.ShopCallpass.FormView
{
    public partial class FormViewOrderList : FormViewBase
    {
        static private string GRID_VIEW_LAYOUT_FILE = "LayoutOrderList.xml";

        private String m_search_filter_text = String.Empty;

        private bool[] m_is_show_order_state = new bool[(int)ObjOrder.STATE_TYPE.ORDER_STATE_END];

        private Int32 m_timer_tick = 0;
        private bool m_flag_need_redraw_list = false;

        private String m_next_focus_order_num = null;

        private Pen m_focus_pen = null;

        private ObjMainWndOrderReport m_order_report = new ObjMainWndOrderReport();
        private object m_lock_datasource = new object();

        public FormViewOrderList()
        {
            // base form data
            setBaseFormData(null, VIEW_TYPE.VIEW_ORDER_LIST);

            // form component init
            InitializeComponent();

            // init
            initLocalVariable();
            initPopupMenu();
            initView();

            // 기본 값 셋팅
            timer_watch_dog.Interval = 1000; //스케쥴 간격을 1초로 준 것이다.
            timer_watch_dog.Tick += new EventHandler(timerWatchdog_Tick);
            timer_watch_dog.Start(); //타이머를 발동시킨다.

            // 초기값 불러오기
            loadReportControlData();

            (gridControl.MainView as GridView).RowHeight = 45;
        }

        private void timerWatchdog_Tick(object sender, System.EventArgs e)
        {
            if (m_flag_need_redraw_list)
            {
                redrawReportControlData();
                m_flag_need_redraw_list = false;
            }

            if (0 >= m_timer_tick--)
            {
                m_timer_tick = 1;
            }
        }

        private void FormViewOrderList_Shown(object sender, EventArgs e)
        {

        }

        private void FormViewOrderList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Kons.ShopCallpass.AppMain.AppCore.APP_MODE.MODE_EXIT == Kons.ShopCallpass.AppMain.AppCore.Instance.getCurrentAppMode())
            {
                if (null != gridView)
                {
                    KnDevexpressFunc.gridviewLayoutSaveToXml(gridView, GRID_VIEW_LAYOUT_FILE);
                }

                e.Cancel = false;
                return;
            }

            if (CloseReason.UserClosing != e.CloseReason)
            {
                return;
            }

            e.Cancel = true;
            this.Hide();
        }

        public void initLocalVariable()
        {
            for(int i=0; i< m_is_show_order_state.Length; i++)
            {
                m_is_show_order_state[i] = true;
            }
        }

        public void initView()
        {
            if (null == gridView)
            {
                Debug.Assert(false);
            }

            // style - common
            KnDevexpressFunc.gridviewSetCommonStyle(gridControl, this.Font);

            // Row에 색이 있기 때문에 인디케이터 등 제거함 - 포커싱된 Row 테두리는 gridView_CustomDrawCell 에서 처리
            gridView.OptionsSelection.EnableAppearanceHideSelection = true;
            gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView.OptionsSelection.EnableAppearanceFocusedRow = false; // 설정 시 포커싱한 Row 전체가 색이 바뀐다.

            gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            gridView.OptionsView.ShowIndicator = false;

            // load saved setting
            if (null != gridView)
            {
                // clear columns
                KnDevexpressFunc.gridviewColumnClearAll(gridView);

                // set columns
                KnDevexpressFunc.gridviewColumnAdd(gridView, "순번", "display_index", 45, true, DevExpress.Utils.HorzAlignment.Far);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "주문일시", "call_datetime", 90, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "연동타입", "order_type", 90, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "주문번호", "order_num", 120, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "상태", "state_cd", 95, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "고객번호", "call_num", 110, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "결제방법", "customer_pay_type_cd", 70, true, DevExpress.Utils.HorzAlignment.Center);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "주문금액", "customer_cost", 70, true, DevExpress.Utils.HorzAlignment.Far);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "요청메모", "customer_request_memo", 100, true, DevExpress.Utils.HorzAlignment.Near);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "배달주소", "arv_locate_address", 270, true, DevExpress.Utils.HorzAlignment.Near);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "배달주소", "arv_locate_alternative_address", 270, true, DevExpress.Utils.HorzAlignment.Near);
                KnDevexpressFunc.gridviewColumnAdd(gridView, "기타메모", "arv_locate_memo", 200, true, DevExpress.Utils.HorzAlignment.Near);

                KnDevexpressFunc.gridviewLayoutRestoreFromXml(gridView, GRID_VIEW_LAYOUT_FILE);

                //// basic sorting
                //gridView.ClearSorting();
                //if (null != gridView.Columns["call_datetime"])
                //{
                //    gridView.Columns["call_datetime"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                //}

                // clear Filter
                clearFilterState();
                clearFilterControl();
            }
        }

        private void initPopupMenu()
        {
            try
            {
                ArrayList link_array = new ArrayList();
                link_array.Clear();

                link_array.Add(KnDevexpressFunc.createPopupmenuLinkPersistInfo("등록주문 상세보기", "popup_btn_detail", Properties.Resources.selectall_16x16, this.popupMenu_ItemClick, false));
                link_array.Add(KnDevexpressFunc.createPopupmenuLinkPersistInfo("등록주문 출력하기", "popup_btn_print", Properties.Resources.print_16x16, this.popupMenu_ItemClick, false));
                link_array.Add(KnDevexpressFunc.createPopupmenuLinkPersistInfo("배달대행 요청하기", "popup_btn_state_1", ObjOrder.getStateImage((int)ObjOrder.STATE_TYPE.ORDER_STATE_1), this.popupMenu_ItemClick, true));

                KnDevexpressFunc.setPopupmenuLinkPersistInfo(popupMenu, link_array);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                throw;
            }
        }

        private void loadReportControlData()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                DataTable view_table = getGridViewDataSource();
                view_table.Clear();
                //DataTable result_dt = null;

                //using (ModelDBServer.MyDbConnection my_db = ModelDBServer.getConnection())
                //{
                //    if (my_db.openDB())
                //    {
                //        my_db.addParameter("@_login_key", AppMain.AppCore.Instance.getLoginUserLoginKey());
                //        my_db.addParameter("@_sel_company_id", m_sel_company_id);
                //        //my_db.addParameter("@_from_ymd", "");
                //        //my_db.addParameter("@_to_ymd", "");
                //        //my_db.addParameter("@_shop_id", 0);

                //        result_dt = my_db.execDatatableProcedure("ccp_Order_List");
                //    }
                //}

                //if (null != result_dt)
                //{
                //    setGridViewDataSource(result_dt);
                //}
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
                this.Cursor = Cursors.Default;
            }

            this.Cursor = Cursors.Default;
        }

        private void redrawListControl(bool _immediatel = false)
        {
            // 현재 이 프로젝트는 주문이 많지 않기 때문에 바로바로 그리게 한다.
            //if (_immediatel)
            {
                redrawReportControlData();
            }
            //else
            //{
            //    m_flag_need_redraw_list = true;
            //}
        }

        public void redrawReportControlData()
        {
            try
            {
                // filter state
                setFilterState();

                // filter string
                setFilterControl();

                // refresh
                //gridControl.Refresh();
                gridView.RefreshData();

                // focus
                if (null != m_next_focus_order_num && 0 < m_next_focus_order_num.Length)
                {
                    int row = gridView.LocateByValue("order_num", m_next_focus_order_num, null);
                    gridView.FocusedRowHandle = row;

                    m_next_focus_order_num = null;
                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        public void setGridViewDataSource(DataTable _dt)
        {
            lock (m_lock_datasource)
            {
                ObjOrder.makeTableSchema(ref _dt); // set table columns with ObjOrder class
                gridControl.DataSource = _dt;
            }
        }

        public DataTable getGridViewDataSource()
        {
            lock (m_lock_datasource)
            {
                DataTable dt = (DataTable)gridControl.DataSource;
                if (null == dt)
                {
                    dt = new DataTable();
                    if (null != dt)
                    {
                        ObjOrder.makeTableSchema(ref dt); // set table columns with ObjOrder class
                        gridControl.DataSource = dt;
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }

                return dt;
            }
        }

        public bool isShowOrder(ObjOrder _obj)
        {
            // 보인다
            return true;
        }

        public void onObjDelete(String _obj_key)
        {
            DataTable view_table = getGridViewDataSource();
            if (null != view_table)
            {
                DataRow[] sel_rows = view_table.Select(String.Format("order_num = '{0}'",_obj_key));
                for (int i = 0; i < sel_rows.Length; i++)
                {
                    sel_rows[i].Delete();
                }
                view_table.AcceptChanges();
            }
        }

        public void onObjChange(String _obj_key)
        {
            Utility.Utility.LogWrite("onChangeObj인자1개짜리");
            ObjOrder recv_obj = new ObjOrder();
            if (null != recv_obj)
            {
                recv_obj.initObj();
                if (recv_obj.loadFromDB(_obj_key))
                {
                    onChangeObj(recv_obj);
                }
            }
        }

        public void onCallbackOrderAdd(FormDlgBase.DLG_TYPE _who, ObjOrder _obj)
        {
            ObjOrder des_obj = (ObjOrder)_obj;
            if (null != des_obj)
            {
                onChangeObj(des_obj);

                // 수정한 것은 redraw 이후 포키싱이 되도록 order_num 를 저장 해 둔다. 
                m_next_focus_order_num = des_obj.m_order_num;
            }
        }

        public void onChangeObj(ObjOrder _obj, bool _is_change_state = false)
        {
            Utility.Utility.LogWrite("onChangeObj인자2개짜리");
            try
            {
                // save
                Kons.ShopCallpass.AppMain.AppCore.Instance.getAppDoc().setObjOrder(_obj);

                // check visible
                if (!isShowOrder(_obj))
                {
                    return;
                }

                // control
                DataTable view_table = getGridViewDataSource();
                if (null != view_table)
                {
                    // 기존 객체 얻기
                    DataRow[] sel_rows = null;
                    try
                    {
                        sel_rows = view_table.Select(String.Format("order_num = '{0}'", _obj.m_order_num));
                    }
                    catch(Exception err)
                    {
                        TsLog.writeLog(err.Message);
                    }

                    // 기존 없으면 생성
                    if (null == sel_rows || 0 == sel_rows.Length)
                    {
                        sel_rows = new DataRow[1];
                        if (null != sel_rows)
                        {
                            sel_rows[0] = view_table.NewRow();
                        }
                        view_table.Rows.InsertAt(sel_rows[0], 0);
                    }
                    else
                    {
                        if (!_is_change_state)
                        {
                            // 상태변경을 해야하는 경우(배달요청후)가 아니면 기존 상태를 안바뀌게 하기위해 아래 채우기전에 이전값으로 넣음
                            // 중복으로 연계 눌렀을경우 상태가 변경되는것을 막기위해 기본적으로 이전 상태를 유지한다.
                            _obj.m_state_cd = KnUtil.parseInt32(sel_rows[0]["state_cd"].ToString());  
                        }
                    }

                    // 새로 받은 내용 채우기
                    if (null != sel_rows[0])
                    {
                        ObjOrder.setTableData(ref sel_rows[0], ref _obj); // obj -> data row
                    }

                    // 다시 그리기
                    redrawListControl();
                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        //private ObjOrder getSelectedOrder(String _order_num)
        //{
        //    DataTable view_table = getGridViewDataSource();
        //    if (null != view_table)
        //    {
        //        DataRow[] sel_rows = null;
        //        try
        //        {
        //            sel_rows = view_table.Select(String.Format("order_num = '{0}'", _order_num));
        //        }
        //        catch (Exception err)
        //        {
        //            TsLog.writeLog(err.Message);
        //        }
        //        if (null != sel_rows[0])
        //        {
        //            ObjOrder sel_order = new ObjOrder();
        //            sel_order.setObj(sel_rows[0]);
        //            return sel_order;
        //        }
        //    }
        //    return null;
        //}

        private void setFilterState()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (null == gridView || null == gridView.Columns["state_cd"] || null == gridView.Columns["state_cd"].FilterInfo)
                {
                    return;
                }

                bool is_show_order_state_all = isCheckedStateAll();
                if (is_show_order_state_all)
                {
                    gridView.Columns["state_cd"].ClearFilter();
                }
                else
                {
                    String order_state_filter = String.Empty;
                    for (int i=0; i< m_is_show_order_state.Length; i++)
                    {
                        if (m_is_show_order_state[i])
                        {
                            if (order_state_filter.Length > 0)
                            {
                                order_state_filter += ",";
                            }
                            order_state_filter += i.ToString();
                        }
                    }
                    gridView.Columns["state_cd"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("[state_cd] in (" + order_state_filter + ")");
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
                this.Cursor = Cursors.Default;
            }
        }

        public void clearFilterState()
        {
            try
            {
                for (int i = 0; i < m_is_show_order_state.Length; i++)
                {
                    m_is_show_order_state[i] = true;
                }
                gridView.Columns["state_cd"].ClearFilter();
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        private void setFilterControl()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                gridView.FindFilterText = m_search_filter_text;
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void clearFilterControl()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                gridView.FindFilterText = "";
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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
                switch (e.Column.FieldName)
                {
                    case "order_type":
                        {
                            int order_type = ((null == e.Value || 0 == e.Value.ToString().Length) ? 0 : (int)e.Value);
                            e.DisplayText = ObjOrder.getOrderTypeString(order_type);
                        }
                        break;
                    case "call_datetime":
                        {
                            DateTime datetime = ((null == e.Value || 8 > e.Value.ToString().Length) ? DateTime.MinValue : (DateTime)e.Value);
                            e.DisplayText = (DateTime.MinValue == datetime ? "" : datetime.ToString("MM-dd HH:mm"));
                        }
                        break;
                    case "date_1":
                        {
                            DateTime datetime = ((null == e.Value || 8 > e.Value.ToString().Length) ? DateTime.MinValue : (DateTime)e.Value);
                            e.DisplayText = (DateTime.MinValue == datetime ? "" : datetime.ToString("HH:mm"));
                        }
                        break;
                    case "date_2":
                        {
                            DateTime datetime = ((null == e.Value || 8 > e.Value.ToString().Length) ? DateTime.MinValue : (DateTime)e.Value);
                            e.DisplayText = (DateTime.MinValue == datetime ? "" : datetime.ToString("HH:mm"));
                        }
                        break;
                    case "date_3":
                        {
                            DateTime datetime = ((null == e.Value || 8 > e.Value.ToString().Length) ? DateTime.MinValue : (DateTime)e.Value);
                            e.DisplayText = (DateTime.MinValue == datetime ? "" : datetime.ToString("HH:mm"));
                        }
                        break;
                    case "date_4":
                        {
                            DateTime datetime = ((null == e.Value || 8 > e.Value.ToString().Length) ? DateTime.MinValue : (DateTime)e.Value);
                            e.DisplayText = (DateTime.MinValue == datetime ? "" : datetime.ToString("HH:mm"));
                        }
                        break;
                    case "date_5":
                        {
                            DateTime datetime = ((null == e.Value || 8 > e.Value.ToString().Length) ? DateTime.MinValue : (DateTime)e.Value);
                            e.DisplayText = (DateTime.MinValue == datetime ? "" : datetime.ToString("HH:mm"));
                        }
                        break;
                    case "date_6":
                        {
                            DateTime datetime = ((null == e.Value || 8 > e.Value.ToString().Length) ? DateTime.MinValue : (DateTime)e.Value);
                            e.DisplayText = (DateTime.MinValue == datetime ? "" : datetime.ToString("HH:mm"));
                        }
                        break;
                    case "date_7":
                        {
                            DateTime datetime = ((null == e.Value || 8 > e.Value.ToString().Length) ? DateTime.MinValue : (DateTime)e.Value);
                            e.DisplayText = (DateTime.MinValue == datetime ? "" : datetime.ToString("HH:mm"));
                        }
                        break;
                    case "customer_pay_type_cd":
                        {
                            int customer_pay_type_cd = ((null == e.Value || 0 == e.Value.ToString().Length) ? 0 : (int)e.Value);
                            e.DisplayText = ObjOrder.getCustPayTypeString(customer_pay_type_cd);
                        }
                        break;
                    case "shop_cost":
                        {
                            e.DisplayText = ((null == e.Value || 0 == e.Value.ToString().Length) ? "" : KnUtil.formatMoney((int)e.Value));
                        }
                        break;
                    case "customer_cost":
                        {
                            e.DisplayText = ((null == e.Value || 0 == e.Value.ToString().Length) ? "" : KnUtil.formatMoney((int)e.Value));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                String erro_msg = (null == ex.Message ? "error - OrderList_gridView_CustomColumnDisplayText" : ex.Message);
                TsLog.writeLog(erro_msg);
                Debug.Assert(false, erro_msg);
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
                if (gridView.IsRowSelected(e.RowHandle) || gridView.FocusedRowHandle == e.RowHandle)
                {
                    Rectangle bounds = e.Bounds; // rowInfo.Bounds;
                    bounds.X -= 1;
                    bounds.Width += 2;
                    if (null == m_focus_pen) { m_focus_pen = new Pen(Brushes.OrangeRed, 2); }
                    e.Graphics.DrawLine(m_focus_pen, new Point(bounds.X, bounds.Y), new Point(bounds.X + bounds.Width, bounds.Y));
                    e.Graphics.DrawLine(m_focus_pen, new Point(bounds.X, bounds.Y + bounds.Height), new Point(bounds.X + bounds.Width, bounds.Y + bounds.Height));
                }

                switch (e.Column.FieldName)
                {
                    case "display_index":
                        {
                            e.DisplayText = (gridControl.FocusedView.RowCount - e.RowHandle).ToString();
                        }
                        break;
                    case "state_cd":
                        {
                            //Debug.WriteLine("e.Column.FieldName : " + e.Column.FieldName);
                            //Debug.WriteLine("ObjOrder.getStateString(state_cd) : " + ObjOrder.getStateString(state_cd));
                            int state_cd = (int)e.CellValue;
                            string text = ObjOrder.getStateString(state_cd);
                            Bitmap img = ObjOrder.getStateImage(state_cd);
                            if (img != null)
                            {
                                Point pos = new Point();
                                pos.X = e.Bounds.Location.X + 2;
                                pos.Y = e.Bounds.Location.Y + ((e.Bounds.Height - img.Height) / 2);

                                e.Graphics.DrawImage(img, pos.X, pos.Y, img.Width, img.Height);

                                int margin_x = img.Width + 4;
                                Rectangle rect = new Rectangle(e.Bounds.X + margin_x, e.Bounds.Y, e.Bounds.Width - margin_x, e.Bounds.Height);
                                e.Appearance.DrawString(e.Cache, text, rect);

                                e.Handled = true;
                            }
                        }
                        break;
                    case "call_num":
                        {
                            e.DisplayText = ((null == e.CellValue || 8 > e.CellValue.ToString().Length) ? e.CellValue.ToString() : KnUtil.formatTelNumber(e.CellValue.ToString()));
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                String erro_msg = (null == ex.Message ? "error - OrderList_gridView_CustomDrawCell" : ex.Message);
                TsLog.writeLog(erro_msg);
                //Debug.Assert(false, erro_msg);
            }
        }

        private void gridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            // row handle & sender check
            if (gridView.FocusedRowHandle < 0 && sender.GetType() != typeof(GridView))
            {
                return;
            }

            // left button double click
            if (2 == e.Clicks && MouseButtons.Left == e.Button)
            {
                String order_num = gridView.GetDataRow(e.RowHandle)["order_num"].ToString();
                showDlgModifyObj(order_num);
            }
            // right button single click
            else if (1 == e.Clicks && MouseButtons.Right == e.Button)
            {
                Point pos = PointToScreen(e.Location);
                pos.X += gridControl.Left;
                pos.Y += gridControl.Top;
                popupMenu.ShowPopup(pos);
            }
        }

        private void showDlgModifyObj(String _order_num = "")
        {
            if (0 == _order_num.Length)
            {
                KnDevexpressFunc.showMessage("선택된 주문이 없습니다.", MessageBoxIcon.Information);
                return;
            }

            ObjOrder sel_order = Kons.ShopCallpass.AppMain.AppCore.Instance.getAppDoc().getObjOrder(_order_num);
            if (null != sel_order)
            {
                FormDlgOrderDetail dlg = new FormDlgOrderDetail(this, sel_order);
                dlg.ShowDialog();
            }
        }

        private void gridView_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (0 > e.RowHandle && sender.GetType() != typeof(GridView))
            {
                return;
            }

            DataRow dr = gridView.GetDataRow(e.RowHandle);
            if (null != dr && dr.Table.Columns.Contains("state_cd"))
            {
                try
                {
                    int state_cd = KnUtil.parseInt32(dr["state_cd"].ToString());
                    Color row_color = ObjOrder.getStateColor(state_cd);

                    // 색상 적용
                    if (row_color != Color.Empty)
                    {
                        e.Appearance.BackColor = row_color;
                    }
                }
                catch (Exception ex)
                {
                    TsLog.writeLog(ex.Message);
                }

            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!base.ProcessCmdKey(ref msg, keyData))
            {
                switch (keyData)
                {
                    //case Keys.F5:
                    //    ctr_btn_search.PerformClick();
                    //    return true;
                    //case Keys.F6:
                    //    ctr_btn_clear.PerformClick();
                    //    return true;
                    //case Keys.F8:
                    //    ctr_btn_refresh.PerformClick();
                    //    return true;
                    //case Keys.F11:
                    //    ctr_btn_export_excel.PerformClick();
                    //    return true;
                    //case Keys.F12:
                    //    ctr_btn_close.PerformClick();
                    //    return true;
                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void popupMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView.FocusedRowHandle >= 0)
            {
                switch (e.Item.Name)
                {
                    case "popup_btn_detail":
                        {
                            String order_num = gridView.GetFocusedRowCellValue("order_num").ToString();
                            showDlgModifyObj(order_num);
                        }
                        break;
                    case "popup_btn_print":
                        requestOrderPrint();
                        break;
                    case "popup_btn_state_1":
                        requestDelivery();
                        break;
                }
            }
        }

        private void changeOrderState(int _new_state_cd, int _extra_data_int = 0, String _extra_data_var = "")
        {
            if (null == gridView.GetFocusedRowCellValue("order_num"))
            {
                KnDevexpressFunc.showMessage("선택된 주문이 없습니다.", MessageBoxIcon.Information);
                return;
            }

            String order_num = gridView.GetFocusedRowCellValue("order_num").ToString();
            Int32 old_state_cd = KnUtil.parseInt32(gridView.GetFocusedRowCellValue("state_cd").ToString());
        }

        // ----------------------------------------------------------
        //
        public void onCheckStateClick(int _state_cd, bool _is_checked)
        {
            if (-1 == _state_cd) // 전체 선택 시
            {
                bool is_show_order_state_all = isCheckedStateAll();
                for (int i = 0; i < m_is_show_order_state.Length; i++)
                {
                    m_is_show_order_state[i] = !is_show_order_state_all;
                }
            }
            else
            {
                if (_state_cd < m_is_show_order_state.Length)
                {
                    m_is_show_order_state[_state_cd] = _is_checked;
                }
            }

            redrawListControl();
        }

        private bool isCheckedStateAll()
        {
            for (int i = 0; i < m_is_show_order_state.Length; i++)
            {
                if (!m_is_show_order_state[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void searchOrder(int _search_df, int _search_dt, String _search_filter)
        {
            FormPopupNotify.Show(this, "현재 기능을 준비 중 입니다. 추후 업데이트 이후 사용 해 주십시오.", "알림", 3000);

            //m_search_filter_text = _search_filter;
            //redrawListControl();
        }

        public void searchFilterChange(String _search_filter)
        {
            m_search_filter_text = _search_filter;

            redrawListControl();
        }

        public void requestDelivery()
        {
            Utility.Utility.LogWrite("requestDelivery");

            if (null == gridView.GetFocusedRowCellValue("order_num"))
            {
                KnDevexpressFunc.showMessage("선택된 주문이 없습니다.", MessageBoxIcon.Information);
                return;
            }

            String sel_order_num = gridView.GetFocusedRowCellValue("order_num").ToString();
            if (null != sel_order_num)
            {
                requestDelivery(Kons.ShopCallpass.AppMain.AppCore.Instance.getAppDoc().getObjOrder(sel_order_num));
            }
        }

        public void requestDelivery(ObjOrder _sel_order)
        {
            Utility.Utility.LogWrite("requestDelivery 재정의 함수");

            if (null == _sel_order)
            {
                KnDevexpressFunc.showMessage("잘못된 주문입니다. 주문을 선택 후 출력하기를 \n시도 해 주십시오.", MessageBoxIcon.Information);
                return;
            }

            if (null != _sel_order)
            {
                if ((int)ObjOrder.STATE_TYPE.ORDER_STATE_0 != _sel_order.m_state_cd)
                {
                    if (DialogResult.Yes != MessageBox.Show("선택한 주문은 이미 배달대행을 요청한 건 입니다. \r다시 배달 요청을 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        return;
                    }
                }

                FormDlgRequestDelivery dlg = new FormDlgRequestDelivery(this, _sel_order);
                dlg.ShowDialog();
            }
        }

        public void requestOrderPrint()
        {
            if (null == gridView.GetFocusedRowCellValue("order_num"))
            {
                KnDevexpressFunc.showMessage("선택된 주문이 없습니다.", MessageBoxIcon.Information);
                return;
            }

            Debug.WriteLine("주문출력");
            String sel_order_num = gridView.GetFocusedRowCellValue("order_num").ToString();
            if (null != sel_order_num)
            {
                Debug.WriteLine("들");
                requestOrderPrint(Kons.ShopCallpass.AppMain.AppCore.Instance.getAppDoc().getObjOrder(sel_order_num));
            }
        }

        public void requestOrderPrint(ObjOrder _sel_order)
        {
            if (null == _sel_order)
            {
                Debug.WriteLine("homg");
                KnDevexpressFunc.showMessage("잘못된 주문입니다. 주문을 선택 후 출력하기를 \n시도 해 주십시오.", MessageBoxIcon.Information);
                return;
            }

            if (null == _sel_order.m_print_port_num
                || 0 == _sel_order.m_print_port_num.Length)
            {
                Debug.WriteLine("jung");
                KnDevexpressFunc.showMessage("해당 주문의 주문연동 설정된 출력포트는 프린트 설정이 \n안되어 있습니다.", MessageBoxIcon.Information);
                return;
            }

            if (null == _sel_order.m_print_raw_data_buf
                || 0 == _sel_order.m_print_raw_data_buf.Length )
            {
                Debug.WriteLine("sun");
                KnDevexpressFunc.showMessage("선택된 주문은 출력할 데이터가 없습니다.", MessageBoxIcon.Information);
                return;
            }

            FormMain main_form = Kons.ShopCallpass.AppMain.AppCore.Instance.Mainform;
            if (null != main_form)
            {
                Debug.WriteLine("bab");
                main_form.printOrder(_sel_order);
            }
        }
    }
}