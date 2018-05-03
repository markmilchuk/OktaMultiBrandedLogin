using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Okta.Clients.SAML2.Startup))]

namespace Okta.Clients.SAML2
{
    /// <summary>
    /// StartUp class
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configure the application
        /// </summary>
        /// <param name="app">The application</param>
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
