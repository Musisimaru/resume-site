using System.Linq.Expressions;
using MU.CV.BLL.Common.Models;
using MU.CV.BLL.Common.User;
using MU.CV.DAL.Common;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Common;

public abstract class BaseAuthorizedRead<TDbEntity, TDtoEntity> : IDtoRead<TDtoEntity>
    where TDbEntity : BaseHistorianEntity 
    where TDtoEntity : ISourceable<TDbEntity>, new()
{


    protected readonly IProjectorRepository<TDbEntity> _readRepo;
    
    protected Func<TDbEntity?, TDtoEntity?> _dtoProjector = x => new TDtoEntity();

    public BaseAuthorizedRead(
        IProjectorRepository<TDbEntity> repository, 
        ICurrentUser currentUser)
    {
        _readRepo = repository;
        _spec = new ByUserSpec() { Criteria = it => it.CreatedBy == currentUser.Id };
    }

    public BaseAuthorizedRead(
        IProjectorRepository<TDbEntity> repository, 
        ICurrentUser currentUser, 
        Func<TDbEntity?, TDtoEntity?> dtoProjector)
        : this(repository, currentUser)
    {
        _dtoProjector = dtoProjector;
    }
    
    public class ByUserSpec : ISpecification<TDbEntity>
    {
        public Expression<Func<TDbEntity, bool>>? Criteria { get; init; }
        public List<Expression<Func<TDbEntity, object>>> Includes { get; } = [];
        public Expression<Func<TDbEntity, object>>? OrderBy => it => it.Id;
        public Expression<Func<TDbEntity, object>>? OrderByDesc => null;
        public int? Skip { get; }
        public int? Take { get; }
    }

    private ISpecification<TDbEntity> _spec = null;
    
    public virtual async Task<IEnumerable<TDtoEntity>> GetPageAsync(int page, int size, CancellationToken ct = default) =>
        await _readRepo.GetPageAsync(page, size, _dtoProjector, ct, _spec);
    
    public virtual async Task<IEnumerable<TDtoEntity>> GetAllAsync(CancellationToken ct = default) =>
        await _readRepo.GetAllAsync(_dtoProjector, ct, _spec);

    // TODO: Проверка прав
    public virtual async Task<TDtoEntity?> GetByIdAsync(Guid id, CancellationToken ct = default) => 
        await _readRepo.GetAsync(id, _dtoProjector, ct);
}