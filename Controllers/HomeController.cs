using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GameCommerce.Models;
using GameCommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCommerce.Controllers;

public class HomeController : Controller
{
    private ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var model = _db.Products.Include(x=>x.Photos).ToList();
        return View(model);
    }

  
}

