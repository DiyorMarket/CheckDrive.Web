namespace CheckDrive.Web.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public ICollection<DoctorReview>? DoctorReviews { get; set; }
    }
}
