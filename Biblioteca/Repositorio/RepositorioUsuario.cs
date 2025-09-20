using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.Enums;
using Biblioteca.Modelo;
using Biblioteca;
using Npgsql;

namespace Biblioteca.Repositorio
{
    public class RepositorioUsuario
    {
        private readonly string _connectionString = "" +
            "Host=localhost;Port=5433;Username=postgres;Password=123456;Database=Biblioteca";
        

        /// Query de consulta 
        public List<Usuario> Usuarios() 
        {

            var usuarios = new List<Usuario>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM usuario";

            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                usuarios.Add(new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Email = reader.GetString(2),
                    Tp = Enum.Parse<Tipo>(reader.GetString(3))

                });
            
            }

            return usuarios;

        }


        public Usuario? BuscarPorId(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT id, nome, email, tipo FROM usuario WHERE id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Email = reader.GetString(2),
                    Tp = Enum.Parse<Tipo>(reader.GetString(3))
                };
            }
            return null; // não achou
        }


        //Query de inserção dos usuários 
        public void Inserir(Usuario usuario)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "INSERT INTO usuario(nome,email,tipo) VALUES (@Nome, @Email, @Tipo)";
            using var cmd = new NpgsqlCommand( sql, conn);

            cmd.Parameters.AddWithValue("Nome", usuario.Nome);
            cmd.Parameters.AddWithValue("Email", usuario.Email);
            cmd.Parameters.AddWithValue("Tipo", usuario.Tp.ToString());

            cmd.ExecuteNonQuery();

        }

        //Query Atualização
        public void Atualizar(Usuario usuario)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE usuario SET nome=@Nome, email=@Email, Tipo=@Tipo WHERE id=@id";
            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("Nome", usuario.Nome);
            cmd.Parameters.AddWithValue("Email", usuario.Email);
            cmd.Parameters.AddWithValue("Tipo", usuario.Tp);
            cmd.ExecuteNonQuery();
        }


        //Query delete
        public void Deletar(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "DELETE FROM usuario WHERE id=@id";
            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();
        }




    }
}
