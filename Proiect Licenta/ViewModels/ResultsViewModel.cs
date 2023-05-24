using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.ViewModels
{
    public class ResultsViewModel
    {
        public UserAnswer UserAnswer { get; set; }
        public int ScorePercentage { get; set; }

        public List<QuestionResponsesViewModel> QuestionResponses { get; set; }
    }
}