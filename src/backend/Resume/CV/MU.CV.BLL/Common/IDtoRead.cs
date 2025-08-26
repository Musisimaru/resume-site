using MU.CV.BLL.Common.Models;
using MU.CV.DAL.Common;

namespace MU.CV.BLL.Common;

public interface IDtoRead<TDtoEntity>
{
    public Task<IEnumerable<TDtoEntity>> GetPageAsync(int page, int size, CancellationToken ct = default);
    public Task<IEnumerable<TDtoEntity>> GetAllAsync(CancellationToken ct = default);
    public Task<TDtoEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
}