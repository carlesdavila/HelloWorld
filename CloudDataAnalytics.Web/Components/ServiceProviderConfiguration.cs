using System.Configuration;

namespace CloudDataAnalytics.Web.Components
{
    public interface IServiceProviderConfiguration
    {
        string LoginUrl { get; }
        string AssertionConsumerUrl { get; }
        string ServiceProviderUrl { get; }
        string LogoutUrl { get; }
    }

    public class ServiceProviderConfiguration : ConfigurationSection, IServiceProviderConfiguration
    {
        [ConfigurationProperty("LoginUrl")]
        public string LoginUrl
        {
            get { return (string) this["LoginUrl"]; }
            set { this["LoginUrl"] = value; }
        }

        [ConfigurationProperty("AssertionConsumerUrl")]
        public string AssertionConsumerUrl
        {
            get { return (string) this["AssertionConsumerUrl"]; }
            set { this["AssertionConsumerUrl"] = value; }
        }

        [ConfigurationProperty("ServiceProviderUrl")]
        public string ServiceProviderUrl
        {
            get { return (string)this["ServiceProviderUrl"]; }
            set { this["ServiceProviderUrl"] = value; }
        }

        [ConfigurationProperty("LogoutUrl")]
        public string LogoutUrl
        {
            get { return (string)this["LogoutUrl"]; }
            set { this["LogoutUrl"] = value; }
        }
    }
}