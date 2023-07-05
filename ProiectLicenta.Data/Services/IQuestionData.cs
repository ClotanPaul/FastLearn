using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProiectLicenta.Data.Services
{
    public interface IQuestionData
    {


        List<Question> getQuestions(int testId);

        Question getQuestion(int questionId);

        void AddQuestion(Question question, Test test);

        void UpdateQuestion(Question question);

        void DeleteQuestion(int questionId);

    }
}
