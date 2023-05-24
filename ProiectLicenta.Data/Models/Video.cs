using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProiectLicenta.Data.Models
{
    public class Video
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}
