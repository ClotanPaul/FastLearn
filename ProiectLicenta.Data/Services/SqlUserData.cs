﻿using Proiect_Licenta.Models;
using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlUserData : IUserData
    {
        private ApplicationDataDbContext db;

        public SqlUserData(ApplicationDataDbContext db)
        {
            this.db = db;
        }

        //returns the id of the userdata table, given the string userid from the proiectLicenta.Web id
        public int getUserId(string userId)
        {
            var user = db.UsersData.FirstOrDefault(x=> x.UserId== userId);

            var id = user.UserDataId;

            return id;
        }

        public UserData getUserData(string userId)
        {
            var user = db.UsersData.FirstOrDefault(x=>x.UserId== userId);
            if(user==null)
                return null;
            return user;
        }

        public void CreateUserDataInst(string currentUserId, string userEmail)
        {
            UserData user = new UserData();
            
            user.UserId = currentUserId;
            user.Email = userEmail;
            user.UserRole = "student";
            user.UserName = "Not Chosen";
            user.SuspendedUntil = DateTime.Now;
            user.IsSuspended = false;

            db.UsersData.Add(user);
            db.SaveChanges();
        }

        public void UpdateUserData(UserData user)
        {
            if(user != null)
            {
                db.UsersData.AddOrUpdate(user);
                db.SaveChanges();
            }

        }

        public List<UserData> GetAllUsers()
        {
            return db.UsersData.ToList();
        }

        public void ChangeRole(string newRole, string userId)
        {
            var model = db.UsersData.FirstOrDefault(ud => ud.UserId == userId);
            model.UserRole = newRole;
            db.SaveChanges();
        }

        public void WarnUser(int UserId, string Reason)
        {
            var user = db.UsersData.FirstOrDefault(u => u.UserDataId == UserId);
            var warning = new Warning
            {
                WarningReason = Reason,
                UserId= UserId,
            };

            user.Warnings.Add(warning);
            db.SaveChanges();

        }

        public UserData getUserByUserDataId(int id)
        {
            return db.UsersData.FirstOrDefault(ud => ud.UserDataId == id);
        }

        public UserData getUserByUserName(string userName)
        {
            var user = db.UsersData.FirstOrDefault(u=> u.Email== userName);
            return user;
        }
    }
}