namespace Okta.Clients.OpenIdConnect
{
    using System.Configuration;

    public static class IdentityProviderManagement
    {
        private static string idpBlueBrand = ConfigurationManager.AppSettings["idp:BlueBrand"];
        private static string idpGreenBrand = ConfigurationManager.AppSettings["idp:GreenBrand"];

        /// <summary>
        /// This fields represents the State key for the data item Brand
        /// </summary>
        internal const string BrandKey = "RFG.BTT.Brand";

        /// <summary>
        /// Gets or sets the Brand for the logged in user in memory. 
        /// </summary>
        public static string Brand
        {
            // Not using Session because Owin Middleware Session becomes null
            // Using Items instead but Items only survives for the current request
            // and not across requests. Only for DEMO purposes.
            get { return System.Web.HttpContext.Current.Items[BrandKey] == null ? "BLUE" : (string)System.Web.HttpContext.Current.Items[BrandKey]; }
            set { System.Web.HttpContext.Current.Items[BrandKey] = value; }
        }

        public static string GetIndentityProvider()
        {
            // Default Identity Provider is Blue
            string identityProvider = idpBlueBrand;

            if (!string.IsNullOrWhiteSpace(Brand))
            {
                switch (Brand.ToUpper())
                {
                    case "BLUE":
                        identityProvider = idpBlueBrand;
                        break;
                    case "GREEN":
                        identityProvider = idpGreenBrand;
                        break;
                    case "OKTA":
                        identityProvider = null;
                        break;
                }
            }

            return identityProvider;
        }
    }
}