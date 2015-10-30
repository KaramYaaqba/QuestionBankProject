using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Qrator.Utility
{
    public static class MasterUtil
    {
 
        private static string[] ExecuteXPath(string xPath, bool isTopic)
        {
            Stream s = typeof(MasterUtil).Assembly.GetManifestResourceStream("Qrator.Utility.master.xml");
            XmlDocument mappingFile = new XmlDocument();
            mappingFile.Load(s);
            XmlNodeList exams = mappingFile.SelectNodes(xPath);
            string[] examNames = new string[exams.Count];
            int i = 0;
            foreach (XmlNode x in exams)
            {
                if (isTopic)
                    examNames[i] = x.InnerText;
                else
                    examNames[i] = x.Attributes[0].Value;
                i++;
            }
            s.Close();
            return examNames;
        }

        public static string[] GetExaminations()
        {
            string exprs = "//examinations/examination";
            return ExecuteXPath(exprs, false);
        }

        public static string[] GetSubjects(string examName)
        {
            string exprs = "//examinations/examination[@name = \"" + examName + "\"]/subjects/subject";
            return ExecuteXPath(exprs, false);
        }

        public static string[] GetTopics(string exam, string subjectName)
        {
            string exprs = "//examination[@name=\"" + exam + "\"]/subjects/subject[@name = \"" + subjectName + "\"]/topics/topic";
            return ExecuteXPath(exprs, true);
        }

        public static bool IsExamValid(string examName)
        {
            string criteria = "//examination[@name=\"" + examName + "\"]";
            return (CountNodes(criteria) > 0);
        }

        public static bool IsSubjectValid(string subjecName, string examName)
        {
            string criteria = "//examination[@name=\"" + examName + "\"]/subjects/subject[@name = \"" + subjecName + "\"]";
            return (CountNodes(criteria) > 0);
        }

        public static bool IsTopicValid(string topicName, string subjecName, string examName)
        {
            try
            {
                string criteria = "//examination[@name=\"" + examName + "\"]/subjects/subject[@name = \"" + subjecName + "\"]/topics/topic[. = \"" + topicName + "\"]";
                return (CountNodes(criteria) > 0); 
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private static int CountNodes(string criteria)
        {
            Stream s = typeof(MasterUtil).Assembly.GetManifestResourceStream("Qrator.Utility.master.xml");
            XmlDocument mappingFile = new XmlDocument();
            mappingFile.Load(s);
            XmlNodeList nodes = mappingFile.SelectNodes(criteria);
            s.Close();
            return nodes.Count;
        }
    }
}
