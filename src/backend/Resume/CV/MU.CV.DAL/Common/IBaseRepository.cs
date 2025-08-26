using System.Runtime.CompilerServices;

namespace MU.CV.DAL.Common;

public interface IBaseRepository<TEntity> where TEntity : BaseDbEntity
{
    public TEntity Add(TEntity entity);
    public void Update(TEntity entity);
    public Task UpdateAsync(Guid id, Action<TEntity> mutate, CancellationToken ct = default);
    public Task DeleteAsync(Guid id, CancellationToken ct = default);
    
    public Task<TEntity?> GetAsync(Guid id, CancellationToken ct = default);
    public Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default);
    public Task<IReadOnlyList<TEntity>> GetPageAsync(int page, int size, CancellationToken ct = default);
    public ConfiguredCancelableAsyncEnumerable<TEntity> StreamAllAsync(CancellationToken ct = default);
}