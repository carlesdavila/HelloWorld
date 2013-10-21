using CloudDataAnalytics.Web.Components;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Profiles.SingleLogout;

using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Web.Security;

namespace CloudDataAnalytics.Web.Controllers
{
    public class LogoutController : Controller
    {
        private IServiceProviderConfiguration _spCfg;

        public LogoutController(IServiceProviderConfiguration spCfg)
        {
            _spCfg = spCfg;
        }

        [Authorize]
        public void Logout()
        {
            FormsAuthentication.SignOut();
            RequestLogout();
        }

        public void RequestLogout()
        {
            var context = System.Web.HttpContext.Current;
            var issuerUrl = new Uri(context.Request.Url, "./").ToString();

            var logoutRequest = new ComponentSpace.SAML2.Protocols.LogoutRequest
                {
                    Issuer = new Issuer(issuerUrl),
                    Destination = _spCfg.ServiceProviderUrl
                };

            var logoutRequestXml = logoutRequest.ToXml();
            var logoutUrl = _spCfg.LogoutUrl;

            var x509Certificate = (X509Certificate2) System.Web.HttpContext.Current.Application["spCer"];

            SingleLogoutService
                .SendLogoutRequestByHTTPRedirect(System.Web.HttpContext.Current.Response, logoutUrl,
                                                 logoutRequestXml, null,
                                                 x509Certificate.PrivateKey);


            FormsAuthentication.SignOut();
        }

    }
}
