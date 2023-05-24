using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface ICourseData
    {
        List<Course> GetAll();

        List<Course> GetUserCourses(string ownerId);

        Course GetCourse(int id);

        void AddCourse(Course course);

        void UpdateCourse(Course course);

        void DeleteCourse(int id);

        void ActivateCourse(int id);

        void DeactivateCourse(int id, string deactivationReason);

        List<Course> GetInactiveCourses();

        void RequestActivation(int id);

        void RejectActivation(int id);
    }
}
