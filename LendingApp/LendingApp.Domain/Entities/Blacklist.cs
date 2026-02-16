namespace LendingApp.Domain.Entities
{
    public class Blacklist
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
