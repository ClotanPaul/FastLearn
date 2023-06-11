using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Models
{
    public class CourseReview
    {

        public int CourseReviewId { get; set; }

        public string UserId { get; set; }

        [DisplayName("Review")]
        public string ReviewText { get; set; }

        [IntegerValidator(MinValue = 1, MaxValue = 5)]
        public int Stars { get; set; }

        public Course Course { get; set; }

        public int CourseId { get; set; }

        public string DeactivationReason { get; set; }

        [DefaultValue(false)]
        public bool IsActive { get; set; }


    }
}
