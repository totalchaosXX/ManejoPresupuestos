using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public class CategoriasRepository : ICategoriasRepository
    {
        private readonly string _connectionString;
        public CategoriasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(_connectionString);

            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId) VALUES (@Nombre,@TipoOperacionId,@UsuarioId);
                SELECT SCOPE_IDENTITY(); ", categoria);

            categoria.Id = id;
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<Categoria>("SELECT * FROM Categorias WHERE UsuarioId = @usuarioId", new { usuarioId });
        }

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Categoria>(@"SELECT * FROM Categorias WHERE Id=@Id and UsuarioId=@UsuarioId",new {id,usuarioId});
        } 

        public async Task Actualizar(Categoria categoria)
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(@"UPDATE Categorias SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId WHERE Id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(@"DELETE Categorias where Id = @Id",new { id });
        }
    }
}
