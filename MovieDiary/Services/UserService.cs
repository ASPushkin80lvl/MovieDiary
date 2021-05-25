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

        public List<string> GetUserNames()
        {
            return _dc
                .Users
                .Where(u => !u.Admin)
                .OrderBy(u => u.Id)
                .Select(u => $"{u.Id}-{u.Login}")
                .ToList();
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

        public User LoggedIn()
        {
            var res = _dc.Users.FirstOrDefault(u => u.LoggedIn);
            if (res != null) return res;

            return null;
        }

        public int SignupUser(string login, string password, bool admin = false)
        {
            int lastUserId = _dc.Users.FirstOrDefault() != null ? _dc.Users.Max(m => m.Id) + 1 : 1;

            var settings = JsonConvert.SerializeObject(new UserSettings());
            _dc.Users.Add(new User { Id = lastUserId, Admin = admin, Login = login, Password = password, Settings = settings, LoggedIn = false });
            _dc.SaveChanges();
            LoginUser(login, password);
            return lastUserId;
        }

        public User LoginUser(string login, string password)
        {
            var res = _dc.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault();
            if (res != null)
            {
                res.LoggedIn = true;
                _dc.Users.Update(res);
                _dc.SaveChanges();
                return res;
            }
            return null;
        }

        public void ExitUser()
        {
            var user = _dc.Users.FirstOrDefault(u => u.LoggedIn);
            if (user == null) return;
            user.LoggedIn = false;
            _dc.Users.Update(user);
            _dc.SaveChanges();
        }
    }
}
