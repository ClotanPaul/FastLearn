using Ganss.Xss;
using Microsoft.AspNet.Identity;
using Proiect_Licenta.ViewModels;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Proiect_Licenta.Controllers
{
    public class SubChapterController : Controller
    {
        private readonly ICourseData courseDb;
        private readonly IUserData userDb;
        private readonly IChapterData chapterDb;
        private readonly ISubChapterData subChapterDb;
        private readonly ISubChapterFileData subchapterFileDb;
        public SubChapterController(ICourseData db, IUserData userDb, IChapterData chapterDb, ISubChapterData subChapterDb, ISubChapterFileData courseFileDb)
        {
            this.courseDb = db;
            this.userDb = userDb;
            this.chapterDb = chapterDb;
            this.subChapterDb = subChapterDb;
            this.subchapterFileDb = courseFileDb;
        }

        // GET: Chapter
        public ActionResult Index(int chapterId)
        {
            var model = subChapterDb.GetSubChapters(chapterId);
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
            return View();
        }
        [HttpPost]
        public ActionResult Create(SubChapter newSubChapter, int chapterId)
        {
            var sanitizer = new HtmlSanitizer();
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

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubChapter subChapter)
        {
            var sanitizer = new HtmlSanitizer();
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
            //set the path to the file
            //string fileFolder = Server.MapPath("~/Content/Videos/Images/Courses");//+subchapter.Chapter.Course.CourseName;

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

    }
}