using CheckDrive.Web.Configurations;
using CheckDrive.Web.Constants;
using CheckDrive.Web.Filters;
using CheckDrive.Web.Helpers;
using CheckDrive.Web.Services;
using CheckDrive.Web.Services.CookieHandler;
using CheckDrive.Web.Services.CurrentUserService;
using CheckDrive.Web.Stores.Auth;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.CheckPoint;
using CheckDrive.Web.Stores.Dashboard;
using CheckDrive.Web.Stores.Debts;
using CheckDrive.Web.Stores.Employee;
using CheckDrive.Web.Stores.Menu;
using CheckDrive.Web.Stores.OilMarks;
using Syncfusion.Licensing;

namespace CheckDrive.Web.Extensions;

internal static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddControllers(services);
        AddConfigurations(services, configuration);
        AddStores(services);
        AddServices(services);
        AddHttpClient(services, configuration);
        RegisterLicenses(configuration);

        return services;
    }

    private static void AddControllers(IServiceCollection services)
    {
        services.AddControllersWithViews(options => options.Filters.Add<ExceptionFilter>());
    }

    private static void AddConfigurations(IServiceCollection services, IConfiguration configuration)
    {

    }

    private static void AddStores(IServiceCollection services)
    {
        services.AddScoped<ICarStore, CarStore>();
        services.AddScoped<IOilMarkStore, OilMarkStore>();
        services.AddScoped<IDashboardStore, DashboardStore>();
        services.AddScoped<IDebtsStore, DebtsStore>();
        services.AddScoped<ICheckPointStore, CheckPointStore>();
        services.AddScoped<IAuthStore, AuthStore>();
        services.AddScoped<IEmployeeStore, EmployeeStore>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICookieHandler, CookieHandler>();
        services.AddTransient<AuthorizationHandler>();
    }

    private static void AddHttpClient(IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(CheckDriveApiSettings.SectionName)
            .Get<CheckDriveApiSettings>();

        if (settings is null)
        {
            throw new InvalidOperationException("Cannot setup API Client without configuration settings.");
        }

        services.AddHttpClient<CheckDriveApi>((client) =>
        {
            client.BaseAddress = new Uri(settings.BaseAddress);
            client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }).AddHttpMessageHandler<AuthorizationHandler>();
    }

    private static void RegisterLicenses(IConfiguration configuration)
    {
        var key = configuration.GetValue<string>(ConfigurationConstants.SynfusionLicenseKey);

        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("Cannot register Syncfusion when key is empty.");
        }

        SyncfusionLicenseProvider.RegisterLicense(key);
    }
}
