using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlMessageData : IMessageData
    {
        private readonly ApplicationDataDbContext db;
        private readonly IChatData chatDb;

        public SqlMessageData(ApplicationDataDbContext db, IChatData chatDb)
        {
            this.chatDb= chatDb;
            this.db= db;
        }
        public void addMessage(Message message, int chatId)
        {
            var chat = chatDb.getChatById(chatId);
            chat.ChatMessages.Add(message);
            db.SaveChanges();
        }

        public void DeleteMessage(int messageId)
        {

            var message = db.Mesages.FirstOrDefault(m => m.MessageId == messageId);
            if (message != null)
            {
                db.Mesages.Remove(message);
                db.SaveChanges();
            }
        }

        public void EditMessage(Message message)
        {
            if (message != null) {
                db.Mesages.AddOrUpdate(message);
                db.SaveChanges();
            }
        }

        public Message getMessage(int messageId)
        {
            var message = db.Mesages.FirstOrDefault(m => m.MessageId == messageId);
            return message;
        }
    }
}
