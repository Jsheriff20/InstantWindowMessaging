using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encrypt_decrypt_namespace
{
    class encrypt_decrypt
    {
     
        //get a unique key for the conversation when sending a message
        private static string get_key(string senders_username, string receivers_username)
        {
            string key_part_one = "";
            string key_part_two = "";

            if (senders_username.Length < 16)
            {
                key_part_one = senders_username;
                while (key_part_one.Length < 16)
                {
                    key_part_one += "E";
                }
            }
            else
            {
                if (senders_username.Length == 16) key_part_one = senders_username;
                else
                {
                    key_part_one = senders_username.Remove(16, senders_username.Length);
                }
            }




            if (receivers_username.Length < 16)
            {
                key_part_two = receivers_username;
                while (key_part_two.Length < 16)
                {
                    key_part_two += "B";
                }
            }
            else
            {
                if (receivers_username.Length == 16) key_part_two = receivers_username;
                else
                {
                    key_part_two = receivers_username.Remove(16, receivers_username.Length);
                }
            }


            string key = key_part_one + key_part_two;
            return key;
        }




        //get a unique iv for the conversation when sending a message
        private static string get_iv(string senders_username, string receivers_username)
        {
            string iv = "";
            char[] sender = senders_username.ToCharArray();
            char[] receiver = receivers_username.ToCharArray();

            for (int i = 0; i < 8; i++)
            {
                if (i > sender.Length - 1)
                {
                    iv += "v";
                }
                else
                {
                    iv += sender[i];
                }


                if (i > receiver.Length - 1)
                {
                    iv += "I";
                }
                else
                {
                    iv += receiver[i];
                }
            }

            return iv;
        }




        //get a key for the password unique to the username
        private static string get_password_key(string username)
        {
            string key = "";
            char[] reverse_username = username.ToCharArray();
            Array.Reverse(reverse_username);



            for (int i = 0; i < 16; i++)
            {

                if (i > reverse_username.Length - 1)
                {

                    key += "y";
                    key += "K";
                }
                else
                {
                    key += reverse_username[i];
                    key += username[i];
                }
            }

            return key;
        }




        //get a key for the password unique to the username
        private static string get_password_iv(string username)
        {
            string iv = "";

            for (int i = 0; i < 8; i++)
            {
                if (i > username.Length - 1)
                {
                    iv += "v";
                }
                else
                {
                    iv += username[i];
                }

                iv += "I";
            }

            return iv;
        }




        //used to encrypt messages
        public static string encrypt(string message, string senders_username, string receivers_username)
        {
            string key = get_key(senders_username, receivers_username);
            string iv = get_iv(senders_username, receivers_username);

            var encrypted = EncryptProvider.AESEncrypt(message, key, iv);
            return encrypted;
        }




        //used to decrypt messages
        public static string decrypt(string message, string senders_username, string receivers_username)
        {
            string key = get_key(senders_username, receivers_username);
            string iv = get_iv(senders_username, receivers_username);

            var decrypted = EncryptProvider.AESDecrypt(message, key, iv);
            return decrypted;
        }



        //used to encrypt passwords
        public static string encrypt_password(string password, string username)
        {
            string key = get_password_key(username);
            string iv = get_password_iv(username);

            var encrypted = EncryptProvider.AESEncrypt(password, key, iv);
            return encrypted;
        }





        //used to decrypt passwords
        public static string decrypt_password(string password, string username)
        {
            string key = get_password_key(username);
            string iv = get_password_iv(username);


            string decrypted = EncryptProvider.AESDecrypt(password, key, iv);
            return decrypted;
        }


    }
}
