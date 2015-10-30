using Qrator.ServerData;
using Qrator.ServerData.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Qrator.Utility
{
    public static class SeedManager
    {
        public static void Seed(string connectionString)
        {
            DataManager mgr = new DataManager(connectionString);
            mgr.AddCountry("IN");

            Stream s = typeof(Qrator.Utility.SeedManager).Assembly.GetManifestResourceStream("Qrator.Utility.master.xml");
            XmlDocument mappingFile = new XmlDocument();
            mappingFile.Load(s);
            XmlNodeList exams = mappingFile.DocumentElement.ChildNodes;
            string[] examNames = new string[exams.Count];
            foreach (XmlNode x in exams)
            {
                //Add Examinations
                string exam = x.Attributes[0].Value;
                mgr.AddExamination("IN", exam);
                foreach (XmlNode y in x.ChildNodes[0].ChildNodes)
                {
                    //Add Subjects
                    string subject= y.Attributes[0].Value;
                    mgr.AddSubject(exam, subject);
                    foreach(XmlNode z in y.ChildNodes[0].ChildNodes)
                    {
                        //Add Topics
                        string topic = z.InnerText;
                        mgr.AddTopic(exam, subject, topic);
                    }
                }
            }
            s.Close();
        }
    }
}
