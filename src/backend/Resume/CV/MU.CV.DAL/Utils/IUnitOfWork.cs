using Microsoft.EntityFrameworkCore.Storage;

namespace MU.CV.DAL.Utils;

public interface IUnitOfWork
{
    ValueTask<int> CommitChangesAsync(CancellationToken cancellationToken = default);
    public IDbContextTransaction BeginTransaction();
}