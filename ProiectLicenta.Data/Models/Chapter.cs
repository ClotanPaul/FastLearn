using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class Chapter
    {
        [Required]
        public int ChapterId { get; set; }

        public int ChapterNumber { get; set; }

        [Required]
        public string ChapterTitle { get; set; }

        [Required]
        public string ChapterDescription { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<SubChapter> Subchapters { get; set; }
    }
}
