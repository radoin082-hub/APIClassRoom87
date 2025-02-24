using APIClassRoom.Model;
using Microsoft.Data.SqlClient;
using System.Data;
using BCrypt.Net;

namespace APIClassRoom.Storage
{
    public class UserStorage
    {
        private readonly string _connectionString;

        public UserStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync().ConfigureAwait(false);

                string query =
                    "SELECT Id, LevelId, Email, Password, FullName, Role FROM Users WHERE Email = @Email";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            string storedHashedPassword = reader.GetString(3); // ✅ Fix wrong index

                            if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword)) // ✅ Ensure BCrypt
                            {
                                return new User
                                {
                                    Id = reader.GetGuid(0),
                                    LevelId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                    Email = reader.GetString(2),
                                    Password = storedHashedPassword,
                                    FullName = reader.GetString(4), // ✅ Fix wrong index
                                    Role = (UserRole)reader.GetInt32(5), // ✅ Fix wrong index
                                };
                            }
                        }
                    }
                }
            }

            return null;
        }



        private static bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = new();

            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync().ConfigureAwait(false);

                string query = "SELECT Id, Email, FullName, Role FROM Users";

                using (SqlCommand cmd = new(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        users.Add(new User
                        {
                            Id = reader.GetGuid(0),
                            Email = reader.GetString(1),
                            FullName = reader.GetString(2),
                            Role = (UserRole)reader.GetInt32(3)
                        });
                    }
                }
            }

            return users;
        }

        public async Task<List<User>> GetUsersByLevelAsync(int levelId)
        {
            List<User> users = new();

            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync().ConfigureAwait(false);

                string query = "SELECT Id, Email, FullName, Role FROM Users WHERE LevelId = @LevelId";

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LevelId", levelId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            users.Add(new User
                            {
                                Id = reader.GetGuid(0),
                                Email = reader.GetString(1),
                                FullName = reader.GetString(2),
                                Role = (UserRole)reader.GetInt32(3)
                            });
                        }
                    }
                }
            }

            return users;
        }
        public async Task<bool> CreateUserAsync(User user)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync().ConfigureAwait(false);

                string query = @"INSERT INTO Users (Id, Email, Password, FullName, Role, LevelId) 
                        VALUES (@Id, @Email, @Password, @FullName, @Role, @LevelId)";

                using (SqlCommand cmd = new(query, conn))
                {
                    
                    user.Id = Guid.NewGuid();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Role", (int)user.Role);
                    cmd.Parameters.AddWithValue("@LevelId", user.LevelId ?? (object)DBNull.Value);

                    try
                    {
                        int result = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                        return result > 0;
                    }
                    catch (SqlException)
                    {
                        return false;
                    }
                }
            }
        }
    }
}