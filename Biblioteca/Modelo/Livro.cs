using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.Enums;

namespace Biblioteca.Modelo

{

    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor {  get; set; }
        public int Ano { get; set; }
        public Disponivel SitDisponivel { get; set; }


        public Livro(string titulo, string autor, int ano, Disponivel sitDisponivel)
        {
            Titulo = titulo;
            Autor = autor;
            Ano = ano;
            SitDisponivel = sitDisponivel;
        }

        public Livro() { }

        public override string ToString()
        {
            return $"Livro: {Titulo}, Autor: {Autor}, Ano: {Ano}, Disponível: {SitDisponivel}";
        }

    }



}
