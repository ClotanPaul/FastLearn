using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface IAnswerData
    {

        List<Answer> getAnswers(int QuestionId);

        List<Answer> getCorrectAnswers(int QuestionId);
        void AddAnswer(Answer answer);

        Answer GetAnswer(int answerId);

        void UpdateAnswer(Answer answer);

        void DeleteAnswer(int answerId);
    }
}
