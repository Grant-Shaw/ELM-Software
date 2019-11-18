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
    class Email : Message
    {

        private string sender;
        private string messageText;
        private string subject;
        private string messagetype;



        public string MessageType
        {
            get { return messagetype; }
            set { messagetype = value; }
        }


        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }


        public override string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public override string MessageText
        {
            get { return messageText; }
            set
            {
                if (value.Length > 1028)
                {
                    throw new Exception("Email cannot be longer than 1028 characters for a tweet");
                }
                else
                {
                    messageText = value;
                }
            }
        }




        public Email(string m) : base(m)
        {          
                MessageText = m;
                MessageType = "Email";
                this.FindSender();
                this.QuarantineEmails();
                MessageFilter.dict.Remove("EMA");
                foreach (var entry in MessageFilter.dict)
                {
                    MessageText = MessageText.Replace(" " + entry.Key + " ", " " + entry.Key + "<" + entry.Value + ">");

                }
               
        }




        private void QuarantineEmails()
        {
            for (int i = 1; i < MessageFilter.emailList.Count; i++)
            {
                MessageText = MessageText.Replace(MessageFilter.emailList[i], " " + "<Email has been quarantined>" + " ");

            }

        }



        private void FindSender()
        {

            //var EmailRegex = new Regex(@"(?:(?:\+?([1-9]|[0-9][0-9]|[0-9][0-9][0-9])\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([0-9][1-9]|[0-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?", RegexOptions.IgnoreCase);

            Regex emailRegex = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
            try
            {
                MatchCollection matches = emailRegex.Matches(MessageText);

                //make the Sender the first email address found
                Sender = matches[0].Value;



                //add all other email addresses to a list

                foreach (Match m in matches)
                {
                    if (MessageFilter.emailList.Contains(m.Value))
                    {
                        continue;
                    }
                    else
                    {
                        MessageFilter.emailList.Add(m.Value);
                        MessageFilter.emailList.Remove(Sender);
                    }
                }

                MessageText = MessageText.Replace(Sender, " ");
                //MessageText = MessageText.Remove(0, 11);

            }
            catch (Exception p)
            {
                MessageBox.Show("You must have a valid sender Email");
            }

        }
    }
}
