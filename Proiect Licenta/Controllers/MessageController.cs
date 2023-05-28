using Microsoft.AspNet.Identity;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Licenta.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageData messageDb;
        private readonly IChatData chatDb;
        private readonly IUserData userDb;

        public MessageController(IMessageData messageDb, IChatData chatDb, IUserData userDb)
        {
            this.messageDb = messageDb;
            this.chatDb = chatDb;
            this.userDb = userDb;
        }

        public ActionResult AddMessage(int chatId, string newMessage)
        {
            Message Message = new Message();
            var userId = User.Identity.GetUserId();
            var userDataId = userDb.getUserId(userId);

            Message.ChatId = chatId;
            Message.MessageContent = newMessage;
            Message.UserId = userDataId; 
            //Message.TimeStamp = DateTime.Now;
            messageDb.addMessage(Message, chatId);
            return RedirectToAction("OpenChat", "Chat", new { chatId = chatId });
        }

        [HttpGet]
        public ActionResult EditMessage(int messageId)
        {
            var message = messageDb.getMessage(messageId);

            if (message == null)
                return View("NotFound");
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMessage(Message message)
        {
            if (ModelState.IsValid)
            {
                messageDb.EditMessage(message);
                return RedirectToAction("OpenChat","Chat", new { chatId = message.ChatId });
            }
            return View();
        }


        [HttpGet]
        public ActionResult DeleteMessage(int messageId)
        {
            var message = messageDb.getMessage(messageId);

            if (message == null)
                return View("NotFound");
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMessage(Message message)
        {
            var ChatId = message.ChatId;
            if (ModelState.IsValid)
            {
                messageDb.DeleteMessage(message.MessageId);
                return RedirectToAction("OpenChat", "Chat", new { chatId = ChatId });
            }
            return View();
        }
    }
}