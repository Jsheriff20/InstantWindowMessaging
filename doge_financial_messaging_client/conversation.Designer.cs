namespace doge_financial_messaging_client
{
    partial class frm_conversation
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
            this.lbl_contact_username = new System.Windows.Forms.Label();
            this.txt_message = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.ckb_tag_as_important = new System.Windows.Forms.CheckBox();
            this.btn_back_to_contacts = new System.Windows.Forms.Button();
            this.lsv_conversation = new System.Windows.Forms.ListView();
            this.notify_icon_important_message = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // lbl_contact_username
            // 
            this.lbl_contact_username.AutoSize = true;
            this.lbl_contact_username.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_contact_username.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_contact_username.Location = new System.Drawing.Point(351, 0);
            this.lbl_contact_username.Name = "lbl_contact_username";
            this.lbl_contact_username.Size = new System.Drawing.Size(110, 25);
            this.lbl_contact_username.TabIndex = 0;
            this.lbl_contact_username.Text = "Username";
            this.lbl_contact_username.Click += new System.EventHandler(this.lbl_contact_username_Click);
            // 
            // txt_message
            // 
            this.txt_message.Location = new System.Drawing.Point(215, 346);
            this.txt_message.Multiline = true;
            this.txt_message.Name = "txt_message";
            this.txt_message.Size = new System.Drawing.Size(289, 55);
            this.txt_message.TabIndex = 2;
            // 
            // btn_send
            // 
            this.btn_send.BackColor = System.Drawing.SystemColors.Highlight;
            this.btn_send.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_send.ForeColor = System.Drawing.SystemColors.Desktop;
            this.btn_send.Location = new System.Drawing.Point(510, 368);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(84, 33);
            this.btn_send.TabIndex = 3;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = false;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // ckb_tag_as_important
            // 
            this.ckb_tag_as_important.AutoSize = true;
            this.ckb_tag_as_important.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ckb_tag_as_important.Location = new System.Drawing.Point(511, 346);
            this.ckb_tag_as_important.Name = "ckb_tag_as_important";
            this.ckb_tag_as_important.Size = new System.Drawing.Size(105, 17);
            this.ckb_tag_as_important.TabIndex = 4;
            this.ckb_tag_as_important.Text = "Tag as important";
            this.ckb_tag_as_important.UseVisualStyleBackColor = false;
            // 
            // btn_back_to_contacts
            // 
            this.btn_back_to_contacts.BackColor = System.Drawing.SystemColors.Highlight;
            this.btn_back_to_contacts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_back_to_contacts.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_back_to_contacts.Location = new System.Drawing.Point(75, 37);
            this.btn_back_to_contacts.Name = "btn_back_to_contacts";
            this.btn_back_to_contacts.Size = new System.Drawing.Size(112, 33);
            this.btn_back_to_contacts.TabIndex = 5;
            this.btn_back_to_contacts.Text = "< Back To Contacts";
            this.btn_back_to_contacts.UseVisualStyleBackColor = false;
            this.btn_back_to_contacts.Click += new System.EventHandler(this.btn_back_to_contacts_Click);
            // 
            // lsv_conversation
            // 
            this.lsv_conversation.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsv_conversation.HideSelection = false;
            this.lsv_conversation.Location = new System.Drawing.Point(215, 46);
            this.lsv_conversation.Margin = new System.Windows.Forms.Padding(2);
            this.lsv_conversation.MultiSelect = false;
            this.lsv_conversation.Name = "lsv_conversation";
            this.lsv_conversation.Size = new System.Drawing.Size(381, 296);
            this.lsv_conversation.TabIndex = 7;
            this.lsv_conversation.UseCompatibleStateImageBehavior = false;
            this.lsv_conversation.View = System.Windows.Forms.View.Details;
            this.lsv_conversation.SelectedIndexChanged += new System.EventHandler(this.lsv_conversation_SelectedIndexChanged);
            // 
            // notify_icon_important_message
            // 
            this.notify_icon_important_message.Text = "notify_icon_important_message";
            this.notify_icon_important_message.Visible = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.groupBox1.Location = new System.Drawing.Point(200, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 376);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // frm_conversation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lsv_conversation);
            this.Controls.Add(this.btn_back_to_contacts);
            this.Controls.Add(this.ckb_tag_as_important);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.txt_message);
            this.Controls.Add(this.lbl_contact_username);
            this.Controls.Add(this.groupBox1);
            this.Name = "frm_conversation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "conversation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_conversation_FormClosing);
            this.Load += new System.EventHandler(this.frm_conversation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbl_contact_username;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.CheckBox ckb_tag_as_important;
        public System.Windows.Forms.TextBox txt_message;
        private System.Windows.Forms.Button btn_back_to_contacts;
        public System.Windows.Forms.ListView lsv_conversation;
        public System.Windows.Forms.NotifyIcon notify_icon_important_message;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}