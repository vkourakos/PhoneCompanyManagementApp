using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.ViewModels.Seller;

public class ChangeProgramVm
{
    public string ClientId { get; set; } = null!;

    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [Display(Name = "Current Program")]
    public string CurrentProgram { get; set; } = null!;

    [Required]
    [Display(Name = "New Program")]
    public string NewProgram { get; set; } = null!;

    public List<SelectListItem>? Programs { get; set; }
}
