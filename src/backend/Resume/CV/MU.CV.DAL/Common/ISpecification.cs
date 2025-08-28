using System.Linq.Expressions;

namespace MU.CV.DAL.Common;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDesc { get; }
    int? Skip { get; }
    int? Take { get; }
}