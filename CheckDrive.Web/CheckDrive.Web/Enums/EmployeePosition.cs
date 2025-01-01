using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Web.Enums;

public enum EmployeePosition
{
    [Display(Name = "Haydovchi")]
    Driver = 1,
    [Display(Name = "Shifokor")]
    Doctor = 2,
    [Display(Name = "Mexanik")]
    Mechanic = 3,
    [Display(Name = "Operator")]
    Operator = 4,
    [Display(Name = "Dispecher")]
    Dispatcher = 5,
    [Display(Name = "Menejer")]
    Manager = 6,
    Custom = 7
}
