using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrainWave.WebUI.Controllers
{
    public class ModeratorController : Controller
    {
        // GET: ModeratorController
        public ActionResult Index()
        {
            return View();
        }
    }
}
