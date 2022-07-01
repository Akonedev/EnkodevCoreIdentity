using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnkodevCoreIdentity.ViewModels;

public class UserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public int? Pace { get; set; }
    public int? Mileage { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? RoleId { get; set; }

    public string? Role { get; set; }
    public List<string>? RoleList { get; set; }


    public string Location => (City, State) switch
    {
        (string city, string state) => $"{city}, {state}",
        (string city, null) => city,
        (null, string state) => state,
        (null, null) => "",
    };
}