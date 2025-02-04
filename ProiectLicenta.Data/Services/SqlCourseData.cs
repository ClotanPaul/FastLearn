﻿using Newtonsoft.Json;
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
        private readonly ISubChapterData subChapterDb;


        public SqlCourseData(ApplicationDataDbContext db, IUserData userDataDb, ISubChapterData subChapterDb)
        {
            this.db = db;
            this.userDataDb = userDataDb;
            this.subChapterDb= subChapterDb;
        }

        public Course GetCourse(int courseId)
        {
            return db.Courses
                .Include("Chapters.Subchapters")
                .FirstOrDefault(x=> x.CourseId == courseId);
        }

        public List<Course> GetAll()
        {
            return db.Courses.Where(c=>c.Active == true).ToList();
        }

        public void AddCourse(Course course)
        {
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
            var courses = db.Courses.Where(c=> c.OwnerId== ownerId && c.Active).ToList();
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

        public List<Course> GetFinishedCourses(int userDataId)
        {
            var user = userDataDb.getUserByUserDataId(userDataId);

            var enrolledStudentInCourses = db.EnrolledStudentsInCourses.Where(u => u.UserDataId == userDataId).ToList();

            var finishedCoursesId = new List<Course>();

            foreach (var enrollment in enrolledStudentInCourses) {

                var lastCompletedSubChapterId = enrollment.LastCompletedSubChapterId;

                var nextSubChapter = subChapterDb.GetNextSubChapter(lastCompletedSubChapterId);
                
                if(nextSubChapter == null)
                {
                    finishedCoursesId.Add(enrollment.Course);
                }
            }
            return finishedCoursesId;
        }

        public string GetFinishedCoursesSerialized(int userDataId)
        {
            var user = userDataDb.getUserByUserDataId(userDataId);

            var enrolledStudentInCourses = db.EnrolledStudentsInCourses.Where(u => u.UserDataId == userDataId).ToList();

            var finishedCoursesId = new List<int>();

            foreach (var enrollment in enrolledStudentInCourses)
            {

                if (enrollment.CompletedCourse)
                {
                    finishedCoursesId.Add(enrollment.CourseId);
                }
            }
            var finishedCourses = SerializeFinishedCourses(finishedCoursesId);
            return finishedCourses;
        }

        public List<int> FinishedCoursesDeserialization(string finishedCourses)
        {
            if (string.IsNullOrEmpty(finishedCourses))
            {
                var list = new List<int>();
                return null;
            }
            else
                return JsonConvert.DeserializeObject<List<int>>(finishedCourses);
        }

        public string SerializeFinishedCourses(List<int> finishedCourses)
        {
            return JsonConvert.SerializeObject(finishedCourses);
        }

        public List<Course> GetEnrolledCoursesByIds(List<int> ids)
        {
            List<Course> courses= new List<Course>();
            if (ids.Count > 0)
            {
                foreach(var id in ids)
                {
                    var course = GetCourse(id);
                    courses.Add(course);
                }
            }
            return courses;
        }

        public List<Course> GetUserInactiveCourses(string ownerId)
        {
            var courses = db.Courses.Where(c => c.OwnerId == ownerId && !c.Active).ToList();
            return courses;
        }

        public bool NameNotTaken(string courseName)
        {
            var course = db.Courses.FirstOrDefault(c=>c.CourseName== courseName);
            if (course != null)
            {
                return false;
            }
            return true;
        }
    }
}
