using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
    public interface ICategoriasRepository
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }
}
