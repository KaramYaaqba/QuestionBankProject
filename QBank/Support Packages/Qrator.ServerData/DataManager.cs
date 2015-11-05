using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qrator.ServerData.Entities;
using System.Diagnostics;
using System.Data.Entity.Validation;
using Qrator.Common;
using System.Data.Entity.Core.EntityClient;

namespace Qrator.ServerData.Entities
{
    public class QuestionModel: Qrator.ServerData.Entities.Question
    {
        public string TopicName { get; set; }
        public string SubjectName { get; set; }
    }

    public class DataManager
    {
        public string ConnectionString { get; set; }

        public DataManager(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        protected internal QBankEntities CreateConnection()
        {
            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = "System.Data.SqlClient";

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = this.ConnectionString;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/Entities.Model1.csdl|res://*/Entities.Model1.ssdl|res://*/Entities.Model1.msl";
            return new QBankEntities(entityBuilder.ToString());
        }

        public void TestConnection()
        {
            QBankEntities context = this.CreateConnection();
            context.Database.Connection.Open();
        }

        public List<Qrator.Common.Question> GetQuestionsForModeration(string examName, string subjectName, string topicName)
        {
            using(QBankEntities context = this.CreateConnection())
            {
                IList<QuestionModel> questions = null;

                if (topicName == null || topicName == "")
                {
                    questions = (from q in context.Questions.Include("Answer")
                                     join t in context.Topics on q.TopicID equals t.ID
                                     join s in context.Subjects on t.SubjectID equals s.ID
                                     join e in context.Examinations on s.ExaminationID equals e.ID
                                     where e.Name == examName && s.Name == subjectName
                                     select new QuestionModel
                                     {
                                         Answers = q.Answers,
                                         Complexity = q.Complexity,
                                         Explanation = q.Explanation,
                                         HeaderText = q.HeaderText,
                                         ID = q.ID,
                                         MasterCode = q.MasterCode,
                                         ModifiedDate = q.ModifiedDate,
                                         QuestionText = q.QuestionText,
                                         Status = q.Status,
                                         SubjectName = s.Name,
                                         TopicName = t.Name
                                     }).ToList();

                }
                else
                {
                    questions = (from q in context.Questions.Include("Answer")
                                 join t in context.Topics on q.TopicID equals t.ID
                                 join s in context.Subjects on t.SubjectID equals s.ID
                                 join e in context.Examinations on s.ExaminationID equals e.ID
                                 where e.Name == examName && s.Name == subjectName && t.Name == topicName
                                 select new QuestionModel
                                 {
                                     Answers = q.Answers,
                                     Complexity = q.Complexity,
                                     Explanation = q.Explanation,
                                     HeaderText = q.HeaderText,
                                     ID = q.ID,
                                     MasterCode = q.MasterCode,
                                     ModifiedDate = q.ModifiedDate,
                                     QuestionText = q.QuestionText,
                                     Status = q.Status,
                                     SubjectName = s.Name,
                                     TopicName = t.Name
                                 }).ToList();
                }

                List<Qrator.Common.Question> questionList = new List<Qrator.Common.Question>();
                //Translate format
                foreach (var q in questions)
                {
                    Qrator.Common.Question qm = new Qrator.Common.Question();

                    qm.AnswerExplanation = q.Explanation;
                    qm.Subject = q.SubjectName;
                    qm.Topic = q.TopicName;
                    qm.Complexity = q.Complexity;
                    qm.LastModifiedDate = q.ModifiedDate;
                    qm.ID = q.ID.ToString();
                    qm.Status = q.Status;
                    qm.Instruction = q.HeaderText;
                    qm.Status = q.Status;
                    qm.MasterCode = q.MasterCode;
                    qm.QuestionText = q.QuestionText;
                    foreach (Qrator.ServerData.Entities.Answer a in q.Answers)
                    {
                        Qrator.Common.Answer am = new Qrator.Common.Answer();
                        am.IsCorrect = a.IsCorrect;
                        am.MasterCode = a.MasterCode;
                        am.Status = a.Status;
                        am.OptionText = a.OptionText;
                        qm.OptionTexts.Add(am);
                    }
                    questionList.Add(qm);
                }
                return questionList;
            }
        }

        public int GetTopicID(int examID, string subject, string topic)
        {
            using(QBankEntities context = this.CreateConnection())
            {
                var query = (from t in context.Topics
                             join s in context.Subjects on t.SubjectID equals s.ID
                             join e in context.Examinations on s.ExaminationID equals e.ID
                             where t.Name == topic
                             && s.Name == subject
                             && e.ID == examID
                             select t);

                Topic t1 = query.FirstOrDefault();
                if (t1 != null)
                    return t1.ID;
                else
                {
                    throw new Exception(String.Format("Topic {0} not found for the subject {1}.", topic, subject));
                }
            }
        }

        public int GetExaminationID(string exam)
        {
            using(QBankEntities context = this.CreateConnection())
            {
                Examination e = context.Examinations.Where(x => x.Name == exam).FirstOrDefault();
                if (e != null)
                    return e.ID;
                else
                {
                    throw new Exception(String.Format("Examination {0} not found", exam));
                }
            }
        }
        public void AddCountry(string countryName)
        {
            using(QBankEntities context = this.CreateConnection())
            {
                Country c1 = context.Countries.Where(x => x.Name == countryName).FirstOrDefault();
                if (c1 == null)
                {
                    Country c = new Country();
                    c.Name = countryName;
                    c.IsActive = true;
                    context.Countries.Add(c);
                    context.SaveChanges();
                }
                else
                {
                    //throw new Exception(String.Format("Country {0} already exists.", countryName));
                    Trace.TraceWarning(String.Format("Country {0} already exists.", countryName));
                }
            }
        }

        public void AddExamination(string countryName, string examName)
        {
            using(QBankEntities context = this.CreateConnection())
            {
                Country c1 = context.Countries.Where(x => x.Name == countryName).FirstOrDefault();
                if (c1 != null)
                {
                    Examination e1 = context.Examinations.Where(x => x.Name == examName).FirstOrDefault();
                    if (e1 == null)
                    {
                        Examination e = new Examination();
                        e.Name = examName;
                        e.IsActive = true;
                        e.CountryID = c1.ID;
                        context.Examinations.Add(e);
                        context.SaveChanges();
                    }
                    else
                    {
                        //throw new Exception(String.Format("Examination {0} already exists.", examName));
                        Trace.TraceWarning(String.Format("Examination {0} already exists.", examName));
                    }
                }
                else
                {
                    //throw new Exception(String.Format("Country {0} doesn't exist.", countryName));
                    Trace.TraceWarning(String.Format("Country {0} doesn't exist.", countryName));
                }
            }
        }

        public void AddSubject(string examName, string subjectName)
        {
            using(QBankEntities context = this.CreateConnection())
            {
                Examination e = context.Examinations.Where(x => x.Name == examName).FirstOrDefault();
                if (e != null)
                {
                    Subject s2 = context.Subjects.Where(x => x.Name == subjectName && x.Examination.Name == examName).FirstOrDefault();
                    if (s2 == null)
                    {
                        Subject s = new Subject();
                        s.Name = subjectName;
                        s.IsActive = true;
                        s.ExaminationID = e.ID;
                        context.Subjects.Add(s);
                        context.SaveChanges();
                    }
                    else
                    {
                        //throw new Exception(String.Format("Subject {0} already exists.", subjectName));
                        Trace.TraceWarning(String.Format("Subject {0} already exists.", subjectName));
                    }
                }
                else
                {
                    //throw new Exception(String.Format("Examination {0} doesn't exist.", examName));
                    Trace.TraceWarning(String.Format("Examination {0} doesn't exist.", examName));
                }
            }
        }

        public void AddTopic(string examName, string subjectName, string topicName)
        {
            using(QBankEntities context = this.CreateConnection())
            {
                Examination e = context.Examinations.Where(x => x.Name == examName).FirstOrDefault();
                if (e != null)
                {
                    Subject s = context.Subjects.Where(x => x.Name == subjectName && x.Examination.Name == examName).FirstOrDefault();
                    if (s != null)
                    {
                        Topic t1 = context.Topics.Where(x => x.Name == topicName && x.Subject.Name == subjectName && x.Subject.Examination.Name == examName).FirstOrDefault();
                        if (t1 == null)
                        {
                            Topic t = new Topic();
                            t.Name = topicName;
                            t.IsActive = true;
                            t.SubjectID = s.ID;
                            context.Topics.Add(t);
                            context.SaveChanges();
                        }
                        else
                        {
                            //throw new Exception(String.Format("Topic {0} already exists.", topicName));
                            Trace.TraceWarning(String.Format("Topic {0} already exists.", topicName));
                        }
                    }
                    else
                    {
                        //throw new Exception(String.Format("Subject {0} doesn't exist.", subjectName));
                        Trace.TraceWarning(String.Format("Subject {0} doesn't exist.", subjectName));
                    }
                }
                else
                {
                    //throw new Exception(String.Format("Examination {0} doesn't exist.", examName));
                    Trace.TraceWarning(String.Format("Examination {0} doesn't exist.", examName));
                }
            }
        }

        public void SaveDataEntryPracticeTest(QuestionBank qb)
        {
            using (QBankEntities context = this.CreateConnection())
            {
                if (qb.MasterCode == null)
                {
                    try
                    {
                        PracticeTest pt = new PracticeTest();
                        pt.AddedBy = qb.CreatedBy;
                        pt.ExaminationID = GetExaminationID(qb.ExamGrade);
                        pt.MasterCode = Guid.NewGuid().ToString();
                        pt.Name = qb.Name;
                        pt.Source = qb.Description;
                        pt.AddedDate = DateTime.Now;
                        pt.Status = 1;

                        context.PracticeTests.Add(pt);
                        context.SaveChanges();
                        qb.MasterCode = pt.MasterCode;
                    }
                    catch (Exception ex)
                    {
                        qb.MasterCode = string.Empty;
                    }
                }
                else
                {
                    PracticeTest p = context.PracticeTests.Where(x => x.MasterCode == qb.MasterCode && qb.Status == 1).FirstOrDefault();
                    bool changed = false;

                    p.ModifiedDate = DateTime.Now;
                    if (p.Name != qb.Name)
                    {
                        p.Name = qb.Name;
                        changed = true;
                    }
                    if (p.Source != qb.Description)
                    {
                        p.Source = qb.Description;
                        changed = true;
                    }
                    //p.ExaminationID = pt.ExaminationID;
                    if (changed)
                        context.SaveChanges();
                }
            }
        }

        public void SaveModeratedPracticeTest(QuestionBank qb)
        {
            using (QBankEntities context = this.CreateConnection())
            {
                if (qb.Status == 1)
                {
                    try
                    {
                        PracticeTest pt = new PracticeTest();
                        pt.AddedBy = qb.CreatedBy;
                        pt.ExaminationID = GetExaminationID(qb.ExamGrade);
                        pt.MasterCode = qb.MasterCode;
                        pt.Name = qb.Name;
                        pt.Source = qb.Description;
                        pt.AddedDate = DateTime.Now;
                        pt.Status = 2;

                        context.PracticeTests.Add(pt);
                        context.SaveChanges();
                        qb.Status = 2;
                    }
                    catch (Exception ex)
                    {
                        //Revert back status code
                        qb.Status = 1;
                    }
                }
                else
                {
                    PracticeTest p = context.PracticeTests.Where(x => x.MasterCode == qb.MasterCode && qb.Status == 2).FirstOrDefault();
                    bool changed = false;

                    p.ModifiedDate = DateTime.Now;
                    if (p.Name != qb.Name)
                    {
                        p.Name = qb.Name;
                        changed = true;
                    }
                    if (p.Source != qb.Description)
                    {
                        p.Source = qb.Description;
                        changed = true;
                    }
                    //p.ExaminationID = pt.ExaminationID;
                    if (changed)
                        context.SaveChanges();
                }
            }
        }

        public void SaveModeratedQuestion(Qrator.Common.Question Q, string psCode)
        {
            using (QBankEntities context = this.CreateConnection())
            {
                if (Q.IsDeleted)
                {
                    //Delete only the one which has been moderated - those brought by moderator and deleted before sync (status = 1) will be left in the qbf file.
                    Qrator.ServerData.Entities.Question qt = context.Questions.Where(x => x.MasterCode == Q.MasterCode && x.Status == 2).FirstOrDefault();
                    context.Questions.Remove(qt);
                    context.SaveChanges();
                    return;
                }

                PracticeTest ps = context.PracticeTests.Where(p => p.MasterCode == psCode && p.Status == 2).FirstOrDefault();

                if (Q.Status == 1)
                {
                    //Save new record
                    try
                    {
                        ServerData.Entities.Question qtn = new ServerData.Entities.Question();
                        qtn.Complexity = Q.Complexity;
                        qtn.Explanation = Q.AnswerExplanation;
                        qtn.HeaderText = Q.Instruction;
                        qtn.QuestionText = Q.QuestionText;
                        qtn.TopicID = GetTopicID(ps.ExaminationID, Q.Subject, Q.Topic);
                        qtn.Status = 2;
                        qtn.MasterCode = Q.MasterCode;
                        qtn.ModifiedDate = DateTime.Now;
                        qtn.PracticeTestID = ps.ID;

                        foreach (var x in Q.OptionTexts)
                        {
                            ServerData.Entities.Answer a = new ServerData.Entities.Answer();
                            a.OptionText = x.OptionText;
                            a.IsCorrect = x.IsCorrect;
                            a.MasterCode = x.MasterCode;
                            a.ModifiedDate = DateTime.Now;
                            a.Status = 2;
                            qtn.Answers.Add(a);
                        }

                        context.Questions.Add(qtn);
                        context.SaveChanges();

                        //Sync back
                        Q.Status = qtn.Status;
                        foreach (var x in Q.OptionTexts)
                        {
                            x.Status = 2;
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                                        validationError.PropertyName,
                                                        validationError.ErrorMessage);
                            }
                        }
                        Q.Status = 1;
                        foreach (var x in Q.OptionTexts)
                        {
                            x.Status = 1;
                        }
                    }
                }
                else
                {
                    //Update record
                    int i = 0;
                    Qrator.ServerData.Entities.Question qt = context.Questions.Where(x => x.MasterCode == Q.MasterCode && x.Status == 2).FirstOrDefault();
                    if (qt.Complexity != Q.Complexity)
                    {
                        qt.Complexity = Q.Complexity;
                        i++;
                    }
                    if (qt.Explanation != Q.AnswerExplanation)
                    {
                        qt.Explanation = Q.AnswerExplanation;
                        i++;
                    }
                    if (qt.HeaderText != Q.Instruction)
                    {
                        qt.HeaderText = Q.Instruction;
                        i++;
                    }
                    if (qt.QuestionText != Q.QuestionText)
                    {
                        qt.QuestionText = Q.QuestionText;
                        i++;
                    }

                    if (qt.Topic.Name != Q.Topic)
                    {
                        qt.TopicID = GetTopicID(ps.ExaminationID, Q.Subject, Q.Topic);
                        i++;
                    }

                    bool answerChanged = false; //Use this variable to update last modified date of each answer option
                    //Update Answers
                    foreach (var item in Q.OptionTexts)
                    {
                        Qrator.ServerData.Entities.Answer a = qt.Answers.Where(x => x.MasterCode == item.MasterCode && x.Status == 2).FirstOrDefault();
                        if (a.IsCorrect != item.IsCorrect)
                        {
                            a.IsCorrect = item.IsCorrect;
                            answerChanged = true;
                            i++;
                        }
                        if (a.OptionText != item.OptionText)
                        {
                            answerChanged = true;
                            a.OptionText = item.OptionText;
                            i++;
                        }
                        if (answerChanged)
                            a.ModifiedDate = DateTime.Now;

                        answerChanged = false;
                    }
                    qt.ModifiedDate = DateTime.Now;

                    if (i > 0)
                        context.SaveChanges();
                }
            }
        }

