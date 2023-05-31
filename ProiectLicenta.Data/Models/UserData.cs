using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.Models
{
    public class UserData
    {
        [Required]
        public int UserDataId { get; set; }

        [Required]
        public string UserId { get; set; } 

        public string UserName { get; set; } // special username

        public string Email { get; set; } // username from the other database

        public int Points { get; set; }

        public string UserRole { get; set; }

        // for now not used
        public int EnrolledCourseId { get; set; }

        public virtual ICollection<Warning> Warnings { get; set; }

        public virtual ICollection<EnrolledStudentInCourse> EnrolledStudentsInCourse { get; set; }

        public DateTime SuspendedUntil { get; set; }

        public bool IsSuspended { get; set; }

        public virtual ICollection<Chat> Chats { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public int? HelpingStudentApplicationId { get; set; }


    }
}