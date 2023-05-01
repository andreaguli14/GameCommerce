using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GameCommerce.Models;
using GameCommerce.Data;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GameCommerce.Controllers;

public class CartController : Controller
{
    private ApplicationDbContext _db ;
    private readonly UserManager<Customer> _user;
    private SignInManager<Customer> _check;

    public CartController(ApplicationDbContext db, UserManager<Customer> user, SignInManager<Customer> check)
    {
        _db = db;
        _user = user;
        _check = check;
    }

    public IActionResult Index()
    {
        if (_check.IsSignedIn(User))
        {
            var model = _db.Users
                .Include(x => x.Cart)
                .ThenInclude(y => y.Objects)
                .ThenInclude(z => z.Product)
                .ThenInclude(x=>x.Photos)
                .FirstOrDefault(x => x.Id.Equals(_user.GetUserId(User)));
            
            return View(model.Cart);
        }
        else return BadRequest("You Must Login");
    }


    public IActionResult UpdateQuantity(int id, int quantity )
    {
       var cartobject = _db.CartObjects.FirstOrDefault(x => x.Id.Equals(id));

        cartobject.Quantity = quantity;

        _db.SaveChanges();

        return RedirectToAction("Index");
    }


    public IActionResult RemoveFromCart(int id)
    {
        try
        {
            _db.CartObjects.Remove(_db.CartObjects.FirstOrDefault(x => x.Id.Equals(id)));
            _db.SaveChanges();
        }
        catch (Exception e) { Console.WriteLine(e.Message); }     

        return RedirectToAction("Index");
    }


    public IActionResult Checkout(string? coupon)
    {

        if (_check.IsSignedIn(User))
        {
            var model = _db.Users
                .Include(x => x.Cart)
                .ThenInclude(y => y.Objects)
                .ThenInclude(z => z.Product)
                .ThenInclude(x => x.Photos)
                .FirstOrDefault(x => x.Id.Equals(_user.GetUserId(User)));
            if (coupon != null && coupon != "")
            {
                model.Cart.Coupon = coupon.ToUpper();
            }
            return View(model.Cart);
        }
        else return BadRequest("You Must Login");
    }

}

