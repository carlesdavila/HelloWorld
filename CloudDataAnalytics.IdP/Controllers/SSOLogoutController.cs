using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Profiles.SingleLogout;
using ComponentSpace.SAML2.Protocols;

namespace CloudDataAnalytics.IdP.Controllers
{
    public class SSOLogoutController : Controller
    {
        private readonly HttpContext context;

        public SSOLogoutController()
        {
            context = System.Web.HttpContext.Current;
        }

        #region Actions

        /// <summary>
        /// Main action of SSO
        /// </summary>
        /// <param name="SAMLRequest"></param>
        /// <param name="RelayState"></param>
        public void Index(string SAMLRequest, string RelayState)
        {
            XmlElement logoutMessage;
            string relayState;
            bool isRequest;
            bool signed;
            X509Certificate2 x509Certificate = (X509Certificate2)System.Web.HttpContext.Current.Application["spCer"];
            
            
            SingleLogoutService.ReceiveLogoutMessageByHTTPRedirect(context.Request, out logoutMessage, out relayState, out isRequest, out signed, x509Certificate.PublicKey.Key);

            if (isRequest)
            {
                ProcessLogoutRequest(new LogoutRequest(logoutMessage), relayState);
            }
            else
            {
                ProcessLogoutResponse(new LogoutRequest(logoutMessage), relayState);
            }
        }
        
        #endregion

        #region SAML Logout Methods

        private void ProcessLogoutResponse(LogoutRequest logoutRequest, string relayState)
        {
            Response.Redirect("~/", false);
        }

        private void ProcessLogoutRequest(LogoutRequest logoutRequest, string relayState)
        {
             FormsAuthentication.SignOut();

             var destination = logoutRequest.Destination;
             context.Response.Redirect(destination);
            
            //We dont implement single logout
            // LogoutResponse logoutResponse = CreateLogoutResponse();
           //  SendLogoutResponse(logoutResponse, null);
        }

        private LogoutResponse CreateLogoutResponse() 
        {
            var logoutResponse = new LogoutResponse();
            logoutResponse.Status = new Status(SAMLIdentifiers.PrimaryStatusCodes.Success, null);

            var urlIssuer = Request.Url.AbsoluteUri;
            if (urlIssuer.IndexOf('?') >= 0)
                urlIssuer = urlIssuer.Remove(urlIssuer.IndexOf('?'));

            logoutResponse.Issuer = new Issuer(urlIssuer);
            
            return logoutResponse;
        }

        ////Send the logout response.
        //private void SendLogoutResponse(LogoutResponse logoutResponse, string relayState )
        //{
        //    //Serialize the logout response for transmission.
        //    XmlElement logoutResponseXml = logoutResponse.ToXml();

        //    string spLogoutURL = "http://admindev/NTRCloud/IdPSLO";
        //    //Send the logout response over HTTP redirect.
        //    X509Certificate2 x509Certificate = (X509Certificate2)System.Web.HttpContext.Current.Application["idpCer"];
        //    SingleLogoutService.SendLogoutResponseByHTTPRedirect(context.Response,spLogoutURL , logoutResponseXml, null, x509Certificate.PrivateKey);
        //}

        #endregion  
    }
}
