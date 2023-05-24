using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProiectLicenta.Data.Services
{
    public class SqlAnswerData : IAnswerData
    {
        private ApplicationDataDbContext db;

        public SqlAnswerData(ApplicationDataDbContext db)
        {
            this.db = db;
        }

        public void AddAnswer(Answer answer)
        {
            db.Answers.Add(answer);
            db.SaveChanges();
        }

        public void DeleteAnswer(int answerId)
        {
            var answer = db.Answers?.FirstOrDefault(a=>a.AnswerId==answerId);
            if(answer != null)
            {
                db.Answers.Remove(answer);
                db.SaveChanges();
            }
        }

        public Answer GetAnswer(int answerId)
        {
            var answer = db?.Answers.FirstOrDefault(a => a.AnswerId == answerId);
            return answer== null ? null : answer;
        }

        public List<Answer> getAnswers(int QuestionId)
        {
            var Answers = db.Answers.Where(a=>a.QuestionId== QuestionId).ToList();
            return Answers;
        }

        public List<Answer> getCorrectAnswers(int QuestionId)
        {
            throw new NotImplementedException();
        }

        public void UpdateAnswer(Answer answer)
        {   
            if (answer != null)
            {
                db.Answers.AddOrUpdate(answer);
                db.SaveChanges();
            }
        }
    }
}
