using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Squash_Template.Startup))]
namespace Squash_Template
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
