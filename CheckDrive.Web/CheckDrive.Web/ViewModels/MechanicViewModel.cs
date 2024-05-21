using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.MechanicAcceptance;

public class MechanicViewModel
{
    public List<MechanicAcceptanceDto> MechanicAcceptances { get; set; }
    public List<MechanicDto> Mechanics { get; set; }
}
