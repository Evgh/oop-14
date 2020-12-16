using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace oop_14
{
    public static class CustomSerializer<T>
    {
        static BinaryFormatter Bformatter = new BinaryFormatter();
        static SoapFormatter Sformatter = new SoapFormatter();
        
        public static void SerializeBinary(T t, string str)
        {
            using (FileStream fs = new FileStream(str, FileMode.OpenOrCreate))
            {
                Bformatter.Serialize(fs, t);
            }
        }
        public static T DeserializeBinary(string str)
        {
            using (FileStream fs = new FileStream(str, FileMode.OpenOrCreate))
            {
                return (T)Bformatter.Deserialize(fs);
            }
        }
        public static void SerializeSoap(T t, string str)
        {
            using (FileStream fs = new FileStream(str, FileMode.OpenOrCreate))
            {
                Sformatter.Serialize(fs, t);
            }
        }
        public static T DeserializeSoap(string str)
        {
            using (FileStream fs = new FileStream(str, FileMode.OpenOrCreate))
            {
                return (T)Sformatter.Deserialize(fs);
            }
        }

        public static void SerializeXML(T t, string str)
        {
            XmlSerializer Xformatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(str, FileMode.OpenOrCreate))
            {
                Xformatter.Serialize(fs, t);
            }
        }
        public static T DeserializeXML(string str)
        {
            XmlSerializer Xformatter = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(str, FileMode.OpenOrCreate))
            {
                return (T)Xformatter.Deserialize(fs);
            }
        }
        public static void SerializeJSON(T t, string str)
        {
            using (StreamWriter sw = new StreamWriter(str, false))
            {
                sw.WriteLine(JsonConvert.SerializeObject(t, Newtonsoft.Json.Formatting.Indented));
            }
        }

        public static T DeserializeJSON(string str)
        {
            using (StreamReader sr = new StreamReader(str))
            {
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
/*            using (StreamReader sw = new StreamReader(str))
            {
                string st = sw.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(st);
            }
*/        }
    }

    [Serializable]
    public struct Tech
    {
        public string Type;
        public Tech( string str)
        {
            Type = str;
        }
    }

    [Serializable]
    public struct Computer
    {
        public Tech Hardware;
        public string Software;

        public Computer(string str, string s)
        {
            Software = str;
            Hardware = new Tech(s);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Computer comp = new Computer("Soft", "Hard");
            CustomSerializer<Computer>.SerializeJSON(comp, "SerializeJ.json"); 
            Computer compDJSON = CustomSerializer<Computer>.DeserializeJSON("SerializeJ.json");
            
            CustomSerializer<Computer>.SerializeXML(comp, "SerializeXML.xml");
            Computer compDXML = CustomSerializer<Computer>.DeserializeXML("SerializeXML.xml");
            
            CustomSerializer<Computer>.SerializeSoap(comp, "SerializeSoap.soap");
            Computer compDSOAP = CustomSerializer<Computer>.DeserializeSoap("SerializeSoap.soap");
            
            CustomSerializer<Computer>.SerializeBinary(comp, "SerializeBin.dat");
            Computer compDBin = CustomSerializer<Computer>.DeserializeBinary("SerializeBin.dat");

            ///
            Computer ccomp = new Computer("UnSoft", "UnHard");
            Computer[] comps = new Computer[] { comp, compDJSON, compDXML, compDSOAP, compDBin, ccomp};
            CustomSerializer<Computer[]>.SerializeXML(comps, "SerializeXML.xml");
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"D:\OOP\oop-14\bin\Debug\SerializeXML1.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            XmlNodeList childnodes = xRoot.SelectNodes("Computer");
            foreach (XmlNode x in childnodes)
                Console.WriteLine(x.OuterXml);
            XmlNode childnode = xRoot.SelectSingleNode("Computer[Software='UnSoft']");
            Console.WriteLine(childnode.OuterXml);


            Console.ReadKey();
        }
    }
}
