using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GameCommerce.Models;
using GameCommerce.Data;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GameCommerce.Controllers;

public class ProductsController : Controller
{
    private ApplicationDbContext _db ;
    private IWebHostEnvironment _host;
    private SignInManager<Customer> _check;

    public ProductsController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment, SignInManager<Customer> check)
    {
        _db = db;
        _host = hostEnvironment;
        _check = check;
    }

    public IActionResult Index()
    {
        var model = _db.Products.ToList();
        return View(model);
    }

    [Authorize]
    public IActionResult AddToCart(int id, string userId, int quantity )
    {
        try
        {
            var @object = new CartObject();
            @object.Product = _db.Products.FirstOrDefault(x => x.Id.Equals(id));
            @object.Quantity = quantity;

            var user = _db.Users.Include(x => x.Cart).ThenInclude(x => x.Objects).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id.Equals(userId));

            if (user.Cart.Objects.Any(x => x.Product.Id == id))
            {
                var oggetto = user.Cart.Objects.FirstOrDefault(x => x.Product.Id == id);
                oggetto.Quantity += quantity;
            }
            else
            {
                user.Cart.Objects.Add(@object);
            }
            _db.SaveChanges();
        }catch(Exception e) { }

        return RedirectToAction("Index","Cart");
    }

    public IActionResult AddProduct()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddProduct([FromForm] Product model)
    {
        if (model.ImageFile.Any())
        {
            foreach (var photo in model.ImageFile)
            {
                string wwwRootPath = _host.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(photo.FileName);
                string extension = Path.GetExtension(photo.FileName);
                filename = filename + DateTime.Now.ToString("ddMMyyyyHHmmss") + extension;
                string path = Path.Combine(wwwRootPath + "/img/", filename);
                string pathsave = "/img/" + filename;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
                var image = new Photo();
                image.Path = pathsave;
                if (!model.Photos.Any())
                {
                    image.main = true;
                } 
                model.Photos.Add(image);
            }
        }
        _db.Products.Add(model);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult ProductDetails(int id)
    {
        var model = _db.Products.Include(x=>x.Photos).FirstOrDefault(x=>x.Id.Equals(id));
        return View(model);
    }


    public IActionResult EditProduct(int id)
    {
        var model = _db.Products.Include(x => x.Photos).FirstOrDefault(x => x.Id.Equals(id));
        return View(model);
    }

    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
        var model = _db.Products.Include(x=>x.Photos).FirstOrDefault(x => x.Id.Equals(product.Id));
        if (product.ImageFile.Any())
        {
            foreach (var photo in product.ImageFile)
            {
                string wwwRootPath = _host.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(photo.FileName);
                string extension = Path.GetExtension(photo.FileName);
                filename = filename + DateTime.Now.ToString("ddMMyyyyHHmmss") + extension;
                string path = Path.Combine(wwwRootPath + "/img/", filename);
                string pathsave = "/img/" + filename;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
                var image = new Photo();
                image.Path = pathsave;
                if (!model.Photos.Any())
                {
                    image.main = true;
                }
                model.Photos.Add(image);
            }
        }
        model.Name = product.Name;
        model.Price = product.Price;
        model.Description = product.Description;
        model.Category = product.Category;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }


    public IActionResult DeleteProduct(int id)
    {
        var model = _db.Products.Include(x=>x.Photos).FirstOrDefault(x => x.Id.Equals(id));


        var cart =  _db.CartObjects.Include(x => x.Product).Where(x=>x.Product.Id == id);
        _db.CartObjects.RemoveRange(cart);
        _db.SaveChanges();

        _db.Photo.RemoveRange(model.Photos);
        _db.SaveChanges();

        
        _db.Products.Remove(model);
        _db.SaveChanges();
      
        return RedirectToAction("Index");
    }

    public IActionResult DeletePhoto(int id, int photoId)
    {
        var product = _db.Products.Include(x=>x.Photos).FirstOrDefault(x => x.Id == id);
        var photo = _db.Photo.FirstOrDefault(x => x.Id == photoId);
        product.Photos.Remove(photo);
        _db.SaveChanges();
        if (product.Photos.Any() && !product.Photos.Any(x=>x.main == true))
        {
            product.Photos.First().main = true;
        }
        _db.SaveChanges();

        return RedirectToAction("EditProduct", new { id = id });
    }

    public IActionResult SetDefault(int id, int photoId)
    {
        var product = _db.Products.Include(x=>x.Photos).FirstOrDefault(x => x.Id == id);

        foreach(var i in product.Photos)
        {

            if (i.Id == photoId) i.main = true;
            else i.main = false;
        }
        
        _db.SaveChanges();

        return RedirectToAction("EditProduct", new { id = id });
    }


    public IActionResult Store(string? query, string? category)
    {
        var model = new Store();
        model.Products = _db.Products.Include(x => x.Photos).ToList();

        if(query != null && query != "")
        {
            model.Query = query;
            model.Products = _db.Products.Include(x => x.Photos).Where(x=>x.Name.ToLower().Contains(query.ToLower())).ToList();
        }
        if (category != null && category != "")
        {
            model.Category = category;
            model.Products = _db.Products.Include(x => x.Photos).Where(x=>x.Category == category).ToList();
        }


        return View(model);
    }
}

