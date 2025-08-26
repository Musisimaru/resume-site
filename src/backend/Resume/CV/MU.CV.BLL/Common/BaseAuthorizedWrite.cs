using MU.CV.BLL.Common.Models;
using MU.CV.DAL.Common;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Common;

public class BaseAuthorizedWrite<TDbEntity, TDtoEntity> : IDtoAuthorizedWrite<TDtoEntity>
    where TDbEntity : BaseDbEntity 
    where TDtoEntity : ISourceable<TDbEntity>, new()
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IHistorianRepository<TDbEntity> _historianRepo;

    protected Action<TDbEntity, TDbEntity> mutate = (st, ed) => {};

    public BaseAuthorizedWrite(IHistorianRepository<TDbEntity> historianRepo, IUnitOfWork unitOfWork)
    {
        _historianRepo = historianRepo;
        _unitOfWork = unitOfWork;
    }

    public BaseAuthorizedWrite(IHistorianRepository<TDbEntity> historianRepo, IUnitOfWork unitOfWork,
        Action<TDbEntity, TDbEntity> mutate)
        : this(historianRepo, unitOfWork)
    {
        this.mutate = mutate;
    }
    
    public async Task<Guid> CreateAsync(TDtoEntity dto, Guid author, CancellationToken ct = default)
    {
        var addedEntity =  _historianRepo.Add(dto.ToDbEntity(), author);
        await _unitOfWork.CommitChangesAsync(ct);
        return addedEntity.Id;
    }

    public async Task UpdateAsync(TDtoEntity dto, Guid editor, CancellationToken ct = default)
    {
        var dbEntity = dto.ToDbEntity();
        await _historianRepo.UpdateAsync(dbEntity, editor, mutate, ct);
        await _unitOfWork.CommitChangesAsync(ct);
    }
    
    public virtual  async Task RemoveAsync(Guid id, CancellationToken ct = default)
    {
        await _historianRepo.DeleteAsync(id, ct);
        await _unitOfWork.CommitChangesAsync(ct);
    }
}