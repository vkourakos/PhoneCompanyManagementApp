using ErgasiaMVC.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ErgasiaMVC.Models;

public class Bill
{
    [Key]
    public string Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public decimal Costs { get; set; }
    public BillStatus Status { get; set; } = BillStatus.Issued;

    #region References
    public Phone? Phone { get; set; }
    #endregion

    #region Collections
    public ICollection<Call> Calls { get; set; } = new List<Call>();
    #endregion
}
