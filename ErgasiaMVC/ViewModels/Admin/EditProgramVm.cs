using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.ViewModels.Admin;

public class EditProgramVm
{
    public string ProgramName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Benefits { get; set; } = null!;

    [Required]
    [DataType(DataType.Currency)]
    public decimal Charge { get; set; }
}

