using Microsoft.EntityFrameworkCore;
using MU.CV.DAL.DataContext;
using MU.CV.DAL.Exceptions;

namespace MU.CV.DAL.Common;

public abstract class BaseCVProjectorRepository<TEntity>(CVDbContext context) : BaseCVRepository<TEntity>(context), IProjectorRepository<TEntity>
    where TEntity : BaseDbEntity
{
    public virtual async Task<TResult?> GetAsync<TResult>(Guid id, Func<TEntity?, TResult?> projector, CancellationToken ct = default)
    {
        var entity = (await GetByIdQuery(id)
            .SingleOrDefaultAsync(ct));
        return projector(entity);
    }

    public virtual async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Func<TEntity, TResult> projector,
        CancellationToken ct = default, ISpecification<TEntity>? spec = null) =>
        (await GetAllQuery(spec)
            .ToListAsync(ct))
            .Select(projector);

    public virtual async Task<IEnumerable<TResult>> GetPageAsync<TResult>(int page, int size, Func<TEntity, TResult> projector, CancellationToken ct = default, ISpecification<TEntity>? spec = null) =>
        (await GetPageQuery(page, size, spec)
            .ToListAsync(ct))
            .Select(projector);
    

    public virtual async IAsyncEnumerable<TResult> StreamAllAsync<TResult>(Func<TEntity, TResult> projector, CancellationToken ct = default, ISpecification<TEntity>? spec = null)
    {
        await foreach (var item in GetAllQuery(spec).AsAsyncEnumerable().WithCancellation(ct))
        {
            yield return projector(item);
        }
    }
       
}