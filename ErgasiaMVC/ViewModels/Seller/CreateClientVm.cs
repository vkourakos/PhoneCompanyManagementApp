using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.ViewModels.Seller;

public class CreateClientVm
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

    [Required]
    [MaxLength(50)]
    public string Afm { get; set; } = null!;

    [Required]
    [MaxLength(15)]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string ProgramName { get; set; } = null!;

    public List<SelectListItem> Programs { get; set; } = [];
}
