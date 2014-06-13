using DAgents.Common;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using System;
//using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace EomToolWeb.Controllers
{
    public class ExtraItemsController : EOMController
    {
        public ExtraItemsController(IMainRepository mainRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        [HttpGet]
        public ActionResult Import()
        {
            SetAccountingPeriodViewData();
            ViewBag.CurrentEomDate = eomEntitiesConfig.CurrentEomDate;
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase uploadFile)
        {
            if (uploadFile == null || uploadFile.ContentLength <= 0)
                return Content("Content was empty");
            if (uploadFile.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                return Content("Must be a valid excel file of version 2007 and above");

            var data = SLExcelReader.ReadExcel(uploadFile.InputStream);
            if (!data.Status.Success)
                return Content(data.Status.Message);

            var rowsConverter = CreateItemRowsConverter(data);
            Session["ItemRowsConverter"] = rowsConverter;

            return RedirectToAction("PreviewImport");
        }

        public ActionResult PreviewImport()
        {
            var rowsConverter = Session["ItemRowsConverter"] as RowsConverter<Item>;
            if (rowsConverter == null)
                return Content("Preview unavailable");

            ViewBag.DebugMode = eomEntitiesConfig.DebugMode;
            ViewBag.CurrentEomDate = eomEntitiesConfig.CurrentEomDate;
            return View(rowsConverter);
        }

        public ActionResult FinishImport()
        {
            var rowsConverter = Session["ItemRowsConverter"] as RowsConverter<Item>;
            if (rowsConverter == null)
                return Content("Import failed");

            foreach (var row in rowsConverter.Rows)
            {
                if (row.Errors.Count == 0)
                    mainRepo.AddItem(row.Object);
            }
            mainRepo.SaveChanges();

            rowsConverter.ImportComplete = true;
            return RedirectToAction("PreviewImport");
        }

        public ActionResult Upload(HttpPostedFileBase uploadFile)
        {
            if (uploadFile.ContentLength > 0)
            {
                string filePath = Path.Combine("C:\\Uploads", //HttpContext.Server.MapPath("../Uploads"),
                    Path.GetFileName(uploadFile.FileName));
                uploadFile.SaveAs(filePath);
            }
            return null;
        }

        //public ActionResult Test()
        //{
        //    var app = new Microsoft.Office.Interop.Excel.Application();
        //    var book = app.Workbooks.Open(@"C:\Uploads\test.xlsx");
        //    var sheet = (Worksheet)book.Worksheets[1];

        //    return Content("okay");
        //}

        public ActionResult Test2()
        {
            var data = SLExcelReader.ReadExcel(null, @"C:\Uploads\test.xlsx");

            var rowsConverter = CreateItemRowsConverter(data);

            return Content("okay");
        }

        private RowsConverter<Item> CreateItemRowsConverter(SLExcelData data)
        {
            int numHeaders = data.Headers.Count;
            var rowsConverter = new RowsConverter<Item>(data.Headers);
            var settableProps = Item.SettableProperties;
            foreach (var dataRow in data.DataRows)
            {
                if (!dataRow.Any(c => !String.IsNullOrWhiteSpace(c)))
                    continue;

                var rowWithObj = rowsConverter.NewRow(dataRow);

                var item = new Item();
                item.SetDefaults();

                // go through the cells in the row. ignore cells beyond the rightmost header
                for (int i = 0; i < dataRow.Count && i < numHeaders; i++)
                {
                    var colName = data.Headers[i].ToLower();
                    if (settableProps.Contains(colName))
                    {
                        var error = item.SetProperty(colName, dataRow[i], mainRepo);
                        if (error != null)
                            rowWithObj.Errors.Add(error);
                    }
                }
                var errors = item.VerifyAndFinalizeProperties(mainRepo);
                if (errors.Count > 0)
                    rowWithObj.Errors.AddRange(errors);

                if (rowWithObj.Errors.Count == 0)
                {
                    if (mainRepo.ItemExists(item))
                        rowWithObj.Errors.Add("Item already exists in the database");
                    else
                        rowWithObj.Object = item; // no errors; item can be added
                }
            }
            return rowsConverter;
        }
    }


}
