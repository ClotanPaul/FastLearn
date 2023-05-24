using ProiectLicenta.Data.Migrations;
using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface IUserAnswerData
    {
        void addTestAnswer(UserAnswer userAnswer);

        UserAnswer getUserAnswer(int userAnswerId);

    }
}
