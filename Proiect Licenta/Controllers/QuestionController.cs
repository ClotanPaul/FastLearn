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
    public class QuestionController : Controller
    {

        private readonly ITestData testDb;
        private readonly ISubChapterData subChapterDb;
        private readonly IQuestionData questionDb;

        public QuestionController(ITestData testDb, ISubChapterData subChapterDb, IQuestionData questionDb)
        {
            this.testDb = testDb;
            this.subChapterDb = subChapterDb;
            this.questionDb = questionDb;
        }


        // GET: Question
        public ActionResult Index(int testId)
        {
            var questions = questionDb.getQuestions(testId);

            ViewData["testId"] = testId;
            ViewData["userId"] = User.Identity.GetUserId();

            return View(questions);
        }

        [HttpGet]
        public ActionResult Create(int testId)
        {
            var test = testDb.getTestById(testId);
            if (test == null)
                return View("NotFound");
            return View();
        }


        [HttpPost]
        public ActionResult Create(Question question, int testId)
        {

            var test = testDb.getTestById(testId);
            if (test == null)
                return View("NotFound");
            if(!ModelState.IsValid) {
                return View("ModelStateNotValid");
            }

            questionDb.AddQuestion(question, test);


            return RedirectToAction("Index", new { testId = testId });
        }

        [HttpGet]
        public ActionResult Edit(int questionId)
        {
            var model = questionDb.getQuestion(questionId);

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                questionDb.UpdateQuestion(question);

            }
            return RedirectToAction("Index", new { testId = question.TestId });
        }

        [HttpGet]
        public ActionResult Delete(int questionId)
        {
         
            var question = questionDb.getQuestion(questionId);

            if (question == null)
                return View("NotFound");
            return View(question);
        }

        [HttpPost]
        public ActionResult Delete(int questionId, FormCollection form)
        {
            var testId = questionDb.getQuestion(questionId).TestId;
            questionDb.DeleteQuestion(questionId);
            return RedirectToAction("Index", new { testId = testId });
        }
    }
}