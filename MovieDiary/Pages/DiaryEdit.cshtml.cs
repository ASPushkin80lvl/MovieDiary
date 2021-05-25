using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDiary.Library;
using MovieDiary.Services;

namespace MovieDiary.Pages
{
    public class DiaryEditModel : PageModel
    {
        private readonly UserService _userService;
        private readonly MarkService _markService;

        [BindProperty]
        public UserSettings Settings { get; set; }

        [BindProperty]
        public FilmRow Row { get; set; }

        [BindProperty]
        public bool Found { get; set; }

        [BindProperty]
        public string[] Suggestion { get; set; }

        [BindProperty]
        public int MarkId { get; set; }

        public DiaryEditModel(DataContext dc)
        {
            _userService = new UserService(dc);
            _markService = new MarkService(dc);

            var id = _userService.LoggedIn().Id;

            Settings = _markService.GetUserSettings(id);
            Row = new FilmRow(Settings.AdditionalFieldNames);
            Found = false;
            Suggestion = new[] { "", "", "" };
        }

        public void OnGet()
        {
            MarkId = 0;
        }

        public void OnGetEdit(string markId)
        {
            MarkId = int.Parse(markId);
            var id = _userService.LoggedIn().Id;
            Row = _markService.GetUserMarks(id).First(m => m.MarkId == MarkId);
        }

        public void OnPostFind()
        {
            var id = _userService.LoggedIn().Id;
            Settings = _markService.GetUserSettings(id);

            if (Row.MovieName == null || Row.MovieName.Length < 3) return;

            Found = true;
            var options = _markService.FindLikelyMovies(Row.MovieName);
            Suggestion = new[] { "", "", "" };
            if (options.Count == 0) return;

            Suggestion = options.Select(a => a.ToString()).ToArray();
        }

        public void OnPostFill(string movieStr)
        {
            var id = _userService.LoggedIn().Id;
            Settings = _markService.GetUserSettings(id);
            Found = false;
            Suggestion = new[] { "", "", "" };
            Row = _markService.GetMovieInfo(Settings, movieStr.Split(' ')[0]);
        }

        public IActionResult OnPostSave()
        {
            var id = _userService.LoggedIn().Id;

            if (MarkId == 0)
            {
                _markService.AddUserMark(id, Row);
            }
            else
            {
                Row.MarkId = MarkId;
                _markService.EditUserMark(Row);
            }

            return RedirectToPage("DiaryTable");
        }
    }
}
