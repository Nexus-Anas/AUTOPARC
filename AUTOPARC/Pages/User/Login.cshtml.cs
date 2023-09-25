using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace AUTOPARC.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly DBC _db;
        public LoginModel(DBC db) => _db = db;




        [BindProperty]
        public Users Users { get; set; }

        public bool user_not_found;




        public async Task<IActionResult> OnPostLogin()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var user = await _db.Users.Where(u => u.Email == Users.Email && u.Password == Users.Password).SingleOrDefaultAsync();

                if (user == null)
                {
                    user_not_found = true;
                    return Page();
                }


                HttpContext.Session.SetString("username", user.Username);
                UsersInfos.Username = user.Username;
                UsersInfos.Email = user.Email;
                UsersInfos.IsAdmin = user.IsAdmin;
                return RedirectToPage("/Index");
            }
            catch (Exception)
            {
                return Page();
            }
        }
    }
}
