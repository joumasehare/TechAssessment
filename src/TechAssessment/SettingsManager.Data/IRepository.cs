using Microsoft.EntityFrameworkCore;
using SettingsManager.Domain.Models;
using System.Linq.Expressions;

namespace SettingsManager.Data;

public interface IRepository<TEntity, in TIdentifier> where TEntity : class, IEntity<TIdentifier>
{
    TEntity? Get(TIdentifier id);

    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> where);

    TEntity? GetDetached(TIdentifier id);
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

    public TEntity? GetDetached(TIdentifier id)
    {
        var entity = DbContext.Find<TEntity>(id);
        if (entity != null)
            DbContext.Entry(entity).State = EntityState.Detached;
        return entity;
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