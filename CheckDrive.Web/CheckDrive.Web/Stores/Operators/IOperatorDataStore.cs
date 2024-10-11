using CheckDrive.ApiContracts.Operator;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Operators;

public interface IOperatorDataStore
{
    Task<GetOperatorResponse> GetOperatorsAsync(string? searchString, int? pageNumber);
    Task<GetOperatorResponse> GetOperatorsAsync();
    Task<OperatorDto> GetOperatorByIdAsync(int id);
    Task<OperatorDto> CreateOperatorAsync(OperatorForCreateDto operatorForCreate);
}


