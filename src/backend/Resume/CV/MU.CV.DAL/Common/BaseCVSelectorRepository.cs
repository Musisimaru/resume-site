using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using MU.CV.DAL.DataContext;
using MU.CV.DAL.Exceptions;

namespace MU.CV.DAL.Common;

public abstract class BaseCVSelectorRepository<TEntity>(CVDbContext context) : BaseCVRepository<TEntity>(context), ISelectorRepository<TEntity>
    where TEntity : BaseDbEntity
{
    public async Task<TResult?> GetAsync<TResult>(Guid id, Expression<Func<TEntity, TResult>> selector, CancellationToken ct = default)
    {
        var entity = (await GetByIdQuery(id)
                         .Select(selector!)
                         .SingleOrDefaultAsync(ct))
                     ?? throw new NotFoundException($"The entity {typeof(TEntity).Name} not found by id '{id}'");
        return entity;
    }

    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        CancellationToken ct = default, ISpecification<TEntity>? spec = null) =>
        (await GetAllQuery(spec)
            .Select(selector)
            .ToListAsync(ct));

    public async Task<IEnumerable<TResult>> GetPageAsync<TResult>(int page, int size, Expression<Func<TEntity, TResult>> selector, CancellationToken ct = default, ISpecification<TEntity>? spec = null) =>
        (await GetPageQuery(page, size, spec)
            .Select(selector)
            .ToListAsync(ct));

    public ConfiguredCancelableAsyncEnumerable<TResult> StreamAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector, CancellationToken ct = default, ISpecification<TEntity>? spec = null) =>
        GetAllQuery(spec).Select(selector).AsAsyncEnumerable().WithCancellation(ct);


}