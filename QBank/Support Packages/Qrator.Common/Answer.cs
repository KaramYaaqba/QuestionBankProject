using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qrator.Common
{
    [Serializable()]
    public class Answer
    {
        public string ID { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }

        public string MasterCode { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public short Status { get; set; }
    }
}
