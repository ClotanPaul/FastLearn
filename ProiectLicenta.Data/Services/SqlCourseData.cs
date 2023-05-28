using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlCourseData : ICourseData
    {

        private ApplicationDataDbContext db;
        private readonly IUserData userDataDb;


        public SqlCourseData(ApplicationDataDbContext db, IUserData userDataDb)
        {
            this.db = db;
            this.userDataDb = userDataDb;
        }

        public Course GetCourse(int courseId)
        {
            return db.Courses.
                FirstOrDefault(x=> x.CourseId == courseId);
            // to add checks for the user owner.
        }

        public List<Course> GetAll()
        {
            return db.Courses.ToList();
        }

        public void AddCourse(Course course)
        {
            //default image
            db.Courses.Add(course);
            db.SaveChanges();
        }

       public void UpdateCourse(Course course)
        {
            if (course != null)
            {
                db.Courses.AddOrUpdate(course);
                db.SaveChanges();
            }
        }

        public void DeleteCourse(int courseId)
        {
            var currentCourse = db.Courses.FirstOrDefault(x=> x.CourseId == courseId);
            // daca are test creat nu poate fi sters cursul??
            db.Courses.Remove(currentCourse);
            db.SaveChanges();
        }

        public void ActivateCourse(int id)
        {
            var course = db.Courses.FirstOrDefault(c=> c.CourseId == id);
            course.Active = true;
            course.DeactivationReason = "";
            course.IssueResolved = true;
            db.Courses.AddOrUpdate(course);
            db.SaveChanges();
        }

        public void DeactivateCourse(int id, string deactivationReason)
        {

            var course = db.Courses.FirstOrDefault(c => c.CourseId == id);
            course.Active = false;
            course.DeactivationReason = deactivationReason;
            course.IssueResolved = false;
            db.Courses.AddOrUpdate(course);
            db.SaveChanges();

        }

        public List<Course> GetUserCourses(string ownerId)
        {
            var courses = db.Courses.Where(c=> c.OwnerId== ownerId).ToList();
            return courses;
        }

        public List<Course> GetInactiveCourses()
        {
            var courses = db.Courses.Where(c => c.Active == false).ToList();
            return courses;
        }

        public void RequestActivation(int id)
        {
            var course = db.Courses.FirstOrDefault(c=>c.CourseId== id);
            course.IssueResolved = true;
            db.SaveChanges();
        }

        public void RejectActivation(int id)
        {
            var course = db.Courses.FirstOrDefault(c => c.CourseId == id);
            course.IssueResolved = false;
            db.SaveChanges();
        }

        public List<int> GetEnrolledCoursesIds(string studentId)
        {
            var studentData = userDataDb.getUserData(studentId);
            var enrollmentData = studentData.EnrolledStudentsInCourse.ToList();
            var courseIds = new List<int>();
            foreach(var enrollment in enrollmentData)
            {
                courseIds.Add(enrollment.CourseId);
            }
            return courseIds;

        }

        
    }
}
