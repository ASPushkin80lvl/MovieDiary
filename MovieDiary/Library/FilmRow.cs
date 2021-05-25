using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDiary.Library
{
    public class FilmRow
    {
        public FilmRow() { }

        public FilmRow(List<string> additionalFieldNames)
        {
            MarkId = 0;
            MovieName = string.Empty;
            Mark = 0;
            Year = 0;
            Director = string.Empty;
            Country = string.Empty;
            ImdbId = string.Empty;
            AdditionalFields = additionalFieldNames.ToDictionary(k => k, v => string.Empty);
        }

        public FilmRow(Mark m)
        {
            var settings = JsonConvert.DeserializeObject<UserSettings>(m.User.Settings);

            MarkId = m.Id;
            MovieName = m.Movie.Title;
            Mark = m.Value;
            Year = settings.ShowYear ? (int?)m.Movie.Year : null;
            Director = settings.ShowDirector ? m.Movie.Director : null;
            Country = settings.ShowCountry ? m.Movie.Country : null;
            ImdbId = settings.ShowImdbId ? m.Movie.ImdbId : null;

            AdditionalFields = new Dictionary<string, string>();
            var allAdditionalFields = JsonConvert.DeserializeObject<Dictionary<string, string>>(m.AdditionalInfo);
            foreach (var name in settings.AdditionalFieldNames)
            {
                if (!allAdditionalFields.ContainsKey(name)) AdditionalFields.Add(name, null);
                else AdditionalFields.Add(name, allAdditionalFields[name]);
            }
        }

        public FilmRow(UserSettings settings, string json, string jsonCrew)
        {
            var details = JObject.Parse(json);
            var crew = JObject.Parse(jsonCrew);

            MarkId = 0;
            MovieName = details.Value<string>("original_title");
            Mark = 1;
            Year = int.Parse(details.Value<string>("release_date").Split('-').First());

            Director = crew
                .Value<JArray>("crew")
                .Where(j => j.Value<string>("job") == "Director")
                .FirstOrDefault()
                ?.Value<string>("name");

            Country = details.Value<JArray>("production_countries").FirstOrDefault()?.Value<string>("iso_3166_1");
            ImdbId = details.Value<string>("imdb_id");

            AdditionalFields = settings.AdditionalFieldNames.ToDictionary(k => k, v => string.Empty);
        }

        public int MarkId { get; set; }
        public string MovieName { get; set; }
        public int Mark { get; set; }
        public int? Year { get; set; }
        public string Director { get; set; }
        public string Country { get; set; }
        public string ImdbId { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
    }
}
