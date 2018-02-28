using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Zhp.Awards.Web.Startup))]
namespace Zhp.Awards.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
