namespace MU.CV.DAL.Common;

public interface IProjectorRepository<TEntity>
{
    public Task<TResult?> GetAsync<TResult>(Guid id, Func<TEntity, TResult> projector, CancellationToken ct = default);
    public Task<IEnumerable<TResult>> GetAllAsync<TResult>(Func<TEntity, TResult> projector, CancellationToken ct = default);
    public Task<IEnumerable<TResult>> GetPageAsync<TResult>(int page, int size, Func<TEntity, TResult> projector, CancellationToken ct = default);
    public IAsyncEnumerable<TResult> StreamAllAsync<TResult>(Func<TEntity, TResult> projector, CancellationToken ct = default);
}