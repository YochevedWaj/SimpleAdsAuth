using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimpleAdsAuth.Data
{
    public class AdsUsersRepository
    {
        private string _connectionString { get; set; }
        public AdsUsersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Users VALUES(@name, @email, @hash)";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", BCrypt.Net.BCrypt.HashPassword(password));
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;

        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                ID = (int)reader["Id"],
                Email = (string)reader["Email"],
                Name = (string)reader["Name"],
                PasswordHash = (string)reader["PasswordHash"]
            };
        }

        public void AddAd(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Ads VALUES(@date, @number, @description, @userId)";
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@number", ad.Number);
            cmd.Parameters.AddWithValue("@description", ad.Description);
            cmd.Parameters.AddWithValue("@userId", ad.UserID);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void DelteAd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Ads WHERE ID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public List<Ad> GetAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT a.*, u.Name FROM Ads a JOIN Users u ON u.ID = a.UserID";
            connection.Open();
            var reader = cmd.ExecuteReader();
            var ads = new List<Ad>();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    ID = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    Number = (string)reader["Number"],
                    Description = (string)reader["Description"],
                    UserID = (int)reader["UserID"]
                });
            }

            return ads;
        }

        public List<Ad> GetAdsByUser(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT a.*, u.Name FROM Ads a JOIN Users u ON u.ID = a.UserID WHERE u.Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            var ads = new List<Ad>();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    ID = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    Number = (string)reader["Number"],
                    Description = (string)reader["Description"],
                    UserID = (int)reader["UserID"]
                });
            }

            return ads;
        }
    }
}
