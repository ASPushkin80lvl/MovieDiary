﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MovieDiary.Services;
using System.Collections.Generic;

namespace MovieDiary.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserService _userService;

        [BindProperty] 
        public IEnumerable<string> UserNames { get; set; }

        public IndexModel(ILogger<IndexModel> logger, DataContext dc)
        {
            _logger = logger;
            _userService = new UserService(dc);
            UserNames = _userService.GetUserNames();
        }

        public void OnGet()
        {
        }
    }
}
