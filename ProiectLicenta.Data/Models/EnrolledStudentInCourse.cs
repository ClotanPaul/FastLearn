using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class EnrolledStudentInCourse
    {

        public int EnrolledStudentInCourseId { get; set; }

        //relatie cu curs - one to many

        //last chapter completedid
        //last subchapter completedId

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public int LastCompletedChapterId { get; set; }

        public int LastCompletedSubChapterId { get; set; }

        public int UserDataId { get; set; }

        public virtual UserData UserData { get; set; }


    }
}
