namespace OktaLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Resources;

    /// <summary>
    /// Sign-in Controller handles login requests (redirected from Okta) 
    /// It must be configured in Okta for each brand Identity Provider 
    /// (Okta --> Security --> IdentityProviders)
    /// </summary>
    public class SignInController : Controller
    {
        /// <summary>
        /// Okta will post this parameter to Login page
        /// </summary>
        private const string RelayState = "RelayState";

        /// <summary>
        /// Brand Code List
        /// </summary>
        private string[] brandCodeArray = { "blue", "green" };

        #region Action Methods

        /// <summary>
        /// This action method returns Okta Multi-branded Login Page/View
        /// It must be configured in Okta for each brand Identity Provider 
        /// (Okta --> Security --> IdentityProviders)
        /// Also, add CORS settings for login page domain (Okta --> Security --> API --> TrustedOrigins)
        /// Brand code will be read from hostname or will be passed as a parameter.
        /// </summary>
        /// <param name="brand">Brand code</param>
        /// <returns>Action Result</returns>
        public ActionResult Index(string brand)
        {
            // initialize variables and log properties
            System.Guid activityID = System.Guid.NewGuid();
            Dictionary<string, object> logProperties = new Dictionary<string, object>();

            try
            {   
                // Read Okta RelayState parameter (Form POST parameter)
                string relayState = Request[RelayState];

                // Read Brand Code from URL hostname or from query string parameter
                string brandCode = this.GetBrandCode(brand);

                // Make sure required parameters are provided and brand code is valid  
                if (!string.IsNullOrWhiteSpace(relayState) && 
                    !string.IsNullOrWhiteSpace(brandCode) &&
                    this.IsValidBrandCode(brandCode))
                {
                    // Decode the RelayState or it will get a 404 when we redirect
                    string decodedRelayState = Uri.UnescapeDataString(relayState);                    

                    // Add Brand information to ViewBag
                    ViewBag.RedirectUrl = Uri.UnescapeDataString(decodedRelayState);                    
                    ViewBag.Brand = brandCode;
                    ViewBag.IdaAuthority = ConfigurationManager.AppSettings["ida:Authority"];
                    this.AddBrandInfoToViewBagFromResourceFile(brandCode);
                    this.AddBrowserInfoToViewBagFromResourceFile();
                }                
                else
                {
                    // If brand or RelayState is missing or invalid then show error message and record in logs 
                    ViewBag.ErrorMessage = GlobalResource.Error_RequiredParamterIsMissingOrInvalid;
                    ViewBag.ActivityID = activityID;
                    return this.View("Error");
                }

                // Show branded login page                
                return this.View();
            }
            catch (System.Exception)
            {
                // Show error message in case of Exception and record in logs
                ViewBag.ActivityID = activityID;
                ViewBag.ErrorMessage = GlobalResource.Error_SystemException;
                return this.View("Error");
            }
        }

        #endregion

        #region private methods
        /// <summary>
        /// Validate Brand Code parameter
        /// </summary>
        /// <param name="brandCode">Brand code</param>
        /// <returns>It returns true if brand code is valid otherwise returns false</returns>
        private bool IsValidBrandCode(string brandCode)
        {            
            if (!string.IsNullOrWhiteSpace(brandCode))
            {
                // Search brand code in predefined brand code list 
                int index = Array.IndexOf(this.brandCodeArray, brandCode.ToLower());
                if (index >= 0)
                {
                    return true;
                }
            }

            return false;
        }        

        /// <summary>
        /// Add Login page information to View Bag
        /// </summary>
        /// <param name="brand">Brand code</param>
        private void AddBrandInfoToViewBagFromResourceFile(string brand)
        {
            ViewBag.Title = this.GetResourceString(brand, "_Title");
            ViewBag.TitleSmall = this.GetResourceString(brand, "_TitleShort");
            ViewBag.Name = this.GetResourceString(brand, "_Name");
            ViewBag.NameShort = this.GetResourceString(brand, "_NameShort");
            ViewBag.Description = this.GetResourceString(brand, "_Description");
            ViewBag.Logo = this.GetResourceString(brand, "_Logo");
            ViewBag.Copyright = DateTime.Now.Year + " " + this.GetResourceString(brand, "_Copyright");
            ViewBag.HelpDesk = this.GetResourceString(brand, "_Helpdesk");
            ViewBag.Disclaimer = this.GetResourceString(brand, "_Disclaimer");
        }

        /// <summary>
        /// Add User current browser settings information to View Bag
        /// </summary>
        private void AddBrowserInfoToViewBagFromResourceFile()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture;
            ViewBag.UserPreferredLang = cultureInfo.Name;
            ViewBag.UrlHostname = Request.Url.Host;
        }        

        /// <summary>
        /// Read resource value from Global Resources
        /// </summary>
        /// <param name="brand">Brand Code</param>
        /// <param name="resourceName">Resource Name</param>
        /// <returns>Resource value</returns>
        private string GetResourceString(string brand, string resourceName)
        {
            return GlobalResource.ResourceManager.GetString(brand + resourceName);                       
        }

        /// <summary>
        /// Get brand code from URL hostname or Query String parameter
        /// </summary>
        /// <param name="brandParam">Brand Parameter</param>
        /// <returns>Brand Code</returns>
        private string GetBrandCode(string brandParam)
        {
            string hostName = string.Empty;

            if ((Request.Url != null) && !string.IsNullOrWhiteSpace(Request.Url.Host))
            {
                // Get hostname from Request URL
                hostName = Request.Url.Host.ToLower();
            }
            
            // Return brand code based on Url Host name
            if (hostName.Contains("bluebrand.com"))
            {
                return "blue";
            }
            else if (hostName.Contains("greenbrand.com"))
            {
                return "green";
            }                        
            else if (!string.IsNullOrWhiteSpace(brandParam))
            {
                // If brand parameter is provided then returns it as BrandCode
                return brandParam.ToLower();
            }

            return string.Empty;
        }
        
        #endregion
    }
}