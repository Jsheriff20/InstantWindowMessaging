using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace doge_financial_messaging_server
{
    class message_decoder
    {

        public List<string> get_message_details(string text)
        {
            
            int counter = 0;
            string salt = "_&*&_";
            int pattern_length = salt.Length;
            int text_length = 1 + text.Length; //the extra 1 is added as without it if the last character of the text is part of the pattern then it wont run the for loop
            int size_to_search = text_length - pattern_length;
            int x = 0;
            int end_of_wanted_characters = 0;

            string found_senders_username = "";
            string found_receivers_username = "";
            string found_message = "";
            string found_important_value = "";

            //search to find the different part of the message
            while (x < size_to_search)
            {
                int y = 0;

                while (y < text_length && text[x + y] == salt[y])
                {   //whist Y is less than the text length (whist the whole pattern has not been found)
                    y = y + 1;                                          // AND whilst the patterns (X+Y)th letter matches up with the texts Yth letter continue
                                                                        // if not return to the for loop and search the next letters;
                    if (y == pattern_length)
                    {                           // if the amount of letters that match equal the size of the pattern then all of the pattern has been found:
                        x++;
                        y = 0;
                        counter++;
                    }
                }

                

                switch (counter)
                {

                    case 0:

                        found_senders_username += text[x];
                        break;

                    case 1:

                        found_receivers_username += text[x];
                        break;

                    case 2:

                        found_message += text[x];
                        break;

                    case 3:
                        
                        found_important_value += text[x];
                        end_of_wanted_characters++;
                        break;

                }

                x++;
                //this gets the final _&*&_bool value and no other characters
                if (end_of_wanted_characters == 9)
                {
                    break;
                }
            }

            //if important message = true? then remove the ? else just remove the first 5 characters that are _&*&_
            if (found_important_value.Contains("?"))
            {
                found_important_value = "true";
            }
            else
            {
                found_important_value = found_important_value.Remove(0, 4);
            }



            List<string> message_details = new List<string> {found_senders_username, found_receivers_username.Remove(0, 4), found_message.Remove(0, 4), found_important_value};
            return message_details;
        }
    }
}
