using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieDiary.Library
{
    public class MovieApiSearch
    {
        [JsonProperty("results")]
        public List<MovieApiRecord> Results { get; set; }
    }

    public class MovieApiRecord
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("original_title")]
        public string Title { get; set; }
        [JsonProperty("release_date")]
        public string Year { get; set; }
    }
}
