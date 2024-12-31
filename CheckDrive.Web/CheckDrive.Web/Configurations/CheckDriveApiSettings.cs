using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Web.Configurations;

public sealed class CheckDriveApiSettings
{
    public const string SectionName = "CheckDriveApi";

    [Required(ErrorMessage = "API Base address is required.")]
    public required string BaseAddress { get; init; }

    [Required(ErrorMessage = "Timeout seconds required.")]
    public int TimeoutSeconds { get; set; }
}
