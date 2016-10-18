using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace Mn.Framework.Web.Mvc
{
    public class PdfResult : PartialViewResult
    {
        // Setting a FileDownloadName downloads the PDF instead of viewing it
        public string FileDownloadName { get; set; }
        public string HeaderTitle { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(this.ViewName))
            {
                this.ViewName = context.RouteData.GetRequiredString("action");
            }

            if (this.View == null)
            {
                this.View = this.FindView(context).View;
            }


            // First get the html from the Html view
            using (var writer = new StringWriter())
            {
                var vwContext = new ViewContext(context, this.View, this.ViewData, this.TempData, writer);
                this.View.Render(vwContext, writer);

                // Convert to pdf

                var response = context.HttpContext.Response;

                using (var pdfStream = new MemoryStream())
                {
                    var pdfDoc = new Document(PageSize.A4, 20, 20, 20, 20);
                    var pdfWriter = PdfWriter.GetInstance(pdfDoc, pdfStream);
                    pdfWriter.PageEvent = new PageEventHelper();
                    pdfDoc.Open();
                    var str = writer.ToString();
                    //Regex.Replace(HTMLCode, @"(<script[^*]*</script>)", "", RegexOptions.IgnoreCase);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(str);
                    foreach (HtmlNode row in doc.DocumentNode.SelectNodes("//tr"))
                    {
                        if (!row.InnerHtml.Contains("</td>") && !row.InnerHtml.Contains("<td>"))
                            row.Remove();
                    }
                    doc.DocumentNode.Descendants()
                    .Where(n => n.Name == "script" || n.Name == "style")
                    .ToList()
                    .ForEach(n => n.Remove());

                    str = doc.DocumentNode.InnerHtml;
                    using (var htmlRdr = new StringReader(str))
                    {
                        var parsed = HTMLWorker.ParseToList(htmlRdr, null);

                        foreach (var parsedElement in parsed)
                        {
                            if (parsedElement is PdfPTable)
                            {
                                (parsedElement as PdfPTable).SplitLate = false;
                                (parsedElement as PdfPTable).KeepTogether = true;
                            }
                            if (parsedElement.GetType().Name.Equals("Paragraph"))
                            {
                                // cast the element to a Paragraph
                                Paragraph htmlPar = ((Paragraph)parsedElement);

                                if (htmlPar[0].ToString()
                                              .Equals("PageBreak", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    pdfDoc.Add(Chunk.NEXTPAGE);
                                    continue;
                                }
                            }

                            pdfDoc.Add(parsedElement);
                        }
                    }

                    pdfDoc.Close();

                    response.ContentType = "application/pdf";
                    response.AddHeader("Content-Disposition", "attachment;filename=" + FileDownloadName + ".pdf");
                    byte[] pdfBytes = pdfStream.ToArray();
                    response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
                }
            }
        }

        class PageEventHelper : PdfPageEventHelper
        {
            PdfContentByte cb;
            PdfTemplate template;


            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                int pageN = writer.PageNumber;
                String text = "Page: " + pageN.ToString();// +" of ";
                //float len = this.RunDateFont.BaseFont.GetWidthPoint(text, this.RunDateFont.Size);

                var pageSize = document.PageSize;

                cb.SetRGBColorFill(100, 100, 100);
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, BaseFont.NOT_EMBEDDED), 12);
                cb.SetTextMatrix(document.LeftMargin, pageSize.GetBottom(document.BottomMargin));
                cb.ShowText(text);

                cb.EndText();

                cb.AddTemplate(template, document.LeftMargin, pageSize.GetBottom(document.BottomMargin));
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
                template.BeginText();
                template.SetFontAndSize(BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, BaseFont.NOT_EMBEDDED), 12);
                template.SetTextMatrix(0, 0);
                //template.ShowText("" + (writer.PageNumber - 1));
                template.EndText();
            }
        }
    }
}