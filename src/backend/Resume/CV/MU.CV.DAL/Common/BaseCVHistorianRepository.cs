using Microsoft.EntityFrameworkCore;
using MU.CV.DAL.DataContext;
using MU.CV.DAL.Exceptions;

namespace MU.CV.DAL.Common;

public class BaseCVHistorianRepository<TEntity>(CVDbContext context)
    : BaseCVRepository<TEntity>(context), IHistorianRepository<TEntity>
    where TEntity : BaseHistorianEntity
{
    public virtual TEntity Add(TEntity entity, Guid author)
    {
        entity.CreatedBy = author;
        entity.UpdatedBy = author;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        return Add(entity);
    }

    public virtual async Task UpdateAsync(TEntity editedEntity, Guid editor, CancellationToken ct = default)
    {
        var storedEntity = (await GetByIdQuery(editedEntity.Id).SingleOrDefaultAsync(ct)) ?? throw new NotFoundException();
        
        editedEntity.CreatedBy = storedEntity.CreatedBy;
        editedEntity.CreatedAt = storedEntity.CreatedAt;
        
        editedEntity.UpdatedBy = editor;
        editedEntity.UpdatedAt = DateTime.UtcNow;
        
        Update(editedEntity);
    }

    public virtual async Task UpdateAsync(TEntity editedEntity, Guid editor, Action<TEntity, TEntity> mutate, CancellationToken ct = default)
    {
        var storedEntity = (await GetTrackedSingleEntity(editedEntity.Id, ct)) ?? throw new NotFoundException();
        mutate(storedEntity, editedEntity);
        storedEntity.UpdatedBy = editor;
        storedEntity.UpdatedAt = DateTime.UtcNow;       
    }
}