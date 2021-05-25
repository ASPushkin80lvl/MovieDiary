using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDiary.Services;

namespace MovieDiary.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserService _userService;

        public IndexModel(DataContext dc) => _userService = new UserService(dc);

        public IActionResult OnGet()
        {
            var user = _userService.LoggedIn();
            if (user != null)
            {
                if (user.Admin) return RedirectToPage("AdminPage", "Logged");
                return RedirectToPage("DiaryTable", "Logged");
            }

            return RedirectToPage("Login");
        }
    }
}
