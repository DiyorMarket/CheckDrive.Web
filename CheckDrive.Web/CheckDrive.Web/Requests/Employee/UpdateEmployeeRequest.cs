namespace CheckDrive.Web.Requests.Employee;

public class UpdateEmployeeRequest : CreateEmployeeRequest
{
    public int Id { get; set; }
    public required string AccountId { get; set; }
}