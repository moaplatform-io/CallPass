namespace Kons.ShopCallpass.FormPopup
{
    partial class FormPopupNotify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPopupNotify));
            this.panel_button = new DevExpress.XtraEditors.PanelControl();
            this.btn_ok = new DevExpress.XtraEditors.SimpleButton();
            this.panel_body = new DevExpress.XtraEditors.PanelControl();
            this.label_notify = new DevExpress.XtraEditors.LabelControl();
            this.label_notify_icon = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panel_button)).BeginInit();
            this.panel_button.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panel_body)).BeginInit();
            this.panel_body.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_button
            // 
            this.panel_button.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel_button.Controls.Add(this.btn_ok);
            this.panel_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_button.Location = new System.Drawing.Point(0, 97);
            this.panel_button.Name = "panel_button";
            this.panel_button.Size = new System.Drawing.Size(453, 44);
            this.panel_button.TabIndex = 0;
            // 
            // btn_ok
            // 
            this.btn_ok.ImageOptions.Image = global::Kons.ShopCallpass.Properties.Resources.apply_16x161;
            this.btn_ok.Location = new System.Drawing.Point(348, 6);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(100, 32);
            this.btn_ok.TabIndex = 0;
            this.btn_ok.TabStop = false;
            this.btn_ok.Text = "확인";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // panel_body
            // 
            this.panel_body.Appearance.BackColor = System.Drawing.Color.White;
            this.panel_body.Appearance.Options.UseBackColor = true;
            this.panel_body.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel_body.Controls.Add(this.label_notify);
            this.panel_body.Controls.Add(this.label_notify_icon);
            this.panel_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_body.Location = new System.Drawing.Point(0, 0);
            this.panel_body.Name = "panel_body";
            this.panel_body.Size = new System.Drawing.Size(453, 97);
            this.panel_body.TabIndex = 1;
            // 
            // label_notify
            // 
            this.label_notify.Appearance.Options.UseTextOptions = true;
            this.label_notify.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.label_notify.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.label_notify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_notify.Location = new System.Drawing.Point(82, 0);
            this.label_notify.Name = "label_notify";
            this.label_notify.Padding = new System.Windows.Forms.Padding(0, 0, 10, 10);
            this.label_notify.Size = new System.Drawing.Size(371, 97);
            this.label_notify.TabIndex = 1;
            this.label_notify.Text = "알림";
            this.label_notify.MouseClick += NotifyClick;
            // 
            // label_notify_icon
            // 
            this.label_notify_icon.Appearance.Image = global::Kons.ShopCallpass.Properties.Resources.if_exclamation_circle_yellow_69299;
            this.label_notify_icon.Appearance.Options.UseImage = true;
            this.label_notify_icon.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.label_notify_icon.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_notify_icon.Location = new System.Drawing.Point(0, 0);
            this.label_notify_icon.Name = "label_notify_icon";
            this.label_notify_icon.Size = new System.Drawing.Size(82, 97);
            this.label_notify_icon.TabIndex = 0;
            // 
            // FormPopupNotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 141);
            this.Controls.Add(this.panel_body);
            this.Controls.Add(this.panel_button);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPopupNotify";
            this.Text = "알림";
            ((System.ComponentModel.ISupportInitialize)(this.panel_button)).EndInit();
            this.panel_button.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panel_body)).EndInit();
            this.panel_body.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panel_button;
        private DevExpress.XtraEditors.PanelControl panel_body;
        private DevExpress.XtraEditors.LabelControl label_notify_icon;
        private DevExpress.XtraEditors.LabelControl label_notify;
        private DevExpress.XtraEditors.SimpleButton btn_ok;
    }
}