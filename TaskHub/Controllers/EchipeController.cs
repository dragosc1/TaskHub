using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskHub.Database;

namespace TaskHub.Controllers
{
    public class EchipeController : Controller
    {
        private readonly TaskHubDbcontext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public EchipeController(TaskHubDbcontext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize(Roles = "organizator,administrator,membru")]
        public async Task<IActionResult> Index()
        {
            var currentUserId = (await _userManager.GetUserAsync(User)).Id;
            var Echipe = _db.Echipe.Where(e => e.IdUtilizator == currentUserId).Include("Proiect");
            ViewBag.Echipe = Echipe;    
            return View();
        }
    }
}
