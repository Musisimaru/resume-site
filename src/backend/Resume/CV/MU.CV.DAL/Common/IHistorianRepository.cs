namespace MU.CV.DAL.Common;

public interface IHistorianRepository<TEntity>
{
    public TEntity Add(TEntity entity, Guid author);
    public Task UpdateAsync(TEntity entity, Guid editor, CancellationToken ct = default);
    public Task UpdateAsync(TEntity entity, Guid editor, Action<TEntity, TEntity> mutate, CancellationToken ct = default);
    public Task DeleteAsync(Guid id, CancellationToken ct = default);

}