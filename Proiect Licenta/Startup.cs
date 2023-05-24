using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Proiect_Licenta.Startup))]
namespace Proiect_Licenta
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
