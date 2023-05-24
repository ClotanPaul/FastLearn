using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProiectLicenta.Data.Models
{
    public class SubChapter
    {
        [Required]
        public int SubchapterId { get; set; }

        public int SubchapterNumber { get; set; }

        [Required]
        public string SubchapterTitle { get; set; }

        [Required]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string SubchapterDescription { get; set; }

        public int ChapterId { get; set; }

        public virtual Chapter Chapter { get; set; }

        public virtual Test Test { get; set; }

        public virtual ICollection<SubChapterFiles> SubchapterFiles { get; set; }
    }
}
