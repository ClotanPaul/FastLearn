using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface IEnrolledSudentInCourse
    {
        void EnrollInCourse(int courseId, int UserDataId);

        void UpdateEnrollment(EnrolledStudentInCourse enrollment);

        void CancelEnrollment(int courseId, int UserDataId);

        EnrolledStudentInCourse getEnrolledStudentInfo(int courseId, string userId);

        bool IsEnrolledInCourse(string userId, int courseId);
    }
}
