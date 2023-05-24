using Proiect_Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;

namespace Proiect_Licenta.Services
{
    public interface IRoleData
    {
        void ChangeRole(string userId, string role);

         void makeStudent(string userId);

        
    }
}