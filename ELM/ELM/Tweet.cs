using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELM
{
    class Tweet : Message
    {

        private string messageText;
        private string sender;

        public List<string> hashtags;
        public List<string> mentions;

        public override string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public override string MessageText
        {
            get { return messageText; }
            set { messageText = value; }

        }


        public Tweet(string m) : base(m)
        {
            this.MessageText = m;

            hashtags = new List<string>();
            mentions = new List<string>();

        }

        private void FindSender()
        {
            throw new NotImplementedException();
        }

    }
}
