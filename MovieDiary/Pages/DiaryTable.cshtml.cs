using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDiary.Library;
using MovieDiary.Services;

namespace MovieDiary.Pages
{
    public class DiaryTableModel : PageModel
    {
        private readonly UserService _userService;
        private readonly MarkService _markService;

        [BindProperty]
        public IEnumerable<FilmRow> FilmRows { get; set; }

        public DiaryTableModel(DataContext dc)
        {
            _userService = new UserService(dc);
            _markService = new MarkService(dc);
        }

        public IActionResult OnGet()
        {
            if (_userService.LoggedIn() == null) return RedirectToPage("Login");

            return RedirectToPage("DiaryTable", "Logged");
        }

        public void OnGetLogged()
        {
            var user = _userService.LoggedIn();
            if (user != null) FilmRows = _markService.GetUserMarks(user.Id);
        }

        public IActionResult OnPostEdit(int markId)
        {
            return RedirectToPage("DiaryEdit", "Edit", new { markId = markId });
        }

        public void OnPostDelete(int markId)
        {
            _markService.DeleteUserMark(markId);
            var user = _userService.LoggedIn();
            FilmRows = _markService.GetUserMarks(user.Id);
        }
    }
}
