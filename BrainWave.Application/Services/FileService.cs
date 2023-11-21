
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using BrainWave.Core.Entities;
using BrainWave.Core.Interfaces;

namespace BrainWave.Application.Services
{
    public class FileService:IFileService
    {
        public PdfDocument Create(Article article)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            graphics.DrawString(article.Title, fontTitle, PdfBrushes.Black, new PointF(0, 0));
            PdfFont fontText = new PdfStandardFont(PdfFontFamily.Helvetica, 14);
            graphics.DrawString(article.Text, fontText, PdfBrushes.Black, new PointF(0, 30));
            return document;
        }
    }
}
