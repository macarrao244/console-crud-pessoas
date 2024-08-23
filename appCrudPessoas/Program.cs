using MySql.Data.MySqlClient;
using Mysqlx.Connection;

namespace appCrudPessoas
{
    public class Program
    {
        private static string connectionString = "Server=sql10.freesqldatabase.com;Database=sql10727350;Uid=sql10727350;Pwd=3y6BlDigUL;";
        private static List<Pessoa> pessoas = new List<Pessoa>();
        private static int proximoId = 1;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Adicionar Pessoa");
                Console.WriteLine("2 - Listar Pessoas");
                Console.WriteLine("3 - Editar Pessoa");
                Console.WriteLine("4 - Excluir Pessoa");
                Console.WriteLine("5 - Sair");
                Console.Write("Escolha uma opção acima: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarPessoa();
                        break;
                    case "2":
                        ListarPessoas();
                        break;
                    case "3":
                        Editar();
                        break;
                    case "4":
                        Excluir();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;
                }
            }
        }
        static void AdicionarPessoa()
        {
            Console.Write("Informe o Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Informe o Email: ");
            string email = Console.ReadLine();

            Console.Write("Informe a idade: ");
            int idade = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO pessoa (nome, email, idade) VALUES (@Nome, @Email, @Idade)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("Email", email);
                cmd.Parameters.AddWithValue("@Idade", idade);
                cmd.ExecuteNonQuery();

            }

            Console.WriteLine("Pessoa cadastrada com sucesso");
        }
        static void ListarPessoas()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Nome, Idade, Email FROM pessoa";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Id: {reader["Id"]}, Nome: {reader["Nome"]}, Idade: {reader["Idade"]}, Email: {reader["Email"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não existe pessoa cadastrada");
                    }
                }
            }

        }


        static void Excluir()
        {
            Console.Write("Informe o Id da pessoa que deseja exlcluir: ");
            int idExclusao = int.Parse(Console.ReadLine());


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM pessoa WHERE id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idExclusao);

                int linhaAfetada = cmd.ExecuteNonQuery();
                if (linhaAfetada > 0)
                {
                    Console.WriteLine("Pessoa excluida com sucesso");
                }

                else
                {
                    Console.WriteLine("Pessoa não encontrada");
                }
            }
        }
        static void Editar()
        {
            Console.Write("Informe o Id da pessoa que deseja editar: ");
            int idEditar = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM pessoa WHERE id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", idEditar);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.Write("Informe o novo nome: (* Deixe o campo em branco, para não alterar)");
                        string novoNome = Console.ReadLine();

                        Console.Write("Informe o novo email: (* Deixe o campo em branco, para não alterar)");
                        string novoEmail = Console.ReadLine();

                        Console.Write("Informe a nova idade: (* Deixe o campo em branco, para não alterar)");
                        string novoIdade = Console.ReadLine();

                        reader.Close();

                        string queryUpdate = "UPDATE pessoa SET Nome = @Nome, Email = @Email, Idade = @Idade WHERE id = @Id";
                        cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Nome", !string.IsNullOrWhiteSpace(novoNome) ? reader["Nome"] : novoNome);
                        cmd.Parameters.AddWithValue("@Email", !string.IsNullOrWhiteSpace(novoEmail) ? reader["Email"] : novoEmail);
                        cmd.Parameters.AddWithValue("@Idade", !string.IsNullOrWhiteSpace(novoNome) ? reader["Idade"] : int.Parse(novoIdade));
                        cmd.Parameters.AddWithValue("@Id", idEditar);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("A Pessoa foi atualizada com sucesso!");

                    }

                    else
                    {
                        Console.WriteLine("O Id da pessoa informada não existe");
                    }
                }

            }
        }

    }

}



