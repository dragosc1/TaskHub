using Microsoft.AspNetCore.Authorization;
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
        private readonly IWebHostEnvironment _env;

        public TasksController(TaskHubDbcontext context,
            ILogger<ProiecteController> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment env)
        {
            this.db = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }

        [Authorize(Roles = "membru,administrator,organizator")]
        [Route("/Tasks/Index/{idProiect}")]
        public IActionResult Index(int idProiect)
        {
            var Tasks = db.Tasks.Where(t => t.Proiect.Id == idProiect);    
            ViewBag.Tasks = Tasks;
            ViewBag.Id = idProiect;

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
        public async Task<IActionResult> New(Models.Task t, IFormFile TaskContent) 
        {
            try
            {
                var storagePath = Path.Combine(
                _env.WebRootPath,
                "media",
                TaskContent.FileName);

                var databaseFileName = "/media/" + TaskContent.FileName;
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await TaskContent.CopyToAsync(fileStream);
                }

                t.ContinutMedia = databaseFileName;

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
            ViewBag.Id = task.Id;
            ViewBag.ProiectId = task.ProiectId;
            return View(task);
        }


        [HttpPost]
        [Authorize(Roles = "administrator,organizator")]
        [Route("/tasks/edit/{model}")]
        public async Task<IActionResult> Edit(Models.Task task, IFormFile TaskContent)
        {
            if (ModelState.IsValid)
            {
                var storagePath = Path.Combine(
               _env.WebRootPath,
               "media",
               TaskContent.FileName);

                var databaseFileName = "/media/" + TaskContent.FileName;
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await TaskContent.CopyToAsync(fileStream);
                }

                task.ContinutMedia = databaseFileName;
                db.Update(task);
                db.SaveChanges();
                return RedirectToAction("", "Tasks", new { id = task.ProiectId });
            }
            else return View();
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
