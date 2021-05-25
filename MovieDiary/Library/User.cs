namespace MovieDiary.Library
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Settings { get; set; }
        public bool Admin { get; set; }
        public bool LoggedIn { get; set; }
    }
}
