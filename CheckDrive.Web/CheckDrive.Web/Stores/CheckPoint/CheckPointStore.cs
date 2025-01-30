using CheckDrive.Web.Models.Enums;
using CheckDrive.Web.Services;
using CheckDrive.Web.ViewModels.CheckPoint;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckDrive.Web.Stores.CheckPoint;

internal sealed class CheckPointStore : ICheckPointStore
{
    private static readonly string _resourceUrl = "checkPoints";
    private readonly CheckDriveApi _apiClient;

    public CheckPointStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public Task<List<CheckPointViewModel>> GetAllAsync()
        => _apiClient.GetAsync<List<CheckPointViewModel>>(_resourceUrl);

    public async Task<CheckPointViewModel> GetCheckPointByIdAsync(int id)
    {
        var checkPoints = await _apiClient.GetAsync<List<CheckPointViewModel>>(_resourceUrl);

        var result = checkPoints.FirstOrDefault(x => x.Id == id);

        return result;
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
