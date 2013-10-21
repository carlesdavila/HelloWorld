using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using System.Web.Configuration;
using CloudDataAnalytics.IdP.Helpers;
using CloudDataAnalytics.IdP.Models;
using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Profiles.SSOBrowser;
using ComponentSpace.SAML2.Protocols;

namespace CloudDataAnalytics.IdP.Controllers
{
    //[ValidateInput(false)]
    public class SSOController : Controller
    {
        private IEnumerable<LoginEntry> _logins;

        public SSOController()
        {
            //var client = getClient();

            //var response = client.GetAsync("api/userlogin").Result;  // Blocking call!
            //if (response.IsSuccessStatusCode)
            //{
            //     Parse the response body. Blocking!
            //    _logins = response.Content.ReadAsAsync<IEnumerable<UserLoginDto>>().Result.Distinct()
            //                      .Where(l => l.IsAdmin)
            //                      .Select(l => new LoginEntry
            //                          {
            //                              id = l.Login,
            //                              name = l.Login,
            //                              password = l.Login
            //                          });


            //}
            //else
            //{
            //    throw new HttpException("Eror getting user list");
            //}
            _logins = new LoginEntry[]{
                    new LoginEntry() {
                                          id = "idCarlos",
                                          name = "loginCarlos",
                                          password = "123123123"
                                      }, 
                                      new LoginEntry()
                                          {
                                            id = "cdavila",
                                          name = "Carlos Davila",
                                          password = "123123123"
                                          }};
        }

        //HttpClient getClient()
        //{
        //    var client = new HttpClient
        //        {
        //            BaseAddress = new Uri(WebConfigurationManager.AppSettings["CloudDataAnalitycsUrl"])
        //        };