        public void SaveDataEntryQuestion(Qrator.Common.Question Q, string psCode)
        {
            using (QBankEntities context = this.CreateConnection())
            {
                PracticeTest ps = context.PracticeTests.Where(p => p.MasterCode == psCode && p.Status == 1).FirstOrDefault();

                if (Q.MasterCode == null)
                {
                    try
                    {
                        ServerData.Entities.Question qtn = new ServerData.Entities.Question();
                        qtn.Complexity = Q.Complexity;
                        qtn.Explanation = Q.AnswerExplanation;
                        qtn.HeaderText = Q.Instruction;
                        qtn.QuestionText = Q.QuestionText;
                        qtn.TopicID = GetTopicID(ps.ExaminationID, Q.Subject, Q.Topic);
                        qtn.MasterCode = Guid.NewGuid().ToString();
                        qtn.Status = 1;
                        qtn.ModifiedDate = DateTime.Now;
                        qtn.PracticeTestID = ps.ID;

                        foreach (var x in Q.OptionTexts)
                        {
                            ServerData.Entities.Answer a = new ServerData.Entities.Answer();
                            a.OptionText = x.OptionText;
                            a.IsCorrect = x.IsCorrect;
                            a.MasterCode = Guid.NewGuid().ToString();
                            a.ModifiedDate = DateTime.Now;
                            a.Status = 1;
                            qtn.Answers.Add(a);
                        }

                        context.Questions.Add(qtn);
                        context.SaveChanges();

                        //Sync back
                        Q.MasterCode = qtn.MasterCode;
                        Q.Status = qtn.Status;
                        foreach (var x in Q.OptionTexts)
                        {
                            ServerData.Entities.Answer a = qtn.Answers.Where(y => y.OptionText == x.OptionText).FirstOrDefault();
                            x.MasterCode = a.MasterCode;
                            x.Status = a.Status;
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                                        validationError.PropertyName,
                                                        validationError.ErrorMessage);
                            }
                        }
                        Q.MasterCode = string.Empty;
                        Q.Status = 0;
                        foreach (var item in Q.OptionTexts)
                        {
                            item.MasterCode = string.Empty;
                            item.Status = 0;
                        }
                    }
                }
                else
                {
                    if (Q.IsDeleted)
                    {
                        Qrator.ServerData.Entities.Question qt = context.Questions.Where(x => x.MasterCode == Q.MasterCode && x.Status == 1).FirstOrDefault();
                        context.Questions.Remove(qt);
                        context.SaveChanges();
                    }
                    else
                    {
                        int i = 0;
                        Qrator.ServerData.Entities.Question qt = context.Questions.Where(x => x.MasterCode == Q.MasterCode && x.Status == 1).FirstOrDefault();
                        if (qt.Complexity != Q.Complexity)
                        {
                            qt.Complexity = Q.Complexity;
                            i++;
                        }
                        if (qt.Explanation != Q.AnswerExplanation)
                        {
                            qt.Explanation = Q.AnswerExplanation;
                            i++;
                        }
                        if (qt.HeaderText != Q.Instruction)
                        {
                            qt.HeaderText = Q.Instruction;
                            i++;
                        }
                        if (qt.QuestionText != Q.QuestionText)
                        {
                            qt.QuestionText = Q.QuestionText;
                            i++;
                        }

                        if (qt.Topic.Name != Q.Topic)
                        {
                            qt.TopicID = GetTopicID(ps.ExaminationID, Q.Subject, Q.Topic);
                            i++;
                        }

                        bool answerChanged = false; //Use this variable to update last modified date of each answer option
                        //Update Answers
                        foreach (var item in Q.OptionTexts)
                        {
                            Qrator.ServerData.Entities.Answer a = qt.Answers.Where(x => x.MasterCode == item.MasterCode && item.Status == 1).FirstOrDefault();
                            if (a.IsCorrect != item.IsCorrect)
                            {
                                a.IsCorrect = item.IsCorrect;
                                answerChanged = true;
                                i++;
                            }
                            if (a.OptionText != item.OptionText)
                            {
                                answerChanged = true;
                                a.OptionText = item.OptionText;
                                i++;
                            }
                            if (answerChanged)
                                a.ModifiedDate = DateTime.Now;

                            answerChanged = false;
                        }
                        qt.ModifiedDate = DateTime.Now;

                        if (i > 0)
                            context.SaveChanges();
                    }
                }
            }
        }
    }
}
