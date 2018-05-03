namespace Okta.Clients.SAML2
{
    using System.IdentityModel.Metadata;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using Sustainsys.Saml2;
    using Sustainsys.Saml2.Configuration;
    using Sustainsys.Saml2.Owin;

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            // Forces redirect to default login page if unauthenticated user directly tries to access protected page
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/SignIn/Link"),
            });

            // Load IDP and Options from web.config
            var saml2Options = new Saml2AuthenticationOptions(true);

            saml2Options.Notifications = new Saml2Notifications
            {
                // Notification called when the SignIn command is about to select what Idp to use for the request. 
                // To select a specicic IdentityProvider simply return it. 
                // Return null to fall back to built in selection.
                SelectIdentityProvider = (entityid, relaydata) =>
                {
                    IdentityProvider idp = null;

                    // Get the user selected Brand IDP so can use Branded Login Page
                    saml2Options.IdentityProviders.TryGetValue(new EntityId(IdentityProviderManagement.GetIndentityProvider()), out idp);

                    return idp;
                }
            };

            // Add SAML2 Authentication to the OWIN pipeline
            app.UseSaml2Authentication(saml2Options);
        }
    }
}