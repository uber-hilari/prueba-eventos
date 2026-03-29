using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{
    public class BaseException : Exception
    {
        public BaseException()
        {
            Codigo = "50000";
        }

        public BaseException(string mensaje) : base(mensaje)
        {
            Codigo = "50000";
        }

        public BaseException(string codigo, string mensaje) : base(mensaje)
        {
            Codigo = codigo;
        }

        public BaseException(string codigo, string mensaje, Exception innerException) : base(mensaje, innerException)
        {
            Codigo = codigo;
        }

        public string Codigo { get; protected set; }
        public bool GrabarData { get; set; } = false;

        public virtual IEnumerable<Error> GetErrors()
        {
            return [GetError()];
        }

        public virtual Error GetError()
        {
            return new Error(Codigo, Message);
        }
    }
}
