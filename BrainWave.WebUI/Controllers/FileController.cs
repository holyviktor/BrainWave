using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BrainWave.Application.Services;
using BrainWave.Infrastructure.Data;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace BrainWave.WebUI.Controllers
{
    public class FileController : Controller
    {
        private readonly FileService _fileService;
        private readonly BrainWaveDbContext _dbContext;
        public FileController(BrainWaveDbContext dbContext) { 
            _fileService= new FileService();
            _dbContext = dbContext;
        }
        public ActionResult Index(int id)
        {
            var article = _dbContext.Articles.Where(a=>a.IsAvailable).SingleOrDefault(a=>a.Id ==id);
            if (article == null)
            {
                throw new InvalidOperationException();
            }
            if (article.Price > 0)
            {
                throw new InvalidOperationException();
            }
            MemoryStream stream = new MemoryStream();
            var document = _fileService.Create(article);
            document.Save(stream);
            stream.Position = 0;
            string contentType = "application/pdf";
            string fileName = "Article.pdf";

            return File(stream, contentType, fileName);

        }

    }
}
