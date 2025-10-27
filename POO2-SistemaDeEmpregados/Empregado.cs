using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace POO2_SistemaDeEmpregados
{

    internal class Empregado
    {
 
        public int Matricula { get; set; } = 0;
        public string? CPF { get; set; }
        public string? Nome { get; set; }
        public string? Endereco { get; set; }

        
        public Empregado() { }


        public Empregado(string cPF, string nome, string endereco)
        {
            CPF = cPF;
            Nome = nome;
            Endereco = endereco;
        }

        public override string? ToString()
        {
            return $"Empregado -> Matricula: {Matricula} | CPF: {CPF}" +
                   $"Nome: {Nome} | Endereco: {Endereco}";
        }


        public void Salvar(SqlConnection conexao)
        {
            Console.WriteLine("== Salvando Empregado ==");
            if (Matricula == 0)
            {
                var Cmd = conexao.CreateCommand();
                Cmd.CommandText = "INSERT INTO Empregado (CPF, Nome, Endereco) VALUES (@cpf, @nome, @endereco)";
                Cmd.Parameters.Add(new SqlParameter("cpf", CPF));
                Cmd.Parameters.Add(new SqlParameter("nome", Nome));
                Cmd.Parameters.Add(new SqlParameter("endereco", Endereco));
                Cmd.ExecuteNonQuery();
            }
        }

        
        public List<Empregado>? Pesquisar(int matriculaParam, SqlConnection conexao)
        {
            Console.WriteLine("== Recuperando Empregado ==");
            List<Empregado>? EmpregadoSLido = null;
            var Cmd = conexao.CreateCommand();

            if (matriculaParam == 0)
                Cmd.CommandText = "SELECT * FROM Empregado";
            else
            {
                Cmd.CommandText = "SELECT * FROM Empregado WHERE matricula = @matriculaBusca";
                Cmd.Parameters.Add(new SqlParameter("matriculaBusca", matriculaParam));
            }

            var resultado = Cmd.ExecuteReader();

            if (resultado != null)
            {
                EmpregadoSLido = new();
                while (resultado.Read())
                {
                    Empregado EmpregadoLido = new()
                    {
                        Matricula = resultado.GetInt32("Matricula"),
                        CPF = resultado.GetString("CPF"),
                        Nome = resultado.GetString("Nome"),
                        Endereco = resultado.GetString("Endereco")
                    };
                    EmpregadoSLido.Add(EmpregadoLido);
                }
                resultado.Close();
            }
            return EmpregadoSLido;
        }


        public void Excluir(SqlConnection conexao)
        {
            if (Matricula > 0)
            {
                var Cmd = conexao.CreateCommand();
                Cmd.CommandText = "DELETE FROM Empregado WHERE Matricula = @matricula";
                Cmd.Parameters.Add(new SqlParameter("matricula", Matricula));
                Cmd.ExecuteNonQuery();
            }
        }

        public static void LimparTudo(SqlConnection conexao)
        {
            var Cmd = conexao.CreateCommand();
            Cmd.CommandText = "TRUNCATE TABLE Empregado";
            Cmd.ExecuteNonQuery();
        }
    }
}