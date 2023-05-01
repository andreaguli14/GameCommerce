using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameCommerce.Controllers {

	[Authorize (Roles = "SuperAdmin")]
	public class AppRolesController : Controller
    {
		private readonly RoleManager<IdentityRole> _roleManager;

		public AppRolesController(RoleManager<IdentityRole> roleManager) {

			_roleManager = roleManager;
		}


		public IActionResult Index()
		{
			var roles = _roleManager.Roles;
			return View(roles);

		}

        public IActionResult Create()
        {
            return View();
        }


        
        public async Task<IActionResult> Delete(string id)
        {
			var x = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
			await _roleManager.DeleteAsync(x);


            return RedirectToAction("Index");
        }



        [HttpPost]
		public async  Task<IActionResult> Create(IdentityRole model )
		{
			if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
			{
				_roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();

			}

			return RedirectToAction("Index");
		}

      




    }
}

