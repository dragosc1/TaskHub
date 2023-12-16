using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TaskHub.Database;
using TaskHub.Models;

namespace TaskHub.Controllers
{
    public class ProiecteController : Controller
    {
        private readonly TaskHubDbcontext db;

        public ProiecteController(TaskHubDbcontext context)
        {
            this.db = context;
        }

        [Authorize(Roles = "membru,administrator,organizator")]
        public IActionResult Index()
        {
            var proiecte = from proiect in db.Proiecte
                           select proiect;
            ViewBag.Proiecte = proiecte;
            return View();
        }

        public ActionResult Show(int idProiect)
        {
            var proiect = db.Proiecte.FirstOrDefault(p => p.Id == idProiect);    
            ViewBag.Proiecte = proiect;
            return View();
        }

        public IActionResult New() 
        { 
            return View(); 
        }

        [HttpPost]
        public IActionResult New(Proiect p)
        {
            try
            {
                db.Proiecte.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception) 
            {
                return View(); 
            }
        }

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
        public ActionResult Delete(int idProiect) 
        {
            var proiect = db.Proiecte.FirstOrDefault(p =>p.Id == idProiect);
            db.Proiecte.Remove(proiect);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
