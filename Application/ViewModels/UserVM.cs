﻿namespace Application.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string ProfileImage { get; set; }
        public bool Approved { get; set; }
        public bool IsActive { get; set; }
        public string PlainPass { get; set; }
    }
}