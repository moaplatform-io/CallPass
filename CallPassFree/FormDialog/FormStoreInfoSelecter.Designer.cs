namespace Kons.ShopCallpass.FormDialog
{
    partial class FormStoreInfoSelecter
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
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ctr_btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.ctr_btn_save = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(3, 3);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(648, 227);
            this.gridControl.TabIndex = 1;
            this.gridControl.TabStop = false;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
            // 
            // gridView
            // 
            this.gridView.DetailHeight = 411;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView_CustomDrawCell);
            this.gridView.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridView_RowStyle);
            this.gridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gridView_CustomColumnDisplayText);
            // 
            // ctr_btn_cancel
            // 
            this.ctr_btn_cancel.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.cancel_32x32;
            this.ctr_btn_cancel.Location = new System.Drawing.Point(456, 232);
            this.ctr_btn_cancel.Name = "ctr_btn_cancel";
            this.ctr_btn_cancel.Size = new System.Drawing.Size(195, 48);
            this.ctr_btn_cancel.TabIndex = 42;
            this.ctr_btn_cancel.TabStop = false;
            this.ctr_btn_cancel.Text = "취소닫기";
            this.ctr_btn_cancel.Click += new System.EventHandler(this.ctr_btn_cancel_Click);
            // 
            // ctr_btn_save
            // 
            this.ctr_btn_save.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.apply_32x32;
            this.ctr_btn_save.Location = new System.Drawing.Point(254, 232);
            this.ctr_btn_save.Name = "ctr_btn_save";
            this.ctr_btn_save.Size = new System.Drawing.Size(195, 48);
            this.ctr_btn_save.TabIndex = 41;
            this.ctr_btn_save.TabStop = false;
            this.ctr_btn_save.Text = "선택";
            this.ctr_btn_save.Click += new System.EventHandler(this.ctr_btn_save_Click);
            // 
            // FormStoreInfoSelecter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(654, 282);
            this.Controls.Add(this.ctr_btn_cancel);
            this.Controls.Add(this.ctr_btn_save);
            this.Controls.Add(this.gridControl);
            this.Name = "FormStoreInfoSelecter";
            this.ShowIcon = false;
            this.Text = "상점 선택";
            this.Load += new System.EventHandler(this.FormDlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.SimpleButton ctr_btn_cancel;
        private DevExpress.XtraEditors.SimpleButton ctr_btn_save;
    }
}