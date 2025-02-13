using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.Services;
using CheckDrive.Web.ViewModels.CheckPoint;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Stores.CheckPoint;

internal sealed class CheckPointStore(CheckDriveApi apiClient) : ICheckPointStore
{
    private static readonly string _resourceUrl = "checkPoints";

    public Task<List<CheckPointViewModel>> GetAllAsync()
        => apiClient.GetAsync<List<CheckPointViewModel>>(_resourceUrl);

    public async Task<CheckPointViewModel> GetByIdAsync(int id)
    {
        var checkPoint = await apiClient.GetAsync<CheckPointViewModel>($"{_resourceUrl}/{id}");

        return checkPoint;
    }

    public List<SelectListItem> GetEnumValues<TEnum>() where TEnum : Enum
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException("TEnum must be an Enum type.");
        }

        return Enum.GetValues(typeof(TEnum))
                   .Cast<TEnum>()
                   .Select(s => new SelectListItem
                   {
                       Value = s.ToString(),
                       Text = s switch
                       {
                           CheckPointStage.DoctorReview => "Doktor tekshiruvi",
                           CheckPointStage.MechanicHandover => "Mexanik (Topshirish)",
                           CheckPointStage.OperatorReview => "Operator tekshiruvi",
                           CheckPointStage.MechanicAcceptance => "Mexanik (Qabul qilish)",
                           CheckPointStage.DispatcherReview => "Dispatcher tekshiruvi",
                           CheckPointStage.ManagerReview => "Menejer tekshiruvi",

                           CheckPointStatus.InProgress => "Jarayonda",
                           CheckPointStatus.Completed => "Yakunlangan",
                           CheckPointStatus.InterruptedByReviewerRejection => "Ko'rib chiquvchi rad etdi",
                           CheckPointStatus.InterruptedByDriverRejection => "Haydovchi rad etdi",
                           CheckPointStatus.AutomaticallyClosed => "Avtomatik ravishda yopilgan",
                           CheckPointStatus.PendingManagerReview => "Menejer (Ko'rilmoqda)",
                           CheckPointStatus.ClosedByManager => "Menejer tomonidan yopilgan",

                           _ => s.ToString()
                       }
                   })
                   .ToList();
    }
}
