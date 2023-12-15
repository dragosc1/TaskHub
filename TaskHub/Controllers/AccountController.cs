using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;

        public AccountController(Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public ActionResult Register()
        {
            var roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name });
            return View();
        }
    }
}
