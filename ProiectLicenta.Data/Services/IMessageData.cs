using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface IMessageData
    {
        void addMessage(Message message, int chatId);

        Message getMessage(int messageId);

        void EditMessage(Message message);

        void DeleteMessage(int messageId);
    }
}
