using Ganss.Xss;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth.Messages;
using Proiect_Licenta.ViewModels;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using static System.Net.WebRequestMethods;

namespace Proiect_Licenta.Controllers
{
    public class SubChapterController : Controller
    {
        private readonly ICourseData courseDb;
        private readonly IUserData userDb;
        private readonly IChapterData chapterDb;
        private readonly ISubChapterData subChapterDb;
        private readonly ISubChapterFileData subchapterFileDb;
        private readonly IEnrolledSudentInCourse enrollmentDb;
        public SubChapterController(ICourseData db, IUserData userDb, IChapterData chapterDb, ISubChapterData subChapterDb, ISubChapterFileData courseFileDb, IEnrolledSudentInCourse enrollmentDb)
        {
            this.courseDb = db;
            this.userDb = userDb;
            this.chapterDb = chapterDb;
            this.subChapterDb = subChapterDb;
            this.subchapterFileDb = courseFileDb;
            this.enrollmentDb = enrollmentDb;
        }

        // GET: Chapter
        public ActionResult Index(int chapterId)
        {
            var model = subChapterDb.GetSubChapters(chapterId);
            var chapter = chapterDb.GetChapter(chapterId);
            ViewData["chapterName"] = chapter.ChapterTitle;
            ViewData["chapterId"] = chapterId;
            ViewData["userId"] = User.Identity.GetUserId();
            ViewData["ownerId"] = chapter.Course.OwnerId;
            ViewData["courseId"] = chapter.CourseId;

            if (model == null)
            {
                // to implement
            }

            return View(model);
        }

        public ActionResult UserIndex(int chapterId)
        {
            var model = subChapterDb.GetSubChapters(chapterId);
            var chapter = chapterDb.GetChapter(chapterId);
            ViewData["chapterName"] = chapter.ChapterTitle;
            ViewData["chapterId"] = chapterId;
            ViewData["userId"] = User.Identity.GetUserId();
 

            if (model == null)
            {
                // to implement
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int chapterId)
        {
            ViewData["chapterId"] = chapterId;
            return View();
        }
        [HttpPost]
        public ActionResult Create(SubChapter newSubChapter, int chapterId)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            newSubChapter.ChapterId = chapterId;

            newSubChapter.SubchapterDescription = sanitizer.Sanitize(newSubChapter.SubchapterDescription);
            if (ModelState.IsValid)
            {
                subChapterDb.AddSubChapter(newSubChapter);

            }
            return RedirectToAction("Index", new { chapterId = newSubChapter.ChapterId });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = subChapterDb.GetSubChapter(id);
            ViewData["chapterId"] = model.ChapterId;

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubChapter subChapter)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            subChapter.SubchapterDescription = sanitizer.Sanitize(subChapter.SubchapterDescription);
            if (ModelState.IsValid)
            {
                subChapterDb.UpdateSubChapter(subChapter);

            }
            return RedirectToAction("Details", new { id = subChapter.SubchapterId });
        }

        public ActionResult Details(int id)
        {

            var model = subChapterDb.GetSubChapter(id);


            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = subChapterDb.GetSubChapter(id);
            var userId = User.Identity.GetUserId();

            if (userId != model.Chapter.Course.OwnerId)
                return View("NoAcces");

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            var subChapter = subChapterDb.GetSubChapter(id);
            var chapterId = subChapter.ChapterId;
            subChapterDb.DeleteSubChapter(id);
            return RedirectToAction("Index", new { chapterId = chapterId });

        }

        [HttpGet]
        public ActionResult UploadFile(int subchapterid)
        {
            var subChapter = subChapterDb.GetSubChapter(subchapterid);
            return View(subChapter);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, int subChapterId)
        {
            var subChapter = subChapterDb.GetSubChapter(subChapterId);

            if (file != null && file.ContentLength > 0)
            {

                string folderName = subChapter.Chapter.Course.CourseName;
                //string extension = Path.GetExtension(file.FileName);
                string fileName = file.FileName;
                string dirPath = Path.Combine(Server.MapPath("~/Content/Images/Courses"), folderName);

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                SubChapterFiles subChapterFile= new SubChapterFiles();
                subChapterFile.Title = file.FileName;

                string path = Path.Combine(dirPath, fileName);
                subChapterFile.UploadedDate = DateTime.Now;
                string urlPath = VirtualPathUtility.ToAbsolute(Path.Combine("~/Content/Images/Courses", folderName, fileName));
                subChapterFile.FilePath = urlPath;
                subChapterFile.SubChapter = subChapter;

                // Save the video data to the database
                file.SaveAs(path);
                subchapterFileDb.UploadFile(subChapterFile);
            }
            return RedirectToAction("Index", new { chapterId = subChapter.ChapterId });
        }


