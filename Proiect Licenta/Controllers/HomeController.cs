using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Licenta.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            if(User.IsInRole("professor"))
            {
                return RedirectToAction("GetUserCourses", "Course");
            }

            if (User.IsInRole("student") || User.IsInRole("helping_student"))
            {
                return RedirectToAction("GetStudentUnEnrolledCourses", "Course");
            }

            return RedirectToAction("Index", "Course");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}