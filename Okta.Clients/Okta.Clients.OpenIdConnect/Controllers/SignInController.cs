namespace Okta.Clients.OpenIdConnect.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OpenIdConnect;

    [AllowAnonymous]
    public class SignInController : Controller
    {
        public ActionResult Link(string brand)
        {
            try
            {
                // Record the user selected Brand so we know which IDP to use in the OnRedirectToIdentityProvider Notification Event
                IdentityProviderManagement.Brand = brand;

                // Send an OpenID Connect sign-in request.
                // Pass the ReplyUrl to the SignIn request so it can use it in its processing
                string replyUrl = this.Url.Action("Index", "UserProfile");

                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = replyUrl }, OpenIdConnectAuthenticationDefaults.AuthenticationType);

                return new EmptyResult();
            }
            catch (System.Exception ex)
            {
                ViewBag.Title = "Error";
                ViewBag.ErrorMessage = ex.Message;
                return this.View("Error");
            }
        }
    }
}