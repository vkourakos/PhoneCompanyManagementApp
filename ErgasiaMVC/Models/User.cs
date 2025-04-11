using Microsoft.AspNetCore.Identity;

namespace ErgasiaMVC.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Property { get; set; }
}
