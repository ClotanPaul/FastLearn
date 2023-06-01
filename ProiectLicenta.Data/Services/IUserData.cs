using Proiect_Licenta.Models;
using ProiectLicenta.Data.Models;
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

        void AddHelpingStudentApplication(HelpingStudentApplication application);

        HelpingStudentApplication getHelpingStudentApplication(int userId);

        List<HelpingStudentApplication> getHelpingStudentApplicationsForProfessor();

        List<HelpingStudentApplication> getHelpingStudentApplicationsForAdmin();

        void DeleteHelpingStudentApplication(int helperApplicationId);

        HelpingStudentApplication getHelpingStudentApplicationById(int id);

        void supportHelpingStudentApplication(int applicationId, int professorId);

        void approveHelpingStudentApplication(int applicationId);

        void AssignPointsToUser(int userDataId, int numberOfPoints);

    }
}
