using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{
    public interface IEntityWriter
    {
        Task Add<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity;
        Task AddRange<TEntity>(IEnumerable<TEntity> entity, CancellationToken cancellationToken) where TEntity : BaseEntity;
    }
}
