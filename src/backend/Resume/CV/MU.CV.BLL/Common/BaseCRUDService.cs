using MU.CV.DAL.Common;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Common;

public abstract class BaseCRUDService<TDbEntity> : IDbEntityRead<TDbEntity>, IDbEntityWrite<TDbEntity> where TDbEntity : BaseDbEntity 
{
    protected readonly IBaseRepository<TDbEntity?> _repository;
    protected readonly IUnitOfWork _unitOfWork;

    public BaseCRUDService(IBaseRepository<TDbEntity?> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public virtual async Task<Guid> CreateAsync(TDbEntity? entity, CancellationToken ct = default)
    {
        var addedEntity =  _repository.Add(entity);
        await _unitOfWork.CommitChangesAsync(ct);
        return addedEntity.Id;
    }

    public virtual  async Task RemoveAsync(Guid id, CancellationToken ct = default)
    {
        await _repository.DeleteAsync(id, ct);
        await _unitOfWork.CommitChangesAsync(ct);
    }

    public virtual  async Task UpdateAsync(TDbEntity? entity, CancellationToken ct = default)
    {
        _repository.Update(entity);
        await _unitOfWork.CommitChangesAsync(ct);
    }

    public virtual async Task<IReadOnlyList<TDbEntity?>> GetPageAsync(int page, int size, CancellationToken ct = default) =>
        await _repository.GetPageAsync(page, size, ct);
    

    public  virtual  async Task<IReadOnlyList<TDbEntity?>> GetAllAsync(CancellationToken ct = default) =>
        await _repository.GetAllAsync(ct);

    public  virtual  async Task<TDbEntity?> GetByIdAsync(Guid id, CancellationToken ct = default) => 
        await _repository.GetAsync(id, ct);
    
}