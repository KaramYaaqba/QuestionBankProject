using Qrator.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


    public static class QBManager
    {
        public static string FileName { get; set; }
        public static QuestionBank QBank { get; set; }

        public static void Save()
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(QBank.GetType());
            TextWriter tw = new StreamWriter(FileName);
            x.Serialize(tw, QBank);
            tw.Close();
            if (ConfigurationManager.AppSettings["Encrypt"] == "1")
                Crytographer.EncryptFile(FileName);
        }

        public static void Load()
        {
            if (ConfigurationManager.AppSettings["Encrypt"] == "1")
                Crytographer.DecryptFile(FileName);

            // Create an instance of the XmlSerializer specifying type.
            XmlSerializer serializer = new XmlSerializer(typeof(QuestionBank));

            // Create a TextReader to read the file. 
            FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
            TextReader reader = new StreamReader(fs);

            // Use the Deserialize method to restore the object's state.
            QBank = (QuestionBank)serializer.Deserialize(reader);
            reader.Close();
        }
    }

