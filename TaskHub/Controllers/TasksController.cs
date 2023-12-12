using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using TaskHub.Database;

namespace TaskHub.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskHubDbcontext db;

        public TasksController(TaskHubDbcontext context)
        {
            this.db = context;
        }

        public IActionResult Index()
        {
            var Tasks = from task in db.Tasks
                        select task;
            ViewBag.Tasks = Tasks;
            return View();
        }

        public ActionResult Show(int idTask) 
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            ViewBag.Task = task;
            return View(); 
        }

        public IActionResult New() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Models.Task t) 
        {
            try
            {
                db.Tasks.Add(t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception) 
            {
                return View();
            }
        }

        public IActionResult Edit(int idTask) 
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            return View(task);
        }

        [HttpPost]
        public ActionResult Edit(int idTask, Models.Task requestTask) 
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            try
            {
                task.Titlu = requestTask.Titlu;
                task.Descriere = requestTask.Descriere;
                task.Status = requestTask.Status;
                task.DataStart = requestTask.DataStart;
                task.DataFinalizare = requestTask.DataFinalizare;
                task.ContinutMedia = requestTask.ContinutMedia;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (Exception) 
            {
                return RedirectToAction("Edit", task.Id);
            }
        }

        [HttpPost]
        public ActionResult Delete(int idTask) 
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == idTask);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
