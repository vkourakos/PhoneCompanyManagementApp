using ErgasiaMVC.Models.Enums;

namespace ErgasiaMVC.ViewModels.Client;

public class ViewBillVm
{
    public string Id { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public decimal Costs { get; set; }
    public string ProgramName { get; set; } = null!;
    public BillStatus Status { get; set; }
}

