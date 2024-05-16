﻿using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Drivers
{
    public interface IDriverDataStore
    {
        Task<GetDriverResponse> GetDrivers();
        Task<Driver> GetDriver(int id);
        Task<Driver> CreateDriver(Driver driver);
        Task<Driver> UpdateDriver(int id, Driver driver);
        Task DeleteDriver(int id);
    }
}
