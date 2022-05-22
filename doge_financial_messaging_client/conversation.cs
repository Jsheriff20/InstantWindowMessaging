using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using socket_async;
using encrypt_decrypt_namespace;

namespace doge_financial_messaging_client
{
    public partial class frm_conversation : Form
    {

        private string senders_username;
        public string receivers_username;
        private TcpClient main_client;
        private doge_socket_client client;
        public frm_conversation(string senders_username_arg, string receivers_username_arg, TcpClient tcp_main_client)
        {

            main_client = tcp_main_client;
            senders_username = senders_username_arg;
            receivers_username = receivers_username_arg;
            InitializeComponent();
        }


        MySqlConnection conn = new MySqlConnection("server=lochnagar.abertay.ac.uk; database=sql1800367; username=sql1800367; password=xqCNtH46949v");
        private string get_username_id(string username)
        {
            conn.Open();

            //gets a users id from the database
            string command_text = "SELECT id FROM software_eng_practices_users WHERE username = @username";
            MySqlCommand cmd = new MySqlCommand(command_text, conn);
            cmd.Parameters.Add("@username", MySqlDbType.String).Value = username;
            cmd.ExecuteNonQuery();
            MySqlDataReader data_read = cmd.ExecuteReader();

            data_read.Read();
            Console.WriteLine("id value: " + data_read.GetValue(0).ToString());



            string id = data_read.GetValue(0).ToString();
            conn.Close();
            return id;

        }

        public bool keep_looping = false;
        private void frm_conversation_Load(object sender, EventArgs e)
        {
            client = new doge_socket_client(this, main_client, true);
            keep_looping = true;
            //start listening for messages
            client.ReadDataAsync(main_client);
            lbl_contact_username.Text = receivers_username;
            lsv_conversation.Columns.Add("", -2);



            //get users id number
            string current_senders_id = get_username_id(senders_username);
            string current_receivers_id = get_username_id(receivers_username);

            //get all previous messages from the database
            conn.Open();

            string command_text = "SELECT * FROM software_eng_practices_messages WHERE sender_id = @current_senders AND retriever_id = @current_receivers" +
                " UNION" +
                " SELECT * FROM software_eng_practices_messages WHERE sender_id = @current_receivers AND retriever_id = @current_senders" +
                " ORDER BY time_sent ASC";
            MySqlCommand cmd = new MySqlCommand(command_text, conn);
            cmd.Parameters.Add("@current_senders", MySqlDbType.String).Value = current_senders_id;
            cmd.Parameters.Add("@current_receivers", MySqlDbType.String).Value = current_receivers_id;
            cmd.ExecuteNonQuery();
            DataTable data_table = new DataTable();
            MySqlDataAdapter data_adapter = new MySqlDataAdapter(cmd);
            data_adapter.Fill(data_table);

            //loop through each row that was selected
            foreach (DataRow row in data_table.Rows)
            {
                //get deatails to display the message
                string message_senders_id = row["sender_id"].ToString();
                string message_senders_username = "";
                string message_message = row["message"].ToString();
                bool message_important_tag = bool.Parse(row["important_tag"].ToString());

                string decrypted_message = "";
                // this sees if the message was sent by the current user or who they are talking to  (the receiver)
                if (message_senders_id == current_senders_id)
                {
                    message_senders_username = "Me";
                    decrypted_message = encrypt_decrypt.decrypt(message_message, senders_username, receivers_username);

                }
                else if (message_senders_id == current_receivers_id)
                {
                    message_senders_username = receivers_username;
                    decrypted_message = encrypt_decrypt.decrypt(message_message, receivers_username, senders_username);
                }

                display_message(message_senders_username, decrypted_message, message_important_tag, false);
            }
        }


