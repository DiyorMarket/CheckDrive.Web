﻿namespace CheckDrive.Web.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Bithdate { get; set; }

        public string RoleName { get; set; }
        public Role Role { get; set; }

        public virtual ICollection<Dispatcher> Dispatchers { get; set; }
        public virtual ICollection<Operator> Operators { get; set; }
        public virtual ICollection<Mechanic> Mechanics { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Technician> Technicians { get; set; }
    }
}

