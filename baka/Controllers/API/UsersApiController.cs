using Microsoft.AspNetCore.Mvc;
using baka.Models;
using baka.Models.Entity;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace baka.Controllers
{
    [Route("/api/users")]
    public class UsersApiController : BakaController
    {
        [Route("create-user")]
        public async Task<IActionResult> CreateUser(NewUserModel details)
        {
            try
            {
                var model = AuthorizeSu();
                if (model.Authorized == false && model.User != null)
                {
                    if (model.User.Disabled)
                    {
                        Response.StatusCode = 401;
                        return Json(new { success = false, error = "Your account has been disabled.", code = 401 });
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return Json(new { success = false, error = "Invalid token.", code = 401 });
                    }
                }
                else if (model.User == null)
                {
                    Response.StatusCode = 401;
                    return Json(new { success = false, error = "Invalid token.", code = 401 });
                }

                BakaUser return_usr;

                using (var context = new BakaContext())
                {
                    var usr = new BakaUser()
                    {
                        Name = details.Name,
                        Username = details.Username,
                        IntialIp = null,
                        Timestamp = DateTime.Now,
                        Token = Globals.GenerateToken(),
                        UploadLimitMB = details.UploadLimit,
                        Deleted = false,
                        Disabled = false
                    };

                    if (Globals.Config.GiveDefaultPermissions)
                    {
                        foreach (string permission in Globals.Config.DefaultPermissions)
                        {
                            usr.Permissions.Add(permission);
                        }
                    }

                    await context.Users.AddAsync(usr);
                    await context.SaveChangesAsync();

                    return_usr = usr;
                }

                return Json(new {
                    username = return_usr.Username,
                    name = return_usr.Name,
                    initial_ip = return_usr.IntialIp,
                    timestamp = return_usr.Timestamp.ToFileTimeUtc().ToString(),
                    token = return_usr.Token,
                    deleted = return_usr.Deleted,
                    disabled = return_usr.Disabled,
                });
            }
            catch
            {
                Response.StatusCode = 500;
                return Json(new { success = false, error = "500 Internal Server Error", code = 500 });
            }
        }

        [Route("{token}/get-user-info")]
        public async Task<IActionResult> GetUserInfo(string token)
        {

        }

        [Route("{token}/delete-user")]
        public async Task<IActionResult> DeleteUser(string token)
        {

        }

        [Route("{token}/disable-user")]
        public async Task<IActionResult> DisableUser(string token)
        {

        }

        [Route("{token}/reset-token")]
        public async Task<IActionResult> ResetUserToken(string token)
        {

        }
    }
}