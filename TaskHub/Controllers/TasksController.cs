using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
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
        [Route("/Task/IndexUser")]
        public async Task<IActionResult> IndexUser()
        {
            var user = await _userManager.GetUserAsync(User);
            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (db.Tasks.Include("Users").Where(t => t.Users.Any(u => u == user)).Count() > 0)
                ViewBag.Tasks = db.Tasks.Include("Users").Where(t => t.Users.Any(u => u == user));
            else ViewBag.Tasks = null;
            return View();
        }

        [Authorize(Roles = "membru,administrator,organizator")]
        [Route("/Tasks/Show/{idTask}")]
        public ActionResult Show(int idTask)
        {
            var task = db.Tasks.Include("Users").FirstOrDefault(t => t.Id == idTask);
            var users = task.Users;
            if (users != null && users.Count() != 0)
            {
                ViewBag.Membrii = users.Select(u => u.Email).ToList();
            }
            ViewBag.Task = task;
            return View(); 
        }

        [Authorize(Roles = "administrator,organizator")]
        public IActionResult New(int idProiect) 
        {
            ViewBag.Id = idProiect;
            ViewBag.AvailableStatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Started", Text = "Started" },
                new SelectListItem { Value = "InProgress", Text = "In Progress" },
                new SelectListItem { Value = "Completed", Text = "Completed" },
            };
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
            ViewBag.AvailableStatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Started", Text = "Started" },
                new SelectListItem { Value = "InProgress", Text = "In Progress" },
                new SelectListItem { Value = "Completed", Text = "Completed" },
            };
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

        [Route("/tasks/editstatus/{idTask}")]
        [Authorize(Roles = "member,administrator,organizator")]
        public IActionResult EditStatus(int idTask)
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            ViewBag.IdTask = task.Id;
            ViewBag.AvailableStatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Started", Text = "Started" },
                new SelectListItem { Value = "InProgress", Text = "In Progress" },
                new SelectListItem { Value = "Completed", Text = "Completed" },
            };
            return View();
        }

        [HttpPost]
        [Route("/tasks/editstatus/{idTask}")]
        [Authorize(Roles = "member,administrator,organizator")]
        public IActionResult EditStatus(int idTask, string status)
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            task.Status = status;
            db.Update(task);
            db.SaveChanges();
            return RedirectToAction("Show", new { idTask = idTask });   
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

        [HttpGet]
        [Route("/tasks/addmember/{idTask}")]
        [Authorize(Roles = "administrator,organizator")]
        public async Task<IActionResult> AddMember(int idTask)
        {
            ViewBag.IdTask = idTask;
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            int? projectId = task.ProiectId;
            ViewBag.AvailableUserOptions = new List<SelectListItem>();
            foreach (var echipa in db.Echipe.Where(e => e.IdProiect == projectId).ToList())
            {
                if (db.Tasks.Where(t => t.ProiectId == projectId).FirstOrDefault(t => t.Users.Any(u => u.Id == echipa.IdUtilizator)) != null)
                {
                    continue;
                }
                var email = (await _userManager.FindByIdAsync(echipa.IdUtilizator)).Email;
                ViewBag.AvailableUserOptions.Add(new SelectListItem { Value = email, Text = email });
            }
            if (ViewBag.AvailableUserOptions.Count > 0)
                return View();
            else return View("ErrorAddMember");
        }
        [HttpPost]
        [Route("/tasks/addmember/{id?}/{e?}")]
        public async Task<IActionResult> AddMember(int idTask, string? email)
        {
            _logger.LogInformation(idTask.ToString());
            _logger.LogInformation("something");
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            var user = (await _userManager.FindByEmailAsync(email));
            if (user != null)
            {
                if (task.Users == null)
                {
                    IEnumerable<ApplicationUser> users = new List<ApplicationUser> { user };
                    task.Users = users;
                }
                else task.Users.Append(user);
                db.Tasks.Update(task);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Show", new { idTask = idTask });
        }

        [HttpPost]
        [Route("Tasks/DeleteMember/{nume?}/{idTask}")]
        [Authorize(Roles = "organizator")]
        public async Task<IActionResult> DeleteMember(string? nume, int idTask)
        {
            var user = await _userManager.FindByEmailAsync(nume);

            if (user == null)
            {
                return NotFound();
            }

            var task = await db.Tasks
            .Include(t => t.Users) 
            .FirstOrDefaultAsync(t => t.Id == idTask);

            task.Users = task.Users.Where(u => u != user).ToList();

            db.Tasks.Update(task);  

            await db.SaveChangesAsync();

            // Redirect to the Index action after successful deletion
            return RedirectToAction("Show", new { idTask = idTask });
        }
    }
}
