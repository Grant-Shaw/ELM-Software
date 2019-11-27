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

    //class which defines the properties of an XML message (used by deserializer)

    [XmlRoot(ElementName = "Message")]
    public class XMLMessage
    {

        public string Header { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            return "Header: " + Header + "\n" + "Message Body: " + Body + "\n";
        }

    }
}
