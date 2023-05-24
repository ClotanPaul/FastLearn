using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.ViewModels
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionString { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
        public int SelectedAnswerId { get; set; }
    }
}