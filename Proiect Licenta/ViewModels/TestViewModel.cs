using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.ViewModels
{
    public class TestViewModel
    {
        public int TestId { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}