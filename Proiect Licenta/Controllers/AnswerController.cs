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
    public class AnswerController : Controller
    {

        private readonly IAnswerData answerDb;
        public AnswerController(IAnswerData answerDb)
        {
            this.answerDb = answerDb;
        }

        // GET: Answer
        public ActionResult Index(int questionId)
        {
            var model = answerDb.getAnswers(questionId);

            ViewData["questionId"] = questionId;
            ViewData["userId"] = User.Identity.GetUserId();

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int questionId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Answer newAnswer, int questionId)
        {

            newAnswer.QuestionId = questionId;
            if (ModelState.IsValid)
            {
                answerDb.AddAnswer(newAnswer);

            }
            return RedirectToAction("Index", new { questionId = newAnswer.QuestionId });
        }

        [HttpGet]
        public ActionResult Edit(int answerId)
        {
            var model = answerDb.GetAnswer(answerId);

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Answer answer)
        {
            if (ModelState.IsValid)
            {
                answerDb.UpdateAnswer(answer);

            }
            return RedirectToAction("Index", new { questionId = answer.QuestionId });
        }

        [HttpGet]
        public ActionResult Delete(int answerId)
        {
            var model = answerDb.GetAnswer(answerId);

            //var userId = User.Identity.GetUserId();

            //if (userId != model.OwnerId)
            //    return View("NoAcces");

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int answerId, FormCollection form)
        {
            var answer = answerDb.GetAnswer(answerId);
            answerDb.DeleteAnswer(answerId);
            return RedirectToAction("Index", new {  questionId = answer.QuestionId });

        }
    }
}