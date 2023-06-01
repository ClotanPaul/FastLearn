using Proiect_Licenta.Models;
using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlChatData : IChatData
    {
        private readonly ISubChapterData subChapterDb;
        private readonly IUserData userDb;
        private readonly ApplicationDataDbContext db;

        public SqlChatData(ISubChapterData subChapterDb, ApplicationDataDbContext db, IUserData userDb)
        {
            this.subChapterDb = subChapterDb;
            this.db = db;
            this.userDb = userDb;
        }

        public void addChat(Chat chat)
        {
            if (chat != null)
            {
                db.Chats.Add(chat);
                db.SaveChanges();
            }
        }

        public void AssignHelpingStudent(int chatId, int userId)
        {
            var chat = getChatById(chatId);
            var helpingStudent = userDb.getUserByUserDataId(userId);
            chat.HelpingStudent = helpingStudent;
            if (helpingStudent != null)
            {
                db.SaveChanges();
            }
        }

        public List<Chat> getAllChats(int userDataId)
        {
            var chatList = new List<Chat>();
            chatList = db.Chats.Include("SubChapter.Chapter.Course")
                .Include("Student")
                .Include("HelpingStudent")
                .Where(chat => chat.StudentId == userDataId).ToList();
            return chatList;

        }

        public List<Chat> getChat(int subChapterId, int userId)
        {
            var subChapter = subChapterDb.GetSubChapter(subChapterId);

            var subchapterChats = subChapter.SubChapterChats.ToList();

            var chats = subchapterChats.Where(chat => chat.StudentId == userId || chat.HelpingStudentId == userId).ToList();

            return chats;
        }

        public Chat getChatById(int chatId)
        {
            var chat = db.Chats
                .Include("ChatMessages")
                .Include("Student")
                .Include("HelpingStudent")
                .FirstOrDefault(ch => ch.ChatId == chatId);
            return chat;
        }

        public List<Chat> getFreeChats(int userDataId)
        {
            var chats = db.Chats.Include("SubChapter.Chapter.Course")
                .Include("Student")
                .Where(ch => ch.StudentId != userDataId && ch.HelpingStudent == null).ToList();
            return chats;
        }

        public List<Chat> getHelpingStudentChats(int userId)
        {
            var chatList = new List<Chat>();
            chatList = db.Chats
                .Include("SubChapter.Chapter.Course")
                .Include("Student")
                .Include("HelpingStudent")
                .Where(chat => chat.HelpingStudentId == userId).ToList();
            return chatList;
        }

        public void UnAssignHelpingStudent(int chatId)
        {
            var chat = getChatById(chatId);
            chat.HelpingStudent = null;

            db.SaveChanges();

        }
    }
}
