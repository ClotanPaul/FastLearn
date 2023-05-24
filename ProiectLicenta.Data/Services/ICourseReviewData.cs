using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface ICourseReviewData
    {

        List<CourseReview> getReviews(int courseId);

        void AddReview(CourseReview review);

        CourseReview GetReview(int courseReview);

        void UpdateReview(CourseReview review);

        void DeleteReview(int courseReviewId);

        void ActivateReview(int id);

        void DeactivateReview(int id, string deactivationReason);
    }
}
