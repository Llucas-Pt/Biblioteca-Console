using Biblioteca.Modelo;
using Biblioteca.Enums;
using Npgsql;
using System;
using System.Collections.Generic;

namespace Biblioteca.Repositorio
{
    public class RepositorioLivro
    {
        private readonly string _connectionString =
            "Host=localhost;Port=5433;Username=postgres;Password=123456;Database=Biblioteca";

        // Buscar livro por ID
        public Livro? BuscarPorId(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT id, titulo, autor, ano, disponivel FROM livros WHERE id=@id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Livro
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(1),
                    Autor = reader.GetString(2),
                    Ano = reader.GetInt32(3),
                    SitDisponivel = reader.GetBoolean(4) ? Disponivel.Sim : Disponivel.Nao
                };
            }

            return null;
        }

        // Listar todos os livros
        public List<Livro> Livros()
        {
            var livros = new List<Livro>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT id, titulo, autor, ano, disponivel FROM livros";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                livros.Add(new Livro
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(1),
                    Autor = reader.GetString(2),
                    Ano = reader.GetInt32(3),
                    SitDisponivel = reader.GetBoolean(4) ? Disponivel.Sim : Disponivel.Nao
                });
            }

            return livros;
        }

        // Inserir livro
        public void Inserir(Livro livro)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "INSERT INTO livros (titulo, autor, ano, disponivel) VALUES (@titulo, @autor, @ano, @disp)";
            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("titulo", livro.Titulo);
            cmd.Parameters.AddWithValue("autor", livro.Autor);
            cmd.Parameters.AddWithValue("ano", livro.Ano);
            cmd.Parameters.AddWithValue("disp", livro.SitDisponivel == Disponivel.Sim);
            cmd.ExecuteNonQuery();
        }

        // Atualizar livro
        public void Atualizar(Livro livro)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE livros SET titulo=@titulo, autor=@autor, ano=@ano, disponivel=@disp WHERE id=@id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("titulo", livro.Titulo);
            cmd.Parameters.AddWithValue("autor", livro.Autor);
            cmd.Parameters.AddWithValue("ano", livro.Ano);
            cmd.Parameters.AddWithValue("disp", livro.SitDisponivel.ToString());
            cmd.Parameters.AddWithValue("id", livro.Id);
            cmd.ExecuteNonQuery();
        }

        // Atualizar apenas a disponibilidade
        public void AtualizarDisponibilidade(int id, Disponivel disp)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "UPDATE livros SET disponivel=@disp WHERE id=@id";
            using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("disp", disp == Disponivel.Sim);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }

        // Deletar livro
        public void Deletar(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "DELETE FROM livros WHERE id=@id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
