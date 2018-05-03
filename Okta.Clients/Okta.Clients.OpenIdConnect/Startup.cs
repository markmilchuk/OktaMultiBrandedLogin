using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Okta.Clients.OpenIdConnect.Startup))]

namespace Okta.Clients.OpenIdConnect
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
