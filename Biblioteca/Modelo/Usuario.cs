using Biblioteca.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Biblioteca.Modelo
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public Tipo Tp { get; set; }


        public Usuario(string nome, string email, Tipo tp)
        {
            Nome = nome;
            Email = email;
            Tp = tp;
        }

        public Usuario() { }



    }




}
