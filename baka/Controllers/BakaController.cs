using Microsoft.AspNetCore.Mvc;
using baka.Models;
using baka.Models.Entity;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace baka.Controllers
{
    [NonController]
    public class BakaController : Controller
    {
        [NonAction]
        internal AuthModel Authorize()
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

                if (usr.Disabled || usr.Deleted)
                    model.Authorized = false;
                else if (!usr.Permissions.Contains("SU_UPLOAD_OBJECTS"))
                {
                    model.Authorized = false;
                }
                else model.Authorized = true;

                model.User = usr;

                return model;
            }
        }

        [NonAction]
        internal AuthModel AuthorizeInfo()
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

                if (usr.Disabled || usr.Deleted)
                    model.Authorized = false;
                else if (!usr.Permissions.Contains("SU_VIEW_USER_INFO"))
                {
                    model.Authorized = false;
                }
                else model.Authorized = true;

                model.User = usr;

                return model;
            }
        }

        [NonAction]
        internal AuthModel AuthorizeSu()
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

                if (usr.Disabled || usr.Deleted)
                    model.Authorized = false;
                else if (!usr.Permissions.Contains("SU_MANAGE_ACCOUNTS"))
                {
                    model.Authorized = false;
                }
                else model.Authorized = true;

                model.User = usr;

                return model;
            }
        }

        [NonAction]
        private AuthModel NullModel()
        {
            return new AuthModel() { Authorized = false, User = null, };
        }

        [NonAction]
        internal string GetIp()
        {
            return Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}