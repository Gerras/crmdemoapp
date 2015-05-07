using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CrmDemoApp.Startup))]
namespace CrmDemoApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
