using System;
using System.Web;
using System.Web.Security;
using CloudDataAnalytics.Shared.Entities;

namespace CloudDataAnalytics.Web.Components
{
    public class CookieHelper
    {
        public static HttpCookie SetAuthCookie(string userName, User user)
        {
            //todo: use user correctly

            //FormsAuthentication.SetAuthCookie(userName, false);
            var ticket = new FormsAuthenticationTicket(1,
                                                       userName,
                                                       DateTime.Now,
                                                       DateTime.Now.AddMinutes(30),
                                                       false,user.Login,
                                                       FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            return new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        }
    }
}