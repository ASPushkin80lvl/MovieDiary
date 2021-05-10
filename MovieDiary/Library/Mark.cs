namespace MovieDiary.Library
{
    public class Mark
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Movie Movie { get; set; }
        public int Value { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
