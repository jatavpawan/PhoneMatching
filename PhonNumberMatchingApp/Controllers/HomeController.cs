using PhonNumberMatchingApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhonNumberMatchingApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [ActionName("Index")]
        [HttpPost]
        public ActionResult Importexcel1()
        {  
            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                string query = null;
                string connString = "";
                Utility Utl = new Utility();
                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                }
                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    { System.IO.File.Delete(path1); }
                    Request.Files["FileUpload1"].SaveAs(path1);
                    if (extension == ".csv")
                    {
                        //DataTable dt = Utility.ConvertCSVtoDataTable(path1);
                       var dt= Utl.FetchPhoneNumberData(path1);
                     //   ViewBag.Data = dt;
                       ExportToExcel(dt);
                    }
                    //Connection String to Excel Workbook  
                    else if (extension.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        DataTable dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        ViewBag.Data = dt;
                    }
                    else if (extension.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        DataTable dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        ViewBag.Data = dt;
                    }

                }
                else
                {
                    ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";

                }

            }

            return View();
        }

        public void ExportToExcel( List<PhoneData> lst )
        {
            var gv = new GridView();
            gv.DataSource = lst;// this.GetEmployeeList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
           
        } 




    }
}
