using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Squash.Startup))]
namespace Squash
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
