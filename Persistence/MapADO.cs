using Npgsql;
using robot_controller_api.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace robot_controller_api.Persistence
{
    public class MapADO : IMapDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=lam789123;Database=sit331";

        public  List<Map> GetMaps()
        {
            var maps = new List<Map>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var id = dr.GetInt32(0);
                var name = dr.GetString(1);
                var description = dr.IsDBNull(2) ? null : dr.GetString(2);
                var columns = dr.GetInt32(3);
                var rows = dr.GetInt32(4);
                var createDate = dr.GetDateTime(5);
                var modifiedDate = dr.GetDateTime(6);

                var map = new Map(
                    Id: id,
                    Name: name,
                    Columns: columns,
                    Rows: rows,
                    CreatedDate: createDate,
                    ModifiedDate: modifiedDate,
                    Description: description
                );

                maps.Add(map);
            }
            return maps;
        }
        public  void InsertMap(Map newMap)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO map (\"Name\", description, columns, rows, createddate, modifieddate) VALUES (@name, @description, @columns, @rows, @createddate, @modifieddate)", conn);
            cmd.Parameters.AddWithValue("@name", newMap.Name);
            cmd.Parameters.AddWithValue("@description", newMap.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@columns", newMap.Columns);
            cmd.Parameters.AddWithValue("@rows", newMap.Rows);
            cmd.Parameters.AddWithValue("@createddate", DateTime.Now);
            cmd.Parameters.AddWithValue("@modifieddate", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public  void UpdateMap(Map updatedMap)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE map SET \"Name\" = @name, description = @description, columns = @columns, rows = @rows, modifieddate = @modifieddate WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", updatedMap.Id);
            cmd.Parameters.AddWithValue("@name", updatedMap.Name);
            cmd.Parameters.AddWithValue("@description", updatedMap.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@columns", updatedMap.Columns);
            cmd.Parameters.AddWithValue("@rows", updatedMap.Rows);
            cmd.Parameters.AddWithValue("@modifieddate", updatedMap.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public  void DeleteMap(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM map WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
    
}
