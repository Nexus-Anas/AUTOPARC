using AUTOPARC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.User
{
    public class RegisterModel : PageModel
    {
        private readonly DBC _db;
        public RegisterModel(DBC db) => _db = db;




        [BindProperty]
        public Users Users { get; set; }




        public async Task<IActionResult> OnPostRegister()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                Users.IsAdmin = Request.Form.TryGetValue("isAdmin", out var def);

                await _db.Users.AddAsync(Users);
                await _db.SaveChangesAsync();
                return RedirectToPage("/User/Login");
            }
            catch (Exception)
            {
                return Page();
            }
        }
    }
}
