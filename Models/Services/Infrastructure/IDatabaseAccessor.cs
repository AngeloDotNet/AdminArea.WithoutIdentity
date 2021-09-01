using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore_SimpleLogin.Models.Services.Infrastructure
{
    public interface IDatabaseAccessor
    {
        Task<DataSet> QueryAsync(FormattableString formattableQuery, CancellationToken token = default(CancellationToken));
        Task<T> QueryScalarAsync<T>(FormattableString formattableQuery, CancellationToken token = default(CancellationToken));
        Task<int> CommandAsync(FormattableString formattableCommand, CancellationToken token = default(CancellationToken));
    }
}