using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MU.CV.DAL.DataContext;
using MU.CV.DAL.Exceptions;
using Npgsql;

namespace MU.CV.DAL.Utils;

public class CVPGUnitOfWork : IUnitOfWork
{
    private readonly CVDbContext _context;

    public CVPGUnitOfWork(CVDbContext context)
    {
        _context = context;
    }
    
    public async ValueTask<int> CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505") 
        {
            throw new ConflictException("Already exists", ex);
        }
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();
    }
}