using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Iris.Web.Areas.Product.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("Product", AreaPrefix = "product-upload")]
    public partial class UploadController : Controller
    {
        private const string TmpPath = "~/Content/tmp/";

        // GET: File/Home
        public virtual ActionResult Index()
        {
            return View();
        }

        [Route("UploadImage")]
        public virtual ActionResult UploadImage(IList<HttpPostedFileBase> files)
        {
            var lstUploadReults = new List<UploadFileResult>(files.Count);

            foreach (var file in files)
            {
                if (file.ContentLength <= 0) continue;

                var guid = Guid.NewGuid();
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = $"{guid}{fileExtension}";
                var path = Server.MapPath(TmpPath) + fileName;
                file.SaveAs(path);

                var thumbnailFileName = $"{guid}-thumb{fileExtension}";

                var fileSize = file.ContentLength;

                GenerateProductThumbnailImage(file.InputStream, Server.MapPath(TmpPath) + thumbnailFileName);

                lstUploadReults.Add(new UploadFileResult
                {
                    Url = Url.Content(TmpPath + fileName),
                    Name = file.FileName,
                    DeleteType = "Post",
                    DeleteUrl = Url.Action(MVC.Product.Upload.ActionNames.DeleteFile, MVC.Product.Upload.Name, new { area = MVC.Product.Name, fileName }),
                    Size = fileSize,
                    ThumbnailUrl = Url.Content(TmpPath + thumbnailFileName),
                    Type = file.ContentType
                });
            }


            return Json(new { Files = lstUploadReults });
        }

        [Route("DeleteFile")]
        public virtual ActionResult DeleteFile(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            var filePath = Path.Combine(Server.MapPath("~/UploadedFiles/ProductImages"), fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return Json("ok");
        }


        private string getFilePath(string fileName, string path)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string newFullPath = Path.Combine(path, fileName); ;

            while (System.IO.File.Exists(newFullPath))
            {
                string tempFileName = $"{fileNameOnly}({count++})";
                newFullPath = Path.Combine(path, tempFileName + extension);
            }

            return newFullPath;
        }

        private void GenerateProductThumbnailImage(Stream inputStream, string savePath)
        {
            var img = new WebImage(inputStream);
            img.Resize(181, 181, true, false).Crop(1, 1);

            img.Save(savePath);
        }

        public class UploadFileResult
        {
            public string Name { get; set; }
            public int Size { get; set; }
            public string Type { get; set; }
            public string Url { get; set; }
            public string DeleteUrl { get; set; }
            public string ThumbnailUrl { get; set; }
            public string DeleteType { get; set; }
        }
    }
}