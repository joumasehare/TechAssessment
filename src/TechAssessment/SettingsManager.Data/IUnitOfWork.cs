using Microsoft.EntityFrameworkCore;

namespace AcmeProduct.Data;

public interface IUnitOfWork : IDisposable
{
    DbContext Context { get; }
    void Commit();
}

public class UnitOfWork : IUnitOfWork
{
    public required DbContext Context { get; init; }
    public void Commit()
    {
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}