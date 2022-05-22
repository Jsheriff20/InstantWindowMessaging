using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;

namespace socket_async
{
    class doge_socket_server
    {
        IPAddress main_ip;
        int main_port;
        TcpListener main_tcp_listener;

        List<TcpClient> main_client;

        public bool keep_running { get; set; }

        public doge_socket_server()
        {
            main_client = new List<TcpClient>();
        }

        public async void start_listening_for_incoming_connection(IPAddress ip_address = null, int port = 23000)
        {
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

            System.Diagnostics.Debug.WriteLine(string.Format("IP Address: {0} - Port: {1}", main_ip.ToString(), main_port));

            main_tcp_listener = new TcpListener(main_ip, main_port);

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

        private async void take_care_of_tcp_client(TcpClient param_client)
        {
            NetworkStream client_stream = null;
            StreamReader client_reader = null;

            try
            {
                client_stream = param_client.GetStream();
                client_reader = new StreamReader(client_stream);

                char[] message = new char[64];

                while (keep_running)
                {
                    Debug.WriteLine("*** Ready to read");

                    int message_return = await client_reader.ReadAsync(message, 0, message.Length);

                    System.Diagnostics.Debug.WriteLine("Returned: " + message_return);

                    if (message_return == 0)
                    {
                        remove_client(param_client);

                        System.Diagnostics.Debug.WriteLine("Socket disconnected");
                        break;
                    }

                    string received_text = new string(message);

                    System.Diagnostics.Debug.WriteLine("*** RECEIVED: " + received_text);

                    Array.Clear(message, 0, message.Length);


                }

            }
            catch (Exception excp)
            {
                remove_client(param_client);
                System.Diagnostics.Debug.WriteLine(excp.ToString());
            }

        }

        private void remove_client(TcpClient param_client)
        {
            if (main_client.Contains(param_client))
            {
                main_client.Remove(param_client);
                Debug.WriteLine(String.Format("Client removed, count: {0}", main_client.Count));
            }
        }

        public async void send_to_all(string sending_Message)
        {
            if (string.IsNullOrEmpty(sending_Message))
            {
                return;
            }

            try
            {
                byte[] built_message = Encoding.ASCII.GetBytes(sending_Message);

                foreach (TcpClient c in main_client)
                {
                    c.GetStream().WriteAsync(built_message, 0, built_message.Length);
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.ToString());
            }

        }
    }
}
