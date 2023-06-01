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
    public class Course
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        [DisplayName("Name")]
        public string CourseName { get; set; }

        [Required]
        [DisplayName("Description")]
        public string CourseDescription { get; set; }

        [DefaultValue(false)]
        public bool Active { get; set; }

        [DisplayName("Reason")]
        public string DeactivationReason { get; set; }

        public string OwnerId { get; set; }

        public virtual UserData Owner { get; set; }


        public virtual ICollection<Chapter> Chapters { get;set; }

        public virtual ICollection<CourseReview> CourseReviews { get; set; }

        [DefaultValue(true)]
        public bool IssueResolved { get; set; }

        public string ImagePath { get; set; }

        public virtual ICollection<EnrolledStudentInCourse> EnrolledStudentsInCourses { get; set; }



        //public int CommentId { get; set; }
    }
}
