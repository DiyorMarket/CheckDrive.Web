using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.Operators
{
    public interface IOperatorDataStore
    {
        Task<GetOperatorResponse> GetOperators();
        Task<Operator> GetOperator(int id);
        Task<Operator> CreateOperator(Operator @operator);
        Task<Operator> UpdateOperator(int id, Operator @operator);
        Task DeleteOperator(int id);
    }
}
    