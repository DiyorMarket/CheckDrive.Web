using CheckDrive.Web.Requests.Employee;
using CheckDrive.Web.ViewModels.Employee;

namespace CheckDrive.Web.Stores.Employee;

public interface IEmployeeStore
{
    Task<List<EmployeeViewModel>> GetAsync();
    Task<EmployeeViewModel> GetByIdAsync(int id);
    Task<EmployeeViewModel> CreateAsync(CreateEmployeeRequest request);
    Task UpdateAsync(UpdateEmployeeRequest request);
    Task DeleteAsync(int id);
}
