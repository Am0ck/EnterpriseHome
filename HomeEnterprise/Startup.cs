using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HomeEnterprise.Startup))]
namespace HomeEnterprise
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
