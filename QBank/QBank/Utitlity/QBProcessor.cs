using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Qrator.Common;
using System.Reflection;
using System.Windows.Forms;


    public static class QBProcessor
    {
        public static void SetDefaultTopic(QuestionBank qb)
        {
            foreach (Question q in qb.Questions)
            {
                if (!MasterUtil.IsTopicValid(q.Topic, q.Subject, qb.ExamGrade))
                {
                    q.Topic = "Others";
                }
            }
            QBManager.Save();
        }

        public static string ValidateQuestionBank(QuestionBank qb)
        {
            string[] topics = new string[] { "Easy", "Moderate", "Hard" };

            StringBuilder sb = new StringBuilder();
            if (!MasterUtil.IsExamValid(qb.ExamGrade))
            {
                sb.AppendLine(string.Format("Examination name {0} is invalid.", qb.ExamGrade));
            }

            ImageProcessor _processor = new ImageProcessor(Path.GetTempPath());

            int i = 1;
            foreach (Question q in qb.Questions)
            {
                if (!MasterUtil.IsSubjectValid(q.Subject, qb.ExamGrade))
                {
                    sb.AppendLine(string.Format("{0}. Subject name {1} is invalid for the selected examination/grade {2}.", i, q.Subject, qb.ExamGrade));
                }

                if (!MasterUtil.IsTopicValid(q.Topic, q.Subject, qb.ExamGrade))
                {
                    sb.AppendLine(string.Format("{0}. Topic name {1} is invalid for the selected subject {2}.", i, q.Topic, q.Subject));
                }

                if (q.Complexity < 1 && q.Complexity > 3)
                {
                    sb.AppendLine(string.Format("Complexity is invalid for the question number {0}.", i));
                }

                if (!_processor.ValidateImage(q.QuestionText))
                {
                    sb.AppendLine(string.Format("Image in the question text for question number {0} is invalid or corrupt.", i));
                }

                if (!_processor.ValidateImage(q.AnswerExplanation))
                {
                    sb.AppendLine(string.Format("Image in the answer explanation text for question number {0} is invalid or corrupt.", i));
                }

                int j = 1;
                foreach (Answer a in q.OptionTexts)
                {
                    if (!_processor.ValidateImage(a.OptionText))
                    {
                        sb.AppendLine(string.Format("Image in the option text {0} for question number {1} is invalid  or corrupt.", j, i));
                    }
                    j++;
                }

                i++;
            }
            return sb.ToString();
        }

        public static void CleanseQuestionBankData(Question q)
        {
            q.QuestionText = RemoveUnwantedTags(q.QuestionText);
            if (q.Instruction != null)
                q.Instruction = RemoveUnwantedTags(q.Instruction);

            foreach (Answer a in q.OptionTexts)
            {
                if (a.OptionText != null)
                    a.OptionText = RemoveUnwantedTags(a.OptionText);
            }
            if (q.AnswerExplanation != null)
                q.AnswerExplanation = RemoveUnwantedTags(q.AnswerExplanation);
        }

        private static string RemoveUnwantedTags(string data)
        {
            //var document = new HtmlDocument();

            //document.LoadHtml(data);

            //RemoveAttributes(document.DocumentNode);

            ////Remove Font tags
            //var pElements = document.DocumentNode.SelectNodes("//font");
            //if (pElements != null)
            //{
            //    for (int i = pElements.Count - 1; i <= 0; i++)
            //    {
            //        pElements.Remove(pElements[i]);
            //    }
            //}

            return data;
        }

        //todo : redo
        private static void RemoveAttributes(object targetNode)
        {
            //string[] attributesToRemove = new string[] { "class", "font", "style", "lang"};

            //foreach (string s in attributesToRemove)
            //{
            //    //remove style elements
            //    var removeableAttribute = targetNode.SelectNodes("//@" + s);

            //    if (removeableAttribute != null)
            //    {
            //        foreach (var element in removeableAttribute)
            //        {
            //            element.Attributes[s].Remove();
            //        }
            //    }
            //}
        }
    }

