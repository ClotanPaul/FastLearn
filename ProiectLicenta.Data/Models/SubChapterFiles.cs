using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class SubChapterFiles
    {

        [Required]
        public int SubChapterFilesId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedDate { get; set; }

        public int SubChapterId { get; set; }

        public virtual SubChapter SubChapter { get; set; }
    }

}

