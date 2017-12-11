using Microsoft.AspNetCore.Mvc;
using baka.Models;
using baka.Models.Entity;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace baka.Controllers
{
    [Route("/api")]
    public class FilesApiController : BakaController
    {
        [Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm(Name = "file")] List<IFormFile> files)
        {
            AuthModel model = Authorize();

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

            IFormFile file;
            if (files == null || !files.Any() || files.FirstOrDefault() == null)
            {
                Response.StatusCode = 400;

                return Json(new
                {
                    success = false,
                    code = 400,
                    error = "Uploads must be sent as 'file' and not be empty",
                    result_id = "e-400"
                });
            }
            else
                file = files.FirstOrDefault();
            
            
        }
    }
}