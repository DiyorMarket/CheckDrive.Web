namespace CheckDrive.Web.Services.CurrentUserService;

public interface ICurrentUserService
{
    string GetUserId();
    string GetAccountId();
    string GetRole();
}
