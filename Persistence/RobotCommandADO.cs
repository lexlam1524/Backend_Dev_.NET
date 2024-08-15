using Npgsql;
using robot_controller_api.Models;


namespace robot_controller_api.Persistence
{
    public class RobotCommandADO : IRobotCommandDataAccess
    {
        private const string CONNECTION_STRING ="Host=localhost;Username=postgres;Password=lam789123;Database=sit331";
        public  List<RobotCommand> GetRobotCommands()
        {
            var robotCommands = new List<RobotCommand>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //read values off the data reader and create a new
                var id = dr.GetInt32(0);
                var name = dr.GetString(1);
                var descr = dr.IsDBNull(2) ? null : dr.GetString(2);
                var isMoveCommand = dr.GetBoolean(3);
                var createDate = dr.GetDateTime(4);
                var modifiedDate = dr.GetDateTime(5);

                var robotCommand = new RobotCommand(
                    Id: id,
                    Name: name,
                    IsMovecommand: isMoveCommand,
                    CreatedDate: createDate,
                    ModifiedDate: modifiedDate,
                    Description: descr
                );

                robotCommands.Add(robotCommand);
            }
            return robotCommands;
        }

        public void InsertRobotCommand(RobotCommand command)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO robotcommand (\"Name\", description, ismovecommand, createddate, modifieddate) VALUES (@name, @description, @ismovecommand, @createddate, @modifieddate)", conn);
            cmd.Parameters.AddWithValue("@name", command.Name);
            cmd.Parameters.AddWithValue("@description", command.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ismovecommand", command.IsMovecommand);
            cmd.Parameters.AddWithValue("@createddate", command.CreatedDate);
            cmd.Parameters.AddWithValue("@modifieddate", command.ModifiedDate);

            cmd.ExecuteNonQuery();
        }
         public  void UpdateRobotCommand(RobotCommand command)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE robotcommand SET \"Name\" = @name, description = @description, ismovecommand = @ismovecommand, createddate = @createddate, modifieddate = @modifieddate WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", command.Id);
            cmd.Parameters.AddWithValue("@name", command.Name);
            cmd.Parameters.AddWithValue("@description", command.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ismovecommand", command.IsMovecommand);
            cmd.Parameters.AddWithValue("@createddate", command.CreatedDate);
            cmd.Parameters.AddWithValue("@modifieddate", command.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public  void DeleteRobotCommand(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM robotcommand WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
