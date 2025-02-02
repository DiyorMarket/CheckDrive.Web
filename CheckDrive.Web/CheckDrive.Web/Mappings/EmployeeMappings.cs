using CheckDrive.Web.Requests.Employee;
using CheckDrive.Web.ViewModels.Employee;

namespace CheckDrive.Web.Mappings;

public static class EmployeeMappings
{
    public static UpdateEmployeeRequest ToUpdateViewModel(this EmployeeViewModel employee) =>
        new()
        {
            Id = employee.Id,
            AccountId = employee.AccountId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Patronymic = employee.Patronymic,
            Passport = employee.Passport,
            UserName = employee.UserName,
            Password = employee.Password,
            Address = employee.Address,
            PhoneNumber = employee.PhoneNumber,
            Email = employee.Email,
            Birthdate = employee.Birthdate,
            Position = employee.Position,
            PositionDescription = employee.PositionDescription,
            AssignedCarId = employee.AssignedCarId
        };
}
