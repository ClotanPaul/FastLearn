using Microsoft.AspNet.Identity;
using Proiect_Licenta.Migrations;
using Proiect_Licenta.Models;
using Proiect_Licenta.Services;
using Proiect_Licenta.ViewModels;
using ProiectLicenta.Data.Migrations;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Proiect_Licenta.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ICourseData courseDb;
        private readonly IUserData userDb;
        private readonly IWarningData warningDb;
        private readonly IRoleData roleDb;
        private readonly IChatData chatDb;

        private readonly int pointsForStudent = 5;
        private readonly int pointsForHelpingStudent = 20;


        public UsersController(ICourseData courseDb, IUserData userDb, IWarningData warningDb, IRoleData roleDb, IChatData chatDb)
        {
            this.courseDb = courseDb;
            this.userDb = userDb;
            this.warningDb = warningDb;
            this.roleDb = roleDb;
            this.chatDb = chatDb;
        }
        // GET: Users
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var userData = userDb.getUserData(currentUserId);
            var userEmail = User.Identity.GetUserName();
            if(userData== null)
            {
                userDb.CreateUserDataInst(currentUserId, userEmail);
                userData = userDb.getUserData(currentUserId);
            }

            return View(userData);
        }

        public ActionResult UsersDetails()
        {
            var users = userDb.GetAllUsers();

            return View(users);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var userId = User.Identity.GetUserId();
            var model = userDb.getUserData(userId);

            if (model == null)
                return View("NotFound");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserData user)
        {
            if (user.UserName.IsEmpty())
            {
                ModelState.AddModelError("UserName", "Can't be empty.");
            }
            else
            {
                if(user.UserName.Count() > 15)
                {
                    ModelState.AddModelError("UserName", "Maximum 15 characters.");
                }
            }
            if (ModelState.IsValid)
            {
                userDb.UpdateUserData(user);   
            }
            else
            {
                return View(user);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChangeRole(string userId)
        {
            var model = userDb.getUserData(userId);

            List<string> possibleRoles = new List<string>
            {
                "admin",
                "student",
                "professor",
                "helping_student"
            };

            if (model == null)
                return View("NotFound");

            var viewModel = new ChangeRoleViewModel
            {
                UserId = model.UserId,
                Email = model.Email,
                currentRole = model.UserRole,
                PossibleRoles= possibleRoles
                
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRole(ChangeRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userLoginDb = new SqlRoleData(new ApplicationDbContext());

                userDb.ChangeRole(model.NewRole, model.UserId);
                userLoginDb.ChangeRole(model.UserId, model.NewRole);

            }
            else
            {
                List<string> possibleRoles = new List<string>
            {
                "admin",
                "student",
                "professor",
                "helping_student"
            };
                model.PossibleRoles = possibleRoles;

                return View(model);
            }
            return RedirectToAction("UsersDetails");
        }



        [HttpGet]
        public ActionResult WarnUser(int userDataId) 
        {
            var userData = userDb.getUserByUserDataId(userDataId);

            var warnViewModel = new WarnUserViewModel
            {
                UserDataId = userData.UserDataId,
                UserName = userData.Email,

            };

            return View(warnViewModel);


           
        }
        
        [HttpPost]
        public ActionResult WarnUser(WarnUserViewModel warningView, FormCollection form)
        {

            if (!ModelState.IsValid)
            {
                return View(warningView);
            }

            var user = userDb.getUserByUserDataId(warningView.UserDataId);


            warningDb.WarnUser(warningView.UserDataId, warningView.Reason);

            return RedirectToAction("UsersDetails", "Users");

        }


        [HttpGet]
        public ActionResult UserWarnings(int userDataId) // userDataId
        {
            var warnings = warningDb.getUserWarnings(userDataId);

            return View(warnings);
        }

        public ActionResult RemoveWarning(int warningId)
        {
            var warning = warningDb.getWarning(warningId);
            var userDataId = warningDb.getWarning(warning.WarningId).UserId; 
            if (!ModelState.IsValid)
            {
                return View("ModelStateNotValid");
            }

            warningDb.RemoveWarning(warning.WarningId);

            return RedirectToAction("UserWarnings","Users", new { userDataId = userDataId });
        }

        [HttpGet]
        public ActionResult SuspendUser(int userDataId)
        {

            var user = userDb.getUserByUserDataId(userDataId);

            if (user.IsSuspended)
            {
                return View("UserAlreadySuspended");
            }


            return View(user);
        }

        [HttpPost]
        public ActionResult SuspendUser(UserData user, FormCollection form)
        {
            var userDataId = user.UserDataId;
            var until = user.SuspendedUntil;
            if(user.SuspendedUntil < DateTime.Now)
                ModelState.AddModelError("SuspendedUntil", "Must be greater than today's date.");
            if (!ModelState.IsValid)
            {

                return View(user);
            }

            warningDb.SuspendUser(userDataId, until);

            return RedirectToAction("UsersDetails", "Users");
        }

        [HttpGet]
        public ActionResult RemoveUserSuspension(int userDataId)
        {

            var user = userDb.getUserByUserDataId(userDataId);

            if (!user.IsSuspended)
            {
                return View("UserNotSuspended");
            }


            return View(user);
        }

        [HttpPost]
        public ActionResult RemoveUserSuspension(int userDataId, FormCollection form)
        {
            if (!ModelState.IsValid)
            {
                return View("ModelStateNotValid");
            }

            warningDb.RemoveSuspension(userDataId);

            return RedirectToAction("UsersDetails", "Users");
        }

        public ActionResult ApplyForHelpingStudent()
        {
            var userId = User.Identity.GetUserId();
            var userData = userDb.getUserData(userId);

            var application = new ProiectLicenta.Data.Models.HelpingStudentApplication();

            application.StudentId = userData.UserDataId;
            userData.HelpingStudentApplicationId= application.HelpingStudentApplicationId;

            application.FinishedCoursesIds = courseDb.GetFinishedCoursesSerialized(userData.UserDataId);

            application.IsApproved = false;

            userDb.AddHelpingStudentApplication(application);

            return RedirectToAction("YourHelpingStudentApplication");

        }

        public ActionResult YourHelpingStudentApplication()
        {
            var userId = User.Identity.GetUserId();
            var userData = userDb.getUserData(userId);

            var application = userDb.getHelpingStudentApplication(userData.UserDataId);

            if(application == null)
            {
                return View((HelperApplicationViewModel)null);
            }

            var helperStudentApplication = new HelperApplicationViewModel()
            {
                HelpingStudentApplicationId = application.HelpingStudentApplicationId,
                FinishedCourses = courseDb.GetEnrolledCoursesByIds(courseDb.FinishedCoursesDeserialization(application.FinishedCoursesIds)),
                ProfessorEmail = application.ProfessorId != null ? userDb.getUserByUserDataId((int)application.ProfessorId)?.Email : null,
                StudentEmail = userData.Email,
            };

            return View(helperStudentApplication);
        }



        public ActionResult SeeHelpingStudentApplicationsForProfessor()
        {
            var applicationsViewModels = new List<HelperApplicationViewModel>();
            var applications = userDb.getHelpingStudentApplicationsForProfessor();

            foreach (var application in applications)
            {
                var courses = courseDb.GetEnrolledCoursesByIds(courseDb.FinishedCoursesDeserialization(application.FinishedCoursesIds));

                var helperStudentApplication = new HelperApplicationViewModel();

                helperStudentApplication.HelpingStudentApplicationId = application.HelpingStudentApplicationId;
                helperStudentApplication.FinishedCourses = courses;

                var student = userDb.getUserByUserDataId(application.StudentId);
                if (student != null)
                {
                    helperStudentApplication.StudentEmail = student.Email;
                }

                string professor = null;

                if (professor == null)
                {
                    helperStudentApplication.ProfessorEmail = null;
                }

                applicationsViewModels.Add(helperStudentApplication);
            }

            return View(applicationsViewModels);
        }


        public ActionResult SeeHelpingStudentApplicationsForAdmin()
        {
            var applicationsViewModels = new List<HelperApplicationViewModel>();
            var applications = userDb.getHelpingStudentApplicationsForAdmin();
            foreach (var application in applications)
            {
                var courses = courseDb.GetEnrolledCoursesByIds(courseDb.FinishedCoursesDeserialization(application.FinishedCoursesIds));

                var helperStudentApplication = new HelperApplicationViewModel();

                helperStudentApplication.FinishedCourses = courses;
                helperStudentApplication.HelpingStudentApplicationId = application.HelpingStudentApplicationId;

                var student = userDb.getUserByUserDataId(application.StudentId);
                helperStudentApplication.StudentEmail = student?.Email;

                var professor = userDb.getUserByUserDataId((int)application.ProfessorId);
                helperStudentApplication.ProfessorEmail = professor?.Email;

                applicationsViewModels.Add(helperStudentApplication);
            }

            return View(applicationsViewModels);
        }
        [HttpGet]

        public ActionResult SupportHelpingStudentApplication(int applicationId)
        {
            var application = userDb.getHelpingStudentApplicationById(applicationId);
            var courses = courseDb.GetEnrolledCoursesByIds(courseDb.FinishedCoursesDeserialization(application.FinishedCoursesIds));

            var viewModel = new HelperApplicationViewModel
            {
                HelpingStudentApplicationId = application.HelpingStudentApplicationId,
                FinishedCourses = courses,
                StudentEmail = userDb.getUserByUserDataId(application.StudentId)?.Email,
                ProfessorEmail = null
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SupportHelpingStudentApplication(int applicationId, FormCollection form)
        {
            var userId = User.Identity.GetUserId();
            var userDataId = userDb.getUserId(userId);

            userDb.supportHelpingStudentApplication(applicationId, userDataId);

            return RedirectToAction("SeeHelpingStudentApplicationsForProfessor");
        }

        //For Admin

        public ActionResult ApproveHelpingStudentApplication(int applicationId)
        {

            var application = userDb.getHelpingStudentApplicationById(applicationId);

            var userId = application.StudentId;
            var userData = userDb.getUserByUserDataId(userId);
            var studentUserIdDb = userData.UserId;

            userDb.approveHelpingStudentApplication(applicationId);
            userDb.ChangeRole("HelpingStudent", studentUserIdDb);
            roleDb.ChangeRole(studentUserIdDb, "helping_student");

            if (User.IsInRole("professor"))
            {
                return RedirectToAction("SeeHelpingStudentApplicationsForProfessor");

            }
            return RedirectToAction("SeeHelpingStudentApplicationsForAdmin");

        }

            public ActionResult ProblemSolved(int chatId)
        {

            var chat = chatDb.getChatById(chatId);

            if (chat.HelpingStudentId == null)
            {
                return View("NoHelpingStudentAssigned");
            }

            chat.IssueSolved = true;

            var studentId = chat.StudentId;
            

            userDb.AssignPointsToUser(studentId, pointsForStudent);
           
            var helpingStudentId = chat.HelpingStudentId;
            userDb.AssignPointsToUser((int)helpingStudentId, pointsForHelpingStudent);


            return RedirectToAction("Index","Chat");

        }

        public ActionResult DeleteApplication(int applicationId)
        {

            userDb.DeleteHelpingStudentApplication(applicationId);

            if (User.IsInRole("student"))
            {
                return RedirectToAction("YourHelpingStudentApplication");
            }

            return RedirectToAction("SeeHelpingStudentApplicationsForAdmin");

        }



    }
}