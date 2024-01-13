using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskHub.Database;

namespace TaskHub.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly TaskHubDbcontext _db;
        private readonly ILogger<AdministratorController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdministratorController(TaskHubDbcontext db, ILogger<AdministratorController> logger, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
        }
        [Route("/Administrator/ManageUsers")]
        public IActionResult ManageUsers()
        {
            ViewBag.Membrii = _db.Users;
            return View();
        }
        [HttpPost]
        [Route("/Administrator/Delete/{email?}")]
        public async Task<IActionResult> Delete(string? email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }
        [HttpGet]
        [Route("/Administrator/EditRole/{email?}")]
        public async Task<IActionResult> EditRole(string? email)
        {
            ViewBag.AvailableRoles = new List<SelectListItem>
            {
                new SelectListItem { Value = "organizator", Text = "organizator" },
                new SelectListItem { Value = "administrator", Text = "administrator" },
                new SelectListItem { Value = "membru", Text = "membru" },
            };
            ViewBag.email = email;
            return View();
        }
        [HttpPost]
        [Route("/Administrator/EditRole/{email?}/{role?}")]
        public async Task<IActionResult> EditRole(string? email, string? role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            // Remove the user from all existing roles
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, role);
            return RedirectToAction("ManageUsers");
        }
    }
}
