/************************************************************************************ 
    Login Page - helper functions 
*************************************************************************************/
// Variables
var brand = '';
var errorMessages = {};
var brandCodeList = ['blue', 'green']; // Valid brand codes list
var redirectUrl = '';   // Okta Sign-in Widget - Redirect User after successful authentication
var oktaSignIn;
var orgUrl = '';        // Okta Sign-in Widget - Okta URL for user authentication
var userPreferredLanguage = 'en';
var urlHostName = '';

// Error Messages
errorMessages['invalid_parameter'] = '<h1 id="message-h1">Error</h1><p id="message-p1">Invalid or missing required parameter.</p>';

// Initialize Okta Login page variables
function init(brandCode, url, idaAuthority, ulang) {
  
    // validate all the required paramters
    if (brandCode && url && idaAuthority && ulang &&
        brandCodeList.indexOf(brandCode)>-1) {

        // initialize the variables
        redirectUrl = url;
        orgUrl = idaAuthority;
        userPreferredLanguage = ulang;
        
        // setup Okta Widget
        oktaSignIn = setupOktaSignIn(orgUrl, userPreferredLanguage);

        // render Okta Widget UI
        oktaSignIn.renderEl(
          { el: '#okta-login-container' },
          function (res) {
              if (res.status === 'SUCCESS') {
                  //console.log('User %s successfully authenticated %o', res.user.profile.login, res.user);
                  res.session.setCookieAndRedirect(redirectUrl);
              }
          }
        );

        return true;
    } else {
        // show error if paramter is invalid
        displayError(errorMessages['invalid_parameter']);
    }
}

// Display Error Page
function displayError(message) {

    // create message container
    var msgContainer = document.createElement("div");
    msgContainer.setAttribute("id", "message-container-center");
    msgContainer.innerHTML = message;

    // Add application logo container to brand-info container
    document.body.innerHTML = '';
    document.body.setAttribute("class", "body-message");
    document.body.appendChild(msgContainer);
    
}

/************************************************************************************ 
    OKTA Sign-in Widget functions 
*************************************************************************************/
//var oktaSignIn = setupOktaSignIn(orgUrl);

function setupOktaSignIn(baseUrl, userLang) {

    return new OktaSignIn({
        baseUrl: baseUrl,
        features: {
            rememberMe: true,
            smsRecovery: true,
            selfServiceUnlock: true
        },
        helpLinks: {
            help: 'https://github.com/markmilchuk'
            /*,custom: [
             { text: 'Custom Link', href: '' }
            ]*/
        },
        language: userLang
        /*i18n: {},
        labels: {
            'primaryauth.title': '',
            'primaryauth.submit': '',            
        }*/
    });
}

/************************************************************************************/    

