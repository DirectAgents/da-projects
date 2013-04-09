using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Models;
using CsvHelper;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private ICakeRepository cakeRepo;

        public FilesController(ICakeRepository cakeRepository)
        {
            this.cakeRepo = cakeRepository;
        }

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

        public ActionResult Process(int id)
        {
            var advId = HomeController.GetAdvertiserId();

            //int advId = 294, offerId = 1604; // scooter store
            var start = new DateTime(2013, 2, 1);
            var endBefore = new DateTime(2013, 3, 1);

            List<ScooterRow> csvRows;
            using (var usersContext = new UsersContext())
            {
                var fileUpload = usersContext.FileUploads.FirstOrDefault(f => f.Id == id && f.CakeAdvertiserId == advId);
                if (fileUpload == null)
                    return null;

                var reader = new StringReader(fileUpload.Text);
                var csv = new CsvReader(reader);
                csvRows = csv.GetRecords<ScooterRow>().ToList();
            }
            var conversions = cakeRepo.Conversions.Where(c => c.Advertiser_Id == advId
                && c.ConversionDate >= start && c.ConversionDate < endBefore).ToList();
            var qry = from conv in conversions
                      from csvRow in csvRows
                      where conv.Transaction_Id == csvRow.TransactionId
                      select new { Conversion = conv, FTNSO1 = csvRow.FTNSO1 };
            //foreach (var item in qry)
            //{
            //    item.Conversion.Positive = (item.FTNSO1 == 1);
            //}
            //cakeRepo.SaveChanges();

            return null;
        }

    }
}
