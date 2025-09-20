# 📚 Banco de Dados - Biblioteca

### 1️⃣ Instalação
Baixe e instale o **PostgreSQL (versão 17)**.

### 2️⃣ Criação do Banco

CREATE DATABASE Biblioteca;

### 2️⃣ Criação das Tabelas

-- Tabela de Livros
CREATE TABLE livro (
    id SERIAL PRIMARY KEY,
    titulo VARCHAR(250) NOT NULL,
    autor VARCHAR(100) NOT NULL,
    ano DATE NOT NULL,
    disponivel BOOLEAN
);

-- Tabela de Usuários
CREATE TABLE usuario (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(250) NOT NULL,
    email VARCHAR(100) NOT NULL,
    tipo VARCHAR(10) NOT NULL
);

-- Tabela de Empréstimos
CREATE TABLE emprestimo (
    id SERIAL PRIMARY KEY,
    data_inicio DATE NOT NULL,
    data_fim DATE,
    status VARCHAR(10) NOT NULL,

    livro_id INT NOT NULL,
    CONSTRAINT fk_livro
        FOREIGN KEY (livro_id)
        REFERENCES livro (id)
        ON DELETE CASCADE,

    usuario_id INT NOT NULL,
    CONSTRAINT fk_usuario
        FOREIGN KEY (usuario_id)
        REFERENCES usuario (id)
        ON DELETE CASCADE
);
