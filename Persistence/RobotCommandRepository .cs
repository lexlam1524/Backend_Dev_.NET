using FastMember;
using Npgsql;
using robot_controller_api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace robot_controller_api.Persistence
{
    public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
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


        public List<Robotcommand> GetRobotCommands()
        {
            var commands = _repo.ExecuteReader<Robotcommand>("SELECT * FROM public.robotcommand");
            return commands;
        }

        public void UpdateRobotCommand(Robotcommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("id", updatedCommand.Id),
                new NpgsqlParameter("name", updatedCommand.Name),
                new NpgsqlParameter("description", updatedCommand.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("ismovecommand", updatedCommand.Ismovecommand)
            };

            _repo.ExecuteReader<Robotcommand>("UPDATE robotcommand SET \"Name\"=@name, description=@description, ismovecommand=@ismovecommand, modifieddate=current_timestamp WHERE id=@id RETURNING *;", sqlParams).Single();

        }

        public void InsertRobotCommand(Robotcommand command)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("name", command.Name),
                new NpgsqlParameter("description", command.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("ismovecommand", command.Ismovecommand),
                new NpgsqlParameter("createddate", command.Createddate),
                new NpgsqlParameter("modifieddate", command.Modifieddate)
            };

            _repo.ExecuteReader<Robotcommand>(
                "INSERT INTO robotcommand (\"Name\", description, ismovecommand, createddate, modifieddate) VALUES (@name, @description, @ismovecommand, @createddate, @modifieddate);", sqlParams);
        }

        public void DeleteRobotCommand(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("id", id)
            };

            _repo.ExecuteReader<Robotcommand>("DELETE FROM robotcommand WHERE id=@id;", sqlParams);
        }

        
    }
}
