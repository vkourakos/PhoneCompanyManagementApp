using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.Models;

public class Phone
{
    [Key]
    public string PhoneNumber { get; set; } = null!;
    public string ProgramName { get; set; } = null!;

    #region References
    public Program? Program { get; set; }
    #endregion
}
