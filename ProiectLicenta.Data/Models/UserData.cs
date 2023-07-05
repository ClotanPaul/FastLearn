using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Username")]
        public string UserName { get; set; }

        public string Email { get; set; } 

        public int Points { get; set; }

        [DisplayName("Role")]
        public string UserRole { get; set; }

       
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