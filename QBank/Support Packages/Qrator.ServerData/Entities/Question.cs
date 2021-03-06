//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Qrator.ServerData.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }
    
        public int ID { get; set; }
        public int TopicID { get; set; }
        public string HeaderText { get; set; }
        public string QuestionText { get; set; }
        public string Explanation { get; set; }
        public short Complexity { get; set; }
        public int PracticeTestID { get; set; }
        public string MasterCode { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public short Status { get; set; }
    
        public virtual PracticeTest PracticeTest { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
