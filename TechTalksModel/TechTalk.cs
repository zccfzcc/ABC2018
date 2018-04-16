using System;

namespace TechTalksModel
{
    public class TechTalk
    {
        public int Id { get; set; }
        public string TechTalkName { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set;}
    }
    
}