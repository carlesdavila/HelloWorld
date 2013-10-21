namespace CloudDataAnalytics.IdP.Models
{
    public class LoginEntry
    {
        public string id { get; set; }
        // this entry should be sha1(lowercase(username)+':'+plain_password)
        public string password { get; set; }
        public string name { get; set; }
    }
}
