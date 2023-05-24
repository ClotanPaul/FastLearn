using Microsoft.Ajax.Utilities;
using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.ViewModels
{
    public class ChangeRoleViewModel
    {

        public string UserId { get; set; }

        public string Email { get; set; }

        public string currentRole { get; set; }

        public List<string> PossibleRoles { get; set; }

        public string NewRole { get; set; }

    }
}