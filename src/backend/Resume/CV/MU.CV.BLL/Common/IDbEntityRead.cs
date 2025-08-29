using MU.CV.DAL.Common;

namespace MU.CV.BLL.Common;

public interface IDbEntityRead<TDbEntity> where TDbEntity : BaseDbEntity
{
    public Task<IReadOnlyList<TDbEntity?>> GetPageAsync(int page, int size, CancellationToken ct = default);
    public Task<IReadOnlyList<TDbEntity?>> GetAllAsync(CancellationToken ct = default);
    public Task<TDbEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
}