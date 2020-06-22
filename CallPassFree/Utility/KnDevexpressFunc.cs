using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid.Helpers;

namespace Kons.Utility
{
    public class KnDevexpressFunc
    {
        public class TsListBoxItem
        {
            public String m_text { get; set; }
            public Object m_obj { get; set; }

            public TsListBoxItem(String _text, Object _obj)
            {
                m_text = _text;
                m_obj = _obj;
            }

            public override String ToString()
            {
                return m_text;
            }
        }

        public class TsDevExpressComboItem
        {
            public String m_value { get; set; }
            public String m_key { get; set; }

            public TsDevExpressComboItem(String _key, String _value)
            {
                m_key = _key;
                m_value = _value;
            }

            public override String ToString()
            {
                return m_value;
            }
        }

        static public void setComboboxCommonStyle(DevExpress.XtraEditors.ComboBoxEdit _control)
        {
            _control.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            _control.Properties.DropDownRows = 20;
        }

        static public DevExpress.XtraGrid.Views.Grid.GridView gridviewSetCommonStyle(DevExpress.XtraGrid.GridControl _grid_ctrl, Font _font)
        {
            // set style
            DevExpress.XtraGrid.Views.Grid.GridView grid_view = (DevExpress.XtraGrid.Views.Grid.GridView)(_grid_ctrl.MainView);
            if (null != grid_view)
            {
                grid_view.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;

                grid_view.OptionsBehavior.Editable = false;
                grid_view.OptionsFilter.AllowFilterEditor = false;

                //grid_view.OptionsMenu.EnableColumnMenu = false;  // 소팅 초기화가 필요하므로 숨기지 않는다.

                grid_view.OptionsView.ShowIndicator = false;
                grid_view.OptionsView.ShowGroupPanel = false;
                grid_view.OptionsView.ColumnAutoWidth = false;
                grid_view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;

                grid_view.OptionsSelection.EnableAppearanceHideSelection = true;
                grid_view.OptionsSelection.EnableAppearanceFocusedCell = false;
                grid_view.OptionsSelection.EnableAppearanceFocusedRow = true; // 설정 시 포커싱한 Row 전체가 색이 바뀐다.

                grid_view.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
                grid_view.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;

                grid_view.OptionsMenu.ShowAutoFilterRowItem = false;
                grid_view.OptionsView.ShowAutoFilterRow = false;

                grid_view.OptionsFilter.AllowAutoFilterConditionChange = DefaultBoolean.False;
                grid_view.OptionsFilter.AllowColumnMRUFilterList = false;
                grid_view.OptionsFilter.AllowFilterEditor = false;
                grid_view.OptionsFilter.AllowFilterIncrementalSearch = false;
                grid_view.OptionsFilter.AllowMRUFilterList = false;
                grid_view.OptionsFilter.AllowMultiSelectInCheckedFilterPopup = false;

                grid_view.OptionsFind.AllowFindPanel = false;
                grid_view.OptionsFind.AlwaysVisible = false;

                // set font
                foreach (AppearanceObject ap in grid_view.Appearance)
                {
                    ap.Font = _font;
                }

                // height
                grid_view.RowHeight = 21;
            }
            return grid_view;
        }

        static public void gridviewSetGroupSummaryStyle(DevExpress.XtraGrid.GridControl _grid_ctrl)
        {
            // set style
            DevExpress.XtraGrid.Views.Grid.GridView grid_view = (DevExpress.XtraGrid.Views.Grid.GridView)(_grid_ctrl.MainView);
            if (null != grid_view)
            {
                grid_view.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
                grid_view.OptionsView.ShowGroupPanel = false;
                grid_view.OptionsMenu.EnableFooterMenu = false;
                grid_view.GroupRowHeight = 0;
            }
        }

        static public String LAYOUT_DIRECTORY_NM = "layout";
        static public bool gridviewLayoutInit()
        {
            // check directory
            DirectoryInfo _dinfo = new DirectoryInfo(LAYOUT_DIRECTORY_NM);
            if (_dinfo.Exists)
            {
                try
                {
                    _dinfo.Delete(true);
                    return true;
                }
                catch (Exception) { }
            }
            return false;
        }

