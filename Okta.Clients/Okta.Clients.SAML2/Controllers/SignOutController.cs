namespace Okta.Clients.SAML2.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;

    [AllowAnonymous]
    public class SignOutController : Controller
    {
        public void Index()
        {
            // Send an SAML2 sign-out request.
            // Send ReplyUrl to the SignOut request so IDP can use it in its processing
            string replyUrl = this.Url.Action("Index", "Home");
            HttpContext.GetOwinContext().Authentication.SignOut(new AuthenticationProperties { RedirectUri = replyUrl }, "Saml2", CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}