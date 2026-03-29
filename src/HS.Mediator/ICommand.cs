#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{

    /// <summary>
    /// Define una solicitud que puede ser procesada por un manejador.
    /// </summary>
    /// <typeparam name="TResponse">Tipo de respuesta esperada.</typeparam>
    public interface ICommand<TResponse>
    {
    }

    /// <summary>
    /// Define una solicitud que no devuelve respuesta.
    /// </summary>
    public interface ICommand
    {
    }

}