using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TaskHub.Database;
using TaskHub.Models;

namespace TaskHub.Controllers
{
    public class ProiecteController : Controller
    {
        private readonly TaskHubDbcontext db;
        private readonly ILogger<ProiecteController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProiecteController(TaskHubDbcontext context, 
            ILogger<ProiecteController> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.db = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "membru,administrator,organizator")]
        public IActionResult Index()
        {
            var proiecte = from proiect in db.Proiecte
                           select proiect;
            ViewBag.Proiecte = proiecte;
            return View();
        }

        [Route("/proiecte/show/{idProiect}")]
        public IActionResult Show([FromRoute] int idProiect)
        {
            var proiect = db.Proiecte.FirstOrDefault(p => p.Id == idProiect);
            ViewBag.Proiect = proiect;
            return View();
        }

        public IActionResult New() 
        { 
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> New(Proiect p)
        {
            try
            {
                db.Proiecte.Add(p);
                db.SaveChanges();
                Echipa echipa = new Echipa() { IdProiect = db.Proiecte.FirstOrDefault(pr => pr == p).Id, IdUtilizator = (await _userManager.GetUserAsync(User)).Id, RolInProiect = await _roleManager.FindByNameAsync("organizator")};
                db.Echipe.Add(echipa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception) 
            {
                return View(); 
            }
        }

        [Route("/proiecte/edit/{idProiect}")]
        public IActionResult Edit(int idProiect) 
        {
            var proiect = db.Proiecte.FirstOrDefault(p => p.Id == idProiect);
            return View(proiect);
        }

        [HttpPost]
        public ActionResult Edit(int idProiect, Proiect p)
        {
            var proiect = db.Proiecte.FirstOrDefault(p => p.Id == idProiect);
            try 
            {
                proiect.NumeProiect = p.NumeProiect;
                proiect.Descriere = p.Descriere;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch(Exception) 
            {
                return View();
            }
        }

        [HttpPost]
        [Route("/proiecte/delete/{idProiect}")]
        public ActionResult Delete(int idProiect) 
        {
            var proiect = db.Proiecte.FirstOrDefault(p =>p.Id == idProiect);
            db.Proiecte.Remove(proiect);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
