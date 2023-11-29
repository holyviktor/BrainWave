using BrainWave.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace BrainWave.WebUI.Controllers
{
    [ApiController]
    public class PhotoController : Controller
    {
        private readonly BrainWaveDbContext _dbContext;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly string FolderPath = "/media/avatars/";
        public PhotoController(BrainWaveDbContext dbContext, IWebHostEnvironment appEnvironment)
        {
            _dbContext = dbContext;
            _appEnvironment = appEnvironment;
        }
        [HttpPatch("/profile/photo/edit")]

        public async Task Index(IList<IFormFile> files)
        {
            var userTag = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            if (userTag == null)
            {
                throw new ArgumentException();
            }
            var user = _dbContext.Users.FirstOrDefault(x => x.Tag == userTag);
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            var formFile = files[0];
            var type = formFile.FileName.Split('.').Last();
            if (formFile.Length > 0)
            {
                DirectoryInfo dir = new DirectoryInfo(_appEnvironment.WebRootPath + FolderPath);
                FileInfo[] existingFilesTag = dir.GetFiles(user.Tag + "*", SearchOption.TopDirectoryOnly);
                foreach (FileInfo existingFile in existingFilesTag)
                {
                    existingFile.Delete();
                }

                var path = FolderPath + user.Tag + '.' + type;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
            }

            user.Photo = user.Tag + '.' + type;
            _dbContext.SaveChanges();
        }


    }
}
