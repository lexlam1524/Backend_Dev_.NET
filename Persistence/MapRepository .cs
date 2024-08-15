using FastMember;
using Npgsql;
using robot_controller_api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace robot_controller_api.Persistence
{
    public class MapRepository : IMapDataAccess, IRepository
    {
        private IRepository _repo => this;
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=lam789123;Database=sit331";
        public List<T> ExecuteReader<T>(string sqlCommand, NpgsqlParameter[] dbParams = null) where T : class, new()
        {
            var entities = new List<T>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(sqlCommand, conn);

            if (dbParams is not null)
            {

                cmd.Parameters.AddRange(dbParams.Where(x => x.Value is not null).ToArray());
            }
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var entity = new T();
                dr.MapTo(entity);
                entities.Add(entity);
            }

            return entities;
        }

        public List<Map> GetMaps()
        {
            var maps = _repo.ExecuteReader<Map>("SELECT * FROM public.map");
            return maps;
        }

        public void UpdateMap(Map updatedMap)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("id", updatedMap.Id),
                new NpgsqlParameter("name", updatedMap.Name),
                new NpgsqlParameter("description", updatedMap.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("@columns", updatedMap.Columns),
                new NpgsqlParameter("@rows", updatedMap.Rows),
                new NpgsqlParameter("modifieddate", updatedMap.ModifiedDate)
            };

            _repo.ExecuteReader<Map>("UPDATE map SET \"Name\"=@name, description=@description, columns = @columns, rows = @rows, modifieddate = @modifieddate WHERE id=@id RETURNING *;",sqlParams).SingleOrDefault();
        }

        public void InsertMap(Map map)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("name", map.Name),
                new NpgsqlParameter("description", map.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("@columns", map.Columns),
                new NpgsqlParameter("@rows", map.Rows),
                new NpgsqlParameter("@createddate", DateTime.Now),
                new NpgsqlParameter("modifieddate", DateTime.Now)
            };

            _repo.ExecuteReader<Map>("INSERT INTO map (\"Name\", description, columns, rows, createddate, modifieddate) VALUES (@name, @description, @columns, @rows, @createddate, @modifieddate);", sqlParams);
        }

        public void DeleteMap(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("id", id)
            };

            _repo.ExecuteReader<Map>( "DELETE FROM map WHERE id=@id;", sqlParams);
        }
    }
}
