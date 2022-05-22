using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace socket_async
{
    public class doge_socket_client{

        IPAddress main_server_ip_address;
        int main_server_port;
        TcpClient main_client;

        public doge_socket_client()
        {
            main_client = null;
            main_server_port = -1;
            main_server_ip_address = null;
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
                Console.WriteLine("Invalid server IP supplied.");
                return false;
            }

            main_server_ip_address = ip_address;

            return true;
        }


        public bool SetPortNumber(string servers_port)
        {
            int port_number = 0;

            if (!int.TryParse(servers_port.Trim(), out port_number))
            {
                Console.WriteLine("Invalid port number supplied, return.");
                return false;
            }

            if (port_number <= 0 || port_number > 65535)
            {
                Console.WriteLine("Port number must be between 0 and 65535.");
                return false;
            }

            main_server_port = port_number;

            return true;
        }

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

        public async Task SendToServer(string sender_message)
        {
            if (string.IsNullOrEmpty(sender_message))
            {
                Console.WriteLine("Empty string supplied to send.");
                return;
            }

            if (main_client != null)
            {
                if (main_client.Connected)
                {
                    StreamWriter clientStreamWriter = new StreamWriter(main_client.GetStream());
                    clientStreamWriter.AutoFlush = true;

                    await clientStreamWriter.WriteAsync(sender_message);
                    Console.WriteLine("Data sent...");
                }
            }

        }

        public async Task ConnectToServer()
        {
            if (main_client == null)
            {
                main_client = new TcpClient();
            }

            try
            {
                await main_client.ConnectAsync(main_server_ip_address, main_server_port);
                Console.WriteLine(string.Format("Connected to server IP/Port: {0} / {1}",
                    main_server_ip_address, main_server_port));

                ReadDataAsync(main_client);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                throw;
            }
        }

        private async Task<string> ReadDataAsync(TcpClient main_client)
        {
            try
            {
                StreamReader reader_for_client_stream = new StreamReader(main_client.GetStream());
                char[] message = new char[64];
                int total_read_bytes = 0;

                while (true)
                {
                    total_read_bytes = await reader_for_client_stream.ReadAsync(message, 0, message.Length);

                    if (total_read_bytes <= 0)
                    {
                        Console.WriteLine("Disconnected from server.");
                        main_client.Close();
                        break;
                    }
                    Console.WriteLine(string.Format("Received bytes: {0} - Message: {1}",
                        total_read_bytes, new string(message)));

                    Array.Clear(message, 0, message.Length);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                throw;
            }
        }
    }
}
