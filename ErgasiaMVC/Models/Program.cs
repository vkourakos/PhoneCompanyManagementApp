using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.Models;

public class Program
{
    [Key]
    public string ProgramName { get; set; } = null!;
    public string Benefits { get; set; } = null!;
    public decimal Charge { get; set; }
}
