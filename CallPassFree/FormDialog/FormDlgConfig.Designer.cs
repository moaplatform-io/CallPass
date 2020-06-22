namespace Kons.ShopCallpass.FormDialog
{
    partial class FormDlgConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDlgConfig));
            this.tabPane = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPage3 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.tabNavigationPage2 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.tabNavigationPage1 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.tabNavigationPage4 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.tabNavigationPage5 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.ctr_btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.ctr_btn_save = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).BeginInit();
            this.tabPane.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPane
            // 
            this.tabPane.AllowCollapse = DevExpress.Utils.DefaultBoolean.Default;
            this.tabPane.Controls.Add(this.tabNavigationPage3);
            this.tabPane.Controls.Add(this.tabNavigationPage2);
            this.tabPane.Controls.Add(this.tabNavigationPage1);
            this.tabPane.Controls.Add(this.tabNavigationPage4);
            this.tabPane.Controls.Add(this.tabNavigationPage5);
            this.tabPane.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabPane.Location = new System.Drawing.Point(0, 0);
            this.tabPane.Name = "tabPane";
            this.tabPane.PageProperties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabPane.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPage1,
            this.tabNavigationPage2,
            this.tabNavigationPage3,
            this.tabNavigationPage4,
            this.tabNavigationPage5});
            this.tabPane.RegularSize = new System.Drawing.Size(657, 472);
            this.tabPane.SelectedPage = this.tabNavigationPage4;
            this.tabPane.Size = new System.Drawing.Size(657, 472);
            this.tabPane.TabIndex = 1;
            this.tabPane.Text = "tabPane";
            this.tabPane.SelectedPageIndexChanged += new System.EventHandler(this.tabPane_SelectedPageIndexChanged);
            // 
            // tabNavigationPage3
            // 
            this.tabNavigationPage3.Caption = "기타 프로그램설정";
            this.tabNavigationPage3.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.viewsetting_16x16;
            this.tabNavigationPage3.Name = "tabNavigationPage3";
            this.tabNavigationPage3.Size = new System.Drawing.Size(580, 442);
            // 
            // tabNavigationPage2
            // 
            this.tabNavigationPage2.Caption = "영수증프린트 출력";
            this.tabNavigationPage2.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.print_16x16;
            this.tabNavigationPage2.Name = "tabNavigationPage2";
            this.tabNavigationPage2.Size = new System.Drawing.Size(580, 442);
            // 
            // tabNavigationPage1
            // 
            this.tabNavigationPage1.Caption = "고객주문 연동설정";
            this.tabNavigationPage1.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.richeditpageorientation_16x16;
            this.tabNavigationPage1.Name = "tabNavigationPage1";
            this.tabNavigationPage1.Size = new System.Drawing.Size(580, 442);
            // 
            // tabNavigationPage4
            // 
            this.tabNavigationPage4.Caption = "고급 설정";
            this.tabNavigationPage4.Name = "tabNavigationPage4";
            this.tabNavigationPage4.Size = new System.Drawing.Size(657, 442);
            // 
            // tabNavigationPage5
            // 
            this.tabNavigationPage5.Caption = "매뉴얼";
            this.tabNavigationPage5.Name = "tabNavigationPage5";
            this.tabNavigationPage5.Size = new System.Drawing.Size(580, 442);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.ctr_btn_cancel);
            this.panelBottom.Controls.Add(this.ctr_btn_save);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 472);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(657, 57);
            this.panelBottom.TabIndex = 2;
            // 
            // ctr_btn_cancel
            // 
            this.ctr_btn_cancel.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.cancel_32x32;
            this.ctr_btn_cancel.Location = new System.Drawing.Point(403, 6);
            this.ctr_btn_cancel.Name = "ctr_btn_cancel";
            this.ctr_btn_cancel.Size = new System.Drawing.Size(171, 44);
            this.ctr_btn_cancel.TabIndex = 40;
            this.ctr_btn_cancel.TabStop = false;
            this.ctr_btn_cancel.Text = "닫기";
            this.ctr_btn_cancel.Click += new System.EventHandler(this.ctr_btn_cancel_Click);
            // 
            // ctr_btn_save
            // 
            this.ctr_btn_save.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.apply_32x32;
            this.ctr_btn_save.Location = new System.Drawing.Point(226, 6);
            this.ctr_btn_save.Name = "ctr_btn_save";
            this.ctr_btn_save.Size = new System.Drawing.Size(171, 44);
            this.ctr_btn_save.TabIndex = 39;
            this.ctr_btn_save.TabStop = false;
            this.ctr_btn_save.Text = "저장";
            this.ctr_btn_save.Click += new System.EventHandler(this.ctr_btn_save_Click);
            // 
            // FormDlgConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(657, 529);
            this.ControlBox = true;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.tabPane);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDlgConfig";
            this.Text = "환경설정";
            this.Load += new System.EventHandler(this.FormDlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabPane)).EndInit();
            this.tabPane.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TabPane tabPane;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage2;
        private System.Windows.Forms.Panel panelBottom;
        private DevExpress.XtraEditors.SimpleButton ctr_btn_cancel;
        private DevExpress.XtraEditors.SimpleButton ctr_btn_save;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage3;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage4;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage5;
    }
}