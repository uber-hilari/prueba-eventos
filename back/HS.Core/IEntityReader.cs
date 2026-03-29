using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{
    public interface IEntityReader
    {
        Task<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) where TEntity : BaseEntity;
        Task<TEntity?> GetOrNull<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) where TEntity : BaseEntity;
        Task<TEntity> GetById<TEntity>(Guid id, CancellationToken cancellationToken) where TEntity : BaseEntity;
        Task<TEntity?> GetOrNullById<TEntity>(Guid id, CancellationToken cancellationToken) where TEntity : BaseEntity;
        TQuery GetQuery<TQuery, TResult>() where TQuery : IQuery<TResult>;
    }
}
