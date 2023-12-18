﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskHub.Database;
using TaskHub.Models;

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
        [Authorize(Roles ="organizator,administrator,membru")]
        public async Task<IActionResult> Show(int? Id)
        {
            var Proiect = _db.Proiecte.FirstOrDefault(p => p.Id == Id);
            ViewBag.Proiect = Proiect;
            var MembriiId = await _db.Echipe
            .Where(e => e.IdProiect == Id)
            .Select(e => e.IdUtilizator)
            .ToListAsync();

            var Membrii = await System.Threading.Tasks.Task.WhenAll(MembriiId.Select(id => _userManager.FindByIdAsync(id)));
            var EmailAddresses = Membrii.Select(user => user?.Email).ToList();

            bool Organizator;
            if (User.IsInRole("organizator"))
                Organizator = true;
            else Organizator = false;
            ViewBag.Membrii = Membrii;
            ViewBag.Organizator = Organizator;  
            return View();
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
    }
}
