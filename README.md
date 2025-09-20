BANCO DE DADOS 

1° - Baixe/instale o postgres(Version 17)

2° - Crie o Banco : 
CREATE DATABASE Biblioteca 

3° - Crie as tabelas 
Cleate table livro(
id serial primary key,
titulo varchar(250) not null,
auto varchar(100) not null,
ano date not null,
disponivel bool,
);

Cleate table usuario(
id serial primary key,
nome varchar(250) not null,
email varchar (100) not null,
tipo varchar(10) not null,
)

CREATE table emprestimo(
id serial primary key,
data_inicio date not null,
data_fim date,
Status varchar(10) NOT NULL,

livro_id INT NOT NULL,
CONSTRAINT fk_livro
FOREIGN KEY (livro_id)
REFERENCES livros (id)
ON DELETE CASCADE,

usuario_id INT NOT NULL,
CONSTRAINT fk_usuario
FOREIGN KEY (usuario_id)
REFERENCES usuario (id)
ON DELETE CASCADE
);

