using System.ComponentModel.DataAnnotations;

namespace CheckDrive.Web.Models.Enums;

public enum DebtStatus
{
    [Display(Name = "To'langan")]
    Paid = 0,

    [Display(Name = "To'lanmagan")]
    Unpaid = 1,

    [Display(Name = "Qisman to'langan")]
    PartiallyPaid,

    [Display(Name = "Menejerni kutilmoqda")]
    PendingManagerApproval
}
