using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class UserAnswer
    {
        public int UserAnswerId { get; set; }

        public bool Passed { get; set; }

        public int Score { get; set; }


        // relationship with user
        [Required]
        public string UserId { get; set; }


        // relationship with test - one test will have many testAnswers

        public int TestId { get; set; }

        public Test Test { get; set; }

        public string ChosenAnswersIdListSerialized { get; set; }

        // Record of chosen answers.
        [NotMapped]
        public List<int> chosenAnswersIdList { get; set; }

        // I can access the answers and questions through the test instance.
    }
}
