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
using System.Text.RegularExpressions;
using System.IO;


namespace ELM
{
    public static class MessageFilter
    {
        public static string JSONpath = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + "\\jsontest.Json");
        public static string QuarantinePath = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + "\\QuarantineList.txt");
        public static string textSpeakPath = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + "\\textwords.csv");
        public static string incidentPath = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + "\\incidentList.csv");

        public static Dictionary<string, string> dict = File.ReadLines(textSpeakPath).Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);
        public static List<string> emailList = new List<string>();
       
   
        


    }
}
