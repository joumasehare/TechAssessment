using Microsoft.EntityFrameworkCore;
using AcmeProduct.Domain.Models;
using System.Linq.Expressions;

namespace AcmeProduct.Data;

public interface IRepository<TEntity, in TIdentifier> where TEntity : class, IEntity<TIdentifier>
{
    TEntity? Get(TIdentifier id);

    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where);

    void DetachEntity(TEntity entity);

    TEntity Save(TEntity entity);
}

public class BaseRepository<TEntity, TIdentifier>(IUnitOfWork unitOfWork) : IRepository<TEntity, TIdentifier>
    where TEntity : class, IEntity<TIdentifier>
{
    protected IUnitOfWork UnitOfWork = unitOfWork;
    protected DbContext DbContext => UnitOfWork.Context;

    public TEntity? Get(TIdentifier id)
    {
        return DbContext.Find<TEntity>(id);
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where)
    {
        var query = DbContext.Set<TEntity>().Where(where);
        return query;
    }

    public void DetachEntity(TEntity entity)
    {
        DbContext.Entry(entity).State = EntityState.Detached;
    }

    public TEntity Save(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
        UnitOfWork.Commit();
        return entity;
    }
}