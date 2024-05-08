﻿namespace CheckDrive.Web.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public double MeduimFuelConsumption { get; set; }
        public double FuelTankCapacity { get; set; }
        public int ManufacturedYear { get; set; }

        public virtual ICollection<MechanicHandover> MechanicHandovers { get; set; }
    }
}
