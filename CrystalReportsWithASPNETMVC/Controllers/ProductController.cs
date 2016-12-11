using CrystalDecisions.CrystalReports.Engine;
using CrystalReportsWithASPNETMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrystalReportsWithASPNETMVC.Controllers
{
    public class ProductController : Controller
    {
        private IK_DemoEntities dbContext = new IK_DemoEntities();
        public ActionResult Index()
        {
            ViewBag.ListProducts = dbContext.Products.ToList();
            return View();
        }

        public ActionResult Export()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportProduct.rpt")));
            rd.SetDataSource(dbContext.Products.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price.Value,
                Quantity = p.Quantity.Value
            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "ListProducts.pdf");
            
        }


        public ActionResult ExportGrouping()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportGrouping.rpt")));
            rd.SetDataSource(dbContext.Products.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price.Value,
                Quantity = p.Quantity.Value,
                CategoryId = p.Category.Id,
                CategoryName = p.Category.Name
            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "ListProductsGrouping.pdf");
        }

    }
}