        static public void gridviewLayoutRestoreFromXml(DevExpress.XtraGrid.Views.Grid.GridView _grid_view, String _xml_file_name)
        {
            if (null != _grid_view)
            {
                string layout_directory = System.Windows.Forms.Application.StartupPath + "\\" + LAYOUT_DIRECTORY_NM;

                // set need option
                _grid_view.OptionsLayout.Columns.StoreLayout = true;

                // check dirctory
                DirectoryInfo _dinfo = new DirectoryInfo(layout_directory);
                if (!_dinfo.Exists)
                {
                    try
                    {
                        _dinfo.Create();
                    }
                    catch (Exception) { return; }
                }

                // restore layout
                String layout_file_path = layout_directory + "\\" + _xml_file_name.Replace("./", "");
                FileInfo _finfo = new FileInfo(layout_file_path);
                if (_finfo.Exists)
                {
                    try
                    {
                        _grid_view.RestoreLayoutFromXml(layout_file_path);
                    }
                    catch (Exception) { }
                }
            }
        }

        static public void gridviewLayoutSaveToXml(DevExpress.XtraGrid.Views.Grid.GridView _grid_view, String _xml_file_name)
        {
            if (null != _grid_view)
            {
                string layout_directory = System.Windows.Forms.Application.StartupPath + "\\" + LAYOUT_DIRECTORY_NM;

                // check directory
                DirectoryInfo _dinfo = new DirectoryInfo(layout_directory);
                if (!_dinfo.Exists)
                {
                    try
                    {
                        _dinfo.Create();
                    }
                    catch (Exception) { return; }
                }

                // save layout
                String layout_file_path = layout_directory + "\\" + _xml_file_name.Replace("./", "");
                try
                {
                    _grid_view.SaveLayoutToXml(layout_file_path);
                }
                catch (Exception) { }
            }
        }

        static public void gridviewSetImages(DevExpress.XtraGrid.Views.Grid.GridView _view, DevExpress.Utils.ImageCollection _imgs)
        {
            if (null != _view)
            {
                _view.Images = _imgs;
            }
        }

        static public void gridviewColumnClearAll(DevExpress.XtraGrid.Views.Grid.GridView _view)
        {
            if (null != _view)
            {
                _view.Columns.Clear();
            }
        }

        static public DevExpress.XtraGrid.Columns.GridColumn gridviewColumnAdd(DevExpress.XtraGrid.Views.Grid.GridView _view, String _caption, String _fild_name, int _width, bool _visible, DevExpress.Utils.HorzAlignment _text_align, bool _isSortable = true, bool _isEditable = false)
        {
            if (null != _view)
            {
                DevExpress.XtraGrid.Columns.GridColumn new_column = new DevExpress.XtraGrid.Columns.GridColumn();

                if (null != new_column)
                {
                    _view.Columns.Add(new_column);

                    new_column.Name = _caption;
                    new_column.Caption = _caption;
                    new_column.FieldName = _fild_name;
                    new_column.Width = _width;
                    new_column.Visible = _visible;
                    new_column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    new_column.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    new_column.AppearanceCell.TextOptions.HAlignment = _text_align;
                    new_column.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    new_column.OptionsFilter.AllowFilter = false;
                    new_column.OptionsColumn.AllowEdit = _isEditable;
                    new_column.OptionsColumn.AllowSort = (_isSortable ? DefaultBoolean.True : DefaultBoolean.False);
                }

                return new_column;
            }

            return null;
        }

        static public void gridviewCheckColumnAdd(DevExpress.XtraGrid.Views.Grid.GridView _view, String _caption, String _fild_name, int _width, bool _visible, DevExpress.Utils.HorzAlignment _text_align, bool _isSortable = true, bool _isEditable = false)
        {
            if (null != _view)
            {
                DevExpress.XtraGrid.Columns.GridColumn new_column = new DevExpress.XtraGrid.Columns.GridColumn();
                if (null != new_column)
                {
                    _view.Columns.Add(new_column);

                    new_column.Caption = _caption;
                    new_column.FieldName = _fild_name;
                    new_column.Width = _width;
                    new_column.Visible = _visible;
                    new_column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    new_column.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    new_column.AppearanceCell.TextOptions.HAlignment = _text_align;
                    new_column.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    new_column.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;

                    new_column.OptionsFilter.AllowFilter = false;
                    new_column.OptionsColumn.AllowEdit = _isEditable;
                    new_column.OptionsColumn.AllowSort = (_isSortable ? DefaultBoolean.True : DefaultBoolean.False);
                }
            }
        }

