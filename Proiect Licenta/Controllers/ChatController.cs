using Microsoft.AspNet.Identity;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Proiect_Licenta.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUserData userDataDb;
        private readonly IChatData chatDb;
        private readonly IMessageData messageDb;
        private readonly ISubChapterData subChapterDb;

        public ChatController(IUserData userDataDb, IChatData chatDb, IMessageData messageDb, ISubChapterData subChapterDb)
        {
            this.userDataDb = userDataDb;
            this.messageDb = messageDb;
            this.chatDb = chatDb;
            this.subChapterDb = subChapterDb;
        }

        // GET: Chat
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userDataId = userDataDb.getUserId(userId);

            var chats = chatDb.getAllChats(userDataId);

            return View(chats);

        }

        public ActionResult GetHelpingStudentChats()
        {
            var userId = User.Identity.GetUserId();
            var userDataId = userDataDb.getUserId(userId);

            var chats = chatDb.getHelpingStudentChats(userDataId);

            if (chats.Count == 0)
            {
                return View("NoChatsFound");
            }

            return View(chats);

        }

        public ActionResult Create(int subChapterId)
        {
            ViewData["subChapterId"] = subChapterId;
            var subchapter = subChapterDb.GetSubChapter(subChapterId);

            ViewData["courseId"] = subchapter.Chapter.CourseId;
            ViewData["courseName"] = subchapter.Chapter.Course.CourseName;
            ViewData["subchapterName"] = subchapter.SubchapterTitle;
            ViewData["chapterName"] = subchapter.Chapter.ChapterTitle;

            var courseId = subchapter.Chapter.CourseId;
            ViewData["courseId"] = courseId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int subChapterId, string topic)
        {

            var userId = User.Identity.GetUserId();
            var userData = userDataDb.getUserData(userId);
            var subchapter = subChapterDb.GetSubChapter(subChapterId);
            ViewData["courseId"] = subchapter.Chapter.CourseId;
            ViewData["courseName"] = subchapter.Chapter.Course.CourseName;
            ViewData["subchapterName"] = subchapter.SubchapterTitle;
            ViewData["chapterName"] = subchapter.Chapter.ChapterTitle;


            var newChat = new Chat
            {
                Student = userData,
                SubChapter = subchapter,
                ChatMessages = new List<Message>(),
                IssueSolved= false,
                Topic = topic
            };

            if (newChat?.Topic!=null && newChat.Topic.IsEmpty())
            {
                ModelState.AddModelError("Topic", "Can't be empty!");
            }

            if(newChat.Topic.Count() > 30)
            {
                ModelState.AddModelError("Topic", "Maximum 30 characters");
            }

            if (!ModelState.IsValid)
            {
                return View(newChat);
            }

            chatDb.addChat(newChat);

            return RedirectToAction("Index");


        }

        public ActionResult OpenChat(int chatId)
        {
            var userId = User.Identity.GetUserId();
            var userDataId = userDataDb.getUserId(userId);

            var chat = chatDb.getChatById(chatId);

            chat.ChatMessages.OrderBy(m => m.TimeStamp);

            if(userDataId == chat.StudentId)
            {
                ViewData["isStudent"] = "true";
            }
            else
                ViewData["isStudent"] = "false";

            if (chat == null)
            {
                return View("ChatNotFound");
            }

            

            return View(chat);
        }

        public ActionResult GetChatRequests()
        {
            var userId = User.Identity.GetUserId();
            var userDataId = userDataDb.getUserId(userId);

            var chats = chatDb.getFreeChats(userDataId);

            if (chats == null)
            {
                return View("NoNewChats");
            }

            return View(chats);
        }

        [HttpGet]
        public ActionResult TakeChat(int chatId)
        {
            var chat = chatDb.getChatById(chatId);

            if(chat.HelpingStudent != null)
            {
                return View("ThisChatAlreadyHasAHelperAssigned");
            }

            if (chat == null)
            {
                return View("ChatNotFound");
            }

            return View(chat);
        }

        [HttpPost]
        public ActionResult TakeChat(int chatId, FormCollection form)
        {
            var userId = User.Identity.GetUserId();
            var userDataId = userDataDb.getUserId(userId);
            chatDb.AssignHelpingStudent(chatId, userDataId);

            return RedirectToAction("GetHelpingStudentChats");
        }

        public ActionResult LeaveChat(int chatId)
        {
            var chat = chatDb.getChatById(chatId);

            if (chat.HelpingStudent == null)
            {
                return View("ThisChatHasNoHelperAssigned");
            }

            if (chat == null)
            {
                return View("ChatNotFound");
            }

            return View(chat);
        }

        [HttpPost]
        public ActionResult LeaveChat(int chatId, FormCollection form)
        {
            chatDb.UnAssignHelpingStudent(chatId);

            return RedirectToAction("GetHelpingStudentChats");
        }
    }
}