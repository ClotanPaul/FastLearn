using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface IChatData
    {
        List<Chat> getChat(int subChapterId, int userDataId);

        List<Chat> getAllChats(int userDataId);

        void addChat(Chat chat);

        Chat getChatById(int chatId);

        List<Chat> getFreeChats(int userDataId);

        void AssignHelpingStudent(int chatId, int userId);

        void UnAssignHelpingStudent(int chatId);

        List<Chat> getHelpingStudentChats(int userId);
    }
}
