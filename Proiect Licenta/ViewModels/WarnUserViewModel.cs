using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.ViewModels
{
    public class WarnUserViewModel
    {

        [DisplayName("Reason")]
        [StringLength(45, MinimumLength = 5, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="String can't be empty.")]
        public string Reason { get; set; }

        public string UserName { get; set; }

        public int UserDataId { get; set; }
    }
}