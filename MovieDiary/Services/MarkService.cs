using MovieDiary.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;

namespace MovieDiary.Services
{
    public class MarkService
    {
        private readonly DataContext _dc;

        public MarkService(DataContext dc) => _dc = dc;

        public void AddUserMark(int userId, FilmRow row)
        {
            int lastMovieId = _dc.Movies.FirstOrDefault() != null ? _dc.Movies.Max(m => m.Id) + 1 : 1;

            var movie = _dc.Movies.Where(m => m.Title == row.MovieName).FirstOrDefault();
            if (movie == null)
            {
                movie = new Movie
                {
                    Country = row.Country,
                    Director = row.Director,
                    Id = lastMovieId,
                    ImdbId = row.ImdbId,
                    Title = row.MovieName,
                    Year = row.Year ?? 0
                };
                _dc.Movies.Add(movie);
            }

            int lastMarkId = _dc.Marks.FirstOrDefault() != null ? _dc.Marks.Max(m => m.Id) + 1 : 1;

            _dc.Marks.Add(new Mark
            {
                AdditionalInfo = JsonConvert.SerializeObject(row.AdditionalFields),
                Id = lastMarkId,
                Movie = movie,
                User = _dc.Users.Where(u => u.Id == userId).FirstOrDefault(),
                Value = row.Mark
            });

            _dc.SaveChanges();
        }

        public void EditUserMark(FilmRow row)
        {
            var mark = _dc.Marks.Where(m => m.Id == row.MarkId).FirstOrDefault();
            mark.Value = row.Mark;
            mark.AdditionalInfo = JsonConvert.SerializeObject(row.AdditionalFields);
            _dc.Marks.Update(mark);
            _dc.SaveChanges();
        }

        public List<FilmRow> GetUserMarks(int userId)
        {
            var marks = _dc.Marks
                .Where(m => m.User.Id == userId)
                .Include(p => p.User).Include(p => p.Movie)
                .ToList();
            return marks.Select(m => new FilmRow(m)).ToList();
        }

        public void DeleteUserMark(int id)
        {
            var mark = _dc.Marks.Where(m => m.Id == id).FirstOrDefault();
            if (mark == null) return;
            _dc.Marks.Remove(mark);
            _dc.SaveChanges();
        }

        public UserSettings GetUserSettings(int id)
        {
            var settingsJson = _dc.Users.Where(m => m.Id == id).FirstOrDefault().Settings;
            return JsonConvert.DeserializeObject<UserSettings>(settingsJson);
        }

        public void EditUserSettings(int id, UserSettings userSettings)
        {
            var updatedUser = _dc.Users.Where(u => u.Id == id).FirstOrDefault();
            updatedUser.Settings = JsonConvert.SerializeObject(userSettings);
            _dc.Users.Update(updatedUser);
            _dc.SaveChanges();
        }

        public List<MovieApiRecord> FindLikelyMovies(string name)
        {
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.themoviedb.org/3/search/movie?api_key=a977236ad6ffd8261b829ee8d876e3c1&query={name}");
            var response = httpClient.SendAsync(request).Result;
            return JsonConvert
                .DeserializeObject<MovieApiSearch>(response.Content.ReadAsStringAsync().Result)
                .Results
                .Take(3)
                .ToList();
        }

        public FilmRow GetMovieInfo(UserSettings settings, string id)
        {
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.themoviedb.org/3/movie/{id}?api_key=a977236ad6ffd8261b829ee8d876e3c1");
            var response = httpClient.SendAsync(request).Result;
            using var requestCrew = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.themoviedb.org/3/movie/{id}/credits?api_key=a977236ad6ffd8261b829ee8d876e3c1");
            var responseCrew = httpClient.SendAsync(requestCrew).Result;

            return new FilmRow(settings, response.Content.ReadAsStringAsync().Result, responseCrew.Content.ReadAsStringAsync().Result);
        }
    }
}
