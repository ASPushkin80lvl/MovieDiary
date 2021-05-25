using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDiary.Services;

namespace MovieDiary.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;

        public LoginModel(DataContext dc) => _userService = new UserService(dc);

        public void OnGet() { }

        public IActionResult OnPostLogin(string username, string password)
        {
            var user = _userService.LoginUser(username, password);
            if (user != null)
            {
                if (user.Admin) return RedirectToPage("AdminPage", "Logged");
                return RedirectToPage("DiaryTable", "Logged");
            }

            return RedirectToPage("Login");
        }
    }
}
