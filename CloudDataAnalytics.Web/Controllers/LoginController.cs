using System.Linq;
using CloudDataAnalytics.Web.Components;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Xml;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Profiles.SSOBrowser;
using ComponentSpace.SAML2.Protocols;

namespace CloudDataAnalytics.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServiceProviderConfiguration _spCfg;

        public LoginController(IServiceProviderConfiguration spCfg)
        {
            _spCfg = spCfg;
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public void Index()
        {
            PerformRequest();
        }

        private void PerformRequest()
        {
            var authnRequestXml = CreateAuthnRequest();
            var spResourceUrl = this.Request.QueryString["ReturnUrl"];

            var x509Certificate = (X509Certificate2)System.Web.HttpContext.Current.Application["spCer"];
            ServiceProvider.SendAuthnRequestByHTTPRedirect(System.Web.HttpContext.Current.Response,
                                                           _spCfg.LoginUrl,
                                                           authnRequestXml,
                                                           Guid.NewGuid().ToString(),
                                                           x509Certificate.PrivateKey);

        }

        private XmlElement CreateAuthnRequest()
        {
            var issuerUrl = new Uri(new Uri(_spCfg.AssertionConsumerUrl), "./").ToString();
            var assertionConsumerServiceUrl = _spCfg.AssertionConsumerUrl;

            var authnRequest = new AuthnRequest
            {
                Destination = _spCfg.LoginUrl,
                Issuer = new Issuer(issuerUrl),
                ForceAuthn = false,
                NameIDPolicy = new NameIDPolicy(null, null, true),
                ProtocolBinding = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST",
                AssertionConsumerServiceURL = assertionConsumerServiceUrl
            };
            var authnRequestXml = authnRequest.ToXml();
            return authnRequestXml;
        }
    }
}