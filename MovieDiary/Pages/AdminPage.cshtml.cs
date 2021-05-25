using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDiary.Services;

namespace MovieDiary.Pages
{
    public class AdminPageModel : PageModel
    {
        private readonly UserService _userService;

        [BindProperty]
        public List<string> UserNames { get; set; }

        public AdminPageModel(DataContext dc)
        {
            _userService = new UserService(dc);
            UserNames = _userService.GetUserNames();
        }

        public IActionResult OnGet()
        {
            var user = _userService.LoggedIn();
            if (user == null) return RedirectToPage("Login");
            if (!user.Admin) return RedirectToPage("DiaryTable", "Logged");

            return RedirectToPage("AdminPage", "Logged");
        }

        public void OnGetLogged() { }

        public void OnPostDelete(string id)
        {
            var actualId = id.Split('-')[0];
            _userService.DeleteUser(actualId);
            UserNames = _userService.GetUserNames();
        }
    }
}
