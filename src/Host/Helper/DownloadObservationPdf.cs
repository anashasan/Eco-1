using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.DbServices;
using Host.Business.IDbServices;

namespace Host.Helper
{
    public class DownloadObservationPdf
    {
        private readonly IActivityService _activityService;

        public DownloadObservationPdf(IActivityService activityService)
        {
            _activityService = activityService;
        }

        public byte[] DownloadObservation(
            int branchId,
            int? locationId,
            DateTime? fromDate,
            DateTime? toDate,
            IHostingEnvironment _hostingEnvironment
          
           )
        {
            try
            {
                var observationReportData = _activityService.GetObservationReport(branchId, locationId, fromDate, toDate);
                var memoryStream = new System.IO.MemoryStream();
                // var document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(288f, 432f));
                Document document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 10, 10, 10, 10);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                Phrase phrase = new Phrase();
                document.Add(phrase);


                var path = Path.Combine(_hostingEnvironment.WebRootPath, @"images\Eco.jpg");

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(path);
                image.ScaleAbsolute(110f, 60f);
                image.SetAbsolutePosition(0f, 540f);
                image.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                //image.SpacingAfter = 40;
                document.Add(image);

                var companyname = observationReportData.Select(i => i.CompanyName).FirstOrDefault();
                var branchname = observationReportData.Select(a => a.BranchName).FirstOrDefault();


                iTextSharp.text.Paragraph company = new iTextSharp.text.Paragraph();
                iTextSharp.text.Paragraph branch = new iTextSharp.text.Paragraph();

                company.SpacingBefore = 10;
                company.SpacingAfter = 25;
                company.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                company.Font = FontFactory.GetFont(FontFactory.TIMES, 20f, BaseColor.Black);
                company.Add(companyname+"  " + branchname);
                document.Add(company);
                PdfPTable table = new PdfPTable(3);

                PdfPCell cell = new PdfPCell(new Phrase("Row 1, Col 1"));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Row 1, Col 2"));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Row 1, Col 3"));
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Row 2 , Col 1"));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Row 2, Col 2"));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Row 2 and Row 3, Col 3"));
                cell.Rowspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Row 3, Col 1"));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Row 3, Col 2"));
                table.AddCell(cell);

                document.Add(table);


                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                return bytes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }
    }
}
