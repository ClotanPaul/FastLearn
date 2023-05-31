using Microsoft.AspNet.Identity;
using Proiect_Licenta.ViewModels;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Licenta.Controllers
{
    public class TestController : Controller
    {
        private readonly ITestData testDb;
        private readonly ISubChapterData subChapterDb;
        private readonly IQuestionData questionDb;
        private readonly IAnswerData answerDb;
        private readonly IUserAnswerData userAnswerDb;
        private readonly IUserData userDataDb;
        private readonly IEnrolledSudentInCourse enrollmentDb;

        public TestController(ITestData testDb, ISubChapterData subChapterDb, IQuestionData questionDb, IAnswerData answerDb, IUserAnswerData userAnswerDb, IUserData userDataDb, IEnrolledSudentInCourse enrollmentDb)
        {
            this.testDb = testDb;
            this.subChapterDb = subChapterDb;
            this.questionDb = questionDb;
            this.answerDb = answerDb;
            this.userAnswerDb = userAnswerDb;
            this.userDataDb = userDataDb;
            this.enrollmentDb = enrollmentDb;
        }


        // GET: Test
        public ActionResult Index(int subChapterId)
        {
            var test = testDb.getTest(subChapterId);
            int chapterId;

            if (test == null)
            {
                var subChapter = subChapterDb.GetSubChapter(subChapterId);
                chapterId = subChapter.ChapterId;
            }
            else
            {
                chapterId = test.SubChapter.ChapterId;
            }

            ViewData["subChapterId"] = subChapterId; // when the controller is called from the subchapter controller, there is not subchapterId initially
            ViewData["chapterId"] = chapterId;
            ViewData["userId"] = User.Identity.GetUserId();

            return View(test);
        }


        [HttpGet]
        public ActionResult Create(int subChapterId)
        {
            var subChapter = subChapterDb.GetSubChapter(subChapterId);
            if (subChapter.Test != null)
                return View("AlreadyExistingTest");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Test newTest, int subChapterId)
        {

            var testSubChapter = subChapterDb.GetSubChapter(subChapterId);
            newTest.SubChapter = testSubChapter;
            if (ModelState.IsValid)
            {
                testDb.AddTest(newTest, subChapterId);

            }
            return RedirectToAction("Index", new { subChapterId = newTest.SubChapter.SubchapterId });
        }

        [HttpGet]
        public ActionResult Edit(int subChapterId)
        {
            var subChapter = subChapterDb.GetSubChapter(subChapterId);
            var test = subChapter.Test;

            if (test == null)
                return View("NotFound");
            return View(test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Test Test, int subChapterId)
        {
            if (ModelState.IsValid)
            {
                testDb.UpdateTest(Test, subChapterId);

            }
            return RedirectToAction("Index", new { subChapterId = subChapterId });
        }

        [HttpGet]
        public ActionResult Delete(int subChapterId)
        {

            if (!ModelState.IsValid)
            {
                return View("ModelStateNotValid");
            }

            var subChapter = subChapterDb?.GetSubChapter(subChapterId);
            var test = subChapter.Test;

            var userId = User.Identity.GetUserId();

            if (userId != subChapter.Chapter.Course.OwnerId)
                return View("NoAcces");

            if (subChapter == null || test == null)
                return View("NotFound");
            return View(test);
        }

        [HttpPost]
        public ActionResult Delete(int subChapterId, FormCollection form)
        {

            if (!ModelState.IsValid)
            {
                return View("ModelStateNotValid");
            }

            var test = testDb.getTest(subChapterId);
            var chapterId = test.SubChapter.ChapterId;

            testDb.DeleteTest(test);

            return RedirectToAction("Index", "SubChapter", new { chapterId = chapterId });

        }

        public ActionResult TakeTest(int testId)
        {
            // Fetch questions and choices from the database
            var test = testDb.getTestById(testId);
            var questions = questionDb.getQuestions(test.TestId);

            if (test == null)
                return View("TestNotFound");



            // Map the questions and choices to the TestViewModel
            var testViewModel = new TestViewModel
            {
                TestId = test.TestId,
                Questions = test.Questions.Select(q => new QuestionViewModel
                {
                    QuestionId = q.QuestionId,
                    QuestionString = q.QuestionString,
                    Answers = q.Answers.Select(a => new AnswerViewModel
                    {
                        AnswerId = a.AnswerId,
                        AnswerText = a.AnswerText,
                    }).ToList(),
                }).ToList(),

            };

            return View(testViewModel);
        }

        public ActionResult SubmitTest(TestViewModel testViewModel, string submitButton)
        {
            // are all the questions answered?

            if (testViewModel.Questions.Any(q => q.SelectedAnswerId == 0))
            {
                ModelState.AddModelError("", "You must answer all the questions");
                return View("TakeTest", testViewModel.TestId);
            }

            //computing the score
            int correctAnswerNumber = 0;
            foreach (var question in testViewModel.Questions)
            {
                var answer = answerDb.GetAnswer(question.SelectedAnswerId);

                if (answer != null && answer.IsCorrect)
                {
                    correctAnswerNumber++;
                }
            }
            int questionNumber = testViewModel.Questions.Count;
            int score = (correctAnswerNumber*100 / questionNumber);

            //saving everything in a userAnswer instance.
            int minimumScore = testDb.getTestById(testViewModel.TestId).MinimumScore;

            if (submitButton == "SubmitTestWithPoints")
            {
                var user = userDataDb.getUserData(User.Identity.GetUserId());

                if (user.Points >= 20)
                {
                    user.Points -= 20;
                    minimumScore -= 15;
                    if (minimumScore < 50)
                    {
                        minimumScore = 50;
                    }
                }
            }
                
            

            var userAnswer = new UserAnswer
            {
                UserId = User.Identity.GetUserId(),
                TestId = testViewModel.TestId,
                Score = score,
                Passed = score >= minimumScore,
                chosenAnswersIdList = testViewModel.Questions.Select(q => q.SelectedAnswerId).ToList(),
            };

            userAnswerDb.addTestAnswer(userAnswer);

            if (userAnswer.Passed)
            {
                var userData = userDataDb.getUserData(userAnswer.UserId);

                var test = testDb.getTestById(userAnswer.TestId);
                var courseId = test.SubChapter.Chapter.CourseId;

                var enrollment = enrollmentDb.getEnrolledStudentInfo(courseId, userAnswer.UserId);

                var nextSubchapter = subChapterDb.GetNextSubChapter(test.SubChapter.SubchapterId);

                if (nextSubchapter == null) // either the course was finished, or an error occured
                {
                    enrollment.CompletedCourse = true;
                    enrollmentDb.UpdateEnrollment(enrollment);
                    return View("YouPromotedTheCourse");
                }

                enrollment.LastCompletedSubChapterId = nextSubchapter.SubchapterId;
                enrollment.LastCompletedChapterId = nextSubchapter.ChapterId;

                enrollmentDb.UpdateEnrollment(enrollment);

            }

            // show the results of the test:
            return RedirectToAction("Results", new { userAnswerId = userAnswer.UserAnswerId });
        }

        public ActionResult Results(int userAnswerId)
        {
            var userAnswer = userAnswerDb.getUserAnswer(userAnswerId);
            if(userAnswer == null)
            {
                return View("UserAnswerNotFound");
            }

            var chosenAnswersIdList = userAnswer.chosenAnswersIdList ?? new List<int>();

            var questionsResponses = userAnswer.Test.Questions.Select(question => new QuestionResponsesViewModel
            {
                Question = question,
                UserAnswer = question.Answers.SingleOrDefault(a => chosenAnswersIdList.Contains(a.AnswerId)).AnswerText,
                CorrectAnswer = question.Answers.SingleOrDefault(a => a.IsCorrect == true).AnswerText
            }
            ).ToList();


            var resultsViewModel = new ResultsViewModel
            {
                UserAnswer = userAnswer,
                ScorePercentage = userAnswer.Test.MinimumScore,
                QuestionResponses = questionsResponses,
            };

            return View(resultsViewModel);
            
        }
    }
}