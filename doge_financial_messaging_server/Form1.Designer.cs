namespace doge_financial_messaging_server
{
    partial class Form1
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
            this.btn_stop_server = new System.Windows.Forms.Button();
            this.btn_accept_connections = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btn_stop_server
            // 
            this.btn_stop_server.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_stop_server.Location = new System.Drawing.Point(400, 24);
            this.btn_stop_server.Name = "btn_stop_server";
            this.btn_stop_server.Size = new System.Drawing.Size(352, 410);
            this.btn_stop_server.TabIndex = 9;
            this.btn_stop_server.Text = "Stop Server";
            this.btn_stop_server.UseVisualStyleBackColor = false;
            this.btn_stop_server.Click += new System.EventHandler(this.btn_stop_server_Click);
            // 
            // btn_accept_connections
            // 
            this.btn_accept_connections.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_accept_connections.Location = new System.Drawing.Point(48, 24);
            this.btn_accept_connections.Name = "btn_accept_connections";
            this.btn_accept_connections.Size = new System.Drawing.Size(352, 410);
            this.btn_accept_connections.TabIndex = 5;
            this.btn_accept_connections.Text = "Accept Incoming Connections";
            this.btn_accept_connections.UseVisualStyleBackColor = false;
            this.btn_accept_connections.Click += new System.EventHandler(this.btn_accept_connections_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 300000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_stop_server);
            this.Controls.Add(this.btn_accept_connections);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Messaging Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_stop_server;
        private System.Windows.Forms.Button btn_accept_connections;
        private System.Windows.Forms.Timer timer;
    }
}

