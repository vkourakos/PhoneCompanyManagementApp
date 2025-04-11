using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.Models;

public class Seller
{
    [Key]
    public string Id { get; set; }
    public string UserId { get; set; }

    #region References
    public User? User { get; set; }
    #endregion
}
