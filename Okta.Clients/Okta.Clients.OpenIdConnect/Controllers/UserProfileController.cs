namespace Okta.Clients.OpenIdConnect.Controllers
{
    using System.Threading;
    using System.Web.Mvc;

    [Authorize]
    public class UserProfileController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ClaimsIdentity = Thread.CurrentPrincipal.Identity;
            return this.View();
        }
    }
}