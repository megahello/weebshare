using Microsoft.AspNetCore.Mvc;
using baka.Models;
using baka.Models.Entity;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace baka.Controllers
{
    [Route("/api")]
    public class FilesApiController : BakaController
    {
        [Route("upload")]
        [RequestSizeLimit(((1000 * 1024L) * 1024L))]
        public async Task<IActionResult> UploadFile([FromForm(Name = "file")] List<IFormFile> files)
        {
            AuthModel model = Authorize("su_upload");
            if (!model.Authorized)
            {
                Response.StatusCode = 401;

                return Json(new
                {
                    success = false,
                    error = model.Reason,
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

            double FileSize = Globals.ConvertBytesToMegabytes(file.Length);

            if (FileSize > model.User.UploadLimitMB)
            {
                Response.StatusCode = 400;

                return Json(new
                {
                    success = false,
                    code = 400,
                    error = "Exceeds max file size",
                    max_file_size = model.User.UploadLimitMB,
                    global_limit = false,
                    result_id = "e-400"
                });
            }

            string extension = Path.GetExtension(file.FileName);

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            BakaFile db_file = new BakaFile()
            {
                FileSizeMB = FileSize,
                Filename = file.FileName,
                Deleted = false,
                ExternalId = Globals.GenerateFileId(),
                BackendFileId = Globals.Config.S3KeyPrefix + Globals.GenerateBackendId() + extension,
                Timestamp = DateTime.Now,
                Extension = extension,
                IpUploadedFrom = GetIp()
            };

            await Globals.UploadFile(file.OpenReadStream(), db_file.BackendFileId, db_file.ContentType);

            using (var context = new BakaContext())
            {
                model.User.Files.Add(db_file);

                await context.Files.AddAsync(db_file);

                if (model.User.InitialIp == null)
                {
                    model.User.InitialIp = GetIp();
                }

                await context.SaveChangesAsync();
            }

            return Json(new
            {
                id = db_file.Id,
                result_id = db_file.ExternalId,
                full_result_id = db_file.ExternalId + db_file.Extension,
                file_size = FileSize,
                timestamp = db_file.Timestamp.ToFileTimeUtc().ToString(),
                file_name = db_file.Filename,
                success = true,
                code = 200
            });
        }
    }
}