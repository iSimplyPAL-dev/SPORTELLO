using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OPENgovSPORTELLO.Startup))]
namespace OPENgovSPORTELLO
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
