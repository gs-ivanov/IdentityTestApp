namespace IdentityTestApp.Data
{
    public class Cat
    {
        public int Id { get; init; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string UserId { get; set; }

        public User User { get; init; }
    }
}
