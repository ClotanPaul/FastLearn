using Proiect_Licenta.Models;
using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlWarningData : IWarningData
    {
        private ApplicationDataDbContext db;

        public SqlWarningData(ApplicationDataDbContext db)
        {
            this.db = db;
        }

        public List<Warning> getUserWarnings(int userDataId)
        {
            var user = db.UsersData.FirstOrDefault(u => u.UserDataId == userDataId);
            
            var warnings= (user.Warnings == null)? new List<Warning>() : user.Warnings.ToList();

            return warnings;
        }

        public Warning getWarning(int warningId)
        {
            var warning = db.Warnings.Include("User")
                .FirstOrDefault(w => w.WarningId == warningId);
            return warning;
        }

        public void RemoveSuspension(int userDataId)
        {
            var user = db.UsersData.FirstOrDefault(u=> u.UserDataId==userDataId); ;
            user.IsSuspended = false;
            db.SaveChanges();
        }

        public void RemoveWarning(int warningId)
        {
            var warning = db.Warnings.FirstOrDefault(w => w.WarningId == warningId);

            var user = db.UsersData.FirstOrDefault(u => u.UserDataId == warning.UserId);

            if(user.Warnings.Count == 3)
            {
                user.IsSuspended = false;
                user.SuspendedUntil= DateTime.Now;
            }
            if(warning!= null)
            {
                db.Warnings.Remove(warning);
                db.SaveChanges();
            }

        }

        public void SuspendUser(int userId, DateTime until)
        {
            var user = db.UsersData.FirstOrDefault(u => u.UserDataId == userId);
            user.IsSuspended = true;
            user.SuspendedUntil = until;
            db.SaveChanges();
        }

        public void WarnUser(int userId, string Reason)
        {
            var user = db.UsersData.FirstOrDefault(x => x.UserDataId == userId);

            var warning = new Warning
            {
                UserId = user.UserDataId,
                WarningReason = Reason
            };

            user.Warnings.Add(warning);

            if (user.Warnings.Count() == 3)
            {
                user.IsSuspended = true;
                user.SuspendedUntil = DateTime.Now.AddDays(3);
            }

            db.SaveChanges();
        }

    }
}
