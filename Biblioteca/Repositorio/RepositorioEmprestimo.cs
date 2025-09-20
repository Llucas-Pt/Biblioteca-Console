using Biblioteca.Enums;
using Biblioteca.Modelo;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Repositorio
{
    public class RepositorioEmprestimo
    {
        private readonly string _connectionString =
            "Host=localhost;Port=5433;Username=postgres;Password=123456;Database=Biblioteca";

        // Criar novo empréstimo
        public void Inserir(Emprestimo emp)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "INSERT INTO emprestimo (usuario_id, livro_id, data_inicio, status) " +
             "VALUES (@usuario, @livro, @inicio, @status)";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("usuario", emp.UsuarioId);
            cmd.Parameters.AddWithValue("livro", emp.LivroId);
            cmd.Parameters.AddWithValue("inicio", emp.DataInicio);
            cmd.Parameters.AddWithValue("status", emp.Status.ToString());
            cmd.ExecuteNonQuery();
        }

        // Listar todos os empréstimos
        public List<Emprestimo> Listar()
        {
            var lista = new List<Emprestimo>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT id, usuario_id, livro_id, datainicio, datafim FROM emprestimo";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Emprestimo
                {
                    Id = reader.GetInt32(0),
                    DataInicio = reader.GetDateTime(1),
                    DataFim = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                    Status = Enum.Parse<StatusEmprestimo>(reader.GetString(3)),
                    LivroId = reader.GetInt32(4),
                    UsuarioId = reader.GetInt32(5)
                });
            }

            return lista;
        }

        // Finalizar empréstimo (devolver livro)
        public void EncerrarEmprestimo(int emprestimoId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE emprestimo SET data_fim=@fim, status=@status WHERE id=@id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("fim", DateTime.Now);
            cmd.Parameters.AddWithValue("status", StatusEmprestimo.Finalizado.ToString());
            cmd.Parameters.AddWithValue("id", emprestimoId);
            cmd.ExecuteNonQuery();
        }

        // Buscar empréstimos ativos de um livro (útil p/ saber se já está emprestado)
        public Emprestimo? BuscarAtivoPorLivro(int livroId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT id, usuario_id, livro_id, datainicio " +
                         "FROM emprestimo WHERE livro_id=@livro AND datafim IS NULL";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("livro", livroId);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Emprestimo
                {
                    Id = reader.GetInt32(0),
                    UsuarioId = reader.GetInt32(1),
                    LivroId = reader.GetInt32(2),
                    DataInicio = reader.GetDateTime(3),
                    DataFim = null
                };
            }

            return null;
        }
    }
}
