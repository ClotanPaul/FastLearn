using Proiect_Licenta.Models;
using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface IWarningData
    {
        void WarnUser(int userId, string Reason);

        void RemoveWarning(int warningId);

        void SuspendUser(int userId, DateTime until);

        List<Warning> getUserWarnings(int userDataId);

        Warning getWarning(int warningId);

        void RemoveSuspension(int userDataId);
    }
}
