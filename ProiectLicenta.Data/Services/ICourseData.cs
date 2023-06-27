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

        List<Course> GetUserInactiveCourses(string ownerId);


        Course GetCourse(int id);

        void AddCourse(Course course);

        void UpdateCourse(Course course);

        void DeleteCourse(int id);

        void ActivateCourse(int id);
        List<int> FinishedCoursesDeserialization(string finishedCourses);

        void DeactivateCourse(int id, string deactivationReason);

        List<Course> GetInactiveCourses();

        void RequestActivation(int id);

        void RejectActivation(int id);

        List<int> GetEnrolledCoursesIds(string studentId);

        List<Course> GetEnrolledCoursesByIds(List<int> ids);

        List<Course> GetFinishedCourses(int userDataId);

        string GetFinishedCoursesSerialized(int userDataId);

        bool NameNotTaken(string courseName);


    }
}
