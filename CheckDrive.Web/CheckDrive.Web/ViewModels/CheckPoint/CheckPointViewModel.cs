using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.ViewModels.CheckPoint
{
    public class CheckPointViewModel
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string Driver { get; set; }
        public string Doctor { get; set; }
        public string Mechanic { get; set; }
        public string Operator { get; set; }
        public string Dispatcher { get; set; }
        public DebtStatus Status { get; set; }

    }
}
