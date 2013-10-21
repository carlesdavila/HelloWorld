using CloudDataAnalytics.Web.Components;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Profiles.SSOBrowser;
using ComponentSpace.SAML2.Protocols;
using ServMan.Domain.Interfaces;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;

namespace CloudDataAnalytics.Web.Controllers
{   
    /// <summary>
    /// Controller of Assertion Consumer when IdP acts like SP (multiple IdP)
    /// </summary>
    public class ACController : Controller
    {
        private IUserService _usrSvc;
        public ACController(IUserService usrSvc)
        {
            _usrSvc = usrSvc;
        }

        //
        // POST: /AC/  
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index()
        {
            SAMLResponse samlResponse;
            string sRelayState;

            CheckSamlResponse(out samlResponse, out sRelayState);

            if (samlResponse == null || !samlResponse.IsSuccess())
            {
                return ProcessErrorSamlResponse();
            }

            //Success SAML Response
            var samlAssertion = (SAMLAssertion)samlResponse.Assertions[0];
            var userName = samlAssertion.Subject.NameID.NameIdentifier;

            var usr = _usrSvc.GetByLogin(userName);

            //var usr = _usrSvc.GetPrincipalDto(userName);
            //if (usr == null)
            //    return ProcessErrorSamlResponse();

            Response.Cookies.Add(CookieHelper.SetAuthCookie(userName, usr));
            return Redirect("~/");
        }

        private void CheckSamlResponse(out SAMLResponse samlResponse, out string relayState)
        {
            XmlElement samlResponseXml;
            ServiceProvider.ReceiveSAMLResponseByHTTPPost(System.Web.HttpContext.Current.Request, out samlResponseXml, out relayState);

            samlResponse = new SAMLResponse(samlResponseXml);


            var customIdPCertificate = (X509Certificate2)System.Web.HttpContext.Current.Application["idpCer"];
            if (!SAMLMessageSignature.Verify(samlResponseXml, customIdPCertificate))
                throw new ArgumentException("The SAML response signature failed to verify.");

            samlResponse = new SAMLResponse(samlResponseXml);
        }

        private ActionResult ProcessErrorSamlResponse()
        {
            //ProcessErrorSamlResponse
            var reason = "Invalid username or password";

            return RedirectToRoute("Logout", new RouteValueDictionary { { "LogoutReason", reason } });
        }

    }
}
