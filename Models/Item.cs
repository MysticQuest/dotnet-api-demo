namespace Models
{
    public class Item : IEntity
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string DateTime { get; set; }
        public string HttpCode { get; set; }
    }
}
