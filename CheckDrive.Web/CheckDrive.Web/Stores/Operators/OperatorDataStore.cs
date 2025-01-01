using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Services;

namespace CheckDrive.Web.Stores.Operators;

public class OperatorDataStore : IOperatorDataStore
{
    private readonly CheckDriveApi _apiClient;

    public OperatorDataStore(CheckDriveApi apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<Operator> CreateOperator(Operator @operator)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOperator(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Operator> GetOperator(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GetOperatorResponse> GetOperators(int accountId)
    {
        throw new NotImplementedException();
    }

    public Task<GetOperatorResponse> GetOperators()
    {
        throw new NotImplementedException();
    }

    public Task<Operator> UpdateOperator(int id, Operator @operator)
    {
        throw new NotImplementedException();
    }
}
