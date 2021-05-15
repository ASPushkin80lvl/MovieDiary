using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MovieDiary.Services;

namespace MovieDiary.Pages
{
    public class AdminPageModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserService _userService;

        [BindProperty]
        public IEnumerable<string> UserNames { get; set; }

        public AdminPageModel(ILogger<IndexModel> logger, DataContext dc)
        {
            _logger = logger;
            _userService = new UserService(dc);
            UserNames = _userService.GetUserNames();
        }

        public void OnGet()
        {
        }

        public void OnPost(string id) 
        {
            var a = 1;
        }
    }
}
