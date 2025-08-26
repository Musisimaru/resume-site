using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MU.CV.DAL.Common;

public interface ISelectorRepository<TEntity>
{
    public Task<TResult?> GetAsync<TResult>(Guid id, Expression<Func<TEntity, TResult>> selector, CancellationToken ct = default);
    public Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken ct = default);
    public Task<IEnumerable<TResult>> GetPageAsync<TResult>(int page, int size, Expression<Func<TEntity, TResult>> selector, CancellationToken ct = default);
    public ConfiguredCancelableAsyncEnumerable<TResult> StreamAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken ct = default);
}