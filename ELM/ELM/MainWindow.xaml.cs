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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //creates an object which stores a list of XML objects
        XMLMessageList XmlData;
        XMLDeserializer DataProcessor = new XMLDeserializer();

        List<SMS> SMSMessageList = new List<SMS>();
        List<Email> emailMessageList = new List<Email>();
        List<Tweet> tweetMessageList = new List<Tweet>();

        int i = 0;
        int y = 0;

        public MainWindow()
        {
            //create a new Serialization object
            //use the Serialization object's deserialize method to read the data from XML file and add to list in XmlData object
            
            XmlData = DataProcessor.deserializeXML();
            InitializeComponent();

            inputHeader.Text = XmlData.messageList[i].Header;
            inputBody.Text = XmlData.messageList[y].Body;


        }

        private void NxtMsg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inputHeader.Text = XmlData.messageList[i += 1].Header;
                inputBody.Text = XmlData.messageList[y += 1].Body;

            }
            catch (Exception)
            {
                MessageBox.Show("No more messages to display");
            }
        }

        //on button click , determines the Message type by the MessageID
        private void ProcessBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //reads the first character from the header and determines which object type to create based on that.
                char[] headerArray = inputHeader.Text.ToCharArray();


                if (headerArray[0] == 'T')
                {
                    Tweet newTweet = new Tweet(Convert.ToString(inputBody.Text));
                    tweetMessageList = new List<Tweet>();
                }

                if (headerArray[0] == 'E')
                {
                    Email newEmail = new Email(Convert.ToString(inputBody.Text));
                    if (newEmail.Sender == null)
                    {
                        MessageBox.Show("Message cannot be processed, Please ensure email has a subject and a sender");
                    }
                    else
                    {
                        emailMessageList.Add(new Email(Convert.ToString(inputBody.Text)));
                        string jsonEmail = JsonConvert.SerializeObject(newEmail, Formatting.Indented);
                        outputBody.Text = jsonEmail;


                    }

                }

                if (headerArray[0] == 'S')
                {
                    //create SMS object , serialize to Json and add to list of Json Strings

                    SMS newSMS = new SMS(inputBody.Text);
                    if (newSMS.Sender == null)
                    {
                        MessageBox.Show("Message cannot be stored, no sender found");
                    }
                    else
                    {
                        SMSMessageList.Add(new SMS(Convert.ToString(inputBody.Text)));
                        string jsonSMS = JsonConvert.SerializeObject(newSMS, Formatting.Indented);
                        outputBody.Text = jsonSMS;
                    }
                }
            }

            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }


        }

        private void FinishBtn_Click(object sender, RoutedEventArgs e)
        {
            

            using (StreamWriter file = new StreamWriter(MessageFilter.JSONpath, true))
            {
                file.WriteLine("This is a compiled list of messages");
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                if (SMSMessageList != null)
                    serializer.Serialize(file, SMSMessageList);
                file.WriteLine();
                if (emailMessageList != null)
                {
                    foreach (var s in emailMessageList)
                    {
                        file.WriteLine("");
                        serializer.Serialize(file, s);
                    }
                    //serializer.Serialize(file, emailMessageList);
                }
                if (tweetMessageList != null)
                    serializer.Serialize(file, tweetMessageList);

                file.Close();
            }
           
            using (StreamWriter file2 = new StreamWriter(MessageFilter.QuarantinePath, true))
            {
                file2.WriteLine("Messages quarantined during session: " + DateTime.Now);
                foreach (String s in MessageFilter.emailList)
                    file2.WriteLine(s);

                file2.Close();

            }


        }
    }
}
