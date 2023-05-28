using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public string MessageContent { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserData User { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime TimeStamp { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }
    }
}
