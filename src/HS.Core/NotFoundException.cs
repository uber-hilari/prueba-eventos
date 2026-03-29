using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{
    public class NotFoundEntityException : BaseException
    {
        public NotFoundEntityException(string entityName)
            : base($"'{entityName}' no encontrada")
        {
            Codigo = "40400";
        }
    }
}
