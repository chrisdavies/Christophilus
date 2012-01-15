namespace Christophilus.Controllers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Christophilus.Extensions;
    using DotNetOpenAuth.Messaging;
    using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
    using DotNetOpenAuth.OpenId.RelyingParty;
    using Christophilus.Models;

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
            if (response.Status != AuthenticationStatus.Authenticated)
            {
                throw new HttpException(
                    (int)HttpStatusCode.Unauthorized,
                    "The open id provider failed to authenticate ({0}).".Formatted(response.Status));
            }

            SetAuthCookie(response);

            return RedirectToRequestedUrl(returnUrl);
        }

        private void SetAuthCookie(IAuthenticationResponse response)
        {
            string email = FetchEmail(response);
            FormsAuthentication.SetAuthCookie(email, false);
        }

        private ActionResult RedirectToRequestedUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToRoute("Entries.Edit", new { day = DateTime.Now.ToString(DataStore.DateFormat) });
            }
            else
            {
                return Redirect(Request.QueryString["ReturnUrl"]);
            }
        }

        private static string FetchEmail(IAuthenticationResponse response)
        {
            var fetch = response.GetExtension<FetchResponse>();

            string email = string.Empty;
            if (fetch != null)
            {
                email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidOperationException("Email is required, but was not supplied by the OpenID provider.");
            }

            return email;
        }
    }
}
