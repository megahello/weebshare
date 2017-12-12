using Microsoft.AspNetCore.Mvc;
using baka.Models;
using baka.Models.Entity;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace baka.Controllers
{
    public class BakaController : Controller
    {
        [NonAction]
        internal AuthModel Authorize(string type)
        {
            AuthModel model = new AuthModel();

            var token = Request.Headers["baka_token"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(token))
                return NullModel();

            using (var context = new BakaContext())
            {
                var usr = context.Users.FirstOrDefault(u => u.Token == token);
                if (usr == null)
                    return NullModel();

                if (usr.Disabled)
                {
                    model.Authorized = false;
                    model.Reason = "Your account has been disabled";
                }
                else if(usr.Deleted)
                {
                    model.Reason = "Invalid token";
                }
                else if (usr.AccountType != type)
                {
                    model.Authorized = false;
                    model.Reason = "Missing permissions";
                }
                else model.Authorized = true;

                model.User = usr;

                return model;
            }
        }

        [NonAction]
        private AuthModel NullModel()
        {
            return new AuthModel() { Authorized = false, User = null, Reason = "Invalid token" };
        }

        [NonAction]
        internal string GetIp()
        {
            return Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}