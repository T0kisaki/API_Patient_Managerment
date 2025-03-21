using Microsoft.AspNetCore.Mvc;

namespace API_Patient_Managerment.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
