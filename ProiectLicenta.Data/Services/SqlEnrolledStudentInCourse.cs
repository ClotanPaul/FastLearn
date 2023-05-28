using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProiectLicenta.Data.Services
{
    public class SqlEnrolledStudentInCourse : IEnrolledSudentInCourse
    {

        private readonly ApplicationDataDbContext db;
        private readonly ICourseData courseDb;
        private readonly IUserData userDataDb;

        public SqlEnrolledStudentInCourse(ApplicationDataDbContext db, ICourseData courseDb, IUserData userDataDb)
        {
            this.db = db;
            this.courseDb = courseDb;
            this.userDataDb = userDataDb;
        }

        public void EnrollInCourse(int courseId, int UserDataId)
        {
            var course = courseDb.GetCourse(courseId);
            var userData = userDataDb.getUserByUserDataId(UserDataId);

            var StudentEnroll = new EnrolledStudentInCourse();

            StudentEnroll.Course = course;
            StudentEnroll.UserData = userData;
            var firstCourseChapter = course.Chapters.OrderBy(ch => ch.ChapterNumber).FirstOrDefault();
            if(firstCourseChapter!= null)
            {
                StudentEnroll.LastCompletedChapterId= firstCourseChapter.ChapterId;
            }

            var firstSubchapter = firstCourseChapter.Subchapters.OrderBy(ch => ch.SubchapterNumber).FirstOrDefault();

            if (firstSubchapter != null)
            {
                StudentEnroll.LastCompletedSubChapterId = firstSubchapter.SubchapterId;
            }

            db.EnrolledStudentsInCourses.Add(StudentEnroll);
            db.SaveChanges();
        }

        public void CancelEnrollment(int courseId, int UserDataId)
        {
            var userData = userDataDb.getUserByUserDataId(UserDataId);

            var studentEnroll = userData.EnrolledStudentsInCourse.FirstOrDefault(esc=>esc.UserDataId== UserDataId);

            db.EnrolledStudentsInCourses.Remove(studentEnroll);
            db.SaveChanges();
        }

        public EnrolledStudentInCourse getEnrolledStudentInfo(int courseId, string userId)
        {
            var course = courseDb.GetCourse(courseId);

            var userData = userDataDb.getUserData(userId);
            if(userData == null)
            {
                return null;
            }

            var enrollmentInfo = course.EnrolledStudentsInCourses.FirstOrDefault(esc => esc.UserDataId == userData.UserDataId);

            if (enrollmentInfo != null)
            {
                return enrollmentInfo;
            }
            else
                return null;
        }

        public void UpdateEnrollment(EnrolledStudentInCourse enrollment)
        {
            db.EnrolledStudentsInCourses.AddOrUpdate(enrollment);
            db.SaveChanges();
        }

        public bool IsEnrolledInCourse(string userId, int courseId)
        {
            var enrollment = getEnrolledStudentInfo(courseId, userId);

            if (enrollment == null)
            {
                return false;
            }
            return true;
        }
    }
}
