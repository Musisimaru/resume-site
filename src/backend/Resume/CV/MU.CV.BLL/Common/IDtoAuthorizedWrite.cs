namespace MU.CV.BLL.Common;

public interface IDtoAuthorizedWrite<TDtoEntity>
{
    public Task<Guid> CreateAsync(TDtoEntity entity, Guid author, CancellationToken ct = default);
    public Task UpdateAsync(TDtoEntity entity, Guid editor, CancellationToken ct = default);
    public Task RemoveAsync(Guid id, CancellationToken ct = default);

}