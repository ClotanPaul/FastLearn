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
    public class ChapterController : Controller
    {

        private readonly ICourseData courseDb;
        private readonly IUserData userDb;
        private readonly IChapterData chapterDb;
        public ChapterController(ICourseData db, IUserData userDb, IChapterData chapterDb)
        {
            this.courseDb = db;
            this.userDb = userDb;
            this.chapterDb = chapterDb;
        }

        // GET: Chapter
        public ActionResult Index(int courseId)
        {
            var model = chapterDb.getCourseChapters(courseId);
            var course = courseDb.GetCourse(courseId);
            ViewData["courseId"] = courseId;
            ViewData["userId"] = User.Identity.GetUserId();
            ViewData["ownerId"] = course.OwnerId;

            if (model == null)
            {
                // to implement
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int courseId)
        {
            ViewData["courseId"] = courseId;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Chapter newChapter, int courseId)
        {

            newChapter.CourseId = courseId;
            if (ModelState.IsValid)
            {
                chapterDb.AddChapter(newChapter);

            }
            return RedirectToAction("Index", new { courseId = newChapter.CourseId });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = chapterDb.GetChapter(id);

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                chapterDb.UpdateChapter(chapter);
                
            }
            return RedirectToAction("Details", new { id = chapter.ChapterId });
        }


        public ActionResult Details(int id)
        {

            var model = chapterDb.GetChapter(id);

            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = chapterDb.GetChapter(id);
            var userId = User.Identity.GetUserId();

            if (userId != model.Course.OwnerId)
                return View("NoAcces");

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            var chapter = chapterDb.GetChapter(id);
            var courseId = chapter.CourseId;
            chapterDb.DeleteChapter(id);
            return RedirectToAction("Index",new {courseId = courseId});

        }
    }
}