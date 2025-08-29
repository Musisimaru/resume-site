using MU.CV.BLL.Common.Models;
using MU.CV.DAL.Common;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Common;

public abstract class BaseDtoRead<TDbEntity, TDtoEntity>: IDtoRead<TDtoEntity>
    where TDbEntity : BaseDbEntity 
    where TDtoEntity : ISourceable<TDbEntity>, new()
{
    protected readonly IProjectorRepository<TDbEntity> _readRepo;
    
    protected Func<TDbEntity?, TDtoEntity?> _dtoProjector = x => new TDtoEntity();

    public BaseDtoRead(IProjectorRepository<TDbEntity> repository)
    {
        _readRepo = repository;
    }

    public BaseDtoRead(IProjectorRepository<TDbEntity> repository, Func<TDbEntity?, TDtoEntity?> dtoProjector)
        : this(repository)
    {
        _dtoProjector = dtoProjector;
    }
    
    
    public virtual async Task<IEnumerable<TDtoEntity>> GetPageAsync(int page, int size, CancellationToken ct = default) =>
        await _readRepo.GetPageAsync(page, size, _dtoProjector, ct);
    
    public virtual async Task<IEnumerable<TDtoEntity>> GetAllAsync(CancellationToken ct = default) =>
        await _readRepo.GetAllAsync(_dtoProjector, ct);

    public virtual async Task<TDtoEntity?> GetByIdAsync(Guid id, CancellationToken ct = default) => 
        await _readRepo.GetAsync(id, _dtoProjector, ct);
}