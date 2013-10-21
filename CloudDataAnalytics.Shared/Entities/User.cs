using System;

namespace CloudDataAnalytics.Shared.Entities
{
    public class User
    {
        public User()
        {
            UserId = Guid.NewGuid();
        }
        //Properties
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string SamAccountName { get; set; }
        public string TimeZoneInfoId { get; set; }
        public string Culture { get; set; }
        public bool IsAdmin { get; set; }

        //Calculated properties
        public TimeZoneInfo TimeZone
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.TimeZoneInfoId)
                    ? null : TimeZoneInfo.FindSystemTimeZoneById(this.TimeZoneInfoId);
            }
        }

    }
}