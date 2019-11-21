using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace ELM
{
    class SMS : Message
    {
        /// <summary>
        /// This class deals with SMS messages that are detected.
        /// </summary>

        private string sender;
        private string messageText;
        private string messagetype;


        //displays the message type in the JSON
        public string MessageType
        {
            get { return messagetype; }
            set { messagetype = value; }
        }

        //Property for Sender
        public override string Sender
        {
            get { return sender; }
            set { sender = value; }

        }

        
       

        //property for the messageBody which checks to ensure that the message does not exceed 140 characters (151 to include sneder as it's read in).
        public override string MessageText

        {
            get { return messageText; }

            set
            {
                if (value.Length > 151)
                {
                    throw new Exception("Message cannot be longer than 140 characters for a tweet");
                }
                else
                {
                    messageText = value;
                }
            }

        }

        //constructor for SMS
        //Calls method findSender to determine the Sender.
        public SMS(string m) : base(m)
        {

            MessageText = m;
            MessageType = "SMS";

            this.FindSender();
            FilterTextSpeak();

            

        }



        private void FilterTextSpeak()
        {
            foreach (var entry in MessageFilter.dict)
            {
                MessageText = MessageText.Replace(" " + entry.Key, " " + entry.Key + "<" + entry.Value + ">");
            }
        }



        //method which determines the sender of the SMS from the first phone number found in the message body.
        private void FindSender()
        {
            //check phone number search
            var phoneNumberSequence = new Regex(@"(?:(?:\+?([1-9]|[0-9][0-9]|[0-9][0-9][0-9])\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([0-9][1-9]|[0-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?", RegexOptions.IgnoreCase);

            MatchCollection matches = phoneNumberSequence.Matches(MessageText);
            try
            {
                Sender = matches[0].Value;
            }
            catch (Exception o)
            {
                MessageBox.Show("You must have a valid phone number");
            }
            MessageText = MessageText.Remove(0, 11);

        }




    }
}
