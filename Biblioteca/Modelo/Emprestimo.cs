using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.Enums;

namespace Biblioteca.Modelo
{
    public class Emprestimo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }   // FK para Usuario
        public int LivroId { get; set; }     // FK para Livro
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; } // null enquanto não devolvido
        public StatusEmprestimo Status  { get; set; }   

        public override string ToString()
        {
            return $"Empréstimo {Id}: Usuário {UsuarioId}, Livro {LivroId}, " +
                   $"Início {DataInicio}, Status: {Status}, " +
                   (DataFim == null ? "Em andamento" : $"Devolvido em {DataFim}");
        }
    }
}
