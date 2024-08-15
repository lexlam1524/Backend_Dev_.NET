using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IMapDataAccess
    {
        void DeleteMap(int id);
        List<Map> GetMaps();
        void InsertMap(Map newMap);
        void UpdateMap(Map updatedMap);

    }
}