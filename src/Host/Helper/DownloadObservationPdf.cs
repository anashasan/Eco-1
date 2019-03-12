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
using System.Drawing;

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
                image.ScaleAbsolute(120f, 60f);
                image.SetAbsolutePosition(8f, 530f);
                image.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                //image.SpacingAfter = 40;
                document.Add(image);

                PdfContentByte cb = writer.DirectContent;
               
                //var Rectangular = new iTextSharp.text.Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                //Rectangular.BorderWidthLeft = 1.1f;
                //Rectangular.BorderWidthRight = 1.1f;
                //Rectangular.BorderWidthTop = 1.1f;
                //Rectangular.BorderWidthBottom = 1.1f;

                var blackListTextFont = (BaseColor.Black);

                //cb.Rectangle(Rectangular);
                //cb.Stroke();
                var companyname = observationReportData.Select(i => i.CompanyName).FirstOrDefault();
                var branchname = observationReportData.Select(a => a.BranchName).FirstOrDefault();


                cb.SetLineWidth(1);
                cb.Rectangle(135, 510, 840, 85); //left width,bottom height,right width,top height
                cb.BeginText();
                BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetFontAndSize(f_cn, 18);
                cb.SetColorFill(BaseColor.LightGray);
                cb.Fill();
                cb.EndText();

                PdfContentByte cb1 = writer.DirectContent;
                cb1.SetLineWidth(1);
                //cb1.Rectangle(95, 510, 840, 80); //left width,bottom height,right width,top height
                cb1.BeginText();
                BaseFont f_cnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb1.SetFontAndSize(f_cn, 18);
                cb1.SetColorFill(BaseColor.White);
                cb1.SetTextMatrix(160, 565);
                cb1.ShowText(companyname+" "+branchname);
                cb1.Fill();
                cb1.EndText();

                PdfContentByte cb2 = writer.DirectContent;
                cb2.SetLineWidth(1);
                //cb1.Rectangle(95, 510, 840, 80); //left width,bottom height,right width,top height
                cb2.BeginText();
                BaseFont f_cnnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb2.SetFontAndSize(f_cn, 18);
                cb2.SetColorFill(BaseColor.White);
                cb2.SetTextMatrix(650, 565); 
                cb2.ShowText("ObservationReport");
               // cb2.SetTextMatrix(25, 125);
                cb2.Fill();
                cb2.EndText();

                PdfContentByte cb3 = writer.DirectContent;
                cb3.SetLineWidth(1);
                //cb1.Rectangle(95, 510, 840, 80); //left width,bottom height,right width,top height
                cb3.BeginText();
                BaseFont f_cnnnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb3.SetFontAndSize(f_cn, 15);
                cb3.SetColorFill(BaseColor.Black);
                cb3.SetTextMatrix(160, 525);
                cb3.ShowText("Report Period");
                // cb2.SetTextMatrix(25, 125);
                cb3.Fill();
                cb3.EndText();


                iTextSharp.text.Paragraph company = new iTextSharp.text.Paragraph();
                iTextSharp.text.Paragraph branch = new iTextSharp.text.Paragraph();

                company.SpacingBefore = 10;
                company.SpacingAfter = 25;
                company.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                company.Font = FontFactory.GetFont(FontFactory.TIMES, 20f, BaseColor.Black);
                company.Add("Company: "+companyname+"    "+"Branch: " + branchname);
                document.Add(company);

                //PdfContentByte cb1 = writer.DirectContent;
                ////cb.MoveTo(document.PageSize.Width / 2, document.PageSize.Height / 2);

                ////cb.LineTo(document.PageSize.Width / 2, document.PageSize.Height);

                ////cb.Stroke();

                //cb.MoveTo(0, document.PageSize.Height / 1.15f);

                //cb.LineTo(document.PageSize.Width, document.PageSize.Height / 1.15f);

                //cb.Stroke();

                



                PdfPTable table = new PdfPTable(9);
                PdfPCell cell = new PdfPCell();

               
                
                table.SpacingBefore = 70;
                table.WidthPercentage = 100;
                table.DefaultCell.Border = Table.NO_BORDER;
                //cell.Phrase = new Phrase("Observation No");
                //table.AddCell(cell);
                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                cell.Phrase = new Phrase("ObservationDate",boldFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase("StationName", boldFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase("StationNo", boldFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase("Location", boldFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase("Observation Description", boldFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase("Images", boldFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase("Action On Client End", boldFont);
                table.AddCell(cell);


                cell.Phrase = new Phrase("Date Of Client End", boldFont);
                table.AddCell(cell);

                cell.Phrase = new Phrase("Status", boldFont);
                table.AddCell(cell);


               
                foreach (var observation in observationReportData)
                {
                    cell.Phrase = new Phrase(observation.ObservationDate);
                    table.AddCell(cell);

                    cell.Phrase = new Phrase(observation.StationName);
                    table.AddCell(cell);

                    cell.Phrase= new Phrase(observation.StationNumber.ToString());
                    table.AddCell(cell);

                    cell.Phrase = new Phrase(observation.LocationName);
                    table.AddCell(cell);

                    cell.Phrase = new Phrase(observation.Description);
                    table.AddCell(cell);


                    string base64 = observation.Images;
                    byte[] imageBytes = Convert.FromBase64String(base64);
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageBytes);



                    
                    //string imageSrc = string.Format("data:image/png;base64,{0}", (observation.Images));
                    cell.AddElement(img);
                    table.AddCell(cell);

                    cell.Phrase = new Phrase(observation.ClientReview);
                    table.AddCell(cell);

                    cell.Phrase = new Phrase(observation.ClientReviewDate.ToString());
                    table.AddCell(cell);

                    cell.Phrase = new Phrase(observation.Status);
                    table.AddCell(cell);

                  

                }

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
