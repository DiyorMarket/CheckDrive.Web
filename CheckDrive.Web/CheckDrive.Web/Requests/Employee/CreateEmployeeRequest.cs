using CheckDrive.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Web.Requests.Employee;

public class CreateEmployeeRequest
{
    [Required(ErrorMessage = "Ism majburiy")]
    [Length(3, 255, ErrorMessage = "Belgilar soni 3 va 255 o'rtasida bo'lishi kerak")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Familiya majburiy")]
    [Length(3, 255, ErrorMessage = "Belgilar soni 3 va 255 o'rtasida bo'lishi kerak")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Sharf majburiy")]
    [Length(3, 255, ErrorMessage = "Belgilar soni 3 va 255 o'rtasida bo'lishi kerak")]
    public required string Patronymic { get; set; }

    [Required(ErrorMessage = "Passport majburiy")]
    [Length(5, 15, ErrorMessage = "Belgilar soni 5 va 15 o'rtasida bo'lishi kerak")]
    public required string Passport { get; set; }

    [Required(ErrorMessage = "Foydalanvuchi ismi majburiy")]
    [Length(3, 25, ErrorMessage = "Belgilar soni 3 va 25 o'rtasida bo'lishi kerak")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Foydalanvuchi ismi majburiy")]
    [Length(3, 25, ErrorMessage = "Belgilar soni 8 va 25 o'rtasida bo'lishi kerak")]
    public required string Password { get; set; }

    [MaxLength(255, ErrorMessage = "Belgilar soni 255dan kam bo'lishi kerak")]
    public string? Address { get; set; }

    [DataType(DataType.PhoneNumber, ErrorMessage = "Telefon raqam noto'gri formatda")]
    public string? PhoneNumber { get; set; }

    [DataType(DataType.EmailAddress, ErrorMessage = "Elektron manzil noto'g'ri formatda")]
    public string? Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime? Birthdate { get; set; }

    public required EmployeePosition Position { get; set; }

    [MaxLength(500, ErrorMessage = "Belgilar soni 500dan kam bo'lishi kerak")]
    public string? PositionDescription { get; set; }

    public int? AssignedCarId { get; set; }
}