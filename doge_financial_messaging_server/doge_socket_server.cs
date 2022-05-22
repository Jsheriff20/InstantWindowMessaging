using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using doge_financial_messaging_server;
using MySql.Data.MySqlClient;
using System.Data;

namespace socket_async
{
    class doge_socket_server
    {
        IPAddress main_ip;
        int main_port;
        TcpListener main_tcp_listener;

        List<TcpClient> main_client;
        List<string> connected_users_list = new List<string> { };

        public bool keep_running { get; set; }

        public doge_socket_server()
        {
            main_client = new List<TcpClient>();
        }


        public async void start_listening_for_incoming_connection(IPAddress ip_address = null, int port = 23000)
        {
            //set the servers ipaddress and port number
            if (ip_address == null)
            {
                ip_address = IPAddress.Any;
            }

            if (port <= 0)
            {
                port = 23000;
            }

            main_ip = ip_address;
            main_port = port;

            //write the servers details in console
            Debug.WriteLine(string.Format("IP Address: {0} - Port: {1}", main_ip.ToString(), main_port));

            main_tcp_listener = new TcpListener(main_ip, main_port);

            //try to start listening for clients connecting 
            try
            {
                main_tcp_listener.Start();

                keep_running = true;
                while (keep_running)
                {
                    var returnedByAccept = await main_tcp_listener.AcceptTcpClientAsync();

                    main_client.Add(returnedByAccept);

                    Debug.WriteLine(
                        string.Format("Client connected successfully, count: {0} - {1}",
                        main_client.Count, returnedByAccept.Client.RemoteEndPoint)
                        );

                    take_care_of_tcp_client(returnedByAccept);
                }
            }
            catch (Exception excp)
            {
                System.Diagnostics.Debug.WriteLine(excp.ToString());
            }
        }

        //stop the server and close any open socket connections
        public void stop_server()
        {
            try
            {
                if (main_tcp_listener != null)
                {
                    main_tcp_listener.Stop();
                }

                foreach (TcpClient c in main_client)
                {
                    c.Close();
                }

                main_client.Clear();
            }
            catch (Exception error)
            {

                Debug.WriteLine(error.ToString());
            }
        }


        //database connection details
        MySqlConnection conn = new MySqlConnection("server=lochnagar.abertay.ac.uk; database=sql1800367; username=sql1800367; password=xqCNtH46949v");

        //will find the usersnames id when a username is passed in
        private string get_username_id(string username)
        {
            conn.Open();
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


        //this receives any messages sent from the clients and will distribute the messages accordingly 
        private async void take_care_of_tcp_client(TcpClient param_client)
        {
            NetworkStream client_stream = null;
            StreamReader client_reader = null;

            try
            {
                client_stream = param_client.GetStream();
                client_reader = new StreamReader(client_stream);

                char[] message = new char[2000];

                while (keep_running)
                {
                    Debug.WriteLine("*** Ready to read");

                    int message_return = await client_reader.ReadAsync(message, 0, message.Length);

                    System.Diagnostics.Debug.WriteLine("Returned: " + message_return);

                    //check if user has disconnected
                    if (message_return == 0)
                    {
                        remove_client(param_client);

                        System.Diagnostics.Debug.WriteLine("Socket disconnected");
                        break;
                    }


                    string received_text = new string(message);

                    //will receive this when a user has logged in
                    if (received_text.Contains("_&*&_user_connected="))
                    {

                        string connected_user = "";

                        connected_user = received_text.Remove(0, 20);

                        if (!connected_users_list.Contains(connected_user))
                        {

                            connected_users_list.Add(connected_user);
                        }
                    }
                    //this notifies the user that the server knows when a converstion has closed
                    else if (received_text.Contains("_&*&_convo_&*&_close_&*&_"))
                    {
                        send_to_user("close_&*&_conversation_close_&*&_false", received_text.Remove(0, 25));
                        Console.WriteLine(received_text.Remove(0, 24));
                    }
                    else
                    {
                        //this will run when a normal message that is going to another client is received
                        message_decoder decode_message = new message_decoder();

                        //this will get the details from the string sent
                        List<string> message_details = decode_message.get_message_details(received_text);


                        string senders_username = message_details[0];
                        string receivers_username = message_details[1];
                        string message_to_send = message_details[2];
                        string important_message = message_details[3];

                        
                        try
                        {
                            //this will send the message it to the correct user
                            send_to_user((senders_username + "_&*&_" + receivers_username + "_&*&_" + message_to_send + "_&*&_" + important_message), receivers_username);
                        }
                        catch (Exception exp)
                        {

                            Console.WriteLine(exp);
                        }

        
                        //get users id
                        string senders_id = get_username_id(senders_username);
                        string receivers_id = get_username_id(receivers_username);

                        //store message in the database
                        conn.Open();
                        string command_text = "INSERT INTO software_eng_practices_messages (message, sender_id, retriever_id, important_tag) values(@message_to_send, @senders_id, @receivers_id, @important_message)";
                        MySqlCommand cmd = new MySqlCommand(command_text, conn);
                        cmd.Parameters.Add("@message_to_send", MySqlDbType.String).Value = message_to_send;
                        cmd.Parameters.Add("@senders_id", MySqlDbType.String).Value = senders_id;
                        cmd.Parameters.Add("@receivers_id", MySqlDbType.String).Value = receivers_id;
                        cmd.Parameters.Add("@important_message", MySqlDbType.String).Value = important_message;
                        cmd.ExecuteNonQuery();
                        
                        conn.Close();
                    }


                    System.Diagnostics.Debug.WriteLine("*** RECEIVED: " + received_text);

                    Array.Clear(message, 0, message.Length);
                    received_text = "";
                }

            }
            catch (Exception excp)
            {
                remove_client(param_client);
                System.Diagnostics.Debug.WriteLine(excp.ToString());
            }

        }

        //remove the client from the connected clients list
        private void remove_client(TcpClient param_client)
        {
            if (main_client.Contains(param_client))
            {
                main_client.Remove(param_client);
                Debug.WriteLine(String.Format("Client removed, count: {0}", main_client.Count));
            }
        }
         
        //sends a message to a client
        public async void send_to_user(string sending_Message, string username)
        {
            if (string.IsNullOrEmpty(sending_Message))
            {
                return;
            }

            try
            {
                byte[] built_message = Encoding.ASCII.GetBytes(sending_Message);
                


                //find index of user in list of connected users, this index number correlates to the tcp client list index
                int users_index = connected_users_list.FindIndex(x => x.StartsWith(username));

                Console.WriteLine("users_index: " + users_index);
                await main_client[users_index].GetStream().WriteAsync(built_message, 0, built_message.Length);
                
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
            }

        }
    }
}
