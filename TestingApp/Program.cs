using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MovieDiary;
using MovieDiary.Library;
using MovieDiary.Services;
using System;
using System.Collections.Generic;

namespace TestingApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            using var context = new MovieDiaryDbContextFactory().CreateDbContext();

            var userService = new UserService(context);

            object res;

            if (userService.LoginUser("admin", "admin") == null) userService.SignupUser("admin", "admin", true);

            userService.DeleteUser("2");
            userService.DeleteUser("3");
            userService.DeleteUser("4");

            userService.SignupUser("User1", "Pass1");
            userService.SignupUser("User2", "Pass2");
            userService.SignupUser("User3", "Pass3");

            res = userService.LoginUser("User1", "Pass1");
            res = userService.LoginUser("User2", "Pass2");
            res = userService.LoginUser("User2", "Pass3");
            res = userService.LoginUser("User1", "Pass4");

            res = userService.GetUserNames();

            userService.DeleteUser("2");
            userService.DeleteUser("3");
            userService.DeleteUser("4");

            var markService = new MarkService(context);

            var userId1 = userService.SignupUser("User1", "Pass1");

            var settings = markService.GetUserSettings(userId1);
            settings.ShowCountry = false;
            settings.AdditionalFieldNames.AddRange(new[] { "Field1", "Field2" });
            markService.EditUserSettings(userId1, settings);

            res = markService.FindLikelyMovies("Pulp Fict");
            res = markService.GetMovieInfo(settings, (res as List<MovieApiRecord>)[0].Id);
            (res as FilmRow).AdditionalFields["Field1"] = "Value1";
            (res as FilmRow).AdditionalFields["Field2"] = "Value2";
            markService.AddUserMark(userId1, res as FilmRow);

            res = markService.FindLikelyMovies("Interstellar");
            res = markService.GetMovieInfo(settings, (res as List<MovieApiRecord>)[0].Id);
            (res as FilmRow).AdditionalFields["Field1"] = "Value3";
            (res as FilmRow).AdditionalFields["Field2"] = "Value4";
            (res as FilmRow).Mark = 4;
            markService.AddUserMark(userId1, res as FilmRow);

            var row1 = markService.GetUserMarks(userId1)[0]; var row2 = markService.GetUserMarks(userId1)[1];
            row1.Mark = 5;
            row1.AdditionalFields["Field1"] = "Aboba";
            markService.EditUserMark(row1);
            row2.AdditionalFields["Field1"] = "Aboba2";
            markService.EditUserMark(row2);

            var userId2 = userService.SignupUser("User2", "Pass2");

            settings = markService.GetUserSettings(userId2);
            settings.ShowDirector = false;
            settings.AdditionalFieldNames.AddRange(new[] { "Field3", "Field4", "Field5" });
            markService.EditUserSettings(userId2, settings);

            res = markService.FindLikelyMovies("Interstellar");
            res = markService.GetMovieInfo(settings, (res as List<MovieApiRecord>)[0].Id);
            (res as FilmRow).AdditionalFields["Field3"] = "Value1";
            (res as FilmRow).AdditionalFields["Field4"] = "Value2";
            (res as FilmRow).AdditionalFields["Field5"] = "Value3";
            markService.AddUserMark(userId2, res as FilmRow);

            res = markService.FindLikelyMovies("Borat");
            res = markService.GetMovieInfo(settings, (res as List<MovieApiRecord>)[0].Id);
            (res as FilmRow).AdditionalFields["Field3"] = "Value3";
            (res as FilmRow).AdditionalFields["Field4"] = "Value4";
            (res as FilmRow).AdditionalFields["Field5"] = "Value5";
            (res as FilmRow).Mark = 4;
            markService.AddUserMark(userId2, res as FilmRow);

            var row3 = markService.GetUserMarks(userId2)[0]; var row4 = markService.GetUserMarks(userId2)[1];
            row3.Mark = 5;
            row3.AdditionalFields["Field5"] = "Aboba";
            markService.EditUserMark(row3);
            row4.AdditionalFields["Field5"] = "Aboba2";
            markService.EditUserMark(row4);

            //markService.DeleteUserMark(row1.MarkId);
            //markService.DeleteUserMark(row2.MarkId);
            //markService.DeleteUserMark(row3.MarkId);
            //markService.DeleteUserMark(row4.MarkId);
        }
    }

    public class MovieDiaryDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private static string _connectionString;

        public DataContext CreateDbContext()
        {
            return CreateDbContext(null);
        }

        public DataContext CreateDbContext(string[] args)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                LoadConnectionString();
            }

            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(_connectionString);

            return new DataContext(builder.Options);
        }

        private static void LoadConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);

            var configuration = builder.Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
