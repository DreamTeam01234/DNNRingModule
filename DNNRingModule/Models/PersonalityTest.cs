using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Ring.Module.DNNRingModule.Models
{
    [TableName("PersonalityTest")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class PersonalityTestAnswer
    {
        public int Id { get; set; }
        public int FilledOutBy { get; set; }
        public DateTime FilledOutAt { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string Answer5 { get; set; }
        public string Answer6 { get; set; }
        public string Answer7 { get; set; }
        public string Answer8 { get; set; }
        public string Answer9 { get; set; }
        public string Answer10 { get; set; }
    }
}
