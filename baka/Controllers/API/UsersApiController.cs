using Microsoft.AspNetCore.Mvc;
using baka.Models;
using baka.Models.Entity;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace baka.Controllers
{
    [Route("/api/users")]
    public class UsersApiController : BakaController
    {
        [Route("create-user")]
        [AcceptVerbs("POST")]
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

                        return Json(new
                        {
                            success = false,
                            error = "Your account has been disabled.",
                            code = 401
                        });
                    }
                    else
                    {
                        Response.StatusCode = 401;

                        return Json(new
                        {
                            success = false,
                            error = "Invalid token.",
                            code = 401
                        });
                    }
                }
                else if (model.User == null)
                {
                    Response.StatusCode = 401;

                    return Json(new
                    {
                        success = false,
                        error = "Invalid token.",
                        code = 401
                    });
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

                    usr.Token = Globals.GenerateToken(usr);

                    await context.Users.AddAsync(usr);
                    await context.SaveChangesAsync();

                    return_usr = usr;
                }

                return Json(new
                {
                    id = return_usr.Id,
                    username = return_usr.Username,
                    name = return_usr.Name,
                    initial_ip = return_usr.IntialIp,
                    timestamp = return_usr.Timestamp.ToFileTimeUtc().ToString(),
                    token = return_usr.Token,
                    deleted = return_usr.Deleted,
                    disabled = return_usr.Disabled,
                    email = return_usr.Email,
                    upload_limit = return_usr.UploadLimitMB,
                });
            }
            catch
            {
                Response.StatusCode = 500;

                return Json(new
                {
                    success = false,
                    error = "500 Internal Server Error",
                    code = 500
                });
            }
        }

        [Route("{token}")]
        [AcceptVerbs("GET")]
        public async Task<IActionResult> GetUserInfo(string token)
        {
            try
            {
                var model = AuthorizeInfo();
                if (model.Authorized == false && model.User != null)
                {
                    if (model.User.Disabled)
                    {
                        Response.StatusCode = 401;

                        return Json(new
                        {
                            success = false,
                            error = "Your account has been disabled.",
                            code = 401
                        });
                    }
                    else
                    {
                        Response.StatusCode = 401;

                        return Json(new
                        {
                            success = false,
                            error = "Invalid token.",
                            code = 401
                        });
                    }
                }
                else if (model.User == null)
                {
                    Response.StatusCode = 401;

                    return Json(new
                    {
                        success = false,
                        error = "Invalid token.",
                        code = 401
                    });
                }

                using (var context = new BakaContext())
                {
                    BakaUser return_usr = await context.Users.FirstOrDefaultAsync(x => x.Token == token);

                    if (return_usr == null)
                        return NotFound(new { success = false, error = "404 Not Found", code = 404 });

                    return Json(new
                    {
                        id = return_usr.Id,
                        username = return_usr.Username,
                        name = return_usr.Name,
                        email = return_usr.Email,
                        upload_limit = return_usr.UploadLimitMB,
                        initial_ip = return_usr.IntialIp,
                        timestamp = return_usr.Timestamp.ToFileTimeUtc().ToString(),
                        token = return_usr.Token,
                        deleted = return_usr.Deleted,
                        disabled = return_usr.Disabled,
                        permissions = return_usr.Permissions,
                        links = return_usr.Links,
                        files = return_usr.Files,
                    });
                }
            }
            catch
            {
                Response.StatusCode = 500;

                return Json(new
                {
                    success = false,
                    error = "500 Internal Server Error",
                    code = 500
                });
            }
        }

        [Route("{token}")]
        [Route("delete/{token}")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> DeleteUser(string token)
        {
            try
            {
                var model = AuthorizeSu();
                if (model.Authorized == false && model.User != null)
                {
                    if (model.User.Disabled)
                    {
                        Response.StatusCode = 401;

                        return Json(new
                        {
                            success = false,
                            error = "Your account has been disabled.",
                            code = 401
                        });
                    }
                    else
                    {
                        Response.StatusCode = 401;

                        return Json(new
                        {
                            success = false,
                            error = "Invalid token.",
                            code = 401
                        });
                    }
                }
                else if (model.User == null)
                {
                    Response.StatusCode = 401;

                    return Json(new
                    {
                        success = false,
                        error = "Invalid token.",
                        code = 401
                    });
                }

                using (var context = new BakaContext())
                {
                    BakaUser return_usr = await context.Users.FirstOrDefaultAsync(x => x.Token == token);

                    if (return_usr == null)
                        return NotFound(new { success = false, error = "404 Not Found", code = 404 });

                    if (!Globals.Config.PreserveDeletedFiles)
                        context.Remove(return_usr);
                    else
                        return_usr.Deleted = true;

                    await context.SaveChangesAsync();

                    return Json(new
                    {
                        success = true,
                        code = 200,
                        deleted = true
                    });
                }


            }
            catch
            {
                Response.StatusCode = 500;

                return Json(new
                {
                    success = false,
                    error = "500 Internal Server Error",
                    code = 500
                });
            }
        }

        [Route("disable/{token}")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> DisableUser(string token)
        {
            try
            {
                var model = AuthorizeSu();
                if (model.Authorized == false && model.User != null)
                {
                    if (model.User.Disabled)
                    {
                        Response.StatusCode = 401;

                        return Json(new
                        {
                            success = false,
                            error = "Your account has been disabled.",
                            code = 401
                        });
                    }
                    else
                    {
                        Response.StatusCode = 401;

                        return Json(new
                        {
                            success = false,
                            error = "Invalid token.",
                            code = 401
                        });
                    }
                }
                else if (model.User == null)
                {
                    Response.StatusCode = 401;

                    return Json(new
                    {
                        success = false,
                        error = "Invalid token.",
                        code = 401
                    });
                }

                using (var context = new BakaContext())
                {
                    BakaUser return_usr = await context.Users.FirstOrDefaultAsync(x => x.Token == token);

                    if (return_usr == null)
                        return NotFound(new { success = false, error = "404 Not Found", code = 404 });

                    return_usr.Disabled = true;
                    await context.SaveChangesAsync();

                    return Json(new
                    {
                        success = true,
                        code = 200,
                        disabled = true
                    });
                }
            }
            catch
            {
                Response.StatusCode = 500;

                return Json(new
                {
                    success = false,
                    error = "500 Internal Server Error",
                    code = 500
                });
            }
        }

        [Route("reset-token/{token}")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> ResetUserToken(string token)
        {
            try
            {
                var model = AuthorizeSu();
                if (model.Authorized == false && model.User != null)
                {
                    if (model.User.Disabled)
                    {
                        Response.StatusCode = 401;

                        return Json(new
                        {
                            success = false,
                            error = "Your account has been disabled.",
                            code = 401
                        });
                    }
                    else
                    {
                        Response.StatusCode = 401;

                        return Json(new
                        {
                            success = false,
                            error = "Invalid token.",
                            code = 401
                        });
                    }
                }
                else if (model.User == null)
                {
                    Response.StatusCode = 401;

                    return Json(new
                    {
                        success = false,
                        error = "Invalid token.",
                        code = 401
                    });
                }

                using (var context = new BakaContext())
                {
                    BakaUser return_usr = await context.Users.FirstOrDefaultAsync(x => x.Token == token);

                    if (return_usr == null)
                        return NotFound(new { success = false, error = "404 Not Found", code = 404 });

                    return_usr.Token = Globals.GenerateToken(return_usr);

                    await context.SaveChangesAsync();

                    return Json(new
                    {
                        new_token = return_usr.Token
                    });
                }
            }
            catch
            {
                Response.StatusCode = 500;

                return Json(new
                {
                    success = false,
                    error = "500 Internal Server Error",
                    code = 500
                });
            }
        }
    }
}