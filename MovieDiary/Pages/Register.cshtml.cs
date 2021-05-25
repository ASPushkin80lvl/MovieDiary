using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDiary.Services;

namespace MovieDiary.Pages
{
    public class RegisterModel : PageModel
    {

        private readonly UserService _userService;

        public RegisterModel(DataContext dc) => _userService = new UserService(dc);

        public void OnGet() { }

        public IActionResult OnPostRegister(string username, string password)
        {
            _userService.SignupUser(username, password);
            return RedirectToPage("DiaryTable", "Logged");
        }
    }
}
