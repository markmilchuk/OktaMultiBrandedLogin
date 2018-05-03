namespace Okta.Clients.SAML2.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Owin.Security;

    [AllowAnonymous]
    public class SignInController : Controller
    {
        public ActionResult Link(string brand)
        {
            try
            {
                // Record the user selected Brand so we know which IDP to use in the SelectIdentityProvider Notification Event
                IdentityProviderManagement.Brand = brand;

                // Send an SAML2 sign-in request.
                // To use a specified idp, the entity id of the idp should be entered in the Owin environment dictionary under the key "saml2.idp"
                // Alternate way to assign IDP
                // HttpContext.GetOwinContext().Environment.Add("saml2.idp", new EntityId(idp));

                // Pass the ReplyUrl to the SignIn request so it can use it in its processing
                string replyUrl = this.Url.Action("Index", "UserProfile");

                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = replyUrl }, "Saml2");

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