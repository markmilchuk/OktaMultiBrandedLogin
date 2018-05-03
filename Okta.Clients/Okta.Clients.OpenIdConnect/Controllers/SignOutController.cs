namespace Okta.Clients.OpenIdConnect.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OpenIdConnect;

    [AllowAnonymous]
    public class SignOutController : Controller
    {
        public void Index()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}