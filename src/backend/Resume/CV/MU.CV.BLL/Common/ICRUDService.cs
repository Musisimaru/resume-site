using MU.CV.DAL.Common;

namespace MU.CV.BLL.Common;

public interface ICRUDService<TDbEntity> where TDbEntity : BaseDbEntity
{
    public Task<Guid> CreateAsync(TDbEntity entity, CancellationToken ct = default);
    public Task RemoveAsync(Guid id, CancellationToken ct = default);
    public Task UpdateAsync(TDbEntity entity, CancellationToken ct = default);
    
    public Task<IReadOnlyList<TDbEntity>> GetPageAsync(int page, int size, CancellationToken ct = default);
    public Task<IReadOnlyList<TDbEntity>> GetAllAsync(CancellationToken ct = default);
    public Task<TDbEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
}