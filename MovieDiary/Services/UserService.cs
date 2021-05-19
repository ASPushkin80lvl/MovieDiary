using MovieDiary.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDiary.Services
{
    public class UserService
    {
        private readonly DataContext _dc;

        public UserService(DataContext dc) => _dc = dc;

        public IEnumerable<string> GetUserNames()
        {
            return _dc.Users.Where(u => !u.Admin).Select(u => $"{u.Id}-{u.Login}");
        }

        public void DeleteUser(string id)
        {
            var user = _dc.Users.Where(u => u.Id == int.Parse(id)).FirstOrDefault();
            if (user == null) return;

            var marks = _dc.Marks.Where(m => m.User.Id == int.Parse(id)).ToArray();

            _dc.Marks.RemoveRange(marks);
            _dc.Users.Remove(user);
            _dc.SaveChanges();
        }

        public int SignupUser(string login, string password, bool admin = false)
        {
            int lastUserId = _dc.Users.FirstOrDefault() != null ? _dc.Users.Max(m => m.Id) + 1 : 1;

            var settings = JsonConvert.SerializeObject(new UserSettings());
            _dc.Users.Add(new User { Id = lastUserId, Admin = admin, Login = login, Password = password, Settings = settings });
            _dc.SaveChanges();
            return lastUserId;
        }

        public User LoginUser(string login, string password)
        {
            var res = _dc.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault();
            if (res != null) return res;
            return null;
        }
    }
}
