using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OTProyect.Startup))]
namespace OTProyect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
