using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class UserADO : IUserDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=lam789123;Database=sit331";

        public List<UserModel> GetUsers()
        {
            var users = new List<UserModel>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM users", conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var id = dr.GetInt32(0);
                var email = dr.GetString(1);
                var firstName = dr.GetString(2);
                var lastName = dr.GetString(3);
                var passwordHash = dr.GetString(4);
                var description = dr.IsDBNull(5) ? null : dr.GetString(5);
                var role = dr.IsDBNull(6) ? null : dr.GetString(6);
                var createdDate = dr.GetDateTime(7);
                var modifiedDate = dr.GetDateTime(8);

                var user = new UserModel(
                    Id: id,
                    email: email,
                    firstName: firstName,
                    lastName: lastName,
                    passwordHash: passwordHash,
                    description: description,
                    role: role,
                    createdDate: createdDate,
                    modifiedDate: modifiedDate
                );

                users.Add(user);
            }
            return users;
        }

        public UserModel GetUserByEmail(string email)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM users WHERE email = @email", conn);
            cmd.Parameters.AddWithValue("@email", email);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var id = reader.GetInt32(0);
                var firstName = reader.GetString(2);
                var lastName = reader.GetString(3);
                var passwordHash = reader.GetString(4);
                var description = reader.IsDBNull(5) ? null : reader.GetString(5);
                var role = reader.IsDBNull(6) ? null : reader.GetString(6);
                var createdDate = reader.GetDateTime(7);
                var modifiedDate = reader.GetDateTime(8);

                var user = new UserModel(
                    Id: id,
                    email: email,
                    firstName: firstName,
                    lastName: lastName,
                    passwordHash: passwordHash,
                    description: description,
                    role: role,
                    createdDate: createdDate,
                    modifiedDate: modifiedDate
                );

                return user;
            }
            return null;
        }


        public void InsertUser(UserModel user)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO users (email, firstname, lastname, passwordhash, description, role, createddate, modifieddate) VALUES (@email, @firstname, @lastname, @passwordhash, @description, @role, @createddate, @modifieddate)", conn);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@firstname", user.FirstName);
            cmd.Parameters.AddWithValue("@lastname", user.LastName);
            cmd.Parameters.AddWithValue("@passwordhash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@description", user.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@role", user.Role ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@createddate", user.CreatedDate);
            cmd.Parameters.AddWithValue("@modifieddate", user.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public void UpdateUser(UserModel user)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE users SET email = @email, firstname = @firstname, lastname = @lastname, passwordhash = @passwordhash, description = @description, role = @role, modifieddate = @modifieddate WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", user.Id);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@firstname", user.FirstName);
            cmd.Parameters.AddWithValue("@lastname", user.LastName);
            cmd.Parameters.AddWithValue("@passwordhash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@description", user.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@role", user.Role ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@modifieddate", user.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public void DeleteUser(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM users WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}