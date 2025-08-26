using MU.CV.DAL.Common;

namespace MU.CV.BLL.Common;

public interface IDbEntityWrite<TDbEntity> where TDbEntity : BaseDbEntity
{
    public Task<Guid> CreateAsync(TDbEntity entity, CancellationToken ct = default);
    public Task UpdateAsync(TDbEntity entity, CancellationToken ct = default);
    public Task RemoveAsync(Guid id, CancellationToken ct = default);

}