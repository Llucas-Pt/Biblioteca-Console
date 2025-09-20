using Biblioteca.Repositorio;
using Biblioteca.Service;
using Biblioteca.Modelo;
using Biblioteca.Enums;
using System;

namespace Biblioteca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var userRepo = new RepositorioUsuario();
            var livroRepo = new RepositorioLivro();
            var emprestimoService = new EmprestimoService(livroRepo, userRepo);

            while (true)
            {
                Console.WriteLine("\n=== MENU BIBLIOTECA ===");
                Console.WriteLine("1 - Listar usuários");
                Console.WriteLine("2 - Listar livros");
                Console.WriteLine("3 - Cadastrar usuário");
                Console.WriteLine("4 - Cadastrar livro");
                Console.WriteLine("5 - Emprestar livro");
                Console.WriteLine("6 - Devolver livro");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha: ");
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        var usuarios = userRepo.Usuarios();
                        foreach (var u in usuarios)
                            Console.WriteLine($"{u.Id} - {u.Nome} ({u.Tp})");
                        break;

                    case "2":
                        var livros = livroRepo.Livros();
                        foreach (var l in livros)
                            Console.WriteLine($"{l.Id} - {l.Titulo} (Disponível: {l.SitDisponivel})");
                        break;

                    case "3": // Cadastrar usuário
                        Console.Write("Nome do usuário: ");
                        var nome = Console.ReadLine();

                        Console.Write("Email do usuário: ");
                        var email = Console.ReadLine();

                        Console.Write("Tipo (Aluno/Professor): ");
                        var tipoStr = Console.ReadLine();
                        if (!Enum.TryParse(tipoStr, out Tipo tipo))
                        {
                            Console.WriteLine("Tipo inválido!");
                            break;
                        }

                        var novoUsuario = new Usuario
                        {
                            Nome = nome,
                            Email = email,
                            Tp = tipo
                        };

                        userRepo.Inserir(novoUsuario);
                        Console.WriteLine("Usuário cadastrado com sucesso!");
                        break;

                    case "4": // Cadastrar livro
                        Console.Write("Título do livro: ");
                        var titulo = Console.ReadLine();

                        Console.Write("Autor do livro: ");
                        var autor = Console.ReadLine();

                        Console.Write("Ano: ");
                        if (!int.TryParse(Console.ReadLine(), out int ano))
                        {
                            Console.WriteLine("Ano inválido!");
                            break;
                        }

                        var novoLivro = new Livro
                        {
                            Titulo = titulo,
                            Autor = autor,
                            Ano = ano,
                            SitDisponivel = Disponivel.Sim
                        };

                        livroRepo.Inserir(novoLivro);
                        Console.WriteLine("Livro cadastrado com sucesso!");
                        break;

                    case "5": // Emprestar livro
                        Console.Write("Id do usuário: ");
                        if (!int.TryParse(Console.ReadLine(), out int userId))
                        {
                            Console.WriteLine("Id inválido!");
                            break;
                        }

                        Console.Write("Id do livro: ");
                        if (!int.TryParse(Console.ReadLine(), out int livroId))
                        {
                            Console.WriteLine("Id inválido!");
                            break;
                        }

                        emprestimoService.EmprestarLivro(userId, livroId);
                        break;

                    case "6": // Devolver livro
                        Console.Write("Id do livro: ");
                        if (!int.TryParse(Console.ReadLine(), out int livroIdDev))
                        {
                            Console.WriteLine("Id inválido!");
                            break;
                        }

                        emprestimoService.DevolverLivro(livroIdDev);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }
    }
}
