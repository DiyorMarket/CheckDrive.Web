using CheckDrive.Web.Configurations;
using CheckDrive.Web.Constants;
using CheckDrive.Web.Filters;
using CheckDrive.Web.Helpers;
using CheckDrive.Web.Services;
using CheckDrive.Web.Services.CookieHandler;
using CheckDrive.Web.Services.CurrentUserService;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Auth;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.CheckPoint;
using CheckDrive.Web.Stores.Dashbord;
using CheckDrive.Web.Stores.Debts;
using CheckDrive.Web.Stores.DispatcherReviews;
using CheckDrive.Web.Stores.Dispatchers;
using CheckDrive.Web.Stores.DoctorReviews;
using CheckDrive.Web.Stores.Doctors;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.Employee;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.Mechanics;
using CheckDrive.Web.Stores.Menu;
using CheckDrive.Web.Stores.MockDashboard;
using CheckDrive.Web.Stores.OilMarks;
using CheckDrive.Web.Stores.OperatorReviews;
using CheckDrive.Web.Stores.Operators;
using CheckDrive.Web.Stores.Roles;
using CheckDrive.Web.Stores.Technicians;
using CheckDrive.Web.Stores.User;
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
        services.AddScoped<IAccountDataStore, AccountDataStore>();
        services.AddScoped<ICarStore, CarStore>();
        services.AddScoped<IDispatcherReviewDataStore, DispatcherReviewDataStore>();
        services.AddScoped<IDispatcherDataStore, DispatcherDataStore>();
        services.AddScoped<IDoctorDataStore, DoctorDataStore>();
        services.AddScoped<IDoctorReviewDataStore, DoctorReviewDataStore>();
        services.AddScoped<IDriverDataStore, DriverDataStore>();
        services.AddScoped<IMechanicAcceptanceDataStore, MechanicAcceptanceDataStore>();
        services.AddScoped<IMechanicHandoverDataStore, MechanicHandoverDataStore>();
        services.AddScoped<IMechanicDataStore, MechanicDataStore>();
        services.AddScoped<IOilMarkStore, OilMarkStore>();
        services.AddScoped<IOperatorReviewDataStore, OperatorReviewDataStore>();
        services.AddScoped<IOperatorDataStore, OperatorDataStore>();
        services.AddScoped<IRoleDataStore, RoleDataStore>();
        services.AddScoped<ITechnicianDataStore, MockTechnicianDataStore>();
        services.AddScoped<IDashboardStore, DashboardStore>();
        services.AddScoped<IMockDashboardStore, MockDashboardStore>();
        services.AddScoped<IDebtsStore, DebtsStore>();
        services.AddScoped<IUserDataStore, UserDataStore>();
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
