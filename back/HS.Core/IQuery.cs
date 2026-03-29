using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{
    public interface IQuery<T>
    {
        Task<T> Execute(CancellationToken cancellationToken);
    }
}
