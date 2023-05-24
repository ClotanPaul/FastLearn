using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlCourseReviewData : ICourseReviewData
    {


        private ApplicationDataDbContext db;

        public SqlCourseReviewData(ApplicationDataDbContext db)
        {
            this.db = db;
        }

      
        public void AddReview(CourseReview review)
        {
            var course = db.Courses.FirstOrDefault(c => c.CourseId == review.CourseId);
            //course.CourseReviews.Append(review);
            review.IsActive = true;
            db.CourseReviews.Add(review);
            db.SaveChanges();
        }

        public void DeleteReview(int courseReviewId)
        {
            var model = db.CourseReviews.FirstOrDefault(cr => cr.CourseReviewId == courseReviewId);
            if (model != null)
            {
                db.CourseReviews.Remove(model);
                db.SaveChanges();
            }
        }

        public CourseReview GetReview(int courseReviewId)
        {
            var review = db.CourseReviews.FirstOrDefault(cr => cr.CourseReviewId == courseReviewId);
            return review;
        }

        public List<CourseReview> getReviews(int courseId)
        {
            var course = db.Courses.FirstOrDefault(c=>c.CourseId == courseId);
            if (course?.CourseReviews == null)
                return new List<CourseReview>();
            else
            {
                return course.CourseReviews.ToList();
            }
        }

        public void UpdateReview(CourseReview review)
        {
            db.CourseReviews.AddOrUpdate(review);
            db.SaveChanges();
        }


        public void ActivateReview(int id)
        {
            var review = GetReview(id);
            review.IsActive = true;
            review.DeactivationReason = "";
            db.SaveChanges();
        }


        public void DeactivateReview(int id, string deactivationReason)
        {
            var review = db.CourseReviews.FirstOrDefault(cr => cr.CourseReviewId == id);
            review.DeactivationReason= deactivationReason;
            review.IsActive = false;
            db.SaveChanges();
        }
    }
}
