using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TesteWeb.Startup))]
namespace TesteWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
