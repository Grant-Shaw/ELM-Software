using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ELM
{
    public class XMLDeserializer
    {
        public XMLMessageList deserializeXML()
        {
            try
            {

                XmlSerializer deserializer = new XmlSerializer(typeof(XMLMessageList));
                var path = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + "\\Messages.xml");
                TextReader reader = new StreamReader(path);
                object obj = deserializer.Deserialize(reader);

                XMLMessageList Data = (XMLMessageList)obj;
                reader.Close();
                return Data;
            }

            catch(Exception e)
            {
                throw new Exception("No XML file found");
            }
        }

    }
}
