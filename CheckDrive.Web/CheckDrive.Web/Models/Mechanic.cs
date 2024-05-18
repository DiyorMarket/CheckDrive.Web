﻿namespace CheckDrive.Web.Models
{
    public class Mechanic
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public virtual ICollection<DispatcherReview> DispetcherReviews { get; set; }
        public virtual ICollection<MechanicHandover> MechanicHandovers { get; set; }
    }
}
