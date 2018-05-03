namespace Okta.Clients.OpenIdConnect.Controllers
{
    using System.Web.Mvc;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Error(string title, string message, string activityid)
        {
            ViewBag.Title = title;
            ViewBag.ErrorMessage = message;
            ViewBag.ActivityID = activityid;

            return this.View("Error");
        }
    }
}