namespace Kons.ShopCallpass.FormDialog
{
    partial class FormDlgUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDlgUpdate));
            this.panel_button = new DevExpress.XtraEditors.PanelControl();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_update = new DevExpress.XtraEditors.SimpleButton();
            this.panel_body = new DevExpress.XtraEditors.PanelControl();
            this.label_message = new DevExpress.XtraEditors.LabelControl();
            this.label_icon = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panel_button)).BeginInit();
            this.panel_button.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panel_body)).BeginInit();
            this.panel_body.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_button
            // 
            this.panel_button.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel_button.Controls.Add(this.btn_cancel);
            this.panel_button.Controls.Add(this.btn_update);
            this.panel_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_button.Location = new System.Drawing.Point(0, 101);
            this.panel_button.Name = "panel_button";
            this.panel_button.Size = new System.Drawing.Size(377, 71);
            this.panel_button.TabIndex = 0;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Appearance.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cancel.Appearance.Options.UseFont = true;
            this.btn_cancel.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.cancel_16x16;
            this.btn_cancel.Location = new System.Drawing.Point(198, 16);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(161, 39);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.TabStop = false;
            this.btn_cancel.Text = "나중에...";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_update
            // 
            this.btn_update.Appearance.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_update.Appearance.Options.UseFont = true;
            this.btn_update.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.apply_16x161;
            this.btn_update.Location = new System.Drawing.Point(19, 16);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(161, 39);
            this.btn_update.TabIndex = 0;
            this.btn_update.Text = "지금 업데이트";
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // panel_body
            // 
            this.panel_body.Appearance.BackColor = System.Drawing.Color.White;
            this.panel_body.Appearance.Options.UseBackColor = true;
            this.panel_body.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel_body.Controls.Add(this.label_message);
            this.panel_body.Controls.Add(this.label_icon);
            this.panel_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_body.Location = new System.Drawing.Point(0, 0);
            this.panel_body.Name = "panel_body";
            this.panel_body.Size = new System.Drawing.Size(377, 101);
            this.panel_body.TabIndex = 1;
            // 
            // label_message
            // 
            this.label_message.Appearance.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_message.Appearance.Options.UseFont = true;
            this.label_message.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.label_message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_message.Location = new System.Drawing.Point(101, 0);
            this.label_message.Name = "label_message";
            this.label_message.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label_message.Size = new System.Drawing.Size(276, 101);
            this.label_message.TabIndex = 1;
            this.label_message.Text = "설치된 프로그램의 새로운 버전이 있습니다. \r\n\r\n업데이트 하시겠습니까?";
            // 
            // label_icon
            // 
            this.label_icon.Appearance.Image = global::Kons.ShopCallpass.Properties.Resources.if_Sync_Center_99944_b_64;
            this.label_icon.Appearance.Options.UseImage = true;
            this.label_icon.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.label_icon.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_icon.Location = new System.Drawing.Point(0, 0);
            this.label_icon.Name = "label_icon";
            this.label_icon.Size = new System.Drawing.Size(101, 101);
            this.label_icon.TabIndex = 0;
            // 
            // FormDlgUpdate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(377, 172);
            this.Controls.Add(this.panel_body);
            this.Controls.Add(this.panel_button);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDlgUpdate";
            this.Text = "업데이트";
            this.Load += new System.EventHandler(this.FormDlgUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel_button)).EndInit();
            this.panel_button.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panel_body)).EndInit();
            this.panel_body.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panel_button;
        private DevExpress.XtraEditors.PanelControl panel_body;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_update;
        private DevExpress.XtraEditors.LabelControl label_message;
        private DevExpress.XtraEditors.LabelControl label_icon;
    }
}