﻿using Microsoft.AspNetCore.Identity;

namespace Email.Models
{
    
    public class ApplicationUser : IdentityUser
    {
        public bool IsFirstLogin {  get; set; } = true;
    }
}
