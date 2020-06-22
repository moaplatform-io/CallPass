namespace Kons.ShopCallpass.FormDialog
{
    partial class FormComportParsingTest
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
            this.components = new System.ComponentModel.Container();
            this.btn_listen_port_connect = new System.Windows.Forms.Button();
            this.btn_listen_port_disconnect = new System.Windows.Forms.Button();
            this.lbx_listen_port = new System.Windows.Forms.ListBox();
            this.tbx_send_message_0 = new System.Windows.Forms.TextBox();
            this.btn_print_port_disconnect = new System.Windows.Forms.Button();
            this.btn_print_port_connect = new System.Windows.Forms.Button();
            this.lbx_print_port = new System.Windows.Forms.ListBox();
            this.btn_send_to_listen_port = new System.Windows.Forms.Button();
            this.btn_send_to_print_port = new System.Windows.Forms.Button();
            this.tbx_send_message_1 = new System.Windows.Forms.TextBox();
            this.btn_recv_data_send_to_print_port = new System.Windows.Forms.Button();
            this.timerWatchDog = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btn_listen_port_connect
            // 
            this.btn_listen_port_connect.BackColor = System.Drawing.Color.LightSalmon;
            this.btn_listen_port_connect.Location = new System.Drawing.Point(3, 3);
            this.btn_listen_port_connect.Name = "btn_listen_port_connect";
            this.btn_listen_port_connect.Size = new System.Drawing.Size(148, 23);
            this.btn_listen_port_connect.TabIndex = 0;
            this.btn_listen_port_connect.Text = "모니터포트 연결시작";
            this.btn_listen_port_connect.UseVisualStyleBackColor = false;
            this.btn_listen_port_connect.Click += new System.EventHandler(this.btn_listen_port_connect_Click);
            // 
            // btn_listen_port_disconnect
            // 
            this.btn_listen_port_disconnect.Location = new System.Drawing.Point(151, 3);
            this.btn_listen_port_disconnect.Name = "btn_listen_port_disconnect";
            this.btn_listen_port_disconnect.Size = new System.Drawing.Size(148, 23);
            this.btn_listen_port_disconnect.TabIndex = 1;
            this.btn_listen_port_disconnect.Text = "모니터포트 연결끊기";
            this.btn_listen_port_disconnect.UseVisualStyleBackColor = true;
            this.btn_listen_port_disconnect.Click += new System.EventHandler(this.btn_listen_port_disconnect_Click);
            // 
            // lbx_listen_port
            // 
            this.lbx_listen_port.FormattingEnabled = true;
            this.lbx_listen_port.ItemHeight = 12;
            this.lbx_listen_port.Location = new System.Drawing.Point(4, 52);
            this.lbx_listen_port.Name = "lbx_listen_port";
            this.lbx_listen_port.Size = new System.Drawing.Size(395, 436);
            this.lbx_listen_port.TabIndex = 2;
            // 
            // tbx_send_message_0
            // 
            this.tbx_send_message_0.Location = new System.Drawing.Point(4, 28);
            this.tbx_send_message_0.Name = "tbx_send_message_0";
            this.tbx_send_message_0.Size = new System.Drawing.Size(146, 21);
            this.tbx_send_message_0.TabIndex = 3;
            // 
            // btn_print_port_disconnect
            // 
            this.btn_print_port_disconnect.Location = new System.Drawing.Point(650, 3);
            this.btn_print_port_disconnect.Name = "btn_print_port_disconnect";
            this.btn_print_port_disconnect.Size = new System.Drawing.Size(148, 23);
            this.btn_print_port_disconnect.TabIndex = 5;
            this.btn_print_port_disconnect.Text = "프린트포트 연결끊기";
            this.btn_print_port_disconnect.UseVisualStyleBackColor = true;
            this.btn_print_port_disconnect.Click += new System.EventHandler(this.btn_print_port_disconnect_Click);
            // 
            // btn_print_port_connect
            // 
            this.btn_print_port_connect.BackColor = System.Drawing.Color.LightSalmon;
            this.btn_print_port_connect.Location = new System.Drawing.Point(502, 3);
            this.btn_print_port_connect.Name = "btn_print_port_connect";
            this.btn_print_port_connect.Size = new System.Drawing.Size(148, 23);
            this.btn_print_port_connect.TabIndex = 4;
            this.btn_print_port_connect.Text = "프린트포트 연결시작";
            this.btn_print_port_connect.UseVisualStyleBackColor = false;
            this.btn_print_port_connect.Click += new System.EventHandler(this.btn_print_port_connect_Click);
            // 
            // lbx_print_port
            // 
            this.lbx_print_port.FormattingEnabled = true;
            this.lbx_print_port.ItemHeight = 12;
            this.lbx_print_port.Location = new System.Drawing.Point(403, 52);
            this.lbx_print_port.Name = "lbx_print_port";
            this.lbx_print_port.Size = new System.Drawing.Size(395, 436);
            this.lbx_print_port.TabIndex = 6;
            // 
            // btn_send_to_listen_port
            // 
            this.btn_send_to_listen_port.Location = new System.Drawing.Point(151, 27);
            this.btn_send_to_listen_port.Name = "btn_send_to_listen_port";
            this.btn_send_to_listen_port.Size = new System.Drawing.Size(148, 23);
            this.btn_send_to_listen_port.TabIndex = 7;
            this.btn_send_to_listen_port.Text = "모니터포트에 전송";
            this.btn_send_to_listen_port.UseVisualStyleBackColor = true;
            this.btn_send_to_listen_port.Click += new System.EventHandler(this.btn_send_to_listen_port_Click);
            // 
            // btn_send_to_print_port
            // 
            this.btn_send_to_print_port.Location = new System.Drawing.Point(650, 27);
            this.btn_send_to_print_port.Name = "btn_send_to_print_port";
            this.btn_send_to_print_port.Size = new System.Drawing.Size(148, 23);
            this.btn_send_to_print_port.TabIndex = 9;
            this.btn_send_to_print_port.Text = "프린트포트에 전송";
            this.btn_send_to_print_port.UseVisualStyleBackColor = true;
            this.btn_send_to_print_port.Click += new System.EventHandler(this.btn_send_to_print_port_Click);
            // 
            // tbx_send_message_1
            // 
            this.tbx_send_message_1.Location = new System.Drawing.Point(403, 28);
            this.tbx_send_message_1.Name = "tbx_send_message_1";
            this.tbx_send_message_1.Size = new System.Drawing.Size(246, 21);
            this.tbx_send_message_1.TabIndex = 8;
            // 
            // btn_recv_data_send_to_print_port
            // 
            this.btn_recv_data_send_to_print_port.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_recv_data_send_to_print_port.Location = new System.Drawing.Point(299, 3);
            this.btn_recv_data_send_to_print_port.Name = "btn_recv_data_send_to_print_port";
            this.btn_recv_data_send_to_print_port.Size = new System.Drawing.Size(100, 46);
            this.btn_recv_data_send_to_print_port.TabIndex = 10;
            this.btn_recv_data_send_to_print_port.Text = "받은내용\r\n프린트포트에\r\n전송";
            this.btn_recv_data_send_to_print_port.UseVisualStyleBackColor = false;
            this.btn_recv_data_send_to_print_port.Click += new System.EventHandler(this.btn_recv_data_send_to_print_port_Click);
            // 
            // FormComportParsingTest
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(802, 492);
            this.Controls.Add(this.btn_recv_data_send_to_print_port);
            this.Controls.Add(this.btn_send_to_print_port);
            this.Controls.Add(this.tbx_send_message_1);
            this.Controls.Add(this.btn_send_to_listen_port);
            this.Controls.Add(this.lbx_print_port);
            this.Controls.Add(this.btn_print_port_disconnect);
            this.Controls.Add(this.btn_print_port_connect);
            this.Controls.Add(this.tbx_send_message_0);
            this.Controls.Add(this.lbx_listen_port);
            this.Controls.Add(this.btn_listen_port_disconnect);
            this.Controls.Add(this.btn_listen_port_connect);
            this.Name = "FormComportParsingTest";
            this.Text = "연동테스트";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormComportParsingTest_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_listen_port_connect;
        private System.Windows.Forms.Button btn_listen_port_disconnect;
        private System.Windows.Forms.ListBox lbx_listen_port;
        private System.Windows.Forms.TextBox tbx_send_message_0;
        private System.Windows.Forms.Button btn_print_port_disconnect;
        private System.Windows.Forms.Button btn_print_port_connect;
        private System.Windows.Forms.ListBox lbx_print_port;
        private System.Windows.Forms.Button btn_send_to_listen_port;
        private System.Windows.Forms.Button btn_send_to_print_port;
        private System.Windows.Forms.TextBox tbx_send_message_1;
        private System.Windows.Forms.Button btn_recv_data_send_to_print_port;
        private System.Windows.Forms.Timer timerWatchDog;
    }
}