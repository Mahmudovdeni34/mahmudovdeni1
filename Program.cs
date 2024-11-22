using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace mahmudov
{
    class Program
    {
        static string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=mahmudov_;User Id=mahmudov_;Password=Md7083549;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управление данными пользователей:");
                Console.WriteLine("1) Посмотреть все записи");
                Console.WriteLine("2) Добавить нового пользователя");
                Console.WriteLine("3) Обновить существующего пользователя");
                Console.WriteLine("4) Удалить существующего пользователя");
                Console.WriteLine("5) Авторизоваться в системе");
                Console.WriteLine("6) Выйти");

                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllUsers();
                        break;
                    case "2":
                        AddNewUser();
                        break;
                    case "3":
                        UpdateUser();
                        break;
                    case "4":
                        DeleteUser();
                        break;
                    case "5":
                        AuthenticateUser();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("Нажмите любую клавишу, чтобы вернуться в меню.");
                Console.ReadKey();
            }
        }

        static void ViewAllUsers()
        {
            Console.Clear();
            Console.WriteLine("Список пользователей:");
            Console.WriteLine("ID\tUsername\tEmail\t\tFirst Name\tLast Name\tDate of Birth\tGender\tAvatar\tBio");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"{reader["id"]}\t{reader["username"]}\t{reader["email"]}\t{reader["first_name"]}\t{reader["last_name"]}\t{reader["date_of_birth"]}\t{reader["gender"]}\t{reader["avatar"]}\t{reader["bio"]}");
                }
            }
        }

        static void AddNewUser()
        {
            Console.Clear();
            Console.WriteLine("Добавление пользователя:");
            Console.WriteLine("Введите следующие данные через запятую \",\"");
            Console.WriteLine("Username,Email,PasswordHash,FirstName,LastName,DateOfBirth(YYYY-MM-DD)");
            Console.WriteLine("Пример: Vanya,vanya@example.com,12345,Иван,Иванов,1990-01-01");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 6)
            {
                Console.WriteLine("Неверный формат, повторите еще раз.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (username, email, password_hash, first_name, last_name, date_of_birth) VALUES (@username, @email, @password_hash, @first_name, @last_name, @date_of_birth)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", data[0]);
                command.Parameters.AddWithValue("@email", data[1]);
                command.Parameters.AddWithValue("@password_hash", data[2]);
                command.Parameters.AddWithValue("@first_name", data[3]);
                command.Parameters.AddWithValue("@last_name", data[4]);
                command.Parameters.AddWithValue("@date_of_birth", data[5]);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Добавление успешно!");
                }
                else
                {
                    Console.WriteLine("Ошибка при добавлении пользователя.");
                }
            }
        }

        static void UpdateUser()
        {
            Console.Clear();
            Console.WriteLine("Обновление пользователя:");
            Console.Write("Введите ID пользователя, которого хотите обновить: ");
            int userId = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите новые данные через запятую \",\"");
            Console.WriteLine("Username,Email,PasswordHash,FirstName,LastName,DateOfBirth(YYYY-MM-DD)");
            Console.WriteLine("Пример: Vanya,vanya@example.com,12345,Иван,Иванов,1990-01-01");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 6)
            {
                Console.WriteLine("Неверный формат, повторите еще раз.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET username = @username, email = @email, password_hash = @password_hash, first_name = @first_name, last_name = @last_name, date_of_birth = @date_of_birth WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", userId);
                command.Parameters.AddWithValue("@username", data[0]);
                command.Parameters.AddWithValue("@email", data[1]);
                command.Parameters.AddWithValue("@password_hash", data[2]);
                command.Parameters.AddWithValue("@first_name", data[3]);
                command.Parameters.AddWithValue("@last_name", data[4]);
                command.Parameters.AddWithValue("@date_of_birth", data[5]);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Обновление успешно!");
                }
                else
                {
                    Console.WriteLine("Ошибка при обновлении пользователя.");
                }
            }
        }

        static void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("Удаление пользователя:");
            Console.Write("Введите ID пользователя, которого хотите удалить: ");
            int userId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", userId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Удаление успешно!");
                }
                else
                {
                    Console.WriteLine("Ошибка при удалении пользователя.");
                }
            }
        }

        static void AuthenticateUser()
        {
            Console.Clear();
            Console.WriteLine("Авторизация пользователя:");
            Console.Write("Введите Email: ");
            string email = Console.ReadLine();
            Console.Write("Введите PasswordHash: ");
            string passwordHash = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE email = @email AND password_hash = @password_hash";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password_hash", passwordHash);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine("Авторизация успешна!");
                }
                else
                {
                    Console.WriteLine("Неверный Email или PasswordHash.");
                }
            }
        }
    }
}