using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GameCommerce.Models;

namespace GameCommerce.Data;

public class ApplicationDbContext : IdentityDbContext<Customer>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

  public  DbSet<Cart> Carts { get; set; }
  public  DbSet<Product> Products { get; set; }
  public  DbSet<CartObject> CartObjects { get; set; }
  public DbSet<Photo> Photo { get; set; }
}

