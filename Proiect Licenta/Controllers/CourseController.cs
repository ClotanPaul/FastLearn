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
using System.Web.WebPages;

namespace Proiect_Licenta.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseData courseDb;
        private readonly IUserData userDb;
        private readonly IVideoData imageDb;
        private readonly IEnrolledSudentInCourse enrollStudentInCourseDb;
        private readonly ISubChapterData subchapterDb;
        public CourseController(ICourseData db, IUserData userDb, IVideoData imageDb, IEnrolledSudentInCourse enrollStudentInCourseDb, ISubChapterData subchapterDb)
        {
            this.courseDb = db;
            this.userDb = userDb;
            this.imageDb = imageDb;
            this.enrollStudentInCourseDb = enrollStudentInCourseDb;
            this.subchapterDb = subchapterDb;
        }
        // GET: Course
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Course> courses = courseDb.GetAll();
            ViewData["userId"] = User.Identity.GetUserId();
            return View(courses);
        }

        public ActionResult StudentIndex()
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


            if (!courseDb.NameNotTaken(newCourse.CourseName))
            {
                ModelState.AddModelError("CourseName", "Name already used.");
            }

            if (newCourse.CourseDescription.IsEmpty())
            {
                ModelState.AddModelError("CourseDescription", "Can't be empty");
            }
            else
            {
                if (newCourse.CourseDescription.Length < 120 || newCourse.CourseDescription.Length > 150)
                {
                    ModelState.AddModelError("CourseDescription", "The description of the course must be between 120 and 150 characters long.");
                }
            }


            if (!ModelState.IsValid)
            {
                return View(newCourse);
            }

            newCourse.OwnerId = User.Identity.GetUserId();
            string fileName = "proiectLicentaPhoto";
            newCourse.DeactivationReason = "Newly created course.";
            string urlPath = Url.Content(Path.Combine("~/Content/Images/Courses", fileName) + ".jpg");
            newCourse.ImagePath = urlPath;
            if (ModelState.IsValid)
            {
                courseDb.AddCourse(newCourse);
            }

            return RedirectToAction("GetUserCourses");
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

            if (!courseDb.NameNotTaken(course.CourseName))
            {
                ModelState.AddModelError("CourseName", "Name already used.");
            }

            if (course.CourseDescription.IsEmpty())
            {
                ModelState.AddModelError("CourseDescription", "Can't be empty");
            }
            else
            {
                if(course.CourseDescription.Length < 120 || course.CourseDescription.Length > 150)
                {
                    ModelState.AddModelError("CourseDescription", "The description of the course must be between 120 and 150 characters long.");
                }
            }
            if (ModelState.IsValid)
            {
                courseDb.UpdateCourse(course);
                return RedirectToAction("GetUserCourses", new { id = course.CourseId });
            }
            return View(course);
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
            var userdata = userDb.getUserData(model.OwnerId);
            ViewData["ownerEmail"] = userdata.Email;
            model.Owner = userdata;

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
            if (ModelState.IsValid)
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
            var userdata = userDb.getUserData(model.OwnerId);

            model.Owner = userdata;

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
            if (course.DeactivationReason.IsEmpty())
            {
                ModelState.AddModelError("DeactivationReason", "Can't be empty.");
            }
            if (!course.DeactivationReason.IsEmpty() && course.DeactivationReason.Length > 35)
            {
                ModelState.AddModelError("DeactivationReason", "No more than 35 characters allowed.");
            }
            if (!ModelState.IsValid)// or courseid-int is null
            {
                var userdata = userDb.getUserData(course.OwnerId);
                course.Owner = userdata;
                ModelState.AddModelError("DeactivationReason", "Can't be empty.");
                return View(course);
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

        public ActionResult GetUserInactiveCourses()
        {
            var userId = User.Identity.GetUserId();
            List<Course> courses = courseDb.GetUserInactiveCourses(userId);
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


        public ActionResult EnrollInCourse(int courseId)
        {
            var model = courseDb.GetCourse(courseId);
            var userId = User.Identity.GetUserId();
            var userdata = userDb.getUserData(model.OwnerId);
            ViewData["ownerEmail"] = userdata.Email;
            if (enrollStudentInCourseDb.IsEnrolledInCourse(userId, courseId))
            {
                return View("AlreadyEnrolledInCourse");
            }

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        public ActionResult EnrollInCourse(int courseId, FormCollection form)
        {
            var userId = User.Identity.GetUserId();
            var userDataId = userDb.getUserId(userId);
            enrollStudentInCourseDb.EnrollInCourse(courseId, userDataId);
            return RedirectToAction("GetStudentEnrolledCourses");

        }

        public ActionResult CancelEnrollment(int courseId)
        {
            var model = courseDb.GetCourse(courseId);
            var userId = User.Identity.GetUserId();
            var userdata = userDb.getUserData(model.OwnerId);
            ViewData["ownerEmail"] = userdata.Email;

            if (!enrollStudentInCourseDb.IsEnrolledInCourse(userId, courseId))
            {
                return View("NotEnrolled");
            }

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        public ActionResult CancelEnrollment(int courseId, FormCollection form)
        {
            var userId = User.Identity.GetUserId();
            var userDataId = userDb.getUserId(userId);

            enrollStudentInCourseDb.CancelEnrollment(courseId, userDataId);
            return RedirectToAction("GetStudentUnEnrolledCourses");

        }

        public ActionResult ExploreCourse(int courseId)
        {
            var userId = User.Identity.GetUserId();

            var courseStudentData = enrollStudentInCourseDb.getEnrolledStudentInfo(courseId, userId);

            if (courseStudentData == null)// student not enrolled/ no data of student
            {
                var course = courseDb.GetCourse(courseId);

                var firstChapter = course.Chapters.OrderBy(ch => ch.ChapterNumber).FirstOrDefault();
                if (firstChapter != null)
                {
                    var firstSubChapter = firstChapter.Subchapters.OrderBy(sc => sc.SubchapterNumber).FirstOrDefault();
                    if (firstSubChapter != null)
                    {
                        return RedirectToAction("ViewSubchapter", "SubChapter", new { subchapterId = firstSubChapter.SubchapterId });
                    }
                }

                // If there's no chapter or subchapter, show an error or some kind of placeholder
                return View("NoContent");
            }

            var lastCompletedSubChapterId = courseStudentData.LastCompletedSubChapterId;

            return RedirectToAction("ViewSubchapter", "SubChapter", new { subchapterId = lastCompletedSubChapterId });

        }

        public ActionResult GetStudentEnrolledCourses()
        {
            var userId = User.Identity.GetUserId();
            var coursesIds = courseDb.GetEnrolledCoursesIds(userId);

            var courses = new List<Course>();
            foreach (var id in coursesIds)
            {
                var course = courseDb.GetCourse(id);
                var enrollment = enrollStudentInCourseDb.getEnrolledStudentInfo(id, userId);
                if (course.Active && !enrollment.CompletedCourse)
                {
                    courses.Add(course);
                }
            }

            ViewData["userId"] = User.Identity.GetUserId();
            return View(courses);
        }

        public ActionResult GetStudentFinishedCourses()
        {
            var userId = User.Identity.GetUserId();
            var coursesIds = courseDb.GetEnrolledCoursesIds(userId);

            var courses = new List<Course>();
            foreach (var id in coursesIds)
            {
                var course = courseDb.GetCourse(id);
                var enrollment = enrollStudentInCourseDb.getEnrolledStudentInfo(id, userId);
                if (course.Active && enrollment.CompletedCourse)
                {
                    courses.Add(course);
                }
            }

            ViewData["userId"] = User.Identity.GetUserId();
            return View(courses);
        }

        public ActionResult GetStudentUnEnrolledCourses()
        {
            var userId = User.Identity.GetUserId();
            var enrolledCoursesId = courseDb.GetEnrolledCoursesIds(userId);
            var courses = courseDb.GetAll();

            var toReturn = new List<Course>();

            foreach (var course in courses)
            {
                if (!enrolledCoursesId.Contains(course.CourseId) && course.Active)
                    toReturn.Add(course);
            }


            ViewData["userId"] = User.Identity.GetUserId();
            return View(toReturn);
        }
        [AllowAnonymous]
        public ActionResult CourseSyllabus(int courseId)
        {
            var course = courseDb.GetCourse(courseId);

            var userId = User.Identity.GetUserId();
            var enrollment = enrollStudentInCourseDb.getEnrolledStudentInfo(courseId, userId);
            if(enrollment == null)
            {
                ViewData["isEnrolled"] = "false";
            }
            else
                ViewData["isEnrolled"] = "true";

            if (course == null)
                return HttpNotFound();

            return View(course);
        }




    }

}