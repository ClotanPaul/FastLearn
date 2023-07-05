using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class Question
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string QuestionString { get; set; }

        public int questionPointPercentage { get; set; }

        public int TestId { get; set; }

        public virtual Test test { get; set; }


        public virtual ICollection<Answer> Answers { get; set; }





    }
}
