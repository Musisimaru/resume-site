using MU.CV.BLL.Common.Models;
using MU.CV.DAL.Common;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Common;

public abstract class BaseDtoWrite<TDbEntity, TDtoEntity> : IDtoWrite<TDtoEntity>
    where TDbEntity : BaseDbEntity 
    where TDtoEntity : ISourceable<TDbEntity>, new()
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IBaseRepository<TDbEntity> _writeRepo;

    public BaseDtoWrite(IBaseRepository<TDbEntity> repository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _writeRepo = repository;
    }
    
    public virtual  async Task UpdateAsync(TDtoEntity dto, CancellationToken ct = default)
    {
        _writeRepo.Update(dto.ToDbEntity());
        await _unitOfWork.CommitChangesAsync(ct);
    }
    public virtual async Task<Guid> CreateAsync(TDtoEntity dto, CancellationToken ct = default)
    {
        var addedEntity =  _writeRepo.Add(dto.ToDbEntity());
        await _unitOfWork.CommitChangesAsync(ct);
        return addedEntity.Id;
    }
    
    public virtual  async Task RemoveAsync(Guid id, CancellationToken ct = default)
    {
        await _writeRepo.DeleteAsync(id, ct);
        await _unitOfWork.CommitChangesAsync(ct);
    }
}