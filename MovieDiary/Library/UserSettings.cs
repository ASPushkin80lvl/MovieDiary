using System.Collections.Generic;

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

        public bool ShowYear { get; set; }
        public bool ShowDirector { get; set; }
        public bool ShowCountry { get; set; }
        public bool ShowImdbId { get; set; }
        public List<string> AdditionalFieldNames { get; set; }
    }
}
