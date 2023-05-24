using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.ViewModels
{
    public class QuestionResponsesViewModel
    {
        public Question Question { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
    }
}