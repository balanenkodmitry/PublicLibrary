using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PublicLibrary.Web.Startup))]
namespace PublicLibrary.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
