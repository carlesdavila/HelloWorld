using System.Web;
using System.Xml;
using ComponentSpace.SAML2.Bindings;

namespace CloudDataAnalytics.IdP.Helpers
{
    public static class SamlDecoder
    {
        public static XmlElement DecodeGetRequest(HttpRequest encodedSaml)
        {
            string algorithm;
            string signature;
            XmlElement saml;
            string relayState;
            HTTPRedirectBinding.ReceiveRequest(encodedSaml, out saml, out relayState, out algorithm, out signature);            
            return saml;
        }
        public static XmlElement DecodePostResponse(HttpRequest encodedSaml)
        {
            XmlElement saml;
            string relayState;
            HTTPPostBinding.ReceiveResponse(encodedSaml, out saml, out relayState);
            return saml;
        }
    }
}