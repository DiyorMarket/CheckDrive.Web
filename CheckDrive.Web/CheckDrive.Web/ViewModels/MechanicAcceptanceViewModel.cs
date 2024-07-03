using CheckDrive.ApiContracts.Car;
using CheckDrive.ApiContracts.Driver;

namespace CheckDrive.Web.ViewModels
{
    public class MechanicAcceptanceViewModel
    {
        public List<DriverDto> Drivers { get; set; }
        public CarDto Car { get; set; }
    }
}
