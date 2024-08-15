using Microsoft.EntityFrameworkCore;
using robot_controller_api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace robot_controller_api.Persistence
{
    public class MapEF : IMapDataAccess
    {
        private readonly RobotContext _context;

        public MapEF(RobotContext context)
        {
            _context = context;
        }

        public List<Map> GetMaps()
        {
            return _context.Maps.ToList();
        }

        public void InsertMap(Map newMap)
        {
            _context.Maps.Add(newMap);
            _context.SaveChanges();
        }

        public void UpdateMap(Map updatedMap)
        {
            _context.Maps.Update(updatedMap);
            _context.SaveChanges();
        }

        public void DeleteMap(int id)
        {
            var map = _context.Maps.Find(id);
            if (map != null)
            {
                _context.Maps.Remove(map);
                _context.SaveChanges();
            }
        }
    }

}
