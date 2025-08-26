using MU.CV.DAL.Common;

namespace MU.CV.BLL.Common.Models;

public abstract record ValueBaseDtoEntity<TSource>(Guid Id) : ISourceable<TSource> where TSource : BaseDbEntity, new()
{
    public ValueBaseDtoEntity() : this(Guid.Empty) { }
    public virtual TSource ToDbEntity(){ return new TSource(){Id = Id};}
}

public interface ISourceable<out TSource>
{
    public TSource ToDbEntity();
}