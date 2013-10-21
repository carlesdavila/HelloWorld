using System;
using System.Web;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.Configuration;
using System.Web.Hosting;

namespace CloudDataAnalytics.IdP.App_Start
{
    public static class CertificateConfig
    {

        public static void RegisterCertificates(HttpApplicationState Application)
        {
            //Load the IdP certificate
            var idpCertificatePass = WebConfigurationManager.AppSettings["CertificatePassword"];
            var fileNameIdp = HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["PrivateCertificate"]);

            Application["idpCer"] = LoadCertificate(fileNameIdp, idpCertificatePass);

            //Load the SP certificate
            var fileNameSP = HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["PublicCertificateSP"]);
            Application["spCer"] = LoadCertificate(fileNameSP, null);
        }

        private static X509Certificate2 LoadCertificate(string filename, string password)
        {
            X509Certificate2 certificate;

            if (!File.Exists(filename)) {
                throw new ArgumentException("The certificate file " + filename + " doesn't exist.");
            }
            try {
                certificate = new X509Certificate2(filename, password);
            }
            catch (Exception e)
            {
                throw new ArgumentException("The certificate file " + filename + " couldn't be loaded - " + e.Message);
            }

            return certificate;
        }
    }
}