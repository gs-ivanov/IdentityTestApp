namespace IdentityTestApp.Data
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public string FullName { get; init; }

        public IEnumerable<Cat> Cats { get; init; } = new List<Cat>();
    }
}
