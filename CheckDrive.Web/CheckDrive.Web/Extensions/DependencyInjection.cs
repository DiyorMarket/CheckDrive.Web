using CheckDrive.Web.Constants;
using CheckDrive.Web.Filters;
using CheckDrive.Web.Service;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.Dashbord;
using CheckDrive.Web.Stores.DispatcherReviews;
using CheckDrive.Web.Stores.Dispatchers;
using CheckDrive.Web.Stores.DoctorReviews;
using CheckDrive.Web.Stores.Doctors;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.Mechanics;
using CheckDrive.Web.Stores.OilMarks;
using CheckDrive.Web.Stores.OperatorReviews;
using CheckDrive.Web.Stores.Operators;
using CheckDrive.Web.Stores.Roles;
using CheckDrive.Web.Stores.Technicians;
using CheckDrive.Web.Stores.User;
using Syncfusion.Licensing;

namespace CheckDrive.Web.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddControllers(services);
        AddStores(services);
        AddServices(services);

        RegisterSyncfusion(configuration);

        services.AddHttpContextAccessor();

        return services;
    }

    private static void AddControllers(IServiceCollection services)
    {
        services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new ApiExceptionFilter());
        });
    }

    private static void AddStores(IServiceCollection services)
    {
        services.AddScoped<IAccountDataStore, AccountDataStore>();
        services.AddScoped<ICarDataStore, CarDataStore>();
        services.AddScoped<IDispatcherReviewDataStore, DispatcherReviewDataStore>();
        services.AddScoped<IDispatcherDataStore, DispatcherDataStore>();
        services.AddScoped<IDoctorDataStore, DoctorDataStore>();
        services.AddScoped<IDoctorReviewDataStore, DoctorReviewDataStore>();
        services.AddScoped<IDriverDataStore, DriverDataStore>();
        services.AddScoped<IMechanicAcceptanceDataStore, MechanicAcceptanceDataStore>();
        services.AddScoped<IMechanicHandoverDataStore, MechanicHandoverDataStore>();
        services.AddScoped<IMechanicDataStore, MechanicDataStore>();
        services.AddScoped<IOilMarkDataStore, OilMarkDataStore>();
        services.AddScoped<IOperatorReviewDataStore, OperatorReviewDataStore>();
        services.AddScoped<IOperatorDataStore, OperatorDataStore>();
        services.AddScoped<IRoleDataStore, RoleDataStore>();
        services.AddScoped<ITechnicianDataStore, MockTechnicianDataStore>();
        services.AddScoped<IDashboardStore, DashboardStore>();
        services.AddScoped<IUserDataStore, UserDataStore>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ApiClient>();
    }

    private static void RegisterSyncfusion(IConfiguration configuration)
    {
        var key = configuration.GetValue<string>(Configurations.SyncfusionSection);

        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("Syncfusion key cannot be empty.");
        }

        SyncfusionLicenseProvider.RegisterLicense(key);
    }
}
