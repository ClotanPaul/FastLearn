using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class Warning
    {
        public int WarningId { get; set; }

        public string WarningReason { get; set; }

        public UserData User { get; set; }

        public int UserId { get; set; }
    }
}