        static public void gridviewGroupSummaryAdd(DevExpress.XtraGrid.Views.Grid.GridView _view, String _fild_name, DevExpress.Data.SummaryItemType _summary_item_type, String _display_format)
        {
            if (null != _view)
            {
                DevExpress.XtraGrid.Columns.GridColumn des_column = _view.Columns[_fild_name];
                if (null != des_column)
                {
                    DevExpress.XtraGrid.GridGroupSummaryItem item = new DevExpress.XtraGrid.GridGroupSummaryItem();
                    if (null != item)
                    {
                        item.FieldName = _fild_name;
                        item.SummaryType = _summary_item_type;
                        if (null != _display_format && 0 < _display_format.Length)
                        {
                            item.DisplayFormat = _display_format;
                        }
                        item.ShowInGroupColumnFooter = des_column;

                        _view.GroupSummary.Add(item);
                    }
                }
            }
        }

        static public void gridviewViewSummaryAdd(DevExpress.XtraGrid.Views.Grid.GridView _view, String _fild_name, DevExpress.Data.SummaryItemType _summary_item_type, String _display_format)
        {
            if (null != _view)
            {
                DevExpress.XtraGrid.Columns.GridColumn des_column = _view.Columns[_fild_name];
                if (null != des_column)
                {
                    des_column.Summary.Add(_summary_item_type, _fild_name, _display_format);
                }
            }
        }

        static public int gridviewFindValueRowHandle(DevExpress.XtraGrid.Views.Grid.GridView _grid_view, String _field_name, object _value, bool _isSelect = false)
        {
            int retRowHandle = -1;

            if (_field_name == String.Empty || _value == null || _grid_view == null)
            {
                return retRowHandle;
            }

            //System.Data.DataTable dt = (System.Data.DataTable)_grid_view.GridControl.DataSource;

            //System.Data.DataRow[] dtRows;
            //dtRows = dt.Select(String.Format("{0} = {1}", _field_name, _value.ToString()));

            //if (dtRows.Count() > 0)
            //{
            //    //dtRows[0];
            //}

            for (int i = 0; i < _grid_view.RowCount; i++)
            {
                object temp_obj;
                temp_obj = _grid_view.GetRowCellValue(i, _field_name);

                if (temp_obj.Equals(_value))
                {
                    retRowHandle = i;
                    break;
                }
            }

            if (_isSelect)
            {
                _grid_view.SelectRow(retRowHandle);
                _grid_view.FocusedRowHandle = retRowHandle;
            }

            return retRowHandle;
        }

        static public void treelistSetCommonStyle(DevExpress.XtraTreeList.TreeList _treelist, Font _font)
        {
            // set style
            if (null != _treelist)
            {
                _treelist.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;

                _treelist.OptionsBehavior.Editable = false;
                _treelist.OptionsBehavior.EnableFiltering = true;

                _treelist.OptionsView.ShowIndicator = false;
                _treelist.OptionsView.ShowCheckBoxes = false;
                //_treelist.OptionsView.ShowRoot = false;
                _treelist.OptionsView.AutoWidth = false;
                _treelist.OptionsView.ShowIndentAsRowStyle = true;
                _treelist.OptionsView.ShowFilterPanelMode = DevExpress.XtraTreeList.ShowFilterPanelMode.Never;

                _treelist.OptionsSelection.SelectNodesOnRightClick = true;

                _treelist.OptionsSelection.EnableAppearanceFocusedCell = false;
                _treelist.OptionsSelection.EnableAppearanceFocusedRow = true;

                _treelist.OptionsBehavior.AllowExpandOnDblClick = false;

                _treelist.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.RowFullFocus;
                _treelist.HorzScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Always;

                _treelist.ActiveFilterEnabled = true;
                _treelist.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Extended;

                // set font
                foreach (AppearanceObject ap in _treelist.Appearance)
                {
                    ap.Font = _font;
                }

                // height
                _treelist.RowHeight = 21;
            }
        }

        static public void treelistColumnClearAll(DevExpress.XtraTreeList.TreeList _view)
        {
            if (null != _view)
            {
                _view.Columns.Clear();
            }
        }

