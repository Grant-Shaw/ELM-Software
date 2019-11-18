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
            try
            {
                MessageText = m;
                this.FindSender();
                this.FindSubject();
                if (MessageType == "SIR")
                {
                    IncidentHandler();
                }
                this.QuarantineEmails();
                MessageFilter.dict.Remove("EMA");
                foreach (var entry in MessageFilter.dict)
                {
                    MessageText = MessageText.Replace(" " + entry.Key + " ", " " + entry.Key + "<" + entry.Value + ">");

                }

            }
            catch(Exception n)
            {
                MessageBox.Show(n.Message);

            }

                         
        }

        private void IncidentHandler()
        {
            try
            {
                Regex centreCodeFinder = new Regex(@"\d{2}-\d{3}-\d{3}", RegexOptions.IgnoreCase);
                MatchCollection centreCodeMatches = centreCodeFinder.Matches(MessageText);

                string incident;

                string centreCode = centreCodeMatches[0].Value;

                foreach (string s in MessageFilter.incidentDescriptions)
                {
                    if (MessageText.Contains(s))
                    {
                        incident = string.Format("Sport centre Code: {0} \n Nature of Incident: {1}", centreCode, s);

                        if (!MessageFilter.incidentDescriptions.Contains(incident))
                              MessageFilter.incidentList.Add(incident);
                                
                    }
                }
            }
            catch(Exception h)
            {
                MessageBox.Show("Please ensure that your SIR centre code and incident type is valid");
            }
        
        }


        //finds the subject of the Email by using substrings
        private void FindSubject()
        {

            try
            {
                var subject1 = MessageText.Substring(0, 21);


                if (subject1.Contains("SIR"))
                {
                    MessageType = "SIR";
                    Subject = subject1.Substring(0, 14);
                    MessageText = MessageText.Replace(Subject, "");
                }
                else
                {
                    MessageType = "Email";
                    subject1 = subject1.Split('.')[0];
                    Subject = subject1;
                    MessageText = MessageText.Replace(Subject, "");
                    MessageText = MessageText.Substring(1);

                }
            }
            catch(Exception v)
            {
                throw new Exception("Please ensure that your subject is in either standard (less than 20 characters) or SIR format");
             }
            
        }


        //adds email addresses within messagetext to quarantine list.
        private void QuarantineEmails()
        {
            foreach (string email in MessageFilter.emailList)
            {
                MessageText = MessageText.Replace(email, "" + "<This email has been quarantined> ");
            }

        }

        //method which finds the sender of the Email by using regex to search for first email address.

        private void FindSender()
        {

            //var EmailRegex = new Regex(@"(?:(?:\+?([1-9]|[0-9][0-9]|[0-9][0-9][0-9])\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([0-9][1-9]|[0-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?", RegexOptions.IgnoreCase);

            try
            {

                Regex emailRegex = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);

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

            catch(Exception v)
            {
                throw new Exception("Please ensure that the senders email is a valid email address");
            }
            


        }
    }
}
