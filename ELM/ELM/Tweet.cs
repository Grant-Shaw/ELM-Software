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
    /// <summary>
    /// Class which deals with Tweets
    /// </summary>

    class Tweet : Message
    {

        private string messageText;
        private string sender;
        private string messagetype;

        

        public override string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public string MessageType
        {
            get
            { return messagetype; }
            set { messagetype = value; }
        }

        public override string MessageText
        {
            get { return messageText; }
            set
            {

                if (value.Length > 140)
                {
                    throw new Exception("Tweet cannot be longer than 140 characters");
                }
                else
                {
                    messageText = value;
                }
            }

        }
            
        //constructor
        public Tweet(string m) : base(m)
        {
            try
            {
                this.MessageText = m;
                MessageType = "Tweet";
                FindSender();
                FilterTextSpeak();
                FindHashtags();
                foreach (var entry in MessageFilter.dict)
                {
                    MessageText = MessageText.Replace(" " + entry.Key + " ", " " + entry.Key + "<" + entry.Value + ">");
                }
                
            }
            catch(Exception b)
            {
                MessageBox.Show(b.Message);
            }


        }

        //expand all textspeak abbreviations in messageText
        private void FilterTextSpeak()
        {
            foreach (var entry in MessageFilter.dict)
            {
                MessageText = MessageText.Replace(" " + entry.Key, " " + entry.Key + "<" + entry.Value + ">");
            }
        }

        //find hashtags and then add them to hashtag list
        private void FindHashtags()
        {
            Regex hashtagRegex = new Regex(@"\B(\#[a-zA-Z]+\b)(?!;)");
            MatchCollection matches = hashtagRegex.Matches(MessageText);

            foreach(Match m in matches)
            {
                MessageFilter.hashtagList.Add(m.Value);
            }


        }

        //Find the sender by matching to the first twitter username
        private void FindSender()
        {
            try
            {
                Regex tweetRegex = new Regex(@"(?<=^|(?<=[^a-zA-Z0-9-\.]))@[A-Za-z0-9-]+(?=[^a-zA-Z0-9-_\.])");

                MatchCollection matches = tweetRegex.Matches(MessageText);

                //make the Sender the first @mention found address found

                Sender = matches[0].Value;

                //add all other mentions to a list

                foreach (Match m in matches)
                {
                    if (MessageFilter.mentionList.Contains(m.Value))
                    {
                        continue;
                    }
                    else
                    {
                        MessageFilter.mentionList.Add(m.Value);
                        MessageFilter.mentionList.Remove(Sender);
                    }
                }

                //remove the Sender from the message body
                MessageText = MessageText.Replace(Sender, " ");
                //MessageText = MessageText.Remove(0, 11);
            }

            catch (Exception v)
            {
                throw new Exception("Please ensure that the senders username is valid");
            }

        }

    }
}
