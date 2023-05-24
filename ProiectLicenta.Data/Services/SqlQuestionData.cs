using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlQuestionData : IQuestionData
    {

        private ApplicationDataDbContext db;

        public SqlQuestionData(ApplicationDataDbContext db)
        {
            this.db = db;
        }


        public List<Question> getQuestions(int testId)
        {
            var test = db.Tests.FirstOrDefault(t => t.TestId == testId);
            List<Question> result;

            if (test == null)
            {
                return new List<Question>();
            }
            else
            {
                result = test.Questions.ToList();
            }
            return result;
        }

        public void AddQuestion(Question question, Test test)
        {
            test.Questions.Add(question);
            db.SaveChanges();
        }

        public Question getQuestion(int questionId)
        {
            var question = db.Questions.FirstOrDefault(q=>q.QuestionId == questionId);
            if (question != null)
            {
                return question;
            }
            else
                return null;
        }

        public void UpdateQuestion(Question question)
        {
            var currentQuestion = db.Questions.FirstOrDefault(q=>q.QuestionId == question.QuestionId);
            db.Questions.AddOrUpdate(currentQuestion, question);
            db.SaveChanges();


        }

        public void DeleteQuestion(int questionId)
        {
            var question = db.Questions.FirstOrDefault(q=>q.QuestionId==questionId);
            db.Questions.Remove(question);
            db.SaveChanges();

        }

    }
}
