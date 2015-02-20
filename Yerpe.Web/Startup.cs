using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Yerpe.Web.Startup))]
namespace Yerpe.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
