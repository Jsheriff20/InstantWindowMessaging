using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using NETCore.Encrypt;
using socket_async;
using encrypt_decrypt_namespace;

namespace doge_financial_messaging_client
{
    public partial class login : Form
    {
        doge_socket_client client;
        TcpClient main_tcp_client;
        public login()
        {
            client = new doge_socket_client(new frm_conversation(username, "from_login", main_tcp_client), main_tcp_client, false);

            InitializeComponent();
        }
        contacts contact_page;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
        {
            
        }


        
        public string username;
        MySqlConnection conn = new MySqlConnection("server=lochnagar.abertay.ac.uk; database=sql1800367; username=sql1800367; password=xqCNtH46949v");
        private async void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_login_password.Text.Length < 15 && txt_login_username.Text.Length < 5)
                {
                    MessageBox.Show("Please enter a valid username (minimum 5 characters long) and password (minimum 15 characters long)");
                    return;
                }

                string decrypted_password = encrypt_decrypt.encrypt_password(txt_login_password.Text, txt_login_username.Text);

                int count;
                conn.Open();
                string command_text = "SELECT * FROM software_eng_practices_users WHERE username = @login_username AND password = @login_password";
                MySqlCommand cmd = new MySqlCommand(command_text, conn);
                cmd.Parameters.Add("@login_username", MySqlDbType.String).Value = txt_login_username.Text;
                cmd.Parameters.Add("@login_password", MySqlDbType.String).Value = decrypted_password;
                cmd.ExecuteNonQuery();
                DataTable data_table = new DataTable();
                MySqlDataAdapter data_adapter = new MySqlDataAdapter(cmd);
                data_adapter.Fill(data_table);
                count = Convert.ToInt32(data_table.Rows.Count.ToString());

                
                //if user exists
                if (count > 0)
                {

                    //get username
                    username = txt_login_username.Text;

                    
                    //connect client to server and store the TcpClient to be used throughout the project
                    client.SetServerIPAddress("127.0.0.1");
                    client.SetPortNumber("23000");
                    await client.ConnectToServer();
                    main_tcp_client = client.main_client;

                    //create a new instance of the conversation form so send to server can be used.
                    frm_conversation convo_page = new frm_conversation(username, "", main_tcp_client);
                    convo_page.SendToServer("_&*&_user_connected=" + username);

                    //create and show the contacts page
                    contact_page = new contacts(username, main_tcp_client);

                    contact_page.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }


                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            conn.Close();

        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            string username = txt_register_username.Text;
            string password = txt_register_password.Text;
            string confirm_password = txt_register_confirm_password.Text;
            string email = txt_register_email.Text;
            string name = txt_register_name.Text;



            //make sure inputs are valid
            if (password.Length < 15) MessageBox.Show("Password must be minimum 15 characters long");
            else if (password.Length > 25) MessageBox.Show("Password must be smaller than 25 characters long");
            else if (username.Length < 5) MessageBox.Show("Username must be minimum 5 characters long");
            else if (username.Length > 16) MessageBox.Show("Username must be smaller than 16 characters long");
            else if (email.Length < 5) MessageBox.Show("Invalid Email");
            else if (name.Length < 5) MessageBox.Show("Name is to short please enter your full name");
            //make sure passwords match
            else if (password != confirm_password) MessageBox.Show("Passwords do not match");
            else
            {

                int count;
                string command_text = "SELECT * FROM software_eng_practices_users WHERE username = @register_username";
                MySqlCommand cmd = new MySqlCommand(command_text, conn);
                cmd.Parameters.Add("@register_username", MySqlDbType.String).Value = txt_register_username.Text;
                conn.Open();
                DataTable data_table = new DataTable();
                MySqlDataAdapter data_adapter = new MySqlDataAdapter(cmd);
                data_adapter.Fill(data_table);
                count = Convert.ToInt32(data_table.Rows.Count.ToString());


                if (count > 0)
                {
                    MessageBox.Show("Username has already been taken");
                }
                else
                {
                    string encypted_password = encrypt_decrypt.encrypt_password(password, username);

                    command_text = "INSERT INTO software_eng_practices_users (username, password, name, email) values(@username,@password, @name, @email)";
                    cmd = new MySqlCommand(command_text, conn);
                    cmd.Parameters.Add("@username", MySqlDbType.String).Value = username;
                    cmd.Parameters.Add("@password", MySqlDbType.String).Value = encypted_password;
                    cmd.Parameters.Add("@name", MySqlDbType.String).Value = name;
                    cmd.Parameters.Add("@email", MySqlDbType.String).Value = email;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("You have successfully registered!");
                }
                conn.Close();
            }
        }

        private void grb_login_Enter(object sender, EventArgs e)
        {

        }
    }
}
