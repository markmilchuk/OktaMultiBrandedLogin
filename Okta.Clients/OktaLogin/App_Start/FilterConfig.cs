namespace OktaLogin
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// ASP.NET MVC Filter is a custom class where you can write custom logic to execute before or after an action method executes.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// You can add custom filters to this list that should be executed on each request.
        /// </summary>
        /// <param name="filters">Custom Filters</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
