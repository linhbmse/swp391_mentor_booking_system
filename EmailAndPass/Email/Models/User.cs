﻿namespace Email.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string ProfilePhoto { get; set; }
        public string Role { get; set; } = "default";
        public bool IsFirstLogin { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}
