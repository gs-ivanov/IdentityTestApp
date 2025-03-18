﻿namespace IdentityTestApp.Data
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public string FullName { get; init; }
    }
}
