using CheckDrive.Web.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CheckDrive.Web.ViewModels.Employee;

public class EmployeeViewModel
{
    public int Id { get; set; }
    public int AssignedCarId { get; set; }
    public required string AccountId { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Patronymic { get; set; }
    public required string Passport { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public DateTime? Birthdate { get; set; }
    public EmployeePosition Position { get; set; }
    public string? PositionDescription { get; set; }

    public string FormattedPosition => Position
        .GetType()
        .GetField(Position.ToString())?
        .GetCustomAttribute<DisplayAttribute>()?.Name
        ?? Position.ToString();
}
