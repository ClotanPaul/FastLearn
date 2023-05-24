using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface IUserData
    {
        int getUserId(string userId);

        UserData getUserByUserDataId(int id);

        UserData getUserData(string userId);

        void CreateUserDataInst(string currentUserId, string userEmail);

        void UpdateUserData(UserData user);

        void ChangeRole(string newRole, string userId);

        List<UserData> GetAllUsers();

        void WarnUser(int UserId, string Reason);

        UserData getUserByUserName(string userName);

    }
}
