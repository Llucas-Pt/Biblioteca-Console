using Biblioteca.Modelo;
using Biblioteca.Repositorio;
using Biblioteca.Enums;
using System;
using System.Collections.Generic;
using Npgsql;

namespace Biblioteca.Service
{
    public class EmprestimoService
    {
        private readonly RepositorioLivro _livroRepo;
        private readonly RepositorioUsuario _usuarioRepo;
        private readonly string _connectionString =
            "Host=localhost;Port=5433;Username=postgres;Password=123456;Database=Biblioteca";

        public EmprestimoService(RepositorioLivro livroRepo, RepositorioUsuario usuarioRepo)
        {
            _livroRepo = livroRepo;
            _usuarioRepo = usuarioRepo;
        }

        // Emprestar livro
        public void EmprestarLivro(int usuarioId, int livroId)
        {
            // Verifica se usuário existe
            var usuario = _usuarioRepo.BuscarPorId(usuarioId);
            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado!");
                return;
            }

            // Verifica se livro existe
            var livro = _livroRepo.BuscarPorId(livroId);
            if (livro == null)
            {
                Console.WriteLine("Livro não encontrado!");
                return;
            }

            // Verifica disponibilidade
            if (livro.SitDisponivel != Disponivel.Sim)
            {
                Console.WriteLine("Livro indisponível para empréstimo!");
                return;
            }

            // Registrar empréstimo no banco
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "INSERT INTO emprestimo (data_inicio, status, livro_id, usuario_id) " +
                         "VALUES (@data, @status, @livroId, @usuarioId)";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("data", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("status", "Emprestado");
            cmd.Parameters.AddWithValue("livroId", livroId);
            cmd.Parameters.AddWithValue("usuarioId", usuarioId);
            cmd.ExecuteNonQuery();

            // Atualiza disponibilidade do livro
            _livroRepo.AtualizarDisponibilidade(livroId, Disponivel.Nao);

            Console.WriteLine($"Livro '{livro.Titulo}' emprestado para '{usuario.Nome}'!");
        }

        // Devolver livro
        public void DevolverLivro(int livroId)
        {
            var livro = _livroRepo.BuscarPorId(livroId);
            if (livro == null)
            {
                Console.WriteLine("Livro não encontrado!");
                return;
            }

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            // Atualiza status do empréstimo ativo
            string sql = "UPDATE emprestimo SET data_fim=@data, status=@status " +
                         "WHERE livro_id=@livroId AND status='Emprestado'";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("data", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("status", "Devolvido");
            cmd.Parameters.AddWithValue("livroId", livroId);
            int linhas = cmd.ExecuteNonQuery();

            if (linhas == 0)
            {
                Console.WriteLine("Nenhum empréstimo ativo encontrado para esse livro!");
                return;
            }

            // Atualiza disponibilidade do livro
            _livroRepo.AtualizarDisponibilidade(livroId, Disponivel.Sim);

            Console.WriteLine($"Livro '{livro.Titulo}' devolvido com sucesso!");
        }

        // Listar empréstimos ativos (opcional)
        public List<(string Usuario, string Livro, DateTime DataInicio)> EmprestimosAtivos()
        {
            var lista = new List<(string, string, DateTime)>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT u.name, l.titulo, e.data_inicio " +
                         "FROM emprestimo e " +
                         "JOIN usuario u ON e.usuario_id = u.id " +
                         "JOIN livros l ON e.livro_id = l.id " +
                         "WHERE e.status='Emprestado'";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add((reader.GetString(0), reader.GetString(1), reader.GetDateTime(2)));
            }

            return lista;
        }
    }
}
