namespace doge_financial_messaging_client
{
    partial class contacts
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
            this.lsb_contacts = new System.Windows.Forms.ListBox();
            this.lbl_connected_users = new System.Windows.Forms.Label();
            this.btn_logout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lsb_contacts
            // 
            this.lsb_contacts.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lsb_contacts.FormattingEnabled = true;
            this.lsb_contacts.Location = new System.Drawing.Point(255, 58);
            this.lsb_contacts.Name = "lsb_contacts";
            this.lsb_contacts.Size = new System.Drawing.Size(241, 316);
            this.lsb_contacts.TabIndex = 0;
            this.lsb_contacts.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.lsb_contacts.DoubleClick += new System.EventHandler(this.lsb_contacts_DoubleClick);
            // 
            // lbl_connected_users
            // 
            this.lbl_connected_users.AutoSize = true;
            this.lbl_connected_users.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_connected_users.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_connected_users.Location = new System.Drawing.Point(296, 32);
            this.lbl_connected_users.Name = "lbl_connected_users";
            this.lbl_connected_users.Size = new System.Drawing.Size(153, 25);
            this.lbl_connected_users.TabIndex = 1;
            this.lbl_connected_users.Text = "Contacts Page";
            this.lbl_connected_users.Click += new System.EventHandler(this.lbl_contact_username_Click);
            // 
            // btn_logout
            // 
            this.btn_logout.BackColor = System.Drawing.SystemColors.Highlight;
            this.btn_logout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_logout.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_logout.Location = new System.Drawing.Point(136, 58);
            this.btn_logout.Name = "btn_logout";
            this.btn_logout.Size = new System.Drawing.Size(112, 33);
            this.btn_logout.TabIndex = 6;
            this.btn_logout.Text = "< Logout";
            this.btn_logout.UseVisualStyleBackColor = false;
            this.btn_logout.Click += new System.EventHandler(this.btn_logout_Click);
            // 
            // contacts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_logout);
            this.Controls.Add(this.lbl_connected_users);
            this.Controls.Add(this.lsb_contacts);
            this.Name = "contacts";
            this.Text = "contacts";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.contacts_FormClosed);
            this.Load += new System.EventHandler(this.contacts_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lsb_contacts;
        public System.Windows.Forms.Label lbl_connected_users;
        private System.Windows.Forms.Button btn_logout;
    }
}