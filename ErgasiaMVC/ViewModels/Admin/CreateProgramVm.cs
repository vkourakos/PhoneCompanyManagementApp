using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.ViewModels.Admin;

public class CreateProgramVm
{
    [Required]
    [MaxLength(50)]
    public string ProgramName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Benefits { get; set; } = null!;

    [Required]
    public decimal Charge { get; set; }
}
