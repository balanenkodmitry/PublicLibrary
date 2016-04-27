using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibraryWeb.Startup))]
namespace LibraryWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
