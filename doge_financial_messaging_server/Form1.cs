using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using socket_async;


namespace doge_financial_messaging_server
{
    public partial class Form1 : Form
    {

        doge_socket_server server;
        public Form1()
        {
            InitializeComponent();
            server = new doge_socket_server();
        }

        private void btn_accept_connections_Click(object sender, EventArgs e)
        {
            //allow clients to connect
            server.start_listening_for_incoming_connection();
        }

        private void btn_stop_server_Click(object sender, EventArgs e)
        {
            //once users has confirmed to stop the server stop clients connecting and disconnect currently connected users
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to stop the server and stop allowing clients to send message to each other?", "Confirm Stopping Server", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                server.stop_server();
            }            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        MySqlConnection conn = new MySqlConnection("server=lochnagar.abertay.ac.uk; database=sql1800367; username=sql1800367; password=xqCNtH46949v");
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                //every 60 seconds this code will run to check the database does not have any messages older than 6 months
                //open connection to database
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                //here we delete any items in the software_eng_practices_messages table where the time stamp is older than 6 months
                cmd.CommandText = "DELETE FROM software_eng_practices_messages WHERE important_tag = 'False' AND time_sent < DATE_SUB(NOW(), INTERVAL 6 MONTH)";

                cmd.ExecuteNonQuery();

                //close connection to database
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to the database check firewall access");
            }
        }
    }
}
