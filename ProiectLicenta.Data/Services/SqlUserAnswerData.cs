using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlUserAnswerData : IUserAnswerData
    {

        private ApplicationDataDbContext db;

        public SqlUserAnswerData(ApplicationDataDbContext db)
        {
            this.db = db;
        }
        public void addTestAnswer(UserAnswer userAnswer)
        {
            if (userAnswer == null)
            {
                throw new ArgumentNullException();
            }
            userAnswer.ChosenAnswersIdListSerialized = string.Join(",", userAnswer.chosenAnswersIdList);
            db.UserAnswers.Add(userAnswer);
            db.SaveChanges();

        }

        public UserAnswer getUserAnswer(int userAnswerId)
        {
            var userAnswer = db.UserAnswers
                .Include("Test.Questions.Answers")
                .Include("Test.SubChapter")
                .FirstOrDefault(ua => ua.UserAnswerId == userAnswerId);
            userAnswer.chosenAnswersIdList = userAnswer.ChosenAnswersIdListSerialized.Split(',').Select(int.Parse).ToList();
            return userAnswer;
        }
    }
}
