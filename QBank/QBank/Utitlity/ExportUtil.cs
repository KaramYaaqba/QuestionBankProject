//using HtmlAgilityPack;
//using OfficeOpenXml;
using Qrator.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Qrator.Utility
{
    public static class ExportUtil
    {
        public static DataSet GetExportableData(QuestionBank qb, string filePath)
        {
            //Creating DataTable for header
            DataTable dtHeader = new DataTable("Practice Test");
            dtHeader.Columns.Add("Header", typeof(String));
            dtHeader.Columns.Add("Value", typeof(String));

            DataRow hr1 = dtHeader.NewRow();
            hr1[0] = "Name";
            hr1[1] = qb.Name;
            dtHeader.Rows.Add(hr1);
            DataRow hr2 = dtHeader.NewRow();
            hr2[0] = "Description";
            hr2[1] = qb.Description;
            dtHeader.Rows.Add(hr2);
            DataRow hr3 = dtHeader.NewRow();
            hr3[0] = "Time Allocated";
            hr3[1] = qb.TimeAllocated.ToString();
            dtHeader.Rows.Add(hr3);
            DataRow hr4 = dtHeader.NewRow();
            hr4[0] = "Country";
            hr4[1] = "IN";
            dtHeader.Rows.Add(hr4);
            DataRow hr5 = dtHeader.NewRow();
            hr5[0] = "Examination";
            hr5[1] = qb.ExamGrade;
            dtHeader.Rows.Add(hr5);

            DataRow hr6 = dtHeader.NewRow();
            hr6[0] = "Positive Marks";
            hr6[1] = qb.PositiveMarks;
            dtHeader.Rows.Add(hr6);

            DataRow hr7 = dtHeader.NewRow();
            hr7[0] = "Negative Marks";
            hr7[1] = qb.NegativeMarks;
            dtHeader.Rows.Add(hr7);

            DataTable dtQuestions = new DataTable("Questions");
            dtQuestions.Columns.Add("Examination", typeof(String));
            dtQuestions.Columns.Add("Subject", typeof(String));
            dtQuestions.Columns.Add("Topic", typeof(String));
            dtQuestions.Columns.Add("Complexity", typeof(String));

            dtQuestions.Columns.Add("Instuction", typeof(String));
            dtQuestions.Columns.Add("Positive Marks", typeof(String));
            dtQuestions.Columns.Add("Negative Marks", typeof(String));

            dtQuestions.Columns.Add("Question", typeof(String));
            dtQuestions.Columns.Add("Option 1", typeof(String));
            dtQuestions.Columns.Add("Option 2", typeof(String));
            dtQuestions.Columns.Add("Option 3", typeof(String));
            dtQuestions.Columns.Add("Option 4", typeof(String));
            dtQuestions.Columns.Add("Option 5", typeof(String));
            dtQuestions.Columns.Add("Answers", typeof(String));
            dtQuestions.Columns.Add("Explanation", typeof(String));

            DataSet ds = new DataSet();
            ds.Tables.Add(dtHeader);

            ImageProcessor _processor = new ImageProcessor(filePath);

            foreach (Question q in qb.Questions)
            {
                //Remove style attribute
                QBProcessor.CleanseQuestionBankData(q);

                DataRow drow = dtQuestions.NewRow();
                drow[0] = qb.ExamGrade;
                drow[1] = q.Subject;
                drow[2] = q.Topic;
                
                if (q.Complexity == 1)
                    drow[3] = "Easy";
                else if (q.Complexity == 3)
                    drow[3] = "Hard";
                else
                    drow[3] = "Moderate";

                drow[4] = _processor.ExtractImage(q.Instruction);
                drow[5] = q.PositiveMarks;
                drow[6] = q.NegativeMarks;

                //Extract images from Question
                drow[7] = _processor.ExtractImage(q.QuestionText);

                //Initialize Option texts
                drow[8] = "";
                drow[9] = "";
                drow[10] = "";
                drow[11] = "";
                drow[12] = "";
                drow[13] = "";
                
                //Extract images from answer explanation
                drow[14] = _processor.ExtractImage(q.AnswerExplanation);

                int i = 8;
                foreach (Answer a in q.OptionTexts)
                {
                    drow[i] = _processor.ExtractImage(a.OptionText);
                    if (a.IsCorrect)
                    {
                        if (drow[13].ToString() == "")
                            drow[13] = (i - 7).ToString();
                        else
                            drow[13] = drow[13]+ ", " + (i - 7).ToString();
                    }
                    i++;
                }
                dtQuestions.Rows.Add(drow);
            }

            ds.Tables.Add(dtQuestions);

            return ds;
        }

        public static void ExportDatasetToExcel(DataSet ds, string destination)
        {
            //string[] columns = new string[]{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O"};

            //FileInfo newFile = new FileInfo(destination);

            //using (ExcelPackage package = new ExcelPackage(newFile))
            //{
            //    //Process first sheet 
            //    DataTable ds1 = ds.Tables[0];
            //    ExcelWorksheet worksheet1 = package.Workbook.Worksheets.Add(ds1.TableName);

            //    //Headers
            //    worksheet1.Cells["A1"].Value = ds1.Columns[0].ColumnName;
            //    worksheet1.Cells["B1"].Value = ds1.Columns[1].ColumnName;
            //    worksheet1.Cells["A1:B1"].Style.Font.Bold = true;

            //    //Data
            //    int i = 2, j = 0;
            //    string colChar = "";
            //    foreach (DataRow row in ds1.Rows)
            //    {
            //        j = 0;
            //        foreach (DataColumn column in ds1.Columns)
            //        {
            //            colChar = columns[j];
            //            worksheet1.Cells[colChar + i.ToString()].Value = row[column];
            //            j++;
            //        }
            //        i++;
            //    }

            //    //Process second sheet 
            //    DataTable ds2 = ds.Tables[1];
            //    ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add(ds2.TableName);

            //    //Headers
            //    j = 0;
            //    foreach (DataColumn column in ds2.Columns)
            //    {
            //        colChar = columns[j];
            //        worksheet2.Cells[colChar + "1"].Value = column.ColumnName;
            //        j++;
            //    }

            //    //Data
            //    i = 2;
            //    colChar = "";
            //    foreach (DataRow row in ds2.Rows)
            //    {
            //        j = 0;
            //        foreach (DataColumn column in ds2.Columns)
            //        {
            //            colChar = columns[j];
            //            worksheet2.Cells[colChar + i.ToString()].Value = row[column];
            //            j++;
            //        }
            //        i++;
            //    }
            //    package.Save();
            //}
        }
    }
}
