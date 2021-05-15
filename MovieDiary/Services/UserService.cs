using Microsoft.EntityFrameworkCore;
using MovieDiary.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDiary.Services
{
    public class UserService
    {
        private readonly DataContext _dc;

        public UserService(DataContext dc)
        {
            _dc = dc;
        }

        public IEnumerable<string> GetUserNames()
        {
            return _dc.Users.Where(u => !u.Admin).Select(u => $"{u.Id}-{u.Login}");
        }

        public void DeleteUser(string id)
        {
            _dc.Users.Remove(new User() { Id = Convert.ToInt32(id) });
        }
    }
}
