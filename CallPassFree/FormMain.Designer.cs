namespace Kons.ShopCallpass
{
    partial class FormMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btn_request_remote_help = new DevExpress.XtraBars.BarButtonItem();
            this.btn_order_request_delivery = new DevExpress.XtraBars.BarButtonItem();
            this.order_bar_btn_search = new DevExpress.XtraBars.BarButtonItem();
            this.bar_label_filter_text = new DevExpress.XtraBars.BarStaticItem();
            this.bar_tbx_filter_text = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemFilterText = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btn_program_setup = new DevExpress.XtraBars.BarButtonItem();
            this.bar_chk_order_state_0 = new DevExpress.XtraBars.BarCheckItem();
            this.bar_chk_order_state_1 = new DevExpress.XtraBars.BarCheckItem();
            this.btn_application_info = new DevExpress.XtraBars.BarButtonItem();
            this.mgr_bar_btn_user_list = new DevExpress.XtraBars.BarButtonItem();
            this.mgr_bar_btn_user_add = new DevExpress.XtraBars.BarButtonItem();
            this.etc_bar_btn_order_input_test = new DevExpress.XtraBars.BarButtonItem();
            this.bar_btn_order_state_all = new DevExpress.XtraBars.BarButtonItem();
            this.bar_chk_order_state_all = new DevExpress.XtraBars.BarCheckItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.btn_order_print = new DevExpress.XtraBars.BarButtonItem();
            this.btn_api_reg_list_today = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem2 = new DevExpress.XtraBars.BarCheckItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbon_order_page_search = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbon_order_page_other = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbon_setup_PageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup_etc = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup_admin = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.repositoryItemSearchDateF = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.repositoryItemSearchDateT = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.ctr_bar_main_status = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.timer_watch_dog = new System.Windows.Forms.Timer(this.components);
            this.panelMain = new System.Windows.Forms.Panel();
            this.alertControl = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.barCheckItem3 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem4 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem5 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem6 = new DevExpress.XtraBars.BarCheckItem();
            this.barToggleSwitchItem1 = new DevExpress.XtraBars.BarToggleSwitchItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barCheckItem7 = new DevExpress.XtraBars.BarCheckItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemFilterText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchDateF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchDateF.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchDateT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchDateT.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl
            // 
            this.ribbonControl.ApplicationButtonImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.if_Sync_Center_99944_b_32;
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.btn_request_remote_help,
            this.btn_order_request_delivery,
            this.order_bar_btn_search,
            this.bar_label_filter_text,
            this.bar_tbx_filter_text,
            this.btn_program_setup,
            this.bar_chk_order_state_0,
            this.bar_chk_order_state_1,
            this.btn_application_info,
            this.mgr_bar_btn_user_list,
            this.mgr_bar_btn_user_add,
            this.etc_bar_btn_order_input_test,
            this.bar_btn_order_state_all,
            this.bar_chk_order_state_all,
            this.barStaticItem1,
            this.barStaticItem2,
            this.barStaticItem3,
            this.btn_order_print,
            this.btn_api_reg_list_today,
            this.barCheckItem1,
            this.barCheckItem2,
            this.barCheckItem3,
            this.barCheckItem4,
            this.barCheckItem5,
            this.barCheckItem6,
            this.barToggleSwitchItem1,
            this.barCheckItem7});
            this.ribbonControl.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ribbonControl.MaxItemId = 38;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1,
            this.ribbonPage2});
            this.ribbonControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSearchDateF,
            this.repositoryItemSearchDateT,
            this.repositoryItemFilterText});
            this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2007;
            this.ribbonControl.Size = new System.Drawing.Size(1066, 147);
            this.ribbonControl.SelectedPageChanged += new System.EventHandler(this.ribbonControl_SelectedPageChanged);
            // 
            // btn_request_remote_help
            // 
            this.btn_request_remote_help.Caption = "원격요청";
            this.btn_request_remote_help.Id = 3;
            this.btn_request_remote_help.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.support_32x32;
            this.btn_request_remote_help.LargeWidth = 80;
            this.btn_request_remote_help.Name = "btn_request_remote_help";
            this.btn_request_remote_help.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.btn_request_remote_help.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_request_remote_help_ItemClick);
            // 
            // btn_order_request_delivery
            // 
            this.btn_order_request_delivery.Caption = "배달요청";
            this.btn_order_request_delivery.Id = 4;
            this.btn_order_request_delivery.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.bike_32x32;
            this.btn_order_request_delivery.LargeWidth = 80;
            this.btn_order_request_delivery.Name = "btn_order_request_delivery";
            this.btn_order_request_delivery.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.btn_order_request_delivery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_order_request_delivery_ItemClick);
            // 
            // order_bar_btn_search
            // 
            this.order_bar_btn_search.Caption = "주문검색";
            this.order_bar_btn_search.Id = 7;
            this.order_bar_btn_search.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.zoom_32x32;
            this.order_bar_btn_search.LargeWidth = 80;
            this.order_bar_btn_search.Name = "order_bar_btn_search";
            this.order_bar_btn_search.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.order_bar_btn_search.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.order_bar_btn_search_ItemClick);
            // 
            // bar_label_filter_text
            // 
            this.bar_label_filter_text.Caption = " 결과내 검색";
            this.bar_label_filter_text.Id = 8;
            this.bar_label_filter_text.Name = "bar_label_filter_text";
            // 
            // bar_tbx_filter_text
            // 
            this.bar_tbx_filter_text.Edit = this.repositoryItemFilterText;
            this.bar_tbx_filter_text.EditWidth = 100;
            this.bar_tbx_filter_text.Id = 9;
            this.bar_tbx_filter_text.Name = "bar_tbx_filter_text";
            // 
            // repositoryItemFilterText
            // 
            this.repositoryItemFilterText.AutoHeight = false;
            this.repositoryItemFilterText.Name = "repositoryItemFilterText";
            this.repositoryItemFilterText.NullText = "입력 후 엔터";
            this.repositoryItemFilterText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repositoryItemFilterText_KeyDown);
            // 
            // btn_program_setup
            // 
            this.btn_program_setup.Caption = "환경설정";
            this.btn_program_setup.Id = 10;
            this.btn_program_setup.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.groupfieldcollection_32x32;
            this.btn_program_setup.LargeWidth = 80;
            this.btn_program_setup.Name = "btn_program_setup";
            this.btn_program_setup.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.btn_program_setup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_program_setup_ItemClick);
            // 
            // bar_chk_order_state_0
            // 
            this.bar_chk_order_state_0.BindableChecked = true;
            this.bar_chk_order_state_0.Caption = "주문등록";
            this.bar_chk_order_state_0.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.bar_chk_order_state_0.Checked = true;
            this.bar_chk_order_state_0.Id = 14;
            this.bar_chk_order_state_0.Name = "bar_chk_order_state_0";
            this.bar_chk_order_state_0.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bar_chk_order_state_0_ItemClick);
            // 
            // bar_chk_order_state_1
            // 
            this.bar_chk_order_state_1.BindableChecked = true;
            this.bar_chk_order_state_1.Caption = "배달요청";
            this.bar_chk_order_state_1.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.bar_chk_order_state_1.Checked = true;
            this.bar_chk_order_state_1.Id = 15;
            this.bar_chk_order_state_1.Name = "bar_chk_order_state_1";
            this.bar_chk_order_state_1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bar_chk_order_state_1_ItemClick);
            // 
            // btn_application_info
            // 
            this.btn_application_info.Caption = "어플정보";
            this.btn_application_info.Id = 19;
            this.btn_application_info.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.if_Sync_Center_99944_b_32;
            this.btn_application_info.LargeWidth = 80;
            this.btn_application_info.Name = "btn_application_info";
            this.btn_application_info.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.btn_application_info.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_application_info_ItemClick);
            // 
            // mgr_bar_btn_user_list
            // 
            this.mgr_bar_btn_user_list.Caption = "가맹목록";
            this.mgr_bar_btn_user_list.Id = 20;
            this.mgr_bar_btn_user_list.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.employees_32x32;
            this.mgr_bar_btn_user_list.LargeWidth = 80;
            this.mgr_bar_btn_user_list.Name = "mgr_bar_btn_user_list";
            this.mgr_bar_btn_user_list.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.mgr_bar_btn_user_list.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.mgr_bar_btn_user_list_ItemClick);
            // 
            // mgr_bar_btn_user_add
            // 
            this.mgr_bar_btn_user_add.Caption = "가맹추가";
            this.mgr_bar_btn_user_add.Id = 21;
            this.mgr_bar_btn_user_add.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.newemployee_32x32;
            this.mgr_bar_btn_user_add.LargeWidth = 80;
            this.mgr_bar_btn_user_add.Name = "mgr_bar_btn_user_add";
            this.mgr_bar_btn_user_add.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.mgr_bar_btn_user_add.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.mgr_bar_btn_user_add_ItemClick);
            // 
            // etc_bar_btn_order_input_test
            // 
            this.etc_bar_btn_order_input_test.Caption = "연동테스트";
            this.etc_bar_btn_order_input_test.Id = 22;
            this.etc_bar_btn_order_input_test.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.selectall_32x32;
            this.etc_bar_btn_order_input_test.LargeWidth = 80;
            this.etc_bar_btn_order_input_test.Name = "etc_bar_btn_order_input_test";
            this.etc_bar_btn_order_input_test.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.etc_bar_btn_order_input_test.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.etc_bar_btn_order_input_test_ItemClick);
            // 
            // bar_btn_order_state_all
            // 
            this.bar_btn_order_state_all.Caption = "전체선택";
            this.bar_btn_order_state_all.Id = 23;
            this.bar_btn_order_state_all.Name = "bar_btn_order_state_all";
            // 
            // bar_chk_order_state_all
            // 
            this.bar_chk_order_state_all.BindableChecked = true;
            this.bar_chk_order_state_all.Caption = "전체선택";
            this.bar_chk_order_state_all.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.bar_chk_order_state_all.Checked = true;
            this.bar_chk_order_state_all.Id = 24;
            this.bar_chk_order_state_all.Name = "bar_chk_order_state_all";
            this.bar_chk_order_state_all.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bar_chk_order_state_all_ItemClick);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "상태보기";
            this.barStaticItem1.Id = 25;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Id = 26;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.Width = 50;
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Id = 27;
            this.barStaticItem3.Name = "barStaticItem3";
            this.barStaticItem3.Width = 50;
            // 
            // btn_order_print
            // 
            this.btn_order_print.Caption = "주문출력";
            this.btn_order_print.Id = 28;
            this.btn_order_print.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.print_32x32;
            this.btn_order_print.LargeWidth = 80;
            this.btn_order_print.Name = "btn_order_print";
            this.btn_order_print.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.btn_order_print.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_order_print_ItemClick);
            // 
            // btn_api_reg_list_today
            // 
            this.btn_api_reg_list_today.Caption = "요청내역";
            this.btn_api_reg_list_today.Id = 29;
            this.btn_api_reg_list_today.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.addheader_32x32;
            this.btn_api_reg_list_today.LargeWidth = 80;
            this.btn_api_reg_list_today.Name = "btn_api_reg_list_today";
            this.btn_api_reg_list_today.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.btn_api_reg_list_today.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_api_reg_list_today_ItemClick);
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "barCheckItem1";
            this.barCheckItem1.Id = 30;
            this.barCheckItem1.Name = "barCheckItem1";
            // 
            // barCheckItem2
            // 
            this.barCheckItem2.Caption = "최적화";
            this.barCheckItem2.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.AfterText;
            this.barCheckItem2.Id = 31;
            this.barCheckItem2.Name = "barCheckItem2";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbon_order_page_search,
            this.ribbon_order_page_other,
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "연동주문";
            // 
            // ribbon_order_page_search
            // 
            this.ribbon_order_page_search.ItemLinks.Add(this.barStaticItem1);
            this.ribbon_order_page_search.ItemLinks.Add(this.barStaticItem2);
            this.ribbon_order_page_search.ItemLinks.Add(this.barStaticItem3);
            this.ribbon_order_page_search.ItemLinks.Add(this.bar_chk_order_state_0);
            this.ribbon_order_page_search.ItemLinks.Add(this.bar_chk_order_state_1);
            this.ribbon_order_page_search.ItemLinks.Add(this.bar_chk_order_state_all);
            this.ribbon_order_page_search.ItemLinks.Add(this.bar_label_filter_text, true);
            this.ribbon_order_page_search.ItemLinks.Add(this.bar_tbx_filter_text);
            this.ribbon_order_page_search.Name = "ribbon_order_page_search";
            this.ribbon_order_page_search.ShowCaptionButton = false;
            this.ribbon_order_page_search.Text = "주문검색";
            // 
            // ribbon_order_page_other
            // 
            this.ribbon_order_page_other.ItemLinks.Add(this.btn_order_request_delivery);
            this.ribbon_order_page_other.ItemLinks.Add(this.btn_order_print);
            this.ribbon_order_page_other.ItemLinks.Add(this.btn_api_reg_list_today, true);
            this.ribbon_order_page_other.ItemLinks.Add(this.btn_program_setup);
            this.ribbon_order_page_other.Name = "ribbon_order_page_other";
            this.ribbon_order_page_other.ShowCaptionButton = false;
            this.ribbon_order_page_other.Text = "업무메뉴";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItem4);
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItem5);
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItem6);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbon_setup_PageGroup1,
            this.ribbonPageGroup_etc,
            this.ribbonPageGroup_admin});
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "기타메뉴";
            // 
            // ribbon_setup_PageGroup1
            // 
            this.ribbon_setup_PageGroup1.ItemLinks.Add(this.btn_application_info);
            this.ribbon_setup_PageGroup1.ItemLinks.Add(this.btn_request_remote_help);
            this.ribbon_setup_PageGroup1.Name = "ribbon_setup_PageGroup1";
            this.ribbon_setup_PageGroup1.ShowCaptionButton = false;
            this.ribbon_setup_PageGroup1.Text = "설정";
            // 
            // ribbonPageGroup_etc
            // 
            this.ribbonPageGroup_etc.ItemLinks.Add(this.etc_bar_btn_order_input_test);
            this.ribbonPageGroup_etc.Name = "ribbonPageGroup_etc";
            this.ribbonPageGroup_etc.ShowCaptionButton = false;
            this.ribbonPageGroup_etc.Text = "기타메뉴";
            // 
            // ribbonPageGroup_admin
            // 
            this.ribbonPageGroup_admin.ItemLinks.Add(this.mgr_bar_btn_user_list);
            this.ribbonPageGroup_admin.ItemLinks.Add(this.mgr_bar_btn_user_add);
            this.ribbonPageGroup_admin.Name = "ribbonPageGroup_admin";
            this.ribbonPageGroup_admin.ShowCaptionButton = false;
            this.ribbonPageGroup_admin.Text = "관리메뉴";
            // 
            // repositoryItemSearchDateF
            // 
            this.repositoryItemSearchDateF.AutoHeight = false;
            this.repositoryItemSearchDateF.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSearchDateF.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSearchDateF.Name = "repositoryItemSearchDateF";
            this.repositoryItemSearchDateF.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemSearchDateT
            // 
            this.repositoryItemSearchDateT.AutoHeight = false;
            this.repositoryItemSearchDateT.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSearchDateT.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSearchDateT.Name = "repositoryItemSearchDateT";
            this.repositoryItemSearchDateT.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // dockManager
            // 
            this.dockManager.Form = this;
            this.dockManager.MenuManager = this.barManager;
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl"});
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.ctr_bar_main_status});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.DockManager = this.dockManager;
            this.barManager.Form = this;
            this.barManager.MaxItemId = 0;
            this.barManager.StatusBar = this.ctr_bar_main_status;
            // 
            // ctr_bar_main_status
            // 
            this.ctr_bar_main_status.BarName = "Custom 2";
            this.ctr_bar_main_status.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.ctr_bar_main_status.DockCol = 0;
            this.ctr_bar_main_status.DockRow = 0;
            this.ctr_bar_main_status.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.ctr_bar_main_status.OptionsBar.AllowQuickCustomization = false;
            this.ctr_bar_main_status.OptionsBar.DrawDragBorder = false;
            this.ctr_bar_main_status.OptionsBar.UseWholeRow = true;
            this.ctr_bar_main_status.Text = "Custom 2";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1066, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 566);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1066, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 566);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1066, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 566);
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 147);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1066, 419);
            this.panelMain.TabIndex = 3;
            // 
            // barCheckItem3
            // 
            this.barCheckItem3.Caption = "barCheckItem3";
            this.barCheckItem3.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItem3.Id = 32;
            this.barCheckItem3.Name = "barCheckItem3";
            // 
            // barCheckItem4
            // 
            this.barCheckItem4.Caption = "배달의민족";
            this.barCheckItem4.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItem4.Id = 33;
            this.barCheckItem4.Name = "barCheckItem4";
            this.barCheckItem4.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem4_CheckedChanged);
            // 
            // barCheckItem5
            // 
            this.barCheckItem5.Caption = "요기요";
            this.barCheckItem5.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItem5.Id = 34;
            this.barCheckItem5.Name = "barCheckItem5";
            this.barCheckItem5.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem5_CheckedChanged);
            // 
            // barCheckItem6
            // 
            this.barCheckItem6.Caption = "POS";
            this.barCheckItem6.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItem6.Id = 35;
            this.barCheckItem6.Name = "barCheckItem6";
            this.barCheckItem6.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem6_CheckedChanged);
            // 
            // barToggleSwitchItem1
            // 
            this.barToggleSwitchItem1.Caption = "barToggleSwitchItem1";
            this.barToggleSwitchItem1.Id = 36;
            this.barToggleSwitchItem1.Name = "barToggleSwitchItem1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barCheckItem7);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "ribbonPageGroup2";
            // 
            // barCheckItem7
            // 
            this.barCheckItem7.Caption = "최적화 모드";
            this.barCheckItem7.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.barCheckItem7.Id = 37;
            this.barCheckItem7.Name = "barCheckItem7";
            this.barCheckItem7.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem7_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1066, 588);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.ribbonControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMain";
            this.Ribbon = this.ribbonControl;
            this.Text = "Callpass";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.VisibleChanged += new System.EventHandler(this.FormMain_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemFilterText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchDateF.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchDateF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchDateT.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchDateT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private System.Windows.Forms.Timer timer_watch_dog;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.BarButtonItem btn_request_remote_help;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbon_setup_PageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbon_order_page_other;
        private DevExpress.XtraBars.BarButtonItem btn_order_request_delivery;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemSearchDateF;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemSearchDateT;
        private DevExpress.XtraBars.BarButtonItem order_bar_btn_search;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbon_order_page_search;
        private System.Windows.Forms.Panel panelMain;
        private DevExpress.XtraBars.BarStaticItem bar_label_filter_text;
        private DevExpress.XtraBars.BarEditItem bar_tbx_filter_text;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemFilterText;
        private DevExpress.XtraBars.BarButtonItem btn_program_setup;
        private DevExpress.XtraBars.BarCheckItem bar_chk_order_state_0;
        private DevExpress.XtraBars.BarCheckItem bar_chk_order_state_1;
        private DevExpress.XtraBars.BarButtonItem btn_application_info;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Bar ctr_bar_main_status;
        private DevExpress.XtraBars.Alerter.AlertControl alertControl;
        private DevExpress.XtraBars.BarButtonItem mgr_bar_btn_user_list;
        private DevExpress.XtraBars.BarButtonItem mgr_bar_btn_user_add;
        private DevExpress.XtraBars.BarButtonItem etc_bar_btn_order_input_test;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_etc;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_admin;
        private DevExpress.XtraBars.BarButtonItem bar_btn_order_state_all;
        private DevExpress.XtraBars.BarCheckItem bar_chk_order_state_all;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraBars.BarButtonItem btn_order_print;
        private DevExpress.XtraBars.BarButtonItem btn_api_reg_list_today;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem2;
        private DevExpress.XtraBars.BarCheckItem barCheckItem3;
        private DevExpress.XtraBars.BarCheckItem barCheckItem4;
        private DevExpress.XtraBars.BarCheckItem barCheckItem5;
        private DevExpress.XtraBars.BarCheckItem barCheckItem6;
        private DevExpress.XtraBars.BarToggleSwitchItem barToggleSwitchItem1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem7;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
    }
}

