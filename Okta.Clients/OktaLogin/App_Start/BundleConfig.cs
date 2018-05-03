namespace OktaLogin
{
    using System.Web;
    using System.Web.Optimization;

    /// <summary>
    /// Bundling and minification improves load time by reducing the number of requests 
    /// to the server and reducing the size of requested assets (such as CSS and JavaScript.)
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// This method which is used to create, register and configure bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        public static void RegisterBundles(BundleCollection bundles)
        {          
            // bundle common JS files
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                      "~/Scripts/login_util.js"));

            // bundle common style files
            bundles.Add(new StyleBundle("~/assets/css").Include(
                      "~/assets/bootstrap.min.css",
                      "~/assets/font-awesome.css",
                      "~/assets/okta-sign-in.css",
                      "~/assets/Style.css"));
            
            // bundle brand specific style files 
            bundles.Add(new StyleBundle("~/assets/Style-Blue").Include("~/assets/Style-Blue.css"));
            bundles.Add(new StyleBundle("~/assets/Style-Green").Include("~/assets/Style-Green.css"));           
        }
    }
}
