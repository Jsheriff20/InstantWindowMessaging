using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using socket_async;
using System.Net.Sockets;

namespace doge_financial_messaging_client
{
    public partial class contacts : Form
    {
        public string users_username;
        public TcpClient main_client;

        public contacts(string username, TcpClient tcp_main_client)
        {
            main_client = tcp_main_client;
            users_username = username;
            InitializeComponent();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbl_contact_username_Click(object sender, EventArgs e)
        {

        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            login login_register_page = new login();
            login_register_page.Show();
            this.Hide();
        }


        MySqlConnection conn = new MySqlConnection("server=lochnagar.abertay.ac.uk; database=sql1800367; username=sql1800367; password=xqCNtH46949v");
        private void contacts_Load(object sender, EventArgs e)
        {
            //will display all users that have an account on the application
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM software_eng_practices_users";
            cmd.ExecuteNonQuery();
            DataTable data_table = new DataTable();
            MySqlDataAdapter data_adapter = new MySqlDataAdapter(cmd);
            data_adapter.Fill(data_table);

            foreach (DataRow row in data_table.Rows)
            {
                string username = row["username"].ToString();
                //display all contacts other than the current users account
                if (username != users_username)
                {

                    //show username in contacts list
                    lsb_contacts.Items.Add(username);
                }
            }
            conn.Close();
        }

        public string selected_contact = "";

        public string get_selected_contact()
        {

            return selected_contact;
        }
        private void lsb_contacts_DoubleClick(object sender, EventArgs e)
        {
            //if user slects a contact then load that conversation
            if (lsb_contacts.SelectedItem != null)
            {
                selected_contact = lsb_contacts.SelectedItem.ToString();
                frm_conversation conversation_page = new frm_conversation(users_username, selected_contact, main_client);
                conversation_page.Show();
                this.Hide();
            }
        }

        private void contacts_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
