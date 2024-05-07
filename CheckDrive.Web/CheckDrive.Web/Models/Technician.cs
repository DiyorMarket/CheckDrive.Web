namespace CheckDrive.Web.Models
{
    public class Technician
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
