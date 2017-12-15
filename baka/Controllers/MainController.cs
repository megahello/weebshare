using Microsoft.AspNetCore.Mvc;
using baka.Models;
using baka.Models.Entity;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace baka.Controllers
{
    public class MainController : BakaController
    {
        [Route("/{url}/{name}/.{ext?}")]
        [Route("/{url}/{name}")]
        [Route("/{name}/.{ext?}")]
        [Route("/{name}/")]
        [AcceptVerbs("GET")]
        public async Task<IActionResult> GetFileOrLink(string name, string ext, string url)
        {
            using (var context = new BakaContext())
            {
                var file = await context.Files.FirstOrDefaultAsync(u => u.ExternalId == name);

                if (file == null || file.Deleted)
                {
                    IActionResult link = await GetLink(name, ext, url);

                    if (link != null)
                        return link;

                    Response.StatusCode = 404;

                    return Json(new
                    {
                        success = false,
                        error = "404 Not Found",
                        code = 404
                    });
                }

                string file_ext = Path.GetExtension(file.Filename);
                string use_url_extention = Request.Query["u"].FirstOrDefault() ?? "0";

                if (use_url_extention == "1" || use_url_extention == "true")
                    file_ext = ext;

                return File(await Globals.GetFile(file.BackendFileId), BakaMime.GetMimeType(file_ext));
            }
        }

        [NonAction]
        private async Task<IActionResult> GetLink(string name, string ext, string url)
        {
            using (var context = new BakaContext())
            {
                var link = await context.Links.FirstOrDefaultAsync(u => u.ExternalId == name);

                if(link == null)
                    return null;

                return Redirect(link.Destination);
            }
        }
    }
}