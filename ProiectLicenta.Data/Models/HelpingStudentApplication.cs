using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class HelpingStudentApplication
    {
        public int HelpingStudentApplicationId { get; set; }

        public string FinishedCoursesIds { get; set; }

        public int StudentId { get; set; }


        public int? ProfessorId { get; set; }


        public bool IsApproved { get; set; }
    }
}
