using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Operators
{
    public class MockOperatorDataStore : IOperatorDataStore
    {
        private readonly List<Operator> _operators;

        public MockOperatorDataStore()
        {
            _operators = new List<Operator>
            {
                new Operator { Id = 1, AccountId = 1 },
                new Operator { Id = 2, AccountId = 2 },
            };
        }

        public async Task<List<Operator>> GetOperators()
        {
            await Task.Delay(100); 
            return _operators.ToList();
        }

        public async Task<Operator> GetOperator(int id)
        {
            await Task.Delay(100);
            return _operators.FirstOrDefault(op => op.Id == id);
        }

        public async Task<Operator> CreateOperator(Operator @operator)
        {
            await Task.Delay(100);
            @operator.Id = _operators.Max(op => op.Id) + 1; 
            _operators.Add(@operator);
            return @operator;
        }

        public async Task<Operator> UpdateOperator(int id, Operator @operator)
        {
            await Task.Delay(100);
            var existingOperator = _operators.FirstOrDefault(op => op.Id == id);
            if (existingOperator != null)
            {
                existingOperator.AccountId = @operator.AccountId;
            }
            return existingOperator;
        }

        public async Task DeleteOperator(int id)
        {
            await Task.Delay(100); 
            var operatorToRemove = _operators.FirstOrDefault(op => op.Id == id);
            if (operatorToRemove != null)
            {
                _operators.Remove(operatorToRemove);
            }
        }
    }
}
