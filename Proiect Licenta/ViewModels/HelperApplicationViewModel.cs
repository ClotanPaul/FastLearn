using Proiect_Licenta.Models;
using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.ViewModels
{
    public class HelperApplicationViewModel
    {
        private string professorEmail;

        public List<Course> FinishedCourses { get; set; }

        [DefaultValue(null)]
        public string ProfessorEmail { get; set; }

        public string StudentEmail { get; set; }

        public int HelpingStudentApplicationId { get; set; }
    }
}