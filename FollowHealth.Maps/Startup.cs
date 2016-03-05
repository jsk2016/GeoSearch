using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FollowHealth.Maps.Startup))]
namespace FollowHealth.Maps
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
