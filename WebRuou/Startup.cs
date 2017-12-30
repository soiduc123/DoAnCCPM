using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebRuou.Startup))]
namespace WebRuou
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
