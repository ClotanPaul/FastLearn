using Microsoft.AspNet.Identity;
using Org.BouncyCastle.Utilities.Collections;
using Proiect_Licenta.Models;
using ProiectLicenta.Data.Migrations;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Proiect_Licenta.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseData courseDb;
        private readonly IUserData userDb;
        private readonly IVideoData imageDb;
        public CourseController(ICourseData db, IUserData userDb, IVideoData imageDb)
        {
            this.courseDb = db;
            this.userDb = userDb;
            this.imageDb = imageDb;
        }
        // GET: Course
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Course> courses = courseDb.GetAll();
            ViewData["userId"] = User.Identity.GetUserId();
            return View(courses);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Course newCourse)
        {
            newCourse.OwnerId = User.Identity.GetUserId();
            string fileName = "proiectLicentaPhoto";
            string urlPath = Url.Content(Path.Combine("~/Content/Images/Courses", fileName) + ".jpg");
            newCourse.ImagePath = urlPath;
            if (ModelState.IsValid)
            {
                courseDb.AddCourse(newCourse);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = courseDb.GetCourse(id);
            var userId = User.Identity.GetUserId();

            if (userId != model.OwnerId)
                return View("NoAcces");


            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                courseDb.UpdateCourse(course);
                return RedirectToAction("Details",new {id = course.CourseId });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = courseDb.GetCourse(id);
            var userId = User.Identity.GetUserId();

            if (userId != model.OwnerId)
                return View("NoAcces");

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            var course = courseDb.GetCourse(id);
            courseDb.DeleteCourse(id);
            return RedirectToAction("Index");

        }
        public ActionResult Details(int id)
        {

            var model = courseDb.GetCourse(id);
            var userId = User.Identity.GetUserId();

            if (userId != model.OwnerId)
                return View("NoAcces");


            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ActivateCourse(int id)
        {
            var model = courseDb.GetCourse(id);

            if (model == null)
                return View("NotFound");

            if (model.Active)
            {
                return View("CourseIsAlreadyActive");
            }

            return View(model);

        }

        
        [HttpPost]
        public ActionResult ActivateCourse(int id, FormCollection form)
        {
            if(ModelState.IsValid)
            {
                courseDb.ActivateCourse(id);
            }
            else
            {
                return View("NotFound");
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult DeactivateCourse(int id)
        {
            var model = courseDb.GetCourse(id);

            if (model == null)
                return View("NotFound");

            if (!model.Active)
            {
                return View("CourseIsAlreadyDeactivated");
            }

            return View(model);

        }


        [HttpPost]
        public ActionResult DeactivateCourse(Course course)
        {

            if(course.DeactivationReason == null)// or courseid-int is null
            {
                return View("NotFound");
            }

            var courseId = course.CourseId;
            var deactivationReason = course.DeactivationReason;
            courseDb.DeactivateCourse(courseId, deactivationReason);

            return RedirectToAction("Index");

        }

        public ActionResult GetUserCourses()
        {
            var userId = User.Identity.GetUserId();
            List<Course> courses = courseDb.GetUserCourses(userId);
            ViewData["userId"] = User.Identity.GetUserId();
            return View(courses);
        }

        public ActionResult GetInactiveCourses()
        {
            List<Course> courses = courseDb.GetInactiveCourses();
            ViewData["userId"] = User.Identity.GetUserId();
            return View(courses);
        }

        public ActionResult RequestActivation(int courseId)
        {
            ViewData["userId"] = User.Identity.GetUserId();
            courseDb.RequestActivation(courseId);
            return RedirectToAction("GetUserCourses");
        }

        public ActionResult RejectActivation(int courseId)
        {
            ViewData["userId"] = User.Identity.GetUserId();
            courseDb.RejectActivation(courseId);
            return RedirectToAction("GetInactiveCourses");
        }

        [HttpGet]
        public ActionResult UpdateCoursePhoto(int courseId)
        {
            var course = courseDb.GetCourse(courseId);
            return View(course);
        }

        [HttpPost]
        public ActionResult UpdateCoursePhoto(HttpPostedFileBase file, int courseId)
        {
            var course = courseDb.GetCourse(courseId);
            if (file != null && file.ContentLength > 0)
            {
                string folderName = course.CourseName;
                string extension = Path.GetExtension(file.FileName);
                string fileName = "CourseImage" + extension;
                string dirPath = Path.Combine(Server.MapPath("~/Content/Images/Courses"), folderName);

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string path = Path.Combine(dirPath, fileName);

                string urlPath = VirtualPathUtility.ToAbsolute(Path.Combine("~/Content/Images/Courses", folderName, fileName));
                course.ImagePath = urlPath;
                file.SaveAs(path);
                courseDb.UpdateCourse(course);
            }
            
            return RedirectToAction("GetUserCourses");
        }




    }

}