﻿using Microsoft.AspNet.Identity;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Proiect_Licenta.Controllers
{
    public class AnswerController : Controller
    {

        private readonly IAnswerData answerDb;
        private readonly IQuestionData questionDb;

        public AnswerController(IAnswerData answerDb, IQuestionData questionDb)
        {
            this.answerDb = answerDb;
            this.questionDb = questionDb;
        }

        // GET: Answer
        public ActionResult Index(int questionId)
        {
            var model = answerDb.getAnswers(questionId);

            ViewData["questionId"] = questionId;
            if (model.Count > 0)
            {
                ViewData["testId"] = model.First().Question.TestId;
            }
            else
            {
                var question = questionDb.getQuestion(questionId);
                ViewData["testId"] = question.TestId;
            }
                ;
            ViewData["userId"] = User.Identity.GetUserId();

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int questionId)
        {
            ViewData["questionId"] = questionId;
            var question = questionDb.getQuestion(questionId);
            var answerModel = new Answer();
            var answers = question.Answers.ToList();
            var trueAnswerExists = false;
            foreach(var answer in answers)
            {
                if (answer.IsCorrect)
                    trueAnswerExists = true;
            }
            if (trueAnswerExists)
            {
                ViewData["trueExists"] = "true";
                answerModel.IsCorrect = false;
            }
            else
                ViewData["trueExists"] = "false";
            return View(answerModel);
        }
        [HttpPost]
        public ActionResult Create(Answer newAnswer, int questionId)
        {

            if (newAnswer.AnswerText.IsEmpty())
            {
                ViewData["questionId"] = questionId;
                var question = questionDb.getQuestion(questionId);
                var answerModel = new Answer();
                var answers = question.Answers.ToList();
                var trueAnswerExists = false;
                foreach (var answer in answers)
                {
                    if (answer.IsCorrect)
                        trueAnswerExists = true;
                }
                if (trueAnswerExists)
                {
                    ViewData["trueExists"] = "true";
                    answerModel.IsCorrect = false;
                }

                ModelState.AddModelError("AnswerText", "Can't be empty");
            }

            if (!ModelState.IsValid)
            {
                return View(newAnswer);
            }

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
            ViewData["courseTitle"] = model.Question.test.SubChapter.Chapter.Course.CourseName;
            ViewData["chapterTitle"] = model.Question.test.SubChapter.Chapter.ChapterTitle;
            ViewData["subchapterTitle"] = model.Question.test.SubChapter.SubchapterTitle;
            ViewData["questionString"] = model.Question.QuestionString;

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Answer answer)
        {
            if (answer.AnswerText.IsEmpty())
            {
                ModelState.AddModelError("AnswerText", "Can't be empty");
            }

            if (ModelState.IsValid)
            {
                answerDb.UpdateAnswer(answer);

            }
            else
            {
                var model = answerDb.GetAnswer(answer.AnswerId);
                ViewData["courseTitle"] = model.Question.test.SubChapter.Chapter.Course.CourseName;
                ViewData["chapterTitle"] = model.Question.test.SubChapter.Chapter.ChapterTitle;
                ViewData["subchapterTitle"] = model.Question.test.SubChapter.SubchapterTitle;
                ViewData["questionString"] = model.Question.QuestionString;

                return View(answer);
            }
            return RedirectToAction("Index", new { questionId = answer.QuestionId });
        }

        [HttpGet]
        public ActionResult Delete(int answerId)
        {
            var model = answerDb.GetAnswer(answerId);


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