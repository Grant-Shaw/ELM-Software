using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

namespace ELM
{
    //a class which creates a list of type XML message.
    public class XMLMessageList
    {
        [XmlElement("Message")]
        public List<XMLMessage> messageList = new List<XMLMessage>();

    }
}
