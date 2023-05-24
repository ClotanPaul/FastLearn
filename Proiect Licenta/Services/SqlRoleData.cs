using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.Services
{
    public class SqlRoleData : IRoleData
    {

        ApplicationDbContext dbContext;

        public SqlRoleData(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void ChangeRole(string userId, string role)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
            string[] roles = new[] { userManager.GetRoles(userId).First() }; 
            userManager.RemoveFromRoles(userId,roles);
            userManager.AddToRole(userId, role);
            dbContext.SaveChanges();
        }


        public void makeStudent(string userId)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
            userManager.AddToRole(userId, "student");
            dbContext.SaveChanges();
        }
    }
}