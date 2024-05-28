using CheckDrive.Web.Service;
using CheckDrive.Web.Stores.Accounts;
using CheckDrive.Web.Stores.Cars;
using CheckDrive.Web.Stores.DispatcherReviews;
using CheckDrive.Web.Stores.Dispatchers;
using CheckDrive.Web.Stores.DoctorReviews;
using CheckDrive.Web.Stores.Doctors;
using CheckDrive.Web.Stores.Drivers;
using CheckDrive.Web.Stores.MechanicAcceptances;
using CheckDrive.Web.Stores.MechanicHandovers;
using CheckDrive.Web.Stores.Mechanics;
using CheckDrive.Web.Stores.OperatorReviews;
using CheckDrive.Web.Stores.Operators;
using CheckDrive.Web.Stores.Roles;
using CheckDrive.Web.Stores.Technicians;

namespace CheckDrive.Web.Extensions
{
    public static class ConfigureServiceExtensions
    {
        public static IServiceCollection ConfigureDataStores(this IServiceCollection services)
        {
            services.AddScoped<IAccountDataStore, AccountDataStore>();
            services.AddScoped<ICarDataStore, CarDataStore>();
            services.AddScoped<IDispatcherReviewDataStore, MockDispatcherReviewDataStore>();
            services.AddScoped<IDispatcherDataStore, MockDispatcherDataStore>();
            services.AddScoped<IDoctorDataStore, MockDoctorDataStore>();
            services.AddScoped<IDoctorReviewDataStore, DoctorReviewDataStore>();
            services.AddScoped<IDriverDataStore, DriverDataStore>();
            services.AddScoped<IMechanicAcceptanceDataStore, MechanicAcceptanceDataStore>();
            services.AddScoped<IMechanicHandoverDataStore, MechanicHandoverDataStore>();
            services.AddScoped<IMechanicDataStore, MechanicDataStore>();
            services.AddScoped<IOperatorReviewDataStore, OperatorReviewDataStore>();
            services.AddScoped<IOperatorDataStore, OperatorDataStore>();
            services.AddScoped<IRoleDataStore, RoleDataStore>();
            services.AddScoped<ITechnicianDataStore, MockTechnicianDataStore>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<ApiClient>();

            return services;
        }
    }
}
