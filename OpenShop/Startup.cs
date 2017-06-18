using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenShop.Startup))]
namespace OpenShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
