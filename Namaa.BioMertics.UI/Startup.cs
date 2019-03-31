using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Namaa.BioMertics.UI.Startup))]
namespace Namaa.BioMertics.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
