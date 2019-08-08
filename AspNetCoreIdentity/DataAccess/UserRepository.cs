using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.DataAccess
{
    public class UserRepository: IUserStore<User>,IUserPasswordStore<User>
    {
        public void Dispose()
        {

        }

        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection("Data Source=.;Initial Catalog=IdentityDb;Integrated Security=True");

            connection.Open();
            return connection;
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                 await connection.ExecuteAsync("INSERT INTO [dbo].[Users]([Id],[UserName],[NormalizedUserName],[PasswordHash]) VALUES (@id,@userName,@normalizedUserName,@passwordHash)",
                 new
                 {
                     id = user.Id,
                     userName = user.UserName,
                     normalizedUserName = user.NormalizedUserName,
                     passwordHash = user.PasswordHash
                  });
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("UPDATE Users SET UserName = @userName, NormalizedUserName = @normalizedUserName, PasswordHash = @PasswordHash WHERE Id = @id",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    });
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("DELETE FROM Users WHERE Id = @id", new {id = user.Id});

            }
            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id = userId });
            }
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE NormalizedUserName = @name", new { name = normalizedUserName });
            }
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}
