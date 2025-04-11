using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.Models;

public class Call
{
    [Key]
    public string Id { get; set; }
    public string Description { get; set; } = null!;
    public string CallerNumber { get; set; } = null!;
    public string CalleeNumber { get; set; } = null!;

    #region References
    public Phone Caller { get; set; }
    public Phone Callee { get; set; }
    #endregion

    #region Collections
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    #endregion
}
