namespace Okta.Clients.SAML2
{
    using System.Configuration;

    public static class IdentityProviderManagement
    {
        /// <summary>
        /// This fields represents the State key for the data item Brand
        /// </summary>
        internal const string BrandKey = "RFG.BTT.Brand";

        private static string idpBlueBrand = ConfigurationManager.AppSettings["idp:BlueBrand"];
        private static string idpGreenBrand = ConfigurationManager.AppSettings["idp:GreenBrand"];
        private static string idpOktaBrand = ConfigurationManager.AppSettings["idp:OktaBrand"];

        /// <summary>
        /// Gets or sets the Brand for the logged in user in memory. 
        /// </summary>
        public static string Brand
        {
            // Not using Session because Owin Middleware Session becomes null
            // Using Items instead but Items only survives for the current request
            // and not across requests. For DEMO purposes only.
            get { return System.Web.HttpContext.Current.Items[BrandKey] == null ? "BLUE" : (string)System.Web.HttpContext.Current.Items[BrandKey]; }
            set { System.Web.HttpContext.Current.Items[BrandKey] = value; }
        }

        public static string GetIndentityProvider()
        {
            // Default Identity Provider is Blue
            string identityProvider = string.Format("http://www.okta.com/{0}", idpBlueBrand);

            if (!string.IsNullOrWhiteSpace(Brand))
            {
                switch (Brand.ToUpper())
                {
                    case "BLUE":
                        identityProvider = string.Format("http://www.okta.com/{0}", idpBlueBrand);
                        break;
                    case "GREEN":
                        identityProvider = string.Format("http://www.okta.com/{0}", idpGreenBrand);
                        break;
                    case "OKTA":
                        identityProvider = string.Format("http://www.okta.com/{0}", idpOktaBrand);
                        break;
                }
            }

            return identityProvider;
        }
    }
}