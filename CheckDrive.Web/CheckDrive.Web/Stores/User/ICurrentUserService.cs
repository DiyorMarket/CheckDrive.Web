using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CheckDrive.Web.Stores.User;

public interface ICurrentUserService
{
    string GetRole();
}
