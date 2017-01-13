using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;

namespace Pcir.Utils.Nimok
{
    public class NKReport
    {
        public string ReportType { get; set; }
        public LocalReport Report { get; set; }
        public string Name { get; set; }

        public NKReport()
        {
            Name = "reporte";
        }
    }

    public class NKReporting : Controller
    {
        //
        // GET: /NKReporting/

        public ActionResult GetReport(NKReport report)
        {
            string reportType = report.ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.2in</MarginTop>" +
            "  <MarginLeft>0.2in</MarginLeft>" +
            "  <MarginRight>0.2in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = report.Report.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            string fileDownloadName = report.Name + "." + fileNameExtension;
            return File(renderedBytes, mimeType, fileDownloadName);
        }
    }

}
