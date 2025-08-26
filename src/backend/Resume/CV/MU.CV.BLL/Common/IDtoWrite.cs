namespace MU.CV.BLL.Common;

public interface IDtoWrite<TDtoEntity>
{
    public Task<Guid> CreateAsync(TDtoEntity entity, CancellationToken ct = default);
    public Task UpdateAsync(TDtoEntity entity, CancellationToken ct = default);
    
    public Task RemoveAsync(Guid id, CancellationToken ct = default);

}