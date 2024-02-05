﻿using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public class CuentasRepository : ICuentasRepository
    {
        private readonly string _connectionString;


        public CuentasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(_connectionString);

            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO Cuentas(Nombre,TipoCuentaId,Balance,Descripcion) values (@Nombre,@TipoCuentaId,@Balance,@Descripcion) SELECT SCOPE_IDENTITY();",cuenta);

            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<Cuenta>(@"SELECT Cuentas.Id, Cuentas.Nombre, Balance, tc.Nombre AS TipoCuenta
            FROM Cuentas
            INNER JOIN TiposCuentas tc
            ON tc.Id = Cuentas.TipoCuentaId
            WHERE tc.UsuarioId = @UsuarioId
            ORDER BY tc.Orden", new { usuarioId });
        }

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Cuenta>(
                @"SELECT Cuentas.Id, Cuentas.Nombre, Balance,Descripcion, TipoCuentaId
                FROM Cuentas
                INNER JOIN TiposCuentas tc
                ON tc.Id = Cuentas.TipoCuentaId
                WHERE tc.UsuarioId = @UsuarioId AND Cuentas.Id = @Id",new {id,usuarioId});
        }

        public async Task Actualizar(CuentaCreacionViewModel cuenta)
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(@"UPDATE Cuentas
            SET Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion, TipoCuentaId = @TipoCuentaId 
            WHERE Id = @Id;", cuenta);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync("DELETE Cuentas WHERE Id = @Id", new {id});
        }

    }
}
