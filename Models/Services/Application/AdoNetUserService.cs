using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdminArea_Without_Identity.Models.Options;
using AspNetCore_SimpleLogin.Models.Services.Infrastructure;
using AspNetCore_SimpleLogin.Models.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCore_SimpleLogin.Models.Services.Application
{
    public class AdoNetUserService : IUserService
    {
        private readonly ILogger<AdoNetUserService> logger;
        private readonly IDatabaseAccessor db;
        private readonly IOptionsMonitor<SecurityOptions> securityOptionsMonitor;

        public AdoNetUserService(ILogger<AdoNetUserService> logger, IDatabaseAccessor db, IOptionsMonitor<SecurityOptions> securityOptionsMonitor)
        {
            this.logger = logger;
            this.db = db;
            this.securityOptionsMonitor = securityOptionsMonitor;
        }

        public async Task<bool> IsLoginCorrectAsync(string username, string password)
        {
            bool userExists = await db.QueryScalarAsync<bool>($"SELECT COUNT(*) FROM Utenti WHERE Username LIKE {username} AND Password LIKE {password}");
            return userExists;
        }

        public async Task<UserDetailViewModel> GetUserAsync(string username)
        {
            FormattableString query = $@"SELECT Id, Cognome, Nome, Email, Username, Password FROM Utenti WHERE Username LIKE {username}";

            DataSet dataSet = await db.QueryAsync(query);

            var userTable = dataSet.Tables[0];

            if (userTable.Rows.Count != 1)
            {
                logger.LogWarning("User not found !");
                throw new Exception();
            }

            var userRow = userTable.Rows[0];
            var userDetailViewModel = UserDetailViewModel.FromDataRow(userRow);

            return userDetailViewModel;
        }

        public string PasswordSalt(string password)
        {
            var options = this.securityOptionsMonitor.CurrentValue;

            //Per le proprietà di HashAlgorithmName consultare il link: https://docs.microsoft.com/it-it/dotnet/api/system.security.cryptography.hashalgorithmname?view=net-5.0
            using var deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(options.PasswordSalt), 
                                                        options.Iterations, HashAlgorithmName.SHA256);
            
            byte[] bytes = deriveBytes.GetBytes(256 / 8);
            string passwordCifrata = Convert.ToBase64String(bytes);

            return passwordCifrata;
        }
    }
}