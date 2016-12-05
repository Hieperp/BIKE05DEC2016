using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCClient.Startup))]
namespace MVCClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
