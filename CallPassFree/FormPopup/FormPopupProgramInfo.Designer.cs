namespace Kons.ShopCallpass.FormPopup
{
    partial class FormPopupProgramInfo
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
            this.panel_body = new DevExpress.XtraEditors.PanelControl();
            this.label_context = new DevExpress.XtraEditors.LabelControl();
            this.label_notify_icon = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ctr_lbl_version_info = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panel_body)).BeginInit();
            this.panel_body.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_body
            // 
            this.panel_body.Appearance.BackColor = System.Drawing.Color.White;
            this.panel_body.Appearance.Options.UseBackColor = true;
            this.panel_body.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel_body.Controls.Add(this.ctr_lbl_version_info);
            this.panel_body.Controls.Add(this.labelControl1);
            this.panel_body.Controls.Add(this.label_context);
            this.panel_body.Controls.Add(this.label_notify_icon);
            this.panel_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_body.Location = new System.Drawing.Point(0, 0);
            this.panel_body.Name = "panel_body";
            this.panel_body.Size = new System.Drawing.Size(466, 252);
            this.panel_body.TabIndex = 1;
            // 
            // label_context
            // 
            this.label_context.Appearance.Options.UseTextOptions = true;
            this.label_context.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.label_context.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.label_context.Location = new System.Drawing.Point(0, 83);
            this.label_context.Name = "label_context";
            this.label_context.Padding = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.label_context.Size = new System.Drawing.Size(466, 43);
            this.label_context.TabIndex = 1;
            this.label_context.Text = "콜패스";
            // 
            // label_notify_icon
            // 
            this.label_notify_icon.Appearance.Image = global::Kons.ShopCallpass.Properties.Resources.if_Sync_Center_99944_b_64;
            this.label_notify_icon.Appearance.Options.UseImage = true;
            this.label_notify_icon.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.label_notify_icon.Location = new System.Drawing.Point(0, 0);
            this.label_notify_icon.Name = "label_notify_icon";
            this.label_notify_icon.Size = new System.Drawing.Size(466, 90);
            this.label_notify_icon.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(0, 225);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.labelControl1.Size = new System.Drawing.Size(466, 22);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Copyright ©KonsEvolution 2018";
            // 
            // ctr_lbl_version_info
            // 
            this.ctr_lbl_version_info.Appearance.Options.UseTextOptions = true;
            this.ctr_lbl_version_info.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ctr_lbl_version_info.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.ctr_lbl_version_info.Location = new System.Drawing.Point(0, 122);
            this.ctr_lbl_version_info.Name = "ctr_lbl_version_info";
            this.ctr_lbl_version_info.Padding = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.ctr_lbl_version_info.Size = new System.Drawing.Size(466, 57);
            this.ctr_lbl_version_info.TabIndex = 4;
            this.ctr_lbl_version_info.Text = "1.0.0.0";
            // 
            // FormPopupProgramInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 252);
            this.Controls.Add(this.panel_body);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormPopupProgramInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "정보";
            ((System.ComponentModel.ISupportInitialize)(this.panel_body)).EndInit();
            this.panel_body.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panel_body;
        private DevExpress.XtraEditors.LabelControl label_notify_icon;
        private DevExpress.XtraEditors.LabelControl label_context;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl ctr_lbl_version_info;
    }
}