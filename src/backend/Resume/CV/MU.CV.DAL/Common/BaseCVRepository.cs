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

    public virtual async Task UpdateAsync(Guid id, Action<TEntity> mutate, CancellationToken ct = default)
    {
        var entity = (await GetTrackedSingleEntity(id, ct)) ?? throw new NotFoundException();
        mutate(entity);
    }
    
    public virtual void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    protected async Task<TEntity?> GetTrackedSingleEntity(Guid id, CancellationToken ct = default) =>
        await _context.Set<TEntity>()
                   .Where(en => en.Id == id)
                   .AsTracking()
                   .SingleOrDefaultAsync(ct);

    public virtual async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = (await GetTrackedSingleEntity(id, ct)) ?? throw new NotFoundException();
        _context.Set<TEntity>().Remove(entity);
    }


    protected IQueryable<TEntity?> GetByIdQuery(Guid id) =>
        _context.Set<TEntity>()
            .Where(en => en.Id == id)
            .AsNoTracking();
    
    public virtual async Task<TEntity?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var entity = (await GetByIdQuery(id)
                   .SingleOrDefaultAsync(ct))
               ?? throw new NotFoundException($"The entity {typeof(TEntity).Name} not found by id '{id}'");
        return entity;
    }

    protected IQueryable<TEntity> GetAllQuery() => _context.Set<TEntity>()
        .AsNoTracking()
        .TagWith($"Repo:GetAll<{typeof(TEntity).Name}>");

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default) =>
        await GetAllQuery()
            .ToListAsync(ct);
    protected IQueryable<TEntity> GetPageQuery(int page, int size) => _context.Set<TEntity>()
        .AsNoTracking()
        .OrderBy(e => EF.Property<Guid>(e, "Id"))
        .Skip((page - 1) * size).Take(size)
        .TagWith($"Repo:GetPage<{typeof(TEntity).Name}>:size{size}:page{page})");

    
    public virtual async Task<IReadOnlyList<TEntity>> GetPageAsync(int page, int size, CancellationToken ct = default) =>
        await GetPageQuery(page, size)
            .ToListAsync(ct);
    
    public ConfiguredCancelableAsyncEnumerable<TEntity> StreamAllAsync(CancellationToken ct = default) =>
        GetAllQuery().AsAsyncEnumerable().WithCancellation(ct);
}