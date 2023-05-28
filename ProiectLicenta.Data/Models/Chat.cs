using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class Chat
    {
        public int ChatId { get; set; }

        public string Topic { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        public UserData Student { get; set; }

        [ForeignKey("HelpingStudent")]
        public int? HelpingStudentId { get; set; }

        public UserData HelpingStudent { get; set; }

        public int SubChapterId { get; set; }

        public SubChapter SubChapter { get; set; }

        public virtual ICollection<Message> ChatMessages { get; set; }
    }
}
