namespace MovieDiary.Library
{
    public class Mark
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
        public int Value { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