        public ActionResult GetFileList(int subchapterId)
        {

            var subchapter = subChapterDb.GetSubChapter(subchapterId);
            ViewData["subChapterId"] = subchapterId;
            ViewData["chapterId"] = subchapter.ChapterId;
            ViewData["subchapterName"] = subchapter.SubchapterTitle;

            // get file list from database and check if they exist.

            var files = subchapterFileDb.GetSubChapterFiles(subchapterId);

            var existFiles = new List<SubChapterFiles>();

            foreach (var file in files)
            {
                var path = Server.MapPath(file.FilePath);

                if (System.IO.File.Exists(path))
                {
                    existFiles.Add(file);
                }
            }

            if(User.IsInRole("student")|| User.IsInRole("helping_student"))
            {
                return View("FilesForStudent", existFiles);
            }

            //return the files to the view:
            return View(existFiles);

        }

        [HttpGet]
        public ActionResult DownloadFile(string fiLeName, int subChapterId)
        {

            if (fiLeName.IsEmpty())
            {
                return View("NotFound");
            }

            var nameofFile = Path.GetFileName(fiLeName);
            var file = subchapterFileDb.getSubChapterFile(subChapterId, nameofFile);


            // folder path
            string fullPath = file.FilePath;

            fullPath = Server.MapPath(fullPath); // we need the physical path, not the url

            if (!System.IO.File.Exists(fullPath))
            {
                return View("NotFound");
            }

            // Determine content type
            var contentType = MimeMapping.GetMimeMapping(fullPath);

            // Serve file for download
            return File(fullPath, contentType, nameofFile);


        }

        public ActionResult ViewSubChapter(int subchapterId)
        {
            var subChapter = subChapterDb.GetSubChapter(subchapterId);
            ViewData["chapterId"] = subChapter.ChapterId;

            if (subChapter == null)
            {
                return View("NoSuchSubchapterFound");
            }
            /*
            if (User.IsInRole("professor"))
            {
                var filess = subchapterFileDb.GetSubChapterFiles(subchapterId);

                var existFiless = new List<SubChapterFiles>();

                foreach (var file in filess)
                {
                    var path = Server.MapPath(file.FilePath);

                    if (System.IO.File.Exists(path))
                    {
                        existFiless.Add(file);
                    }
                }
                subChapter.SubchapterFiles = existFiless.ToArray();
                return View(subChapter);
            }
            */
            if (User.IsInRole("admin") || User.IsInRole("professor"))
            {
                return View("ViewSubChapterForAdmin", subChapter);
            }

            //user should be enrolled in course in order to be able to access this.
            var studentId = User.Identity.GetUserId();
            var isEnrolled = enrollmentDb.IsEnrolledInCourse(studentId, subChapter.Chapter.CourseId);

            var enrollmentData = enrollmentDb.getEnrolledStudentInfo(subChapter.Chapter.CourseId, studentId);
            var lastsubchapterId = enrollmentData.LastCompletedSubChapterId;


            /////////////////


            var files = subchapterFileDb.GetSubChapterFiles(subchapterId);

            var existFiles = new List<SubChapterFiles>();

            foreach (var file in files)
            {
                var path = Server.MapPath(file.FilePath);

                if (System.IO.File.Exists(path))
                {
                    existFiles.Add(file);
                }
            }


            subChapter.SubchapterFiles= existFiles.ToArray();
            ////////////

            if (subchapterId == lastsubchapterId && !enrollmentData.CompletedCourse)
            {
                ViewData["IsCurrentSubchapter"] = "true";
            }
            else
            {
                ViewData["IsCurrentSubchapter"] = "false";
            }
            

            if (!isEnrolled)
            {
                return View("YouAreNotEnrolledInThisCourse");
            }

            var lastsubch = subChapterDb.GetNextSubChapter(subchapterId);
            if(lastsubch != null)
            {
                ViewData["lastSubch"] = "false";
            }



            return View(subChapter);
        }

        public ActionResult NextSubChapter(int currentSubChapterId)
        {
            var nextSubChapter = subChapterDb.GetNextSubChapter(currentSubChapterId);

            var currentSubchapter = subChapterDb.GetSubChapter(currentSubChapterId);
            var userId = User.Identity.GetUserId();
            var enrollment = enrollmentDb.getEnrolledStudentInfo(currentSubchapter.Chapter.CourseId, userId);
            if (enrollment != null && enrollment.CompletedCourse && nextSubChapter == null)
            {
                return RedirectToAction("GetStudentFinishedCourses","Course");
            }

            if (nextSubChapter == null) // either the course was finished, or an error occured
            {
                return View("YouPromotedTheCourse");
            }


            return RedirectToAction("ViewSubChapter", new { subchapterId = nextSubChapter.SubchapterId });
        }

        [HttpGet]
        public ActionResult DeleteFile(int fileId)
        {
            var model = subchapterFileDb.getSubChapterFileById(fileId);
            var subchapterId = model.SubChapterId;

            var fullPath = Server.MapPath(model.FilePath);


            if (System.IO.File.Exists(fullPath))
            {

                System.IO.File.Delete(fullPath);

                subchapterFileDb.DeleteFile(fileId);
            }
            return RedirectToAction("GetFileList", new { subchapterId = subchapterId });
        }

    }
}