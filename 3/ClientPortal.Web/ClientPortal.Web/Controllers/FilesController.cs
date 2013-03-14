using ClientPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientPortal.Web.Controllers
{
    public class FilesController : Controller
    {
        public ActionResult Index()
        {
            var files = GetFiles();
            return PartialView(files);
        }

        public ActionResult List()
        {
            var files = GetFiles();
            return PartialView(files);
        }

        private List<FileUploadInfo> GetFiles()
        {
            var advId = HomeController.GetAdvertiserId();
            using (var usersContext = new UsersContext())
            {
                var files = usersContext.FileUploads.AsQueryable();
                if (advId.HasValue)
                    files = files.Where(f => f.CakeAdvertiserId == advId);
                else
                    files = files.Where(f => f.CakeAdvertiserId == null);

                var fileUploadInfos = files
                    .Select(f => new FileUploadInfo()
                    {
                        Id = f.Id,
                        UploadDate = f.UploadDate,
                        Filename = f.Filename,
                        CakeAdvertiserId = f.CakeAdvertiserId
                    }); // avoid reading the Text

                return fileUploadInfos.ToList();
            }
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            var advId = HomeController.GetAdvertiserId();
            using (var reader = new StreamReader(file.InputStream))
            {
                var fileUpload = new FileUpload()
                {
                    CakeAdvertiserId = advId,
                    UploadDate = DateTime.Now,
                    Filename = file.FileName,
                    Text = reader.ReadToEnd()
                };
                using (var usersContext = new UsersContext())
                {
                    usersContext.FileUploads.Add(fileUpload);
                    usersContext.SaveChanges();
                }
            }
            return null;
        }

    }
}