        //display message to display any time the current user sends a message
        public void display_message(string username, string message, bool important_or_not, bool alert_or_not)
        {
            int size_of_conversation = 0;
            if (important_or_not)
            {
                lsv_conversation.Items.Add("<< IMPORTANT >>");
                size_of_conversation = lsv_conversation.Items.Count;
                lsv_conversation.Items[size_of_conversation - 1].BackColor = Color.Red;

                if (alert_or_not)
                {
                    if (username != "Me")
                    {
                        notify_icon_important_message.Icon = SystemIcons.Exclamation;
                        notify_icon_important_message.Visible = true;
                        notify_icon_important_message.ShowBalloonTip(2500, "Important Message Received", "Message received from: " + username, ToolTipIcon.Info);
                    }
                }

            }


            lsv_conversation.Items.Add(username + ":");

            //check if message needs to be broken down further
            if (message.Length > 60)
            {
                double how_many_lines_for_message = Math.Ceiling(message.Length / 60.0);
                char[] chars = message.ToCharArray();


                int extra_characters_position_for_indent = 0;
                for (int i = 0; i < how_many_lines_for_message; i++)
                {
                    string message_to_display = "";
                    int j;

                    //loop through the first 60 characters unless some have already been seen
                    for (j = i * 60 + extra_characters_position_for_indent; j < (i * 60) + 60; j++)
                    {

                        if (j == chars.Length)
                        {
                            break;
                        }

                        message_to_display += chars[j];
                    }


                    //reset extra characters position for next loop
                    extra_characters_position_for_indent = 0;

                    //while the next character is still part of a word then keep adding  charactres untill the word is complete 
                    //and whilst we are not at the final letter
                    for (int x = 0; j + x < chars.Length && chars[((i * 60) + 60) + x].ToString() != " "; x++)
                    {
                        
                        //add character to string (finishing off word
                        message_to_display += chars[j + x];

                        //this will ensure that the next loop ignores any already seen characters 
                        extra_characters_position_for_indent++;

                        //if a message has not got a space in it for a while then just indent anyway
                        if (x == 5) break;
                    }

                    lsv_conversation.Items.Add("     " + message_to_display);

                    //change the background colour
                    size_of_conversation = lsv_conversation.Items.Count;
                    if (username == "Me")
                    {
                        lsv_conversation.Items[size_of_conversation - 1].BackColor = Color.LightCyan;
                    }
                    else
                    {
                        lsv_conversation.Items[size_of_conversation - 1].BackColor = Color.LightSkyBlue;
                    }
                }

            }
            else
            {
                lsv_conversation.Items.Add("     " + message);


                //change the background colour
                size_of_conversation = lsv_conversation.Items.Count;
                if (username == "Me")
                {
                    lsv_conversation.Items[size_of_conversation - 1].BackColor = Color.LightCyan;
                }
                else
                {
                    lsv_conversation.Items[size_of_conversation - 1].BackColor = Color.LightSkyBlue;
                }

            }

            lsv_conversation.Items.Add("");

            //go to the bottom of the messages
            lsv_conversation.Items[lsv_conversation.Items.Count - 1].EnsureVisible();
        }


        private void btn_back_to_contacts_Click(object sender, EventArgs e)
        {
            contacts contacts_page = new contacts(senders_username, main_client);
            contacts_page.Show();
            this.Hide();
            keep_looping = false;

            //alert server that conversation has closed
            SendToServer("_&*&_convo_&*&_close_&*&_" + senders_username);

        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            string message = txt_message.Text;

            //validate message is valid length and not trying to break the server
            if (message.Length < 2) { MessageBox.Show("Message Invalid"); return; }
            if (message.Length > 400) { MessageBox.Show("Message Invalid, Max characters is 400"); return; }
            if (message.Contains("_&*&_")){ MessageBox.Show("Error please try again"); return; }


            //encrypts and sends the message
            string encrypted_message = encrypt_decrypt.encrypt(message, senders_username, receivers_username);
            string server_message = senders_username + "_&*&_" + receivers_username + "_&*&_" + encrypted_message + "_&*&_" + ckb_tag_as_important.Checked.ToString();
            SendToServer(server_message);
            display_message("Me", message, ckb_tag_as_important.Checked, true);
            txt_message.Clear();

        }


        public async Task SendToServer(string sender_message)
        {

            Console.WriteLine(main_client.ToString());
            if (string.IsNullOrEmpty(sender_message))
            {
                MessageBox.Show("No message was entered.");
                return;
            }

            if (main_client != null)
            {

                if (main_client.Connected)
                {
                    StreamWriter clientStreamWriter = new StreamWriter(main_client.GetStream());
                    clientStreamWriter.AutoFlush = true;

                    await clientStreamWriter.WriteAsync(sender_message);
                }
            }
        }

        private void frm_conversation_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.CloseAndDisconnect();
        }

        private void lsv_conversation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbl_contact_username_Click(object sender, EventArgs e)
        {

        }
    }
}
