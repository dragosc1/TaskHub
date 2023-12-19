using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskHub.Database;
using TaskHub.Models;

namespace TaskHub.Controllers
{
    public class EchipeController : Controller
    {
        private readonly TaskHubDbcontext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<EchipeController> _logger; 
        public EchipeController(TaskHubDbcontext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<EchipeController> logger)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [Authorize(Roles = "organizator,administrator,membru")]
        public async Task<IActionResult> Index()
        {
            var currentUserId = (await _userManager.GetUserAsync(User)).Id;
            var Echipe = _db.Echipe.Where(e => e.IdUtilizator == currentUserId).Include("Proiect");
            ViewBag.Echipe = Echipe;   
            return View();
        }
        [Authorize(Roles ="organizator,administrator,membru")]
        public IActionResult Show(int? Id)
        {
            var Proiect = _db.Proiecte.FirstOrDefault(p => p.Id == Id);
            ViewBag.Proiect = Proiect;
            ViewBag.Id = Id;

            var MembriiId = _db.Echipe
                .Where(e => e.IdProiect == Id)
                .Select(e => e.IdUtilizator)
                .ToList();

            var Membrii = MembriiId.Select(id => _userManager.FindByIdAsync(id).Result).ToList();
            var EmailAddresses = Membrii.Select(user => user?.Email).ToList();

            bool Organizator;
            if (User.IsInRole("organizator"))
                Organizator = true;
            else
                Organizator = false;

            ViewBag.Membrii = Membrii;
            ViewBag.Organizator = Organizator;

            return View();
        }
        public class MemberViewModel
        {
            public string email { get; set; }
            public List<SelectListItem> Members { get; set; }
        }
        [HttpPost]
        [Route("Echipe/Delete/{nume?}/{idProiect}")]
        [Authorize(Roles = "organizator")]
        public async Task<IActionResult> Delete(string? nume, int idProiect)
        {
            var user = await _userManager.FindByEmailAsync(nume);

            if (user == null)
            {
                // Handle the case where the user is not found
                // You might want to return a specific view or a NotFound result
                return NotFound();
            }

            Echipa echipa = new Echipa { IdProiect = idProiect, IdUtilizator = user.Id };
            _db.Echipe.Remove(echipa);

            await _db.SaveChangesAsync();

            // Redirect to the Index action after successful deletion
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Echipe/New/{id?}")]
        [Authorize(Roles = "organizator")]
        public async Task<IActionResult> New(int idProiect)
        {
            var newMembers = _db.Users.Select(u => u.Email).ToList();
            var newMembersSelection = new List<SelectListItem>();

            foreach (var member in newMembers)
            {
                var user = _db.Users.FirstOrDefault(u => u.Email == member);
                if (await _userManager.IsInRoleAsync(user, "administrator"))
                    continue;
                var existsInEchipe = _db.Echipe.Any(e => e.IdProiect == idProiect && e.IdUtilizator == user.Id);
                if (existsInEchipe)
                    continue;
                newMembersSelection.Add(new SelectListItem { Value = member, Text = member });
            }

            foreach (var member in newMembers)
            {
                newMembersSelection.Add(new SelectListItem { Value = member, Text = member });
            }

            ViewBag.IdProiect = idProiect;

            var viewModel = new MemberViewModel
            {
                Members = newMembersSelection
            };

            return View(viewModel);

        }
        [HttpPost]
        [Route("Echipe/New/{id?}/{email?}")]
        [Authorize(Roles = "organizator")]
        public async Task<IActionResult> New(int idProiect, string? email)
        {
            Echipa echipa = new Echipa { IdProiect = idProiect, IdUtilizator = (await _userManager.FindByEmailAsync(email)).Id };
            _db.Echipe.Add(echipa);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
