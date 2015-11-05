using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qrator.Common
{
    [Serializable()]
    public class Question
    {
        public string ID { get; set; }
        public string Instruction { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public string QuestionText { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public short Status { get; set; }

        public List<Answer> OptionTexts { get; set; }

        public short Complexity { get; set; }

        public string AnswerExplanation { get; set; }

        public double TotalTimeSpent { get; set; }

        public int PositiveMarks { get; set; }
        public int NegativeMarks { get; set; }

        public string MasterCode { get; set; }
        public Question()
        {
            OptionTexts = new List<Answer>();
            this.PositiveMarks = 1;
            this.NegativeMarks = 0;
        }

        public bool IsDeleted { get; set; }
    }
}