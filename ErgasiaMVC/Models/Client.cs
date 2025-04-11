using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.Models;

public class Client
{
    [Key]
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Afm { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    #region References
    public User? User { get; set; }
    public Phone? Phone { get; set; }
    #endregion
}
