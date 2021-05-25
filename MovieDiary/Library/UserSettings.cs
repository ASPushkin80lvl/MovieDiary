using System.Collections.Generic;
using System.Linq;

namespace MovieDiary.Library
{
    public class UserSettings
    {
        public UserSettings()
        {
            ShowYear = true;
            ShowDirector = true;
            ShowCountry = true;
            ShowImdbId = true;
            AdditionalFieldNames = new List<string>();
        }

        public override string ToString()
        {
            return $"{BoolToString(ShowYear)}{BoolToString(ShowDirector)}{BoolToString(ShowCountry)}{BoolToString(ShowImdbId)}{string.Join(",", AdditionalFieldNames)}";
        }

        public UserSettings(string query)
        {
            ShowYear = query.Substring(0, 1) == "1";
            ShowDirector = query.Substring(1, 1) == "1";
            ShowCountry = query.Substring(2, 1) == "1";
            ShowImdbId = query.Substring(3, 1) == "1";
            AdditionalFieldNames = query.Substring(4, query.Length).Split(",").ToList();
        }

        private string BoolToString(bool b) => b ? "1" : "0";

        public bool ShowYear { get; set; }
        public bool ShowDirector { get; set; }
        public bool ShowCountry { get; set; }
        public bool ShowImdbId { get; set; }
        public List<string> AdditionalFieldNames { get; set; }
    }
}
