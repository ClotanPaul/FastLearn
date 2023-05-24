using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class Test
    {
        [ForeignKey("SubChapter")]
        public int TestId { get; set; }

        [Required]
        public int MinimumScore { get; set; }

        public string TestDescription { get; set; }

        public virtual SubChapter SubChapter { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<UserAnswer> TakenTests { get; set; }



    }
}
