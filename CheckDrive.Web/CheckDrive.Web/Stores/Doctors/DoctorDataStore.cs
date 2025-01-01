using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Doctor;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.Doctors;

public class DoctorDataStore : IDoctorDataStore
{
    private readonly CheckDriveApi _apiClient;

    public DoctorDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<DoctorDto> CreateDoctor(DoctorForCreateDto doctorForCreate)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDoctor(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorDto> GetDoctor(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetDoctorResponse> GetDoctors(int accountId)
    {
        throw new NotImplementedException();
    }

    public Task<GetDoctorResponse> GetDoctors()
    {
        throw new NotImplementedException();
    }

    public Task<DoctorDto> UpdateDoctor(int id, AccountForUpdateDto doctorForUpdate)
    {
        throw new NotImplementedException();
    }
}
