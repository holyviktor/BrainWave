using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrainWave.WebUI.Controllers
{
    public class ProfileController : Controller
    {
        // GET: ProfileController
        public ActionResult Index()
        {
            return View();
        }
    }
}
