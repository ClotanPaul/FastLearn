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
using System.Web;
using System.Web.Mvc;

namespace Proiect_Licenta.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ICourseData courseDb;
        private readonly IUserData userDb;
        private readonly IWarningData warningDb;


        public UsersController(ICourseData courseDb, IUserData userDb, IWarningData warningDb)
        {
            this.courseDb = courseDb;
            this.userDb = userDb;
            this.warningDb= warningDb;
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

        //not done
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
            if (ModelState.IsValid)
            {
                userDb.UpdateUserData(user);   
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
            return RedirectToAction("UsersDetails");
        }



        [HttpGet]
        public ActionResult WarnUser(int userDataId) // userDataId
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
                return View("ModelStateNotValid");
            }

            var user = userDb.getUserByUserDataId(warningView.UserDataId);


            warningDb.WarnUser(warningView.UserDataId, warningView.Reason);

            return RedirectToAction("UsersDetails", "Users");

        }


        [HttpGet]
        public ActionResult UserWarnings(int userDataId) // userDataId
        {
            var warnings = warningDb.getUserWarnings(userDataId);
            if (warnings.Count > 0 && warnings != null)
            {
                return View(warnings);
            }
            else
                return View("ThisUserHasNoWarnings");

        }

        [HttpGet]
        public ActionResult RemoveWarning(int warningId)
        {

            var warning = warningDb.getWarning(warningId);


            return View(warning);
        }

        [HttpPost]
        public ActionResult RemoveWarning(Warning warning, FormCollection form)
        {
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
            if (!ModelState.IsValid)
            {
                return View("ModelStateNotValid");
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




    }
}