        static public void treelistColumnAdd(DevExpress.XtraTreeList.TreeList _view, String _caption, String _fild_name, int _width, bool _visible, DevExpress.Utils.HorzAlignment _text_align, bool _isSortable = true, bool _isEditable = false)
        {
            //DevExpress.XtraTreeList.ViewInfo.ColumnsInfo col_info = _view.ColumnsInfo;
            if (null != _view)
            {
                DevExpress.XtraTreeList.Columns.TreeListColumn new_column = new DevExpress.XtraTreeList.Columns.TreeListColumn();

                if (null != new_column)
                {
                    _view.Columns.Add(new_column);

                    new_column.Caption = _caption;
                    new_column.FieldName = _fild_name;
                    new_column.Width = _width;
                    new_column.Visible = _visible;
                    new_column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    new_column.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    new_column.AppearanceCell.TextOptions.HAlignment = _text_align;
                    new_column.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    new_column.OptionsFilter.AllowFilter = false;
                    new_column.OptionsColumn.AllowEdit = _isEditable;
                    new_column.OptionsColumn.AllowSort = _isSortable;
                }
            }
        }

        static public DevExpress.XtraBars.BarButtonItem createPopupmenuButton(String _caption, String _name, Bitmap _img, DevExpress.XtraBars.ItemClickEventHandler _handler)
        {
            DevExpress.XtraBars.BarButtonItem popup_item = new DevExpress.XtraBars.BarButtonItem();
            if (null != popup_item)
            {
                popup_item.Caption = _caption;
                popup_item.Name = _name;
                if (null != _img)
                {
                    popup_item.Glyph = _img;
                }
                popup_item.ItemClick += _handler;
            }
            return popup_item;
        }

        static public DevExpress.XtraBars.BarSubItem createPopupmenuSubitem(String _caption, String _name, Bitmap _img)
        {
            DevExpress.XtraBars.BarSubItem popup_item = new DevExpress.XtraBars.BarSubItem();
            if (null != popup_item)
            {
                popup_item.Caption = _caption;
                popup_item.Name = _name;
                if (null != _img)
                {
                    popup_item.Glyph = _img;
                }
            }
            return popup_item;
        }

        static public void setPopupmenuItems(DevExpress.XtraBars.PopupMenu _ctr_popup_menu, ArrayList _item_array)
        {
            if (null == _ctr_popup_menu || null == _item_array)
            {
                return;
            }

            for (int i = 0; i < _item_array.Count; i++)
            {
                if (_item_array[i].GetType() == typeof(DevExpress.XtraBars.BarSubItem))
                {
                    DevExpress.XtraBars.BarSubItem item = (DevExpress.XtraBars.BarSubItem)(_item_array[i]);
                    _ctr_popup_menu.AddItem(item);
                }
                else
                {
                    DevExpress.XtraBars.BarButtonItem item = (DevExpress.XtraBars.BarButtonItem)(_item_array[i]);
                    _ctr_popup_menu.AddItem(item);
                }
            }
        }

        static public DevExpress.XtraBars.LinkPersistInfo createPopupmenuLinkPersistInfo(String _caption, String _name, Bitmap _img, DevExpress.XtraBars.ItemClickEventHandler _handler, Boolean _is_begingroup)
        {
            DevExpress.XtraBars.BarButtonItem popup_item = KnDevexpressFunc.createPopupmenuButton(_caption, _name, _img, _handler);
            return new DevExpress.XtraBars.LinkPersistInfo(popup_item, _is_begingroup);
        }

        static public DevExpress.XtraBars.LinkPersistInfo createPopupmenuLinkPersistInfo(DevExpress.XtraBars.BarButtonItem _popup_item, Boolean _is_begingroup)
        {
            return new DevExpress.XtraBars.LinkPersistInfo(_popup_item, _is_begingroup);
        }

        static public void setPopupmenuLinkPersistInfo(DevExpress.XtraBars.PopupMenu _ctr_popup_menu, ArrayList _link_item_array)
        {
            if (null == _ctr_popup_menu || null == _link_item_array)
            {
                return;
            }

            DevExpress.XtraBars.LinkPersistInfo[] links = new DevExpress.XtraBars.LinkPersistInfo[_link_item_array.Count];
            for (int i = 0; i < _link_item_array.Count; i++)
            {
                DevExpress.XtraBars.LinkPersistInfo link_info = (DevExpress.XtraBars.LinkPersistInfo)(_link_item_array[i]);
                links[i] = link_info;
            }

            _ctr_popup_menu.LinksPersistInfo.AddRange(links);
        }