        //    // Add an Accept header for JSON format.
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));

        //    return client;
        //}

        #region  Actions


        // GET: /SSO/
        [Authorize]
        public void Index(string SAMLRequest, string RelayState)
        {
            AuthnRequest authnRequest;
            string relayState;
            XmlElement authRequestBaseXml;

            //Recieve Authentication request from SP
            ReceiveAuthnRequest(out authnRequest, out relayState, out authRequestBaseXml);
            if (authnRequest == null) return;

            //We are already IDP logged and trust SP: Create a SAML response with the user's local identity.
            SAMLResponse samlResponse = CreateSAMLResponse(authnRequest.AssertionConsumerServiceURL);

            // Send the SAML response to the service provider with custom parameters from AuthRequest.
            SendSAMLResponse(samlResponse, relayState, authnRequest.AssertionConsumerServiceURL);

        }


        //
        // GET: /SSO/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Logins = string.Join(", ", _logins.Select(l => l.id.ToLowerInvariant()));
            return View();
        }


        //
        // POST: /SSO/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginRequest model, string returnUrl)
        {
            string usernameCookie = string.Empty;
            if (ModelState.IsValid && CustomAuthenticate(model.UserName, model.Password, out usernameCookie))
            {
                FormsAuthentication.SetAuthCookie(usernameCookie, model.RememberMe);
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Logins = string.Join(", ", _logins.Select(l => l.id.ToLowerInvariant()));
            ViewBag.ErrorMessage = "Bad username or password";
            return View(model);
        }




        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "SSO");
            }
        }

        #endregion

        #region Methods

        private void ReceiveAuthnRequest(out AuthnRequest authnRequest, out string relayState, out XmlElement authRequestBaseXml)
        {
            AsymmetricAlgorithm certPublicKey = null;
            XmlElement authnRequestXml;
            bool signed;

            //Decode SAML request to get Inner Text wich has information of SP
            var saml = SamlDecoder.DecodeGetRequest(System.Web.HttpContext.Current.Request);
            authRequestBaseXml = saml;
            var innerText = saml.InnerText;
            //we would use innerText to select correct CER of SP but in our case we only accept certificates from NTRCloud Idp acting as SP
            X509Certificate2 certificate = (X509Certificate2)System.Web.HttpContext.Current.Application["spCer"];

            certPublicKey = certificate.PublicKey.Key;
            IdentityProvider.ReceiveAuthnRequestByHTTPRedirect(System.Web.HttpContext.Current.Request, out authnRequestXml, out relayState, out signed, certPublicKey);

            if (SAMLMessageSignature.IsSigned(authnRequestXml))
            {
                if (certificate == null)
                    throw new ArgumentException("Unable to retrieve vertificate to validate signed request.");
                if (!SAMLMessageSignature.Verify(authnRequestXml, certificate))
                    throw new ArgumentException("The authentication request signature failed to verify.");
            }
            authnRequest = new AuthnRequest(authnRequestXml);
            return;
        }

        private SAMLResponse CreateSAMLResponse(string AssertionConsumerServiceURL)
        {
            SAMLResponse samlResponse = new SAMLResponse();
            samlResponse.Destination = AssertionConsumerServiceURL;

            //Create nameID Issuer for the assertion without query string
            var urlIssuer = Request.Url.AbsoluteUri;
            if (urlIssuer.IndexOf('?') >= 0)
                urlIssuer = urlIssuer.Remove(urlIssuer.IndexOf('?'));

            Issuer issuer = new Issuer(urlIssuer);
            samlResponse.Issuer = issuer;
            samlResponse.Status = new Status(SAMLIdentifiers.PrimaryStatusCodes.Success, null);

            SAMLAssertion samlAssertion = new SAMLAssertion();
            samlAssertion.Issuer = issuer;

            //We use the identity name of idp logged user
            Subject subject = new Subject(new NameID(User.Identity.Name));
            SubjectConfirmation subjectConfirmation = new SubjectConfirmation(SAMLIdentifiers.SubjectConfirmationMethods.Bearer);
            SubjectConfirmationData subjectConfirmationData = new SubjectConfirmationData();
            subjectConfirmationData.Recipient = AssertionConsumerServiceURL;
            subjectConfirmation.SubjectConfirmationData = subjectConfirmationData;
            subject.SubjectConfirmations.Add(subjectConfirmation);
            samlAssertion.Subject = subject;

            AuthnStatement authnStatement = new AuthnStatement();
            authnStatement.AuthnContext = new AuthnContext();
            authnStatement.AuthnContext.AuthnContextClassRef = new AuthnContextClassRef(SAMLIdentifiers.AuthnContextClasses.Password);
            samlAssertion.Statements.Add(authnStatement);

            samlResponse.Assertions.Add(samlAssertion);

            return samlResponse;
        }

        /// <summary>
        ///  Send the SAML response to the SP.
        /// </summary>
        /// <param name="samlResponse"></param>
        /// <param name="relayState"></param>
        /// <param name="AssertionConsumerServiceURL"></param>
        private void SendSAMLResponse(SAMLResponse samlResponse, string relayState, string AssertionConsumerServiceURL)
        {
            // Serialize the SAML response for transmission.
            XmlElement samlResponseXml = samlResponse.ToXml();

            // Sign the SAML response.
            X509Certificate2 x509Certificate = (X509Certificate2)System.Web.HttpContext.Current.Application["idpCer"];
            SAMLMessageSignature.Generate(samlResponseXml, x509Certificate.PrivateKey, x509Certificate);

            IdentityProvider.SendSAMLResponseByHTTPPost(System.Web.HttpContext.Current.Response, AssertionConsumerServiceURL, samlResponseXml, relayState);
        }

        /// <summary>
        /// Method to authenticate users from CustomUsers parameters from web.config
        /// </summary>
        /// <param name="username">User to check</param>
        /// <param name="password">password</param>
        /// <param name="fakeName">output name to set in cookie</param>
        /// <returns>true if authenticated</returns>
        private bool CustomAuthenticate(string username, string password, out string fakeName)
        {

            //var hashPass = sha1(string.Format("{0}:{1}", username.ToLowerInvariant(), password));

            var ret = _logins
                .FirstOrDefault(l =>
                                l.id.ToLowerInvariant() == username.ToLowerInvariant()
                                &&
                                l.password.ToLowerInvariant() == password.ToLowerInvariant());

            //var ret = _logins.FirstOrDefault(l => l.ToLowerInvariant() == username.ToLowerInvariant());

            fakeName = ret != null
                           ? ret.name
                           : string.Empty;
            return ret != null;
        }

        //private string sha1(string str)
        //{
        //    var data = Encoding.Default.GetBytes(str);
        //    var sha = new SHA1CryptoServiceProvider();
        //    var result = sha.ComputeHash(data);
        //    return BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();
        //}

        #endregion


    }
}
