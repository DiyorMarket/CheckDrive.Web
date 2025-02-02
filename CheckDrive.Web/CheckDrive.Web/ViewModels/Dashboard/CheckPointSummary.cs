using CheckDrive.Web.Models.Enums;

namespace CheckDrive.Web.ViewModels.Dashboard;

public sealed record CheckPointSummary(
    int Id,
    DateTime StartDate,
    string DriverName,
    string Car,
    CheckPointStage Stage,
    CheckPointStatus Status);