using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Employees
{
    public interface IEmployeeDataStore
    {
        Task<GetEmployeeResponse> GetEmployeesAsync(string? searchString, int? pageNumber);
        Task<GetEmployeeResponse> GetEmployeesAsync();
        Task<EmployeeDto> GetEmployeeAsync(int id);
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeForCreateDto employeeForCreate);
    }
}
