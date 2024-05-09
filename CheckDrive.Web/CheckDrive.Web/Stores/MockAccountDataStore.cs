using CheckDrive.Web.Models;

namespace CheckDrive.Web.Stores.Accounts
{
    public class MockAccountDataStore : IAccountDataStore
    {
        private readonly List<Account> _accounts;

        public MockAccountDataStore()
        {
            _accounts = new List<Account>
            {
                new Account { Id = 1, Login = "user1", Password = "password1", PhoneNumber = "123456789", FirstName = "John", LastName = "Doe", Bithdate = new DateTime(1990, 1, 1), RoleId = 1 },
                new Account { Id = 2, Login = "user2", Password = "password2", PhoneNumber = "987654321", FirstName = "Jane", LastName = "Siu", Bithdate = new DateTime(1995, 5, 15), RoleId = 2 },
            };
        }

        public async Task<List<Account>> GetAccounts()
        {
            await Task.Delay(100);
            return _accounts.ToList();
        }

        public async Task<Account> GetAccount(int id)
        {
            await Task.Delay(100);
            return _accounts.FirstOrDefault(a => a.Id == id);
        }

        public async Task<Account> CreateAccount(Account account)
        {
            await Task.Delay(100); 
            account.Id = _accounts.Max(a => a.Id) + 1; 
            _accounts.Add(account);
            return account;
        }

        public async Task<Account> UpdateAccount(int id, Account account)
        {
            await Task.Delay(100); 
            var existingAccount = _accounts.FirstOrDefault(a => a.Id == id);
            if (existingAccount != null)
            {
                existingAccount.Login = account.Login;
                existingAccount.Password = account.Password;
                existingAccount.PhoneNumber = account.PhoneNumber;
                existingAccount.FirstName = account.FirstName;
                existingAccount.LastName = account.LastName;
                existingAccount.Bithdate = account.Bithdate;
                existingAccount.RoleId = account.RoleId;
            }
            return existingAccount;
        }

        public async Task DeleteAccount(int id)
        {
            await Task.Delay(100); 
            var accountToRemove = _accounts.FirstOrDefault(a => a.Id == id);
            if (accountToRemove != null)
            {
                _accounts.Remove(accountToRemove);
            }
        }
    }
}
