namespace Christophilus.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using Christophilus.Extensions;
    using DotNetOpenAuth.Messaging;
    using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
    using DotNetOpenAuth.OpenId.RelyingParty;

    /// <summary>
    /// The controller which handles authentication for the entire app.
    /// </summary>
    public class AuthenticationController : Controller
    {
        public ActionResult Show()
        {
            return View();
        }

        /// <summary>
        /// The authentication logic for OpenId.
        /// </summary>
        /// <param name="returnUrl">
        /// The url which the user originally requested before being redirected
        /// through the login process.
        /// </param>
        /// <returns>The MVC action containing the response.</returns>
        [ValidateInput(false)]
        public ActionResult Login(string returnUrl)
        {
            using (var openid = new OpenIdRelyingParty())
            {
                var response = openid.GetResponse();
                if (response == null)
                {
                    return BeginAuthentication(openid);
                }
                else
                {
                    return EndAuthentication(response, returnUrl);
                }
            }
        }

        /// <summary>
        /// The action which logs the current user out of the system.
        /// </summary>
        /// <returns>The MVC action containing the response.</returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            string logoutUrl = "https://www.google.com/accounts/logout";
            return Redirect(logoutUrl);
        }

        /// <summary>
        /// Begins an open id authentication request.
        /// </summary>
        /// <param name="openid">The open id object to be used.</param>
        /// <returns>The MVC action containing the response.</returns>
        private ActionResult BeginAuthentication(OpenIdRelyingParty openid)
        {
            const string GoogleOpenAuthUrl = "https://www.google.com/accounts/o8/id";

            try
            {
                var request = openid.CreateRequest(GoogleOpenAuthUrl);
                var fetch = new FetchRequest();
                fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                request.AddExtension(fetch);
                return request.RedirectingResponse.AsActionResult();
            }
            catch (ProtocolException ex)
            {
                ModelState.AddDefaultError(ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Handles the incoming open id authentication response.
        /// </summary>
        /// <param name="response">The open id response.</param>
        /// <param name="returnUrl">
        /// The url which the user originally requested before being redirected
        /// through the login process.
        /// </param>
        /// <returns>The MVC action containing the response.</returns>
        private ActionResult EndAuthentication(IAuthenticationResponse response, string returnUrl)
        {
            if (response.Status == AuthenticationStatus.Authenticated)
            {
                var fetch = response.GetExtension<FetchResponse>();

                string email = string.Empty;
                if (fetch != null)
                {
                    email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                }

                if (string.IsNullOrEmpty(email))
                {
                    ModelState.AddDefaultError("A valid e-mail address could not be aquired from the OpenID provider.");
                    return View();
                }

                FormsAuthentication.SetAuthCookie(email, false);

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToRoute("Entries.Edit", new { day = DateTime.Now.ToString("yyyy-MM-dd") });
                }
                else
                {
                    return Redirect(Request.QueryString["ReturnUrl"]);
                }
            }

            ModelState.AddDefaultError(string.Format("Login failed with status: {0}", response.Status));
            return View();
        }
    }
}
