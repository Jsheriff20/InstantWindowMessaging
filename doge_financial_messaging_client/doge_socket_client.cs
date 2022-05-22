using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using doge_financial_messaging_client;
using System.Drawing;
using encrypt_decrypt_namespace;


namespace socket_async
{
    public class doge_socket_client
    {

        private frm_conversation conversation_page;
        IPAddress main_server_ip_address;
        int main_server_port;
        public TcpClient main_client;
        bool not_login_value;

        public doge_socket_client(frm_conversation form, TcpClient main_tcp_client, bool not_login)
        {
            conversation_page = form;
            main_client = main_tcp_client;
            main_server_port = -1;
            main_server_ip_address = null;

            not_login_value = not_login;
        }

        public IPAddress server_ip_address
        {
            get
            {
                return main_server_ip_address;
            }
        }

        public int server_port
        {
            get
            {
                return main_server_port;
            }
        }


        public bool SetServerIPAddress(string server_ip_address)
        {
            IPAddress ip_address = null;

            if (!IPAddress.TryParse(server_ip_address, out ip_address))
            {
                MessageBox.Show("Invalid server IP supplied.");
                return false;
            }

            main_server_ip_address = ip_address;

            return true;
        }


        public bool SetPortNumber(string servers_port)
        {
            int port_number = 0;

            //if port number is not a int
            if (!int.TryParse(servers_port.Trim(), out port_number))
            {
                MessageBox.Show("Invalid port number supplied, return.");
                return false;
            }

            //if port number is invalid
            if (port_number <= 0 || port_number > 65535)
            {
                MessageBox.Show("Port number must be between 0 and 65535.");
                return false;
            }

            main_server_port = port_number;

            return true;
        }

        //close the connection and disconnect from server
        public void CloseAndDisconnect()
        {
            if (main_client != null)
            {
                if (main_client.Connected)
                {
                    main_client.Close();
                }
            }
        }


        //connect to the local network using the 2300 port
        public async Task ConnectToServer()
        {
            if (main_client == null)
            {
                main_client = new TcpClient();
            }

            try
            {
                //try connect to server
                await main_client.ConnectAsync(main_server_ip_address, main_server_port);
                Console.WriteLine(string.Format("Connected to server IP/Port: {0} / {1}",
                    main_server_ip_address, main_server_port));

                if (not_login_value) ReadDataAsync(main_client);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                throw;
            }
        }



        //start listening for messages being received
        message_decoder message_details = new message_decoder();
        public async Task ReadDataAsync(TcpClient main_client)
        {

            try
            {
                //get message from the stream
                StreamReader reader_for_client_stream = new StreamReader(main_client.GetStream());
                char[] whole_message = new char[2000];
                int total_read_bytes = 0;

                while (conversation_page.keep_looping)
                {
                    total_read_bytes = await reader_for_client_stream.ReadAsync(whole_message, 0, whole_message.Length);

                    //decode message received and get each aspect of message
                    List<string> message_details_list = new List<string> { };
                    message_details_list = message_details.get_message_details(string.Format(new string(whole_message)));
                    
                    string senders_username = message_details_list[0];
                    string receivers_username = message_details_list[1];
                    string text_part_of_message = message_details_list[2];
                    bool important_value = bool.Parse(message_details_list[3]);

                    //if server has closed it will run this
                    if (total_read_bytes <= 0)
                    {
                       
                        MessageBox.Show("Disconnected from server.");

                        main_client.Close();
                        break;
                        
                    }

                    // if the message is from the current conversation 
                    if (senders_username == conversation_page.lbl_contact_username.Text)
                    {
                        display_message(senders_username, receivers_username, text_part_of_message, important_value);
                    }

                    //clear the array to allow for new message
                    Array.Clear(whole_message, 0, whole_message.Length);

                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                throw;
            }
        }

        //display the message received
        public void display_message(string senders_username, string receivers_username, string message, bool important_or_not)
        {
            int size_of_conversation = 0;
            //if the message is important run this
            if (important_or_not)
            {
                conversation_page.lsv_conversation.Items.Add("<< IMPORTANT >>");
                size_of_conversation = conversation_page.lsv_conversation.Items.Count;
                conversation_page.lsv_conversation.Items[size_of_conversation - 1].BackColor = Color.Red;


                if (senders_username != "Me")
                {
                    conversation_page.notify_icon_important_message.Icon = SystemIcons.Exclamation;
                    conversation_page.notify_icon_important_message.Visible = true;
                    conversation_page.notify_icon_important_message.ShowBalloonTip(2500, "Important Message Received", "Message received from: " + senders_username, ToolTipIcon.Info);
                }

            }


            //display the message like this every time
            conversation_page.lsv_conversation.Items.Add(senders_username + ":");

            string decrypted_message = encrypt_decrypt.decrypt(message, senders_username, receivers_username);
            conversation_page.lsv_conversation.Items.Add("     " + decrypted_message);
            size_of_conversation = conversation_page.lsv_conversation.Items.Count;
            conversation_page.lsv_conversation.Items[size_of_conversation - 1].BackColor = Color.LightSkyBlue;

            conversation_page.lsv_conversation.Items.Add("");

            //go to the bottom of the messages
            conversation_page.lsv_conversation.Items[conversation_page.lsv_conversation.Items.Count - 1].EnsureVisible();
        }
    }
}
