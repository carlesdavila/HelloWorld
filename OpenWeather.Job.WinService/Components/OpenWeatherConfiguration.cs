using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.Job.WinService.Components
{

    public interface IOpenWeatherConfiguration
    {
        string Appid { get; }
    }

    public class OpenWeatherConfiguration : ConfigurationSection, IOpenWeatherConfiguration
    {
        [ConfigurationProperty("Appid")]
        public string Appid
        {
            get { return (string)this["Appid"]; }
            set { this["Appid"] = value; }
        }
    }
}
