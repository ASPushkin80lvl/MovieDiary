using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDiary.Services;

namespace MovieDiary.Pages
{
    public class ExitModel : PageModel
    {
        private readonly UserService _userService;

        public ExitModel(DataContext dc)
        {
            _userService = new UserService(dc);
        }

        public IActionResult OnGet()
        {
            _userService.ExitUser();
            return RedirectToPage("Login");
        }
    }
}
