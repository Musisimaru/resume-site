using MU.CV.BLL.Common.Models;
using MU.CV.BLL.Common.User;
using MU.CV.DAL.Common;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Common;

public abstract class BaseAuthorizedWrite<TDbEntity, TDtoEntity> : IDtoWrite<TDtoEntity>
    where TDbEntity : BaseHistorianEntity 
    where TDtoEntity : ISourceable<TDbEntity>, new()
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IHistorianRepository<TDbEntity> _historianRepo;
    protected readonly ICurrentUser _user;

    protected Action<TDbEntity, TDbEntity> mutate = (st, ed) => {};

    public BaseAuthorizedWrite(
        IHistorianRepository<TDbEntity> historianRepo, 
        IUnitOfWork unitOfWork, 
        ICurrentUser currentUser)
    {
        _historianRepo = historianRepo;
        _unitOfWork = unitOfWork;
        _user = currentUser;
    }

    public BaseAuthorizedWrite(
        IHistorianRepository<TDbEntity> historianRepo, 
        IUnitOfWork unitOfWork, 
        ICurrentUser currentUser,
        Action<TDbEntity, TDbEntity> mutate)
        : this(historianRepo, unitOfWork, currentUser)
    {
        this.mutate = mutate;
    }
    
    public async Task<Guid> CreateAsync(TDtoEntity dto, CancellationToken ct = default)
    {
        var addedEntity =  _historianRepo.Add(dto.ToDbEntity(), _user.Id);
        await _unitOfWork.CommitChangesAsync(ct);
        return addedEntity.Id;
    }

    public async Task UpdateAsync(TDtoEntity dto, CancellationToken ct = default)
    {
        var dbEntity = dto.ToDbEntity();
        await _historianRepo.UpdateAsync(dbEntity, _user.Id, mutate, ct);
        await _unitOfWork.CommitChangesAsync(ct);
    }
    
    public virtual  async Task RemoveAsync(Guid id, CancellationToken ct = default)
    {
        await _historianRepo.DeleteAsync(id, ct);
        await _unitOfWork.CommitChangesAsync(ct);
    }
}