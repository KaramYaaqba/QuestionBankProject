using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qrator.Common
{
    [Serializable()]
    public class QuestionBank
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TimeAllocated { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ExamGrade { get; set; }

        public int PositiveMarks { get; set; }
        public int NegativeMarks { get; set; }
        public string MasterCode { get; set; }
        public List<Question> Questions { get; set; }
        public DateTime LastSyncDate { get; set; }
        public short Status { get; set; }
        public string Mode { get; set; }

        public QuestionBank()
        {
            this.PositiveMarks = 1;
            this.NegativeMarks = 0;
        }

        public Question GetPreviousQuestion(string Id)
        {
            int currentInd = this.Questions.IndexOf(this.Questions.Where(x => x.ID == Id).FirstOrDefault());
            if (currentInd > 0)
                return this.Questions.ElementAt(currentInd - 1);
            else
                return null;
        }

        public Question GetNextQuestion(string Id)
        {
            int currentInd = this.Questions.IndexOf(this.Questions.Where(x => x.ID == Id).FirstOrDefault());
            if (currentInd < this.Questions.Count - 1)
                return this.Questions.ElementAt(currentInd + 1);
            else
                return null;
        }

        public int GetQuestionIndex(string Id)
        {
            return this.Questions.IndexOf(this.Questions.Where(x => x.ID == Id).FirstOrDefault());
        }

        public Question GetQuestion(string Id)
        {
            return this.Questions.Where(x => x.ID == Id).FirstOrDefault();
        }
    }
}