        static public void ComboBoxSetStyleEditDisable(DevExpress.XtraEditors.ComboBoxEdit _ctrl)
        {
            _ctrl.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        static public void ComboBoxAddItem(DevExpress.XtraEditors.ComboBoxEdit _ctrl, String _key, String _value)
        {
            if (_ctrl != null)
            {
                TsDevExpressComboItem item = new TsDevExpressComboItem(_key, _value);
                _ctrl.Properties.Items.Add(item);
            }
        }

        static public String ComboBoxGetSelectedItemKey(DevExpress.XtraEditors.ComboBoxEdit _ctrl)
        {
            if (null == _ctrl.SelectedItem || _ctrl.SelectedItem.GetType() != typeof(TsDevExpressComboItem))
            {
                return "";
            }

            TsDevExpressComboItem item = (TsDevExpressComboItem)_ctrl.SelectedItem;
            if (item != null)
            {
                return item.m_key;
            }
            else
            {
                return "";
            }
        }

        static public String ComboBoxGetSelectedItemValue(DevExpress.XtraEditors.ComboBoxEdit _ctrl)
        {
            if (null == _ctrl.SelectedItem || _ctrl.SelectedItem.GetType() != typeof(TsDevExpressComboItem))
            {
                return "";
            }

            TsDevExpressComboItem item = (TsDevExpressComboItem)_ctrl.SelectedItem;
            if (item != null)
            {
                return item.m_value;
            }
            else
            {
                return "";
            }
        }

        static public bool ComboBoxSetSelectByKey(DevExpress.XtraEditors.ComboBoxEdit _ctrl, object _key, bool _use_default = true)
        {
            foreach (TsDevExpressComboItem item in _ctrl.Properties.Items)
            {
                if (item.m_key.ToString() == _key.ToString())
                {
                    _ctrl.SelectedItem = item;
                    return true;
                }
            }

            if (_ctrl.Properties.Items.Count > 0 && _use_default)
            {
                _ctrl.SelectedItem = _ctrl.Properties.Items[0];
            }
            return false;
        }

        static public void ComboBoxSetSelectByValue(DevExpress.XtraEditors.ComboBoxEdit _ctrl, object _value, bool _use_default = true)
        {
            foreach (TsDevExpressComboItem item in _ctrl.Properties.Items)
            {
                if (item.m_value.ToString() == _value.ToString())
                {
                    _ctrl.SelectedItem = item;
                    return;
                }
            }

            if (_ctrl.Properties.Items.Count > 0 && _use_default)
            {
                _ctrl.SelectedItem = _ctrl.Properties.Items[0];
            }
        }

        /// <summary>
        /// 포함된 콤보박스에 아이템을 추가할때 사용한다.
        /// 초기값을 선택하거나 선택된 값을 가져올때는 콤보박스를 포함하고 있는 DevExpress.XtraBars.BarEditItem 의 .EditValue 사용.
        /// 즉 
        /// </summary>
        /// <param name="_ctrl"></param>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        static public TsDevExpressComboItem RepositoryItemComboBoxAddItem(DevExpress.XtraEditors.Repository.RepositoryItemComboBox _ctrl, String _key, String _value)
        {
            if (_ctrl != null)
            {
                TsDevExpressComboItem item = new TsDevExpressComboItem(_key, _value);
                _ctrl.Items.Add(item);
                return item;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public class HotTrackCellHelper
        {
            public HotTrackCellHelper(GridView view)
            {
                _View = view;
                view.GridControl.Paint += GridControl_Paint;
                view.MouseMove += view_MouseMove;
            }

            private int _BorderWidth = 4;
            private int _HotTrackedRow;
            private readonly GridView _View;
            public int HotTrackedRow
            {
                get { return _HotTrackedRow; }
                set
                {
                    if (_HotTrackedRow == value)
                        return;
                    if (value < 0)
                        value = GridControl.InvalidRowHandle;
                    RefreshRow(_HotTrackedRow);
                    _HotTrackedRow = value;
                    RefreshRow(_HotTrackedRow);
                }
            }

            public Rectangle GetRowBounds(int rowHandle)
            {
                GridViewInfo info = _View.GetViewInfo() as GridViewInfo;
                GridRowInfo rowInfo = info.RowsInfo.GetInfoByHandle(rowHandle);
                return rowInfo == null ? Rectangle.Empty : rowInfo.Bounds;
            }

            private void UpdateHotTrackedRow(Point location)
            {
                GridHitInfo hi = _View.CalcHitInfo(location);
                if (hi.HitTest == GridHitTest.Row || hi.HitTest == GridHitTest.RowEdge)
                    return;
                if (hi.InRow)
                    HotTrackedRow = hi.RowHandle;
                else
                    HotTrackedRow = GridControl.InvalidRowHandle;
            }

            void view_MouseMove(object sender, MouseEventArgs e)
            {
                UpdateHotTrackedRow(e.Location);
            }

            private void RefreshRow(int row)
            {
                Rectangle rect = GetRowBounds(row);
                rect.Inflate(_BorderWidth, _BorderWidth);
                _View.InvalidateRect(rect);
            }

            void GridControl_Paint(object sender, PaintEventArgs e)
            {
                DrawHotTrackedRow(e);
            }

            private void DrawHotTrackedRow(PaintEventArgs e)
            {
                Rectangle bounds = GetRowBounds(HotTrackedRow);
                e.Graphics.DrawRectangle(new Pen(Brushes.Black, _BorderWidth), bounds);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static class CellDrawHelper
        {
            public static void DrawCellBorder(DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
            {
                Brush brush = Brushes.Black; e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width + 2, 2));
                e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Right - 1, e.Bounds.Y - 1, 2, e.Bounds.Height + 2));
                e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Bottom - 1, e.Bounds.Width + 2, 2));
                e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, 2, e.Bounds.Height + 2));
            }

