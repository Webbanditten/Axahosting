using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AxaHosting.Model;
using Newtonsoft.Json;

namespace AxaHosting.Web.utils
{
    public class FormAuthUtil
    {
        public static HttpCookie CreateCookie(User user)
        {
            var ticket = new FormsAuthenticationTicket(
                1, // ticket version
                user.Username,                              // authenticated username
                DateTime.Now,                          // issueDate
                DateTime.Now.AddMinutes(120),           // expiryDate
                true,                          // true to persist across browser sessions
                JsonConvert.SerializeObject(user),                              // can be used to store additional user data
                FormsAuthentication.FormsCookiePath
            );  // the path for the cookie

            // Encrypt the ticket using the machine key
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // Add the cookie to the request to save it
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true
            };
            return cookie;
        }
        public static User GetUser(HttpCookieCollection cookies)
        {
            var authCookie = cookies[FormsAuthentication.FormsCookieName];
            if (authCookie?.Value != null)
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null)
                {
                    var usr = (User) JsonConvert.DeserializeObject(ticket.UserData, typeof (User));
                    return usr;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }
    }
}