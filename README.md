# Sistema de Gerenciamento de Empregados (WPF)

Este é um projeto de CRUD desenvolvido em C# e WPF, demonstrando a interação entre uma interface gráfica e um banco de dados SQL Server (LocalDB) usando ADO.NET.

O projeto foi baseado em material de aula da disciplina de Programação Orientada a Objetos 2, seguindo os padrões apresentados.

## Funcionalidades Implementadas

* **Salvar:** Insere novos empregados no banco (com Matrícula em branco).
* **Pesquisar:** Busca todos os empregados (com Matrícula em branco) ou um empregado específico por sua matrícula.
* **Limpar:** Limpa os dados do banco de dados.
* **Excluir:** Remove um empregado do banco de dados.
* **DataGrid:** Exibe os resultados da pesquisa em tempo real.
* **Conexão Assíncrona:** A conexão com o banco de dados é feita em segundo plano para que a interface do usuário não trave ao iniciar.

## Como Rodar o Projeto

Para rodar este projeto na sua máquina, siga os passos abaixo.

### Pré-requisitos

* Visual Studio 2022
* SQL Server Express LocalDB

### Passos para Instalação

1.  Clone este repositório.
2.  Abra o arquivo `.sln` (Solução) no Visual Studio.
3.  No "Solution Explorer" (Explorador de Soluções), encontre e abra o arquivo `database_setup.sql`.
4.  Com o script aberto, verifique se a conexão no topo da janela do script está apontando para `(localdb)\MSSQLLocalDB` (ou o servidor LocalDB da sua máquina).
5.  Execute o script clicando no botão verde de "Play" (Executar). Isso criará o banco `DBEmpregadoGUI` e a tabela `Empregado`.
    * **Solução de Problemas:** Se você receber um erro de login (ex: `Login failed for user...`), execute os comandos de permissão do `database_setup.sql` ou siga o tutorial da Microsoft para adicionar seu usuário Windows como `sysadmin` no LocalDB.
6.  Pressione **F5** ou clique em "Start" para compilar e rodar o projeto.

O aplicativo deve agora conectar-se ao banco de dados recém-criado.
