using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProiectLicenta.Data.Services
{
    public interface ITestData
    {

        Test getTest(int subChapterId);

        Test getTestById(int testId);
        void AddTest(Test test, int subChapterId);

        void UpdateTest(Test test, int subChapterId);

        void DeleteTest(Test test);


    }
}
