using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class DatabaseHelper
    {
        private string connectionString = "Data Source=C:\\databaseforproject\\databaseproject.db;Version=3;";

        // Veritabanı bağlantısını açan metod
        private SQLiteConnection OpenConnection()
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }

        // Kullanıcı kayıt ekleme
        public void AddUser(string username, string password)
        {
            try
            {
                using (SQLiteConnection connection = OpenConnection())
                {
                    string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        // Eşya ekleme
        public void AddItem(string itemName, string itemType, string itemPrice)
        {
            try
            {
                using (SQLiteConnection connection = OpenConnection())
                {
                    string query = "INSERT INTO Items (ItemName, Type, Price) VALUES (@ItemName, @Type, @Price)";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.Parameters.AddWithValue("@Type", itemType);
                        command.Parameters.AddWithValue("@Price", itemPrice);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        // Tüm eşyaları listeleme
        public string GetAllItems()
        {
            StringBuilder items = new StringBuilder();
            try
            {
                using (SQLiteConnection connection = OpenConnection())
                {
                    string query = "SELECT ItemName, Type, Price FROM Items";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.AppendLine($"Item: {reader["ItemName"]}, Type: {reader["Type"]}, Price: {reader["Price"]}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            return items.ToString();
        }

        // Kullanıcıyı doğrulama (login)
        public bool ValidateUser(string username, string password)
        {
            try
            {
                using (SQLiteConnection connection = OpenConnection())
                {
                    string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }
    }

}
