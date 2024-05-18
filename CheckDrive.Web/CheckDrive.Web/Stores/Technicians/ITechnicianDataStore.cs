using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Technicians
{
    public interface ITechnicianDataStore
    {
        Task<List<Technician>> GetTechnicians();
        Task<Technician> GetTechnician(int id);
        Task<Technician> CreateTechnician(Technician technician);
        Task<Technician> UpdateTechnician(int id, Technician technician);
        Task DeleteTechnician(int id);
    }
}
