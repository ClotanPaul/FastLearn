using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public string AnswerText { get; set; }

        [Required]
        public bool IsCorrect { get; set; }


        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
