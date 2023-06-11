using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class Warning
    {
        public int WarningId { get; set; }

        [DisplayName("Reason")]
        [StringLength(45, MinimumLength = 5, ErrorMessage = "The {0} must be between {1} and {2} characters long.")]
        public string WarningReason { get; set; }

        public UserData User { get; set; }

        public int UserId { get; set; }
    }
}
