using Microsoft.Data.SqlClient;
using POO2_SistemaDeEmpregados;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace POO2_SistemaDeEmpregados

{
    public partial class MainWindow : Window
    {
        SqlConnection? conexao = null;
        BindingList<Empregado> objClasseListDataGridEmpregado = new();

        string stringConexao = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBEmpregadosGUI;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public MainWindow()
        {
            InitializeComponent();
            DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;

            LabelStatus.Content = "Conectando...";
            Loaded += MainWindow_Loaded;
            Closed += Window_Closed;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                conexao = new(stringConexao);
                await conexao.OpenAsync();
                LabelStatus.Content = "Conexão OK";
            }
            catch (Exception ex)
            {
                LabelStatus.Content = "Conexão NOT";
                Console.WriteLine(ex.Message);
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (conexao != null && conexao.State == System.Data.ConnectionState.Open)
            {
                conexao.Close();
                conexao.Dispose();
                Console.WriteLine("Conexão fechada.");
            }
        }


        private bool VerificarCampos()
        {
            return TextBoxMatricula.Text == "" &&
                   TextBoxCPF.Text != "" &&
                   TextBoxNome.Text != "" &&
                   TextBoxEndereco.Text != "";
        }

        private void LimparCampos()
        {
            TextBoxMatricula.Text = ""; TextBoxCPF.Text = "";
            TextBoxNome.Text = ""; TextBoxEndereco.Text = "";
        }


        private void ButtonSalvar_Click(object sender, RoutedEventArgs e)
        {
            Empregado emp = new()
            {
                CPF = TextBoxCPF.Text,
                Nome = TextBoxNome.Text,
                Endereco = TextBoxEndereco.Text,
            };

            if (conexao != null && VerificarCampos())
            {
                try
                {
                    emp.Salvar(conexao);
                    LabelStatus.Content = "Salvo OK";
                    LimparCampos();

                    List<Empregado>? ret = emp.Pesquisar(0, conexao);
                    if (ret != null)
                    {
                        objClasseListDataGridEmpregado = new(ret);
                        DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;
                    }
                }
                catch (Exception ex)
                {
                    LabelStatus.Content = "Erro ao Salvar!";
                    MessageBox.Show("Erro ao salvar no banco: " + ex.Message, "Erro de SQL");
                    Console.WriteLine("### ERRO AO SALVAR: " + ex.Message);
                }
            }
            else
            {
                LabelStatus.Content = "Salvo NOT";
            }
        }

        private void ButtonPesquisar_Click(object sender, RoutedEventArgs e)
        {
            Empregado emp = new();
            List<Empregado>? ret = null;

            try
            {
                if (conexao != null)
                {
                    if (TextBoxMatricula.Text == "")
                    {
                        ret = emp.Pesquisar(0, conexao);
                    }
                    else
                    {
                        if (int.TryParse(TextBoxMatricula.Text, out int matricula))
                        {
                            ret = emp.Pesquisar(matricula, conexao);
                        }
                        else
                        {
                            LabelStatus.Content = "Matrícula inválida. Digite apenas números.";
                        }
                    }
                }

                if (ret != null)
                {
                    LabelStatus.Content = "Pesquisa OK";

                    objClasseListDataGridEmpregado = new(ret);
                    DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Content = "Erro ao Pesquisar!";
                MessageBox.Show("Erro ao pesquisar no banco: " + ex.Message, "Erro de SQL");
                Console.WriteLine("### ERRO AO PESQUISAR: " + ex.Message);
            }

        }

        private void ButtonLimpar_Click(object sender, RoutedEventArgs e)
        {
            if (conexao == null || conexao.State != System.Data.ConnectionState.Open)
            {
                LabelStatus.Content = "Erro de conexão.";
                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show(
                "ATENÇÃO!\n\nIsso apagará TODOS os registros do banco de dados permanentemente.\n\nTem certeza que deseja continuar?",
                "Confirmar Limpeza Total",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error);

            if (confirmacao == MessageBoxResult.No)
            {
                LabelStatus.Content = "Limpeza cancelada.";
                return;
            }

            try
            {
                Empregado.LimparTudo(conexao);

                LabelStatus.Content = "Banco de dados limpo.";
                LimparCampos();

                List<Empregado>? ret = new Empregado().Pesquisar(0, conexao);
                if (ret != null)
                {
                    objClasseListDataGridEmpregado = new(ret);
                    DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Content = "Erro ao limpar o banco!";
                Console.WriteLine(ex.Message);
            }
        }

        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (conexao == null || conexao.State != System.Data.ConnectionState.Open)
            {
                LabelStatus.Content = "Erro de conexão.";
                return;
            }

            if (!int.TryParse(TextBoxMatricula.Text, out int matriculaParaExcluir) || matriculaParaExcluir == 0)
            {
                LabelStatus.Content = "Matrícula inválida para excluir.";
                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show(
                $"Tem certeza que deseja excluir a matrícula {matriculaParaExcluir}?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmacao == MessageBoxResult.No)
            {
                LabelStatus.Content = "Exclusão cancelada.";
                return;
            }

            try
            {
                Empregado emp = new() { Matricula = matriculaParaExcluir };
                emp.Excluir(conexao);

                LabelStatus.Content = "Excluído OK";
                LimparCampos();

                List<Empregado>? ret = emp.Pesquisar(0, conexao);
                if (ret != null)
                {
                    objClasseListDataGridEmpregado = new(ret);
                    DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Content = "Erro ao Excluir!";
                Console.WriteLine(ex.Message);
            }
        }


    }
}