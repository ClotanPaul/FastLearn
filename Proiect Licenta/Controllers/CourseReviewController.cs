using Microsoft.AspNet.Identity;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Licenta.Controllers
{
    public class CourseReviewController : Controller
    {

        private readonly ICourseReviewData courseReviewDb;
        private readonly ICourseData courseDb;

        public CourseReviewController(ICourseReviewData courseReviewDb, ICourseData courseDb)
        {
            this.courseReviewDb = courseReviewDb;
            this.courseDb= courseDb;
        }


        // GET: CourseReview
        public ActionResult Index(int courseId)
        {
            var reviews = courseReviewDb.getReviews(courseId);
            ViewData["courseId"] = courseId;
            ViewData["UserId"] = User.Identity.GetUserId();
            var course = courseDb.GetCourse(courseId);

            // determine if the user can add a new review or not
            if (!User.IsInRole("admin") && !User.IsInRole("professor"))
            {
                ViewData["CanAdd"] = "true";
            }
            else
                ViewData["CanAdd"] = "false";


            return View(reviews);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CourseReview newReview, int courseId)
        {
            newReview.UserId = User.Identity.GetUserId();
            newReview.CourseId = courseId;

            if (ModelState.IsValid)
            {
                courseReviewDb.AddReview(newReview);
            }

            return RedirectToAction("Index", new { courseId = newReview.CourseId});
        }


        public ActionResult Edit(int id)
        {
            var model = courseReviewDb.GetReview(id);

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseReview courseReview)
        {
            if (ModelState.IsValid)
            {
                courseReviewDb.UpdateReview(courseReview);

            }
            else
                return View("ModelStateNotValid");
            return RedirectToAction("Details", new { id = courseReview.CourseReviewId });
        }


        [HttpGet]
        public ActionResult Delete(int courseReviewId)
        {

            var courseReview = courseReviewDb.GetReview(courseReviewId);

            if (courseReview == null)
                return View("NotFound");
            return View(courseReview);
        }

        [HttpPost]
        public ActionResult Delete(int courseReviewId, FormCollection form)
        {
            var courseId = courseReviewDb.GetReview(courseReviewId).CourseId;
            if (!ModelState.IsValid)
            {
                return View("ModelStateNotValid");
            }

            courseReviewDb.DeleteReview(courseReviewId);
            return RedirectToAction("Index", new { courseId = courseId });

        }

        public ActionResult Details(int id)
        {

            var model = courseReviewDb.GetReview(id);

            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }
        
        [HttpGet]
        public ActionResult ActivateReview(int id)
        {
            var model = courseReviewDb.GetReview(id);

            if (model == null)
                return View("NotFound");

            if (model.IsActive)
            {
                return View("CourseIsAlreadyActive");
            }

            return View(model);

        }


        [HttpPost]
        public ActionResult ActivateReview(int id, FormCollection form)
        {
            var courseId = courseReviewDb.GetReview(id).CourseId;

            if (ModelState.IsValid)
            {
                courseReviewDb.ActivateReview(id);
            }
            else
            {
                return View("NotFound");
            }
            return RedirectToAction("Index", new { courseId = courseId});

        }
        
        

        [HttpGet]
        public ActionResult DeactivateReview(int id)
        {
            var model = courseReviewDb.GetReview(id);

            if (model == null)
                return View("NotFound");

            if (!model.IsActive)
            {
                return View("ReviewIsAlreadyDeactivated");
            }

            return View(model);

        }


        [HttpPost]
        public ActionResult DeactivateReview(CourseReview review)
        {
            var deactivationReason = review.DeactivationReason;
            var courseReviewId = review.CourseReviewId;
            courseReviewDb.DeactivateReview(courseReviewId, deactivationReason);

            return RedirectToAction("Details", new { id = review.CourseReviewId});

        }
        


    }
}