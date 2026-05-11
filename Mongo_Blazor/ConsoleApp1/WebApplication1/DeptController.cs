using Microsoft.AspNetCore.Mvc;

namespace WebApplication1;

public class DeptController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}