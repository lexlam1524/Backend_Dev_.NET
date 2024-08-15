using Npgsql;
using FastMember;
namespace robot_controller_api.Persistence
{
    public interface IRepository
    {
        List<T> ExecuteReader<T>(string sqlCommand, NpgsqlParameter[] dbParams = null) where T : class, new();
    }
}
