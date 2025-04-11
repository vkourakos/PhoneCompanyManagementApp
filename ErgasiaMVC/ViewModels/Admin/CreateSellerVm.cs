using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.ViewModels.Admin;

public class CreateSellerVm
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(25)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
