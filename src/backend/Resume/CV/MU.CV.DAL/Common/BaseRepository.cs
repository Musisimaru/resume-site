using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using MU.CV.DAL.DataContext;
using MU.CV.DAL.Exceptions;

namespace MU.CV.DAL.Common;

public abstract class BaseCVRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseDbEntity
{
    protected readonly CVDbContext _context;

    public BaseCVRepository(CVDbContext context)
    {
        _context = context;
    }


    public TEntity Add(TEntity entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        return _context.Set<TEntity>().Add(entity).Entity;
    }

    public async Task UpdateAsync(Guid id, Action<TEntity> mutate, CancellationToken ct = default)
    {
        var entity = (await GetTrackedSingleEntity(id, ct)) ?? throw new NotFoundException();
        mutate(entity);
    }
    
    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    protected async Task<TEntity?> GetTrackedSingleEntity(Guid id, CancellationToken ct = default) =>
        await _context.Set<TEntity>()
                   .Where(en => en.Id == id)
                   .AsTracking()
                   .SingleOrDefaultAsync(ct);

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = (await GetTrackedSingleEntity(id, ct)) ?? throw new NotFoundException();
        _context.Set<TEntity>().Remove(entity);
    }

    public async Task<TEntity?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var entity = (await _context.Set<TEntity>()
                   .Where(en => en.Id == id)
                   .AsNoTracking()
                   .SingleOrDefaultAsync(ct))
               ?? throw new NotFoundException($"The entity {typeof(TEntity).Name} not found by id '{id}'");
        return entity;
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Set<TEntity>()
            .AsNoTracking()
            .TagWith($"Repo:GetAllAsync<{typeof(TEntity).Name}>")
            .ToListAsync(ct);
    
    public async Task<IReadOnlyList<TEntity>> GetPageAsync(int page, int size, CancellationToken ct = default) =>
        await _context.Set<TEntity>()
            .AsNoTracking()
            .OrderBy(e => EF.Property<Guid>(e, "Id"))
            .Skip((page - 1) * size).Take(size)
            .ToListAsync(ct);


    public ConfiguredCancelableAsyncEnumerable<TEntity> StreamAllAsync(CancellationToken ct = default) =>
        _context.Set<TEntity>().AsNoTracking().AsAsyncEnumerable().WithCancellation(ct);
}