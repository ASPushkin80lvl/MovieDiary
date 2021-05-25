using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MovieDiary.Library;
using MovieDiary.Services;

namespace MovieDiary.Pages
{
    public class DiarySettingsModel : PageModel
    {
        private readonly UserService _userService;
        private readonly MarkService _markService;

        [BindProperty]
        public UserSettings Settings { get; set; }

        public DiarySettingsModel(DataContext dc)
        {
            _userService = new UserService(dc);
            _markService = new MarkService(dc);
        }

        public IActionResult OnPostSave(bool year, bool director, bool country, bool ImbdId, string additionalFields)
        {
            _markService.EditUserSettings(_userService.LoggedIn().Id, new UserSettings
            {
                ShowYear = year,
                ShowDirector = director,
                ShowCountry = country,
                ShowImdbId = ImbdId,
                AdditionalFieldNames = additionalFields.Split(',').ToList()
            });
            return RedirectToPage("DiaryTable");
        }

        public void OnGet() 
        {
            var user = _userService.LoggedIn();
            if (user != null) Settings = _markService.GetUserSettings(user.Id);
        }
    }
}
