using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Proiect_Licenta.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_Licenta.App_Start
{
    public class ContainerConfig
    {
        internal static void RegisterContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers((typeof(MvcApplication).Assembly));

            builder.RegisterType<SqlCourseData>()
                .As<ICourseData>()
                .InstancePerRequest();

            builder.RegisterType<SqlChapterData>()
                .As<IChapterData>()
                .InstancePerRequest();

            builder.RegisterType<SqlSubChapterData>()
                .As<ISubChapterData>()
                .InstancePerRequest();

            builder.RegisterType<SqlUserData>()
                .As<IUserData>()
                .InstancePerRequest();

            builder.RegisterType<SqlVideoData>()
                .As<IVideoData>()
                .InstancePerRequest();

            builder.RegisterType<SqlTestData>()
                .As<ITestData>()
                .InstancePerRequest();

            builder.RegisterType<SqlQuestionData>()
                .As<IQuestionData>()
                .InstancePerRequest();

            builder.RegisterType<SqlAnswerData>()
                .As<IAnswerData>()
                .InstancePerRequest();

            builder.RegisterType<SqlWarningData>()
                .As<IWarningData>()
                .InstancePerRequest();

            builder.RegisterType<SqlChapterFileData>()
                .As<ISubChapterFileData>()
                .InstancePerRequest();

            builder.RegisterType<SqlUserAnswerData>()
                .As<IUserAnswerData>().InstancePerRequest();

            builder.RegisterType<SqlCourseReviewData>()
                .As<ICourseReviewData>().InstancePerRequest();


            builder.RegisterType<ApplicationDataDbContext>().InstancePerRequest();

            //builder.RegisterType<Principal>();

            builder.RegisterType<ApplicationUser>()
                .As<IdentityUser>().InstancePerRequest();

            builder.RegisterType<ApplicationUser>()
                .As<IdentityUser>().InstancePerRequest();

            



            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}