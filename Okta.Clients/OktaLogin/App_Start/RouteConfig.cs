namespace OktaLogin
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// ASP.NET introduced Routing to eliminate needs of mapping each URL with a physical file. 
    /// Routing enable us to define URL pattern that maps to the request handler
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Every MVC application must configure (register) at least one route, which is configured by MVC framework by default.
        /// </summary>
        /// <param name="routes">Routes Collection</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "SignIn", action = "Index", id = UrlParameter.Optional });
        }
    }
}
