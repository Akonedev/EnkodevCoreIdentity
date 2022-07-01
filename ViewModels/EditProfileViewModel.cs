﻿namespace EnkodevCoreIdentity.ViewModels
{
    public class EditProfileViewModel
    {
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public IFormFile? Image { get; set; }
        public string? RoleId { get; set; }

        public string? Role { get; set; }
    }
}