namespace Okta.Clients.OpenIdConnect
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.IdentityModel.Protocols;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Notifications;
    using Microsoft.Owin.Security.OpenIdConnect;
    using Owin;

    /// <summary>
    /// StartUp class
    /// </summary>
    public partial class Startup
    {
        private static string authorizationServer = ConfigurationManager.AppSettings["ida:AuthorizationServer"];
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string clientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        /// <summary>
        /// Configure Authentication
        /// </summary>
        /// <param name="app">The application</param>
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
            new OpenIdConnectAuthenticationOptions
            {
                // The Client ID is used by the application to uniquely identify itself to Okta.
                ClientId = clientId,
                ClientSecret = clientSecret,
                Authority = authorizationServer,
                RedirectUri = redirectUri,

                // Okta optional parameter: Callback location to redirect to after the logout has 
                // been performed. It must match the value preregistered in Okta during client 
                // registration.
                // This request initiates a logout and will redirect to the post_logout_redirect_uri on success.
                // If omit parameter, This request initiates a logout and will redirect to the Okta login page on success.
                PostLogoutRedirectUri = redirectUri,

                // Requesting the following scopes from the Authorization Server
                Scope = "openid profile",

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    RedirectToIdentityProvider = this.OnRedirectToIdentityProvider,
                    AuthenticationFailed = this.OnAuthenticationFailed,
                    SecurityTokenValidated = this.OnSecurityTokenValidated,
                }
            });
        }

        /// <summary>
        /// Invoked to manipulate redirects to the identity provider for SignIn, SignOut, or Challenge.
        /// </summary>
        /// <param name="notification">Notification context</param>
        /// <returns>OWIN OpenId Connect Response</returns>
        private Task OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            if (notification.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.AuthenticationRequest)
            {
                // Need to add a custom query string parameter to the authentication request so can request which
                // identity provider we want to use in Okta. Custom Brand Login Page.
                string idp = IdentityProviderManagement.GetIndentityProvider();
                if (!string.IsNullOrWhiteSpace(idp))
                {
                    notification.ProtocolMessage.Parameters.Add("idp", idp);
                }
            }
            else if (notification.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
            {
                // Okta REQUIRED parameter: A valid ID token with a subject matching the current session.
                if (notification.OwinContext.Authentication.User.FindFirst("id_token") != null)
                {
                    notification.ProtocolMessage.IdTokenHint = notification.OwinContext.Authentication.User.FindFirst("id_token").Value;
                }
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Notification handler for exceptions thrown during request processing
        /// </summary>
        /// <param name="notification">Notification context</param>
        /// <returns>Redirect to Error view</returns>
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            // initialize logging values
            System.Guid activityID = System.Guid.NewGuid();

            // Report the error to the user
            string errorMessage = string.Empty;
            if (notification.ProtocolMessage != null)
            {
                errorMessage = notification.ProtocolMessage.Error + "   ErrorDescription: " + notification.ProtocolMessage.ErrorDescription;
            }
            else
            {
                errorMessage = "Please re-try your action. If you continue to get this error, please contact the Administrator.";
            }

            // Discontinue all processing of this Request and report the error
            notification.HandleResponse();

            Uri baseUri = new Uri("https://" + notification.Request.Host + notification.Request.PathBase.Value.TrimEnd('/') + '/');
            var errorUrl = new Uri(baseUri, string.Format("home/error?title={0}&message={1}&activityid={2}", HttpUtility.UrlEncode("Authentication Error"), HttpUtility.UrlEncode(errorMessage), HttpUtility.UrlEncode(activityID.ToString())));

            notification.Response.Redirect(errorUrl.ToString());
            return Task.FromResult(0);
        }

        /// <summary>
        /// Invoked after the security token has passed validation and a ClaimsIdentity has been generated.
        /// </summary>
        /// <param name="notification">Notification context</param>
        /// <returns>OWIN OpenId Connect Response</returns>
        private Task OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
           // For Logout Request Okta requires idTokenHint so must maintain state for id_token
           // This should NOT be added to user's principal claims. For DEMO purposes only.
           // Okta REQUIRED parameter: A valid ID token with a subject matching the current session.
           notification.AuthenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("id_token", notification.ProtocolMessage.IdToken));

            return Task.FromResult(0);
        }
    }
}