            public static void DoDefaultDrawCell(GridView view, RowCellCustomDrawEventArgs e)
            {
                e.Appearance.FillRectangle(e.Cache, e.Bounds);
                ((IViewController)view.GridControl).EditorHelper.DrawCellEdit(new GridViewDrawArgs(e.Cache, (view.GetViewInfo() as GridViewInfo), e.Bounds), (e.Cell as GridCellInfo).Editor, (e.Cell as GridCellInfo).ViewInfo, e.Appearance, (e.Cell as GridCellInfo).CellValueRect.Location);
            }
        }

        static public void detectException(String _message, Boolean _isShow = false, Boolean _isSaveLog = false)
        {
            if (_isShow)
            {
                try
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(_message, "예외", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch
                {
                    MessageBox.Show(_message, "예외", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (_isSaveLog)
            {

            }
        }

        static public void detectError(String _message, Boolean _isShow = true, Boolean _isSaveLog = false)
        {
            if (_isShow)
            {
                try
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(_message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    MessageBox.Show(_message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (_isSaveLog)
            {

            }
        }

        static public DialogResult showMessage(String _message)
        {
            try
            {
                return DevExpress.XtraEditors.XtraMessageBox.Show(_message, "알림", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch
            {
                return MessageBox.Show(_message, "알림", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        static public DialogResult showMessage(String _message, MessageBoxIcon _icon)
        {
            try
            {
                return DevExpress.XtraEditors.XtraMessageBox.Show(_message, "알림", MessageBoxButtons.OK, _icon);
            }
            catch
            {
                return MessageBox.Show(_message, "알림", MessageBoxButtons.OK, _icon);
            }
        }

        static public DialogResult showMessage(String _message, MessageBoxButtons _button = MessageBoxButtons.OK, MessageBoxIcon _icon = MessageBoxIcon.Information)
        {
            try
            {
                return DevExpress.XtraEditors.XtraMessageBox.Show(_message, "알림", _button, _icon);
            }
            catch
            {
                return MessageBox.Show(_message, "알림", _button, _icon);
            }
        }

        static public DialogResult showMessage(String _message, String _caption, MessageBoxButtons _button = MessageBoxButtons.OK, MessageBoxIcon _icon = MessageBoxIcon.Information)
        {
            try
            {
                return DevExpress.XtraEditors.XtraMessageBox.Show(_message, _caption, _button, _icon);
            }
            catch
            {
                return MessageBox.Show(_message, _caption, _button, _icon);
            }
        }
    }
}
