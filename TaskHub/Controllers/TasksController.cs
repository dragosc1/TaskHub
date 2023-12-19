﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using TaskHub.Database;
using TaskHub.Models;

namespace TaskHub.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskHubDbcontext db;
        private readonly ILogger<ProiecteController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TasksController(TaskHubDbcontext context,
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
        [Route("/Tasks/Index/{idProiect}")]
        public IActionResult Index(int idProiect)
        {
            var Tasks = db.Tasks.Where(t => t.Proiect.Id == idProiect);    
            ViewBag.Tasks = Tasks;
            ViewBag.Id = idProiect;
            _logger.LogInformation("something");
            _logger.LogInformation("something");
            _logger.LogInformation(idProiect.ToString());

            return View();
        }

        [Authorize(Roles = "membru,administrator,organizator")]
        [Route("/Tasks/Show/{idTask}")]
        public ActionResult Show(int idTask) 
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            ViewBag.Task = task;
            return View(); 
        }

        [Authorize(Roles = "administrator,organizator")]
        public IActionResult New(int idProiect) 
        {
            ViewBag.Id = idProiect;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "administrator,organizator")]
        public IActionResult New(Models.Task t) 
        {
            try
            {
                db.Tasks.Add(t);
                db.SaveChanges();
                return RedirectToAction("", "Tasks", new { id = t.ProiectId });
            }
            catch (Exception) 
            {
                return View();
            }
        }

        [Route("/tasks/edit/{idTask}")]
        [Authorize(Roles = "administrator,organizator")]
        public IActionResult Edit(int idTask) 
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            return View(task);
        }


        [HttpPost]
        [Authorize(Roles = "administrator,organizator")]
        [Route("tasks/edit/{t}")]
        public ActionResult Edit(Models.Task model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var task = db.Tasks.Find(model.Id);

                    if (task == null)
                    {
                        return NotFound();
                    }

                    task.Titlu = model.Titlu;
                    task.Descriere = model.Descriere;
                    task.Status = model.Status;
                    task.DataStart = model.DataStart;
                    task.DataFinalizare = model.DataFinalizare;
                    task.ContinutMedia = model.ContinutMedia;
                    db.SaveChanges();
                    return RedirectToAction("", "Tasks", new { id = task.ProiectId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Concurrency conflict occurred.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("/tasks/delete/{idTask}")]
        [Authorize(Roles = "administrator,organizator")]
        public ActionResult Delete(int idTask) 
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            int? idProiect = task.ProiectId;
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("", "Tasks", new { id = idProiect });
        }
    }
}
