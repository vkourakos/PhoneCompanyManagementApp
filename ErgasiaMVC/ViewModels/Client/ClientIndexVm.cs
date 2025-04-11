using ErgasiaMVC.Models;

namespace ErgasiaMVC.ViewModels.Client;

public class ClientIndexVm
{
    public string ClientId { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public List<Bill> Bills { get; set; } = [];